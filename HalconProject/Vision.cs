using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HalconDotNet;

namespace HalconProject
{
    /// <summary>单项检测结果（表格里的一行）</summary>
    public class CheckItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsOk { get; set; }
    }

    /// <summary>一帧图像的完整检测结果</summary>
    public class InspectionResult
    {
        public List<CheckItem> Items { get; } = new List<CheckItem>();
        public List<double[]> ScratchPoints { get; } = new List<double[]>();
        public List<double[]> Matches { get; } = new List<double[]>();
        public double ElapsedMs { get; set; }

        public bool IsAllOk
        {
            get
            {
                foreach (var item in Items)
                    if (!item.IsOk) return false;
                return true;
            }
        }

        /// <summary>第一个不合格项的名字（用于错误码），全OK返回null</summary>
        public string FirstNgItem
        {
            get
            {
                foreach (var item in Items)
                    if (!item.IsOk) return item.Name;
                return null;
            }
        }
    }

    /// <summary>垫圈视觉检测器：模板匹配 + 尺寸测量 + 划痕检测</summary>
    public class WasherInspector : IDisposable
    {
        private HTuple _modelID;   // 形状模型句柄，程序启动加载一次

        // ===== 可调参数（以后可以挪到配置文件）=====
        public double MinMatchScore { get; set; } = 0.2;   // 匹配最低分数
        public int ExpectedWashers { get; set; } = 4;      // 期望找到几片
        public double OuterDiaMin { get; set; } = 660;     // 外径下限(px)
        public double OuterDiaMax { get; set; } = 680;     // 外径上限(px)
        public double InnerDiaMin { get; set; } = 380;     // 内径下限(px)
        public double InnerDiaMax { get; set; } = 400;     // 内径上限(px)
                                                           // ============ 划痕检测参数（HDevelop调参定稿）============
        public double ErosionRadius { get; set; } = 15;          // erosion_circle 半径
        public int MeanWindowSize { get; set; } = 151;           // mean_image 窗口
        public double ScratchSensitivity { get; set; } = 18;     // dyn_threshold 灵敏度
        public double ScratchMinArea { get; set; } = 150;        // 面积下限
        public double ScratchMinAnisometry { get; set; } = 3;    // 细长度下限

        private const string ModelFileName = "washer_model.shm";

        public WasherInspector()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                       "Models", ModelFileName);
            if (!File.Exists(path))
                throw new FileNotFoundException(
                    $"找不到形状模型文件：{path}\n请把 HDevelop 生成的 {ModelFileName} 放到该目录");
            HOperatorSet.ReadShapeModel(path, out _modelID);
        }

        /// <summary>主入口：传入一帧灰度图，返回完整检测结果</summary>
        public InspectionResult Inspect(HObject grayImage)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = new InspectionResult();

            // ① 模板匹配定位
            HOperatorSet.FindShapeModel(grayImage, _modelID,
                -Math.PI, 2 * Math.PI, MinMatchScore, 0, 0.5,
                "least_squares", 0, 0.7,
                out HTuple rows, out HTuple cols, out HTuple angles, out HTuple scores);

            string matchValue = scores.Length > 0
    ? $"找到 {scores.Length} 片, 最高分: {scores.TupleMax().D.ToString("F2")}"
    : "未找到任何工件";

            result.Items.Add(new CheckItem
            {
                Name = "模板匹配",
                Value = matchValue,
                IsOk = scores.Length >= ExpectedWashers
            });
            for (int i = 0; i < scores.Length; i++)
                result.Matches.Add(new double[] { rows[i].D, cols[i].D, angles[i].D });

            // ② Blob 分割
            HOperatorSet.Threshold(grayImage, out HObject regions, 11, 243);
            HOperatorSet.Connection(regions, out HObject conn);
            HOperatorSet.SelectShape(conn, out HObject washers,
                "area", "and", 340100, 500000);
            HOperatorSet.CountObj(washers, out HTuple nWashers);

            // ③ 尺寸测量（逐片测）
            bool sizeOk = nWashers.I > 0;
            double outerMeas = 0, innerMeas = 0;
            for (int i = 1; i <= nWashers.I; i++)
            {
                HOperatorSet.SelectObj(washers, out HObject one, i);
                HOperatorSet.FillUp(one, out HObject fillUp);
                HOperatorSet.Difference(fillUp, one, out HObject holeRaw);
                HOperatorSet.Connection(holeRaw, out HObject holeConn);
                HOperatorSet.SelectShape(holeConn, out HObject hole,
                    "area", "and", 115527, 147514);
                HOperatorSet.CountObj(hole, out HTuple nHole);

                HOperatorSet.AreaCenter(fillUp, out HTuple aOut, out _, out _);

                // ★ 修正：aIn 在 if 外面先声明，两个 if 块都能用
                HTuple aIn = new HTuple();
                if (nHole.I > 0)
                    HOperatorSet.AreaCenter(hole, out aIn, out _, out _);

                if (aOut.Length > 0 && aIn.Length > 0)
                {
                    outerMeas = 2 * Math.Sqrt(aOut[0].D / Math.PI);
                    innerMeas = 2 * Math.Sqrt(aIn[0].D / Math.PI);
                    if (outerMeas < OuterDiaMin || outerMeas > OuterDiaMax ||
                        innerMeas < InnerDiaMin || innerMeas > InnerDiaMax)
                        sizeOk = false;
                }
                else sizeOk = false;

                fillUp.Dispose(); holeRaw.Dispose(); holeConn.Dispose();
                hole.Dispose(); one.Dispose();
            }

            result.Items.Add(new CheckItem
            {
                Name = "尺寸测量",
                Value = nWashers.I > 0
                    ? $"外径 {outerMeas:F1} / 内径 {innerMeas:F1} px"
                    : "未找到工件",
                IsOk = sizeOk
            });

            // ④ 划痕检测（与HDevelop定稿脚本一一对应）
            // 合并所有垫圈区域 → 腐蚀掉边缘，得到有效检测区
            HOperatorSet.Union1(washers, out HObject ring);
            HOperatorSet.ErosionCircle(ring, out HObject ringInner, ErosionRadius);

            // 大窗口均值作为背景估计
            HOperatorSet.MeanImage(grayImage, out HObject mean,
                MeanWindowSize, MeanWindowSize);

            // 动态阈值抓暗偏差
            HOperatorSet.DynThreshold(grayImage, mean, out HObject darkRegions,
                ScratchSensitivity, "dark");

            // 闭运算：把断成几段的划痕连成整体
            HOperatorSet.ClosingCircle(darkRegions, out HObject darkClosed, 3.5);

            // 限定在工件表面内
            HOperatorSet.Intersection(darkClosed, ringInner, out HObject cand);

            // 连通域
            HOperatorSet.Connection(cand, out HObject candConn);

            // 双特征过滤：面积 + 细长度（anisometry，不是elongation！）
            HOperatorSet.SelectShape(candConn, out HObject scratches,
                new HTuple("area", "anisometry"), "and",
                new HTuple(ScratchMinArea, ScratchMinAnisometry),
                new HTuple(999999, 100));
            HOperatorSet.CountObj(scratches, out HTuple nScratch);

            // 记录结果
            result.Items.Add(new CheckItem
            {
                Name = "划痕检测",
                Value = nScratch.I == 0 ? "无" : $"发现 {nScratch.I} 处",
                IsOk = nScratch.I == 0
            });
            for (int i = 1; i <= nScratch.I; i++)
            {
                HOperatorSet.SelectObj(scratches, out HObject s, i);
                HOperatorSet.AreaCenter(s, out _, out HTuple sr, out HTuple sc);
                result.ScratchPoints.Add(new double[] { sr.D, sc.D });
                s.Dispose();
            }

            // 释放本段创建的HALCON对象
            ring.Dispose(); ringInner.Dispose(); mean.Dispose();
            darkRegions.Dispose(); darkClosed.Dispose(); cand.Dispose();
            candConn.Dispose(); scratches.Dispose();


            sw.Stop();
            result.ElapsedMs = sw.ElapsedMilliseconds;
            return result;
        }

        public void Dispose()
        {
            if (_modelID != null)
            {
                HOperatorSet.ClearShapeModel(_modelID);
                _modelID = null;
            }
        }
    }
}
