namespace HalconProject
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel9 = new TableLayoutPanel();
            lblCameraStatus = new Label();
            lblPLCStatus = new Label();
            tableLayoutPanel8 = new TableLayoutPanel();
            btnStop = new Button();
            btnStart = new Button();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnConfigSetting = new Button();
            btnSingleStart = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            btnConnectCamera = new Button();
            btnDisconnectCamera = new Button();
            tableLayoutPanel7 = new TableLayoutPanel();
            btnDisconnect = new Button();
            btnConnectPLC = new Button();
            comboBox1 = new ComboBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel10 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            lbTime = new Label();
            lbResult = new Label();
            lbProductID = new Label();
            lblCycleTime = new Label();
            lblJudgeResult = new Label();
            lblCurrentProductId = new Label();
            dgvDetails = new DataGridView();
            colItem = new DataGridViewTextBoxColumn();
            colValue = new DataGridViewTextBoxColumn();
            colJudge = new DataGridViewTextBoxColumn();
            hsmartDisplay = new HalconDotNet.HSmartWindowControl();
            tableLayoutPanel4 = new TableLayoutPanel();
            lstLog = new ListBox();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel9.SuspendLayout();
            tableLayoutPanel8.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel10.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = SystemColors.ButtonHighlight;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.Size = new Size(984, 561);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = SystemColors.ActiveBorder;
            tableLayoutPanel2.ColumnCount = 6;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel9, 5, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel8, 3, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel5, 4, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 1, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel7, 2, 0);
            tableLayoutPanel2.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(984, 50);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel9
            // 
            tableLayoutPanel9.ColumnCount = 2;
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel9.Controls.Add(lblCameraStatus, 0, 0);
            tableLayoutPanel9.Controls.Add(lblPLCStatus, 1, 0);
            tableLayoutPanel9.Location = new Point(823, 3);
            tableLayoutPanel9.Name = "tableLayoutPanel9";
            tableLayoutPanel9.RowCount = 1;
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel9.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel9.Size = new Size(127, 44);
            tableLayoutPanel9.TabIndex = 9;
            // 
            // lblCameraStatus
            // 
            lblCameraStatus.Anchor = AnchorStyles.None;
            lblCameraStatus.AutoSize = true;
            lblCameraStatus.Font = new Font("Microsoft YaHei UI", 7F);
            lblCameraStatus.Location = new Point(3, 14);
            lblCameraStatus.Name = "lblCameraStatus";
            lblCameraStatus.Size = new Size(57, 16);
            lblCameraStatus.TabIndex = 3;
            lblCameraStatus.Text = "相机未连接";
            // 
            // lblPLCStatus
            // 
            lblPLCStatus.Anchor = AnchorStyles.None;
            lblPLCStatus.AutoSize = true;
            lblPLCStatus.Font = new Font("Microsoft YaHei UI", 7F);
            lblPLCStatus.Location = new Point(67, 14);
            lblPLCStatus.Name = "lblPLCStatus";
            lblPLCStatus.Size = new Size(55, 16);
            lblPLCStatus.TabIndex = 2;
            lblPLCStatus.Text = "PLC未连接";
            // 
            // tableLayoutPanel8
            // 
            tableLayoutPanel8.ColumnCount = 2;
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel8.Controls.Add(btnStop, 1, 0);
            tableLayoutPanel8.Controls.Add(btnStart, 0, 0);
            tableLayoutPanel8.Location = new Point(495, 3);
            tableLayoutPanel8.Name = "tableLayoutPanel8";
            tableLayoutPanel8.RowCount = 1;
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel8.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel8.Size = new Size(127, 44);
            tableLayoutPanel8.TabIndex = 8;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(63, 0);
            btnStop.Margin = new Padding(0);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(64, 44);
            btnStop.TabIndex = 1;
            btnStop.Text = "停止";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(0, 0);
            btnStart.Margin = new Padding(0);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(63, 44);
            btnStart.TabIndex = 0;
            btnStart.Text = "启动";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Controls.Add(btnConfigSetting, 1, 0);
            tableLayoutPanel5.Controls.Add(btnSingleStart, 0, 0);
            tableLayoutPanel5.Location = new Point(659, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Size = new Size(127, 44);
            tableLayoutPanel5.TabIndex = 5;
            // 
            // btnConfigSetting
            // 
            btnConfigSetting.Location = new Point(63, 0);
            btnConfigSetting.Margin = new Padding(0);
            btnConfigSetting.Name = "btnConfigSetting";
            btnConfigSetting.Size = new Size(63, 44);
            btnConfigSetting.TabIndex = 2;
            btnConfigSetting.Text = "设置";
            btnConfigSetting.UseVisualStyleBackColor = true;
            // 
            // btnSingleStart
            // 
            btnSingleStart.Font = new Font("Microsoft YaHei UI", 8F);
            btnSingleStart.Location = new Point(0, 0);
            btnSingleStart.Margin = new Padding(0);
            btnSingleStart.Name = "btnSingleStart";
            btnSingleStart.Size = new Size(63, 44);
            btnSingleStart.TabIndex = 1;
            btnSingleStart.Text = "单次触发";
            btnSingleStart.UseVisualStyleBackColor = true;
            btnSingleStart.Click += btnSingleStart_Click;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 2;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(btnConnectCamera, 0, 0);
            tableLayoutPanel6.Controls.Add(btnDisconnectCamera, 1, 0);
            tableLayoutPanel6.Location = new Point(167, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel6.Size = new Size(127, 44);
            tableLayoutPanel6.TabIndex = 6;
            // 
            // btnConnectCamera
            // 
            btnConnectCamera.Dock = DockStyle.Fill;
            btnConnectCamera.Location = new Point(0, 0);
            btnConnectCamera.Margin = new Padding(0);
            btnConnectCamera.Name = "btnConnectCamera";
            btnConnectCamera.Size = new Size(63, 44);
            btnConnectCamera.TabIndex = 0;
            btnConnectCamera.Text = "连相机";
            btnConnectCamera.UseVisualStyleBackColor = true;
            btnConnectCamera.Click += btnConnectCamera_Click;
            // 
            // btnDisconnectCamera
            // 
            btnDisconnectCamera.Dock = DockStyle.Fill;
            btnDisconnectCamera.Location = new Point(63, 0);
            btnDisconnectCamera.Margin = new Padding(0);
            btnDisconnectCamera.Name = "btnDisconnectCamera";
            btnDisconnectCamera.Size = new Size(64, 44);
            btnDisconnectCamera.TabIndex = 1;
            btnDisconnectCamera.Text = "断相机";
            btnDisconnectCamera.UseVisualStyleBackColor = true;
            btnDisconnectCamera.Click += btnDisconnectCamera_Click;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 2;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Controls.Add(btnDisconnect, 1, 0);
            tableLayoutPanel7.Controls.Add(btnConnectPLC, 0, 0);
            tableLayoutPanel7.Location = new Point(331, 3);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Size = new Size(127, 44);
            tableLayoutPanel7.TabIndex = 7;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Location = new Point(63, 0);
            btnDisconnect.Margin = new Padding(0);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(64, 44);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.TabStop = false;
            btnDisconnect.Text = "断PLC";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnectPLC
            // 
            btnConnectPLC.Location = new Point(0, 0);
            btnConnectPLC.Margin = new Padding(0);
            btnConnectPLC.Name = "btnConnectPLC";
            btnConnectPLC.Size = new Size(63, 44);
            btnConnectPLC.TabIndex = 0;
            btnConnectPLC.TabStop = false;
            btnConnectPLC.Text = "连PLC";
            btnConnectPLC.UseVisualStyleBackColor = true;
            btnConnectPLC.Click += btnConnectPLC_Click;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(158, 25);
            comboBox1.TabIndex = 3;
            comboBox1.TabStop = false;
            comboBox1.Text = "方案";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel10, 1, 0);
            tableLayoutPanel3.Controls.Add(hsmartDisplay, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 50);
            tableLayoutPanel3.Margin = new Padding(0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(984, 461);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel10
            // 
            tableLayoutPanel10.ColumnCount = 1;
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel10.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel10.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel10.Controls.Add(dgvDetails, 0, 1);
            tableLayoutPanel10.Dock = DockStyle.Fill;
            tableLayoutPanel10.Location = new Point(495, 3);
            tableLayoutPanel10.Name = "tableLayoutPanel10";
            tableLayoutPanel10.RowCount = 2;
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel10.Size = new Size(486, 455);
            tableLayoutPanel10.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lbTime);
            groupBox1.Controls.Add(lbResult);
            groupBox1.Controls.Add(lbProductID);
            groupBox1.Controls.Add(lblCycleTime);
            groupBox1.Controls.Add(lblJudgeResult);
            groupBox1.Controls.Add(lblCurrentProductId);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Font = new Font("Microsoft YaHei UI", 9F);
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(480, 221);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "检测结果";
            // 
            // lbTime
            // 
            lbTime.AutoSize = true;
            lbTime.Font = new Font("Microsoft YaHei UI", 13F);
            lbTime.Location = new Point(81, 115);
            lbTime.Name = "lbTime";
            lbTime.Size = new Size(42, 24);
            lbTime.TabIndex = 3;
            lbTime.Text = "----";
            // 
            // lbResult
            // 
            lbResult.AutoSize = true;
            lbResult.Font = new Font("Microsoft YaHei UI", 13F);
            lbResult.Location = new Point(81, 76);
            lbResult.Name = "lbResult";
            lbResult.Size = new Size(42, 24);
            lbResult.TabIndex = 3;
            lbResult.Text = "----";
            // 
            // lbProductID
            // 
            lbProductID.AutoSize = true;
            lbProductID.Font = new Font("Microsoft YaHei UI", 13F);
            lbProductID.Location = new Point(67, 36);
            lbProductID.Name = "lbProductID";
            lbProductID.Size = new Size(42, 24);
            lbProductID.TabIndex = 3;
            lbProductID.Text = "----";
            // 
            // lblCycleTime
            // 
            lblCycleTime.AutoSize = true;
            lblCycleTime.Font = new Font("Microsoft YaHei UI", 13F);
            lblCycleTime.Location = new Point(3, 115);
            lblCycleTime.Name = "lblCycleTime";
            lblCycleTime.Size = new Size(100, 24);
            lblCycleTime.TabIndex = 2;
            lblCycleTime.Text = "检测耗时：";
            // 
            // lblJudgeResult
            // 
            lblJudgeResult.AutoSize = true;
            lblJudgeResult.Font = new Font("Microsoft YaHei UI", 13F);
            lblJudgeResult.Location = new Point(0, 76);
            lblJudgeResult.Name = "lblJudgeResult";
            lblJudgeResult.Size = new Size(100, 24);
            lblJudgeResult.TabIndex = 1;
            lblJudgeResult.Text = "检测结果：";
            // 
            // lblCurrentProductId
            // 
            lblCurrentProductId.AutoSize = true;
            lblCurrentProductId.Font = new Font("Microsoft YaHei UI", 13F);
            lblCurrentProductId.Location = new Point(0, 36);
            lblCurrentProductId.Name = "lblCurrentProductId";
            lblCurrentProductId.Size = new Size(83, 24);
            lblCurrentProductId.TabIndex = 0;
            lblCurrentProductId.Text = "产品ID：";
            // 
            // dgvDetails
            // 
            dgvDetails.AllowUserToAddRows = false;
            dgvDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDetails.Columns.AddRange(new DataGridViewColumn[] { colItem, colValue, colJudge });
            dgvDetails.Dock = DockStyle.Left;
            dgvDetails.Location = new Point(3, 230);
            dgvDetails.Name = "dgvDetails";
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.Size = new Size(385, 222);
            dgvDetails.TabIndex = 1;
            // 
            // colItem
            // 
            colItem.HeaderText = "\t检测项";
            colItem.Name = "colItem";
            colItem.ReadOnly = true;
            colItem.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colValue
            // 
            colValue.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colValue.HeaderText = "结果值";
            colValue.Name = "colValue";
            colValue.ReadOnly = true;
            colValue.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colJudge
            // 
            colJudge.HeaderText = "判定";
            colJudge.Name = "colJudge";
            colJudge.ReadOnly = true;
            colJudge.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // hsmartDisplay
            // 
            hsmartDisplay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            hsmartDisplay.AutoValidate = AutoValidate.EnableAllowFocusChange;
            hsmartDisplay.Dock = DockStyle.Fill;
            hsmartDisplay.HDoubleClickToFitContent = true;
            hsmartDisplay.HDrawingObjectsModifier = HalconDotNet.HSmartWindowControl.DrawingObjectsModifier.None;
            hsmartDisplay.HImagePart = new Rectangle(11, 22, 615, 433);
            hsmartDisplay.HKeepAspectRatio = true;
            hsmartDisplay.HMoveContent = true;
            hsmartDisplay.HZoomContent = HalconDotNet.HSmartWindowControl.ZoomContent.WheelForwardZoomsIn;
            hsmartDisplay.Location = new Point(0, 0);
            hsmartDisplay.Margin = new Padding(0);
            hsmartDisplay.Name = "hsmartDisplay";
            hsmartDisplay.Size = new Size(492, 461);
            hsmartDisplay.TabIndex = 2;
            hsmartDisplay.WindowSize = new Size(492, 461);
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = SystemColors.ActiveBorder;
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Controls.Add(lstLog, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 511);
            tableLayoutPanel4.Margin = new Padding(0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(984, 50);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lstLog
            // 
            lstLog.Dock = DockStyle.Fill;
            lstLog.FormattingEnabled = true;
            lstLog.ItemHeight = 17;
            lstLog.Location = new Point(0, 0);
            lstLog.Margin = new Padding(0);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(984, 50);
            lstLog.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 561);
            Controls.Add(tableLayoutPanel1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "视觉检测系统";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel9.ResumeLayout(false);
            tableLayoutPanel9.PerformLayout();
            tableLayoutPanel8.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel10.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDetails).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Button btnConnectPLC;
        private Button btnDisconnect;
        private ListBox lstLog;
        private Label lblPLCStatus;
        private ComboBox comboBox1;
        private TableLayoutPanel tableLayoutPanel5;
        private Button btnStart;
        private Button btnStop;
        private TableLayoutPanel tableLayoutPanel6;
        private Button btnConnectCamera;
        private Button btnDisconnectCamera;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel9;
        private TableLayoutPanel tableLayoutPanel8;
        private Label lblCameraStatus;
        private Button btnConfigSetting;
        private Button btnSingleStart;
        private TableLayoutPanel tableLayoutPanel10;
        private GroupBox groupBox1;
        private Label lblJudgeResult;
        private Label lblCurrentProductId;
        private Label lblCycleTime;
        private Label lbTime;
        private Label lbResult;
        private Label lbProductID;
        private DataGridView dgvDetails;
        private HalconDotNet.HSmartWindowControl hsmartDisplay;
        private DataGridViewTextBoxColumn colItem;
        private DataGridViewTextBoxColumn colValue;
        private DataGridViewTextBoxColumn colJudge;
    }
}
