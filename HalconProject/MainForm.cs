using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;
using MvCameraControl;
using HalconDotNet;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;



namespace HalconProject
{
    public partial class MainForm : Form
    {
        // ================= PLC =================
        private bool isRunning = false;   // 用户是否已按下"启动"
        private Plc plc;
        private bool isMonitoring = false;
        private bool lastTrigger = false;
        private bool manualDisconnect = false;
        private const string ADDR_TRIGGER = "DB2.DBX0.0";
        private const string ADDR_PRODUCTID = "DB2.DBW2";
        private const string ADDR_RESULTOK = "DB2.DBX4.0";
        private const string ADDR_ERRORCODE = "DB2.DBW6";

        // ================= 相机 =================
        private IDevice device;
        private bool isGrabbing = false;
        private Task _receiveTask;
        private static object saveImageLock = new object();
        private IFrameOut freForSave;          // 最新一帧缓存
        private bool _firstFrame = true;

        // ================= 视觉检测 =================
        private WasherInspector _inspector;    // 检测器（模型只加载一次）

        public MainForm()
        {
            InitializeComponent();
            SDKSystem.Initialize();

            // 加载视觉模型
            try
            {
                _inspector = new WasherInspector();
                AddLog("视觉模型加载成功");
            }
            catch (Exception ex)
            {
                _inspector = null;
                AddLog("视觉模型加载失败：" + ex.Message);
            }

            AddLog("系统启动");
        }

        // ============================================================
        //                      PLC 通信部分
        // ============================================================

        private async Task PollingLoopAsync()
        {
            while (isMonitoring)
            {
                try
                {
                    object triggerObj = await plc.ReadAsync(ADDR_TRIGGER);
                    bool trigger = (bool)triggerObj;
                    if (trigger && !lastTrigger)
                    {
                        ushort productId = (ushort)await plc.ReadAsync(ADDR_PRODUCTID);
                        AddLog($"[{DateTime.Now:HH:mm:ss.fff}]检测到触发！ProductID={productId}");

                        
                        string savedImagePath;
                        InspectionResult result = RunDetectionSafe(productId, out savedImagePath);

                        bool isOk = result != null && result.IsAllOk;
                        short errorCode = 0;
                        if (!isOk)
                        {
                            if (result == null) errorCode = 99;
                            else
                            {
                                switch (result.FirstNgItem)
                                {
                                    case "模板匹配": errorCode = 1; break;
                                    case "尺寸测量": errorCode = 2; break;
                                    case "划痕检测": errorCode = 3; break;
                                    default: errorCode = 9; break;
                                }
                            }
                        }

                        // 回写 PLC
                        await plc.WriteAsync(ADDR_RESULTOK, isOk);
                        await plc.WriteAsync(ADDR_ERRORCODE, errorCode);
                        AddLog($"[{DateTime.Now:HH:mm:ss.fff}] 已回写结果：{(isOk ? "OK" : "NG")}，错误码={errorCode}");

                        // 存数据库
                        if (result != null)
                            SaveToDatabase(result, productId, savedImagePath);

                        // 刷新 UI
                        if (result != null) ShowResult(result);

                        // 最后复位 Trigger
                        await plc.WriteAsync(ADDR_TRIGGER, false);
                        AddLog("Trigger 已复位，等待下一个工件");
                    }

                    lastTrigger = trigger;
                }
                catch (Exception ex)
                {
                    if (manualDisconnect || !isRunning) return;   // 用户已停止就不重连
                    AddLog("通讯异常：" + ex.Message + "，尝试重连...");
                    SetStatus("重连中...", Color.Orange);
                    isMonitoring = false;
                    await ReconnectAsync();
                    return;
                }

            }
        }

        private async Task ReconnectAsync()
        {
            // 按了启动才自动重连；手动停止/断开不重连
            while (isRunning && !manualDisconnect && !this.IsDisposed)
            {
                try
                {
                    await Task.Delay(2000);
                    plc = new Plc(CpuType.S71500, "192.168.231.129", 0, 1);
                    await plc.OpenAsync();
                    SetStatus("已连接", Color.Green);
                    AddLog("重连成功");
                    lastTrigger = (bool)await plc.ReadAsync(ADDR_TRIGGER);
                    isMonitoring = true;           // 恢复循环
                    _ = Task.Run(PollingLoopAsync);
                    return;
                }
                catch
                {
                    AddLog("重连失败，2秒后再试...");
                }
            }
        }


        private async void btnConnectPLC_Click(object sender, EventArgs e)
        {
            btnConnectPLC.Enabled = false;
            manualDisconnect = false;
            try
            {
                plc = new Plc(CpuType.S71500, "192.168.1.100", 0, 1);
                await plc.OpenAsync();
                SetStatus("PLC已连接", Color.Green);
                AddLog($"PLC连接成功{plc.IP}");
                btnDisconnect.Enabled = true;
                btnStart.Enabled = true;          // 连上了才允许启动
            }
            catch (Exception ex)
            {
                SetStatus("PLC连接失败", Color.Red);
                AddLog("PLC连接失败" + ex.Message);
            }
            btnConnectPLC.Enabled = true;
        }


        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // 断开 = 连接和运行全停
            StopLoop();                            // 
            manualDisconnect = true;
            isMonitoring = false;
            try { plc?.Close(); } catch { }
            SetStatus("PLC未连接", Color.Gray);
            AddLog("PLC已经断开连接");
            btnStart.Enabled = false;
            btnStop.Enabled = false;
            btnDisconnect.Enabled = false;
        }

        // ============================================================
        //                      视觉检测部分
        // ============================================================

        /// <summary>安全地取最新帧并跑检测，失败时在日志里写明原因</summary>
        private InspectionResult RunDetectionSafe(ushort productId,out string savedImagePath)
        {
            savedImagePath = null;
            if (_inspector == null)
            {
                AddLog("检测失败原因：视觉模型未加载");
                return null;
            }

            IFrameOut frameForDetection;
            lock (saveImageLock)
            {
                frameForDetection = freForSave?.Clone() as IFrameOut;
            }
            if (frameForDetection == null)
            {
                AddLog($"检测失败原因：没有可用图像。相机已连接={device != null}，正在取流={isGrabbing}");
                return null;
            }

            try
            {
                HObject hImg = FrameToHObject(frameForDetection);
                try
                {
                    if (hImg == null)
                    {
                        AddLog($"检测失败原因：不支持的像素格式 {frameForDetection.Image.PixelType}");
                        return null;
                    }

                    // 不管相机出的是彩色还是黑白，统一转成单通道灰度
                    HOperatorSet.CountChannels(hImg, out HTuple ch);
                    HObject gray;
                    if (ch.I >= 3)
                    {
                        HOperatorSet.Rgb1ToGray(hImg, out gray);   // 彩色 → 灰度
                    }
                    else
                    {
                        gray = hImg.CopyObj(1, 1);                 // 已经是单通道，取出来
                    }

                    InspectionResult result;
                    try
                    {
                        result = _inspector.Inspect(gray);
                    }
                    finally
                    {
                        //唯一文件名：产品ID+时间戳，每次一张，不被覆盖
                        string folder = @"E:\Images";
                        Directory.CreateDirectory(folder);   // 文件夹不存在会自动建
                        savedImagePath = Path.Combine(folder,
                            $"{productId}_{DateTime.Now:yyyyMMdd_HHmmss_fff}.bmp");

                        HOperatorSet.WriteImage(gray, "bmp", 0, savedImagePath);
                        gray.Dispose();
                    }


                    frameForDetection.Dispose();
                    return result;
                }
                finally { hImg?.Dispose(); }
            }
            catch (Exception ex)
            {
                AddLog("检测异常：" + ex.Message);
                frameForDetection.Dispose();
                return null;
            }
        }



        /// <summary>刷新右侧结果区</summary>
        private void ShowResult(InspectionResult r)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => ShowResult(r)));
                return;
            }
            bool ok = r.IsAllOk;
            lbResult.Text = ok ? "OK" : "NG";
            lbResult.ForeColor = ok ? Color.LimeGreen : Color.Red;
            lbTime.Text = $"{r.ElapsedMs:F0} ms";
            dgvDetails.Rows.Clear();
            foreach (var item in r.Items)
                AddDetailRow(item.Name, item.Value, item.IsOk);
        }

        private void AddDetailRow(string item, string value, bool isOk)
        {
            int rowIndex = dgvDetails.Rows.Add();
            dgvDetails.Rows[rowIndex].Cells["colItem"].Value = item;
            dgvDetails.Rows[rowIndex].Cells["colValue"].Value = value;
            dgvDetails.Rows[rowIndex].Cells["colJudge"].Value = isOk ? "√ OK" : "× NG";
            if (!isOk)
            {
                dgvDetails.Rows[rowIndex].DefaultCellStyle.BackColor = Color.MistyRose;
                dgvDetails.Rows[rowIndex].Cells["colJudge"].Style.ForeColor = Color.Red;
            }
        }

        /// <summary>单次触发按钮：手动跑一次真检测</summary>
        private async void btnSingleStart_Click(object sender, EventArgs e)
        {
            btnSingleStart.Enabled = false;
            string path;   // ★ 先声明
            var result = await Task.Run(() => RunDetectionSafe(0, out path));
            if (result == null)
                AddLog("检测失败：无图像或模型未加载");
            else
                ShowResult(result);
            btnSingleStart.Enabled = true;
        }


        // ============================================================
        //                      相机部分
        // ============================================================

        private void btnConnectCamera_Click(object sender, EventArgs e)
        {
            try
            {
                List<IDeviceInfo> deviceList;
                int result = DeviceEnumerator.EnumDevices(DeviceTLayerType.MvGigEDevice
                    | DeviceTLayerType.MvUsbDevice, out deviceList);
                if (result != MvError.MV_OK || deviceList.Count == 0)
                {
                    AddLog("未找到相机，请检查连接");
                    MessageBox.Show("未找到相机！");
                    return;
                }

                device = DeviceFactory.CreateDevice(deviceList[0]);
                result = device.Open();
                if (result != MvError.MV_OK)
                {
                    MessageBox.Show($"打开设备失败：0x{result:X8}");
                    device.Dispose();
                    device = null;
                    return;
                }

                device.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
                device.Parameters.SetEnumValueByString("TriggerMode", "Off");
                device.Parameters.SetFloatValue("ExposureTime", 12000f);

                device.Parameters.GetStringValue("DeviceModelName", out IStringValue modelName);
                AddLog($"相机连接成功：{modelName.CurValue}");

                lblCameraStatus.Text = "相机已连接";
                lblCameraStatus.ForeColor = Color.Green;
                btnConnectCamera.Enabled = false;
                btnDisconnectCamera.Enabled = true;
                isGrabbing = true;
                _receiveTask = Task.Run(() => ReceiveThreadProcess());

                int grabRet = device.StreamGrabber.StartGrabbing();
                if (grabRet != MvError.MV_OK)
                {
                    isGrabbing = false;
                    AddLog($"开始取流失败：0x{grabRet:X8}");
                    return;
                }
                AddLog("开始取流");
            }
            catch (Exception ex)
            {
                AddLog("相机连接异常：" + ex.Message);
                MessageBox.Show("连接异常：" + ex.Message);
            }
        }

        private async void btnDisconnectCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (isGrabbing)
                {
                    isGrabbing = false;
                    if (_receiveTask != null) await _receiveTask;
                    device?.StreamGrabber.StopGrabbing();
                }
                if (device != null)
                {
                    device.Close();
                    device.Dispose();
                    device = null;
                }
                AddLog("相机已断开");
                lblCameraStatus.Text = "相机未连接";
                lblCameraStatus.ForeColor = Color.Gray;
                btnConnectCamera.Enabled = true;
                btnDisconnectCamera.Enabled = false;
            }
            catch (Exception ex)
            {
                AddLog("断开异常：" + ex.Message);
            }
        }

        /// <summary>图像接收线程：缓存最新帧 + 显示</summary>
        private void ReceiveThreadProcess()
        {
            while (isGrabbing)
            {
                int nRet = device.StreamGrabber.GetImageBuffer(1000, out IFrameOut frameOut);
                if (nRet != MvError.MV_OK)
                {
                    Thread.Sleep(5);
                    continue;
                }

                // 缓存最新帧
                lock (saveImageLock)
                {
                    freForSave?.Dispose();
                    freForSave = frameOut.Clone() as IFrameOut;
                }

                // 显示
                HObject hImg = FrameToHObject(frameOut);
                if (hImg != null)
                {
                    try
                    {
                        this.Invoke(() =>
                        {
                            if (_firstFrame)
                            {
                                HOperatorSet.GetImageSize(hImg, out HTuple wT, out HTuple hT);
                                hsmartDisplay.HalconWindow.SetPart(0, 0, hT.I - 1, wT.I - 1);
                                _firstFrame = false;
                            }
                            hsmartDisplay.HalconWindow.DispObj(hImg);
                        });
                    }
                    finally { hImg.Dispose(); }
                }

                device.StreamGrabber.FreeImageBuffer(frameOut);
            }
        }

        /// <summary>海康SDK帧 → Halcon HObject</summary>
        private HObject FrameToHObject(IFrameOut frame)
        {
            var img = frame.Image;
            int w = (int)img.Width;
            int h = (int)img.Height;

            if (img.PixelType == MvGvspPixelType.PixelType_Gvsp_Mono8)
            {
                byte[] data = img.PixelData;
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    HOperatorSet.GenImage1(out HObject hImage, "byte", w, h,
                                           handle.AddrOfPinnedObject());
                    return hImage;
                }
                finally { handle.Free(); }
            }
            else if (img.PixelType == MvGvspPixelType.PixelType_Gvsp_RGB8_Packed)
            {
                byte[] data = img.PixelData;
                GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
                try
                {
                    HOperatorSet.GenImageInterleaved(out HObject hImage,
                        handle.AddrOfPinnedObject(), "rgb", w, h, 0,
                        "byte", w, h, 0, 0, -1, 0);
                    return hImage;
                }
                finally { handle.Free(); }
            }
            return null;
        }

        // ============================================================
        //                      显示辅助,鼠标滚轮
        // ============================================================

        private void HsmartDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            if (hsmartDisplay.HalconWindow == null) return;
            hsmartDisplay.HalconWindow.GetPart(out HTuple row1, out HTuple col1,
                out HTuple row2, out HTuple col2);
            double imagePartHeight = (row2 - row1).D;
            if (imagePartHeight <= 0.001) return;

            double currentZoom = hsmartDisplay.Height / imagePartHeight;
            double mouseRow = row1.D + (e.Y / currentZoom);
            double mouseCol = col1.D + (e.X / currentZoom);

            double newZoom = e.Delta > 0 ? currentZoom * 1.2 : currentZoom / 1.2;
            if (newZoom < 0.01) newZoom = 0.01;
            if (newZoom > 100.0) newZoom = 100.0;

            double newHeightImage = hsmartDisplay.Height / newZoom;
            double newWidthImage = hsmartDisplay.Width / newZoom;
            double newRow1 = mouseRow - (e.Y / newZoom);
            double newCol1 = mouseCol - (e.X / newZoom);

            hsmartDisplay.HalconWindow.SetPart(newRow1, newCol1,
                newRow1 + newHeightImage, newCol1 + newWidthImage);
        }

        // ============================================================
        //                      日志 / 状态 / 生命周期
        // ============================================================

        private void SetStatus(string text, Color color)
        {
            if (lblPLCStatus.InvokeRequired)
                lblPLCStatus.BeginInvoke(new Action(() =>
                { lblPLCStatus.Text = text; lblPLCStatus.ForeColor = color; }));
            else
            { lblPLCStatus.Text = text; lblPLCStatus.ForeColor = color; }
        }

        private void AddLog(string msg)
        {
            if (lstLog.InvokeRequired)
                lstLog.BeginInvoke(new Action(() =>
                {
                    lstLog.Items.Insert(0, msg);
                    if (lstLog.Items.Count > 200) lstLog.Items.RemoveAt(200);
                }));
            else
            {
                lstLog.Items.Insert(0, msg);
                if (lstLog.Items.Count > 200) lstLog.Items.RemoveAt(200);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            hsmartDisplay.MouseWheel += HsmartDisplay_MouseWheel;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnDisconnect_Click(null, null);
            btnDisconnectCamera_Click(null, null);
            StopLoop();
            manualDisconnect = true;
            isMonitoring = false;
            try { plc?.Close(); } catch { }

            try
            {
                if (isGrabbing)
                {
                    isGrabbing = false;
                    _receiveTask?.Wait(1000);
                    device?.StreamGrabber.StopGrabbing();
                }
                device?.Close();
                device?.Dispose();
                device = null;
            }
            catch { }

            _inspector?.Dispose();
            SDKSystem.Finalize();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (plc == null || !plc.IsConnected)
            {
                MessageBox.Show("请先连接PLC", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isRunning) return;                 // 防重复点击

            isRunning = true;
            isMonitoring = true;
            manualDisconnect = false;
            lastTrigger = false;                   // 启动瞬间强制清边沿记忆，避免错过第一个触发

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            AddLog("检测循环已启动，等待PLC触发...");
            _ = Task.Run(PollingLoopAsync);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopLoop();
            AddLog("检测循环已停止");
        }
        private void StopLoop()
        {
            isRunning = false;
            isMonitoring = false;                  // while退出，循环结束
            btnStop.Enabled = false;
            btnStart.Enabled = plc != null && plc.IsConnected;
        }
        /// <summary>
        /// 只保存路径字符串到数据库
        /// </summary>
        private void SaveToDatabase(InspectionResult result, ushort productId, string imagePath)
        {
            // 连接字符串
            string connStr = "Data Source=localhost;Initial Catalog=HalconProject;Integrated Security=True;TrustServerCertificate=True;";


            // 拼接测量值和缺陷信息
            var measList = result.Items.Where(x => x.Name.Contains("尺寸") || x.Name.Contains("测量"));
            string measureStr = string.Join("; ", measList.Select(x => $"{x.Name}={x.Value}"));

            var defectList = result.Items.Where(x => x.Name.Contains("划痕") || x.Name.Contains("缺陷"));
            string defectStr = defectList.Any() ? string.Join("; ", defectList.Select(x => $"{x.Name}={x.Value}")) : "无";

            string resultStr = result.IsAllOk ? "OK" : "NG";

            string sql = @"INSERT INTO InspectionRecords (CheckTime, ProductId, Result, Measurements, DefectInfo, ImagePath)
                   VALUES (@Time, @Pid, @Res, @Meas, @Def, @Img)";

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Pid", (int)productId);   // 加 (int) 强转
                    cmd.Parameters.AddWithValue("@Res", resultStr);
                    cmd.Parameters.AddWithValue("@Meas", measureStr);
                    cmd.Parameters.AddWithValue("@Def", defectStr);

                    // ★ 核心点：这里直接存路径字符串，非常快，不占数据库空间
                    cmd.Parameters.AddWithValue("@Img", imagePath);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                AddLog("数据及路径已保存到数据库");
            }
            catch (Exception ex)
            {
                AddLog($"数据库保存失败: {ex.Message}");
            }
        }
    }
}
