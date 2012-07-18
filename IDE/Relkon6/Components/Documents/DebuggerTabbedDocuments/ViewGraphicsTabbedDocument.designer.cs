namespace Kontel.Relkon.Components.Documents
{
    partial class ViewGraphicsTabbedDocument
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.borderPanel1 = new Kontel.Relkon.BorderPanel();
            this.gbGraphics = new System.Windows.Forms.GroupBox();
            this.plVarsView = new Kontel.Relkon.Plotter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudVisibleTime = new System.Windows.Forms.NumericUpDown();
            this.cbVar_5 = new System.Windows.Forms.ComboBox();
            this.cbVar_4 = new System.Windows.Forms.ComboBox();
            this.cbVar_3 = new System.Windows.Forms.ComboBox();
            this.cbVar_2 = new System.Windows.Forms.ComboBox();
            this.cbVar_1 = new System.Windows.Forms.ComboBox();
            this.cbVar_0 = new System.Windows.Forms.ComboBox();
            this.cbXScale = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.borderPanel1.SuspendLayout();
            this.gbGraphics.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVisibleTime)).BeginInit();
            this.SuspendLayout();
            // 
            // borderPanel1
            // 
            this.borderPanel1.AutoScroll = true;
            this.borderPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.borderPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.borderPanel1.Controls.Add(this.gbGraphics);
            this.borderPanel1.Controls.Add(this.groupBox1);
            this.borderPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.borderPanel1.Location = new System.Drawing.Point(5, 5);
            this.borderPanel1.Name = "borderPanel1";
            this.borderPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.borderPanel1.Size = new System.Drawing.Size(850, 489);
            this.borderPanel1.TabIndex = 0;
            // 
            // gbGraphics
            // 
            this.gbGraphics.BackColor = System.Drawing.SystemColors.Control;
            this.gbGraphics.Controls.Add(this.plVarsView);
            this.gbGraphics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbGraphics.Location = new System.Drawing.Point(5, 5);
            this.gbGraphics.Name = "gbGraphics";
            this.gbGraphics.Padding = new System.Windows.Forms.Padding(5);
            this.gbGraphics.Size = new System.Drawing.Size(840, 326);
            this.gbGraphics.TabIndex = 1;
            this.gbGraphics.TabStop = false;
            this.gbGraphics.Text = "Графики";
            // 
            // plVarsView
            // 
            this.plVarsView.AutoScroll = true;
            this.plVarsView.AutoSize = true;
            this.plVarsView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.plVarsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plVarsView.EditButtons = System.Windows.Forms.MouseButtons.Left;
            this.plVarsView.EditModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.plVarsView.IsAutoScrollRange = false;
            this.plVarsView.IsEnableHEdit = false;
            this.plVarsView.IsEnableHPan = true;
            this.plVarsView.IsEnableHZoom = true;
            this.plVarsView.IsEnableVEdit = false;
            this.plVarsView.IsEnableVPan = true;
            this.plVarsView.IsEnableVZoom = true;
            this.plVarsView.IsPrintFillPage = true;
            this.plVarsView.IsPrintKeepAspectRatio = true;
            this.plVarsView.IsScrollY2 = false;
            this.plVarsView.IsShowContextMenu = true;
            this.plVarsView.IsShowCopyMessage = true;
            this.plVarsView.IsShowCursorValues = false;
            this.plVarsView.IsShowHScrollBar = false;
            this.plVarsView.IsShowPointValues = false;
            this.plVarsView.IsShowVScrollBar = false;
            this.plVarsView.IsSynchronizeXAxes = false;
            this.plVarsView.IsSynchronizeYAxes = false;
            this.plVarsView.IsZoomOnMouseCenter = false;
            this.plVarsView.LinkButtons = System.Windows.Forms.MouseButtons.Left;
            this.plVarsView.LinkModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.None)));
            this.plVarsView.Location = new System.Drawing.Point(5, 18);
            this.plVarsView.Name = "plVarsView";
            this.plVarsView.PanButtons = System.Windows.Forms.MouseButtons.Left;
            this.plVarsView.PanButtons2 = System.Windows.Forms.MouseButtons.Middle;
            this.plVarsView.PanModifierKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.None)));
            this.plVarsView.PanModifierKeys2 = System.Windows.Forms.Keys.None;
            this.plVarsView.PointDateFormat = "g";
            this.plVarsView.PointValueFormat = "G";
            this.plVarsView.ScrollMaxX = 0;
            this.plVarsView.ScrollMaxY = 0;
            this.plVarsView.ScrollMaxY2 = 0;
            this.plVarsView.ScrollMinX = 0;
            this.plVarsView.ScrollMinY = 0;
            this.plVarsView.ScrollMinY2 = 0;
            this.plVarsView.Size = new System.Drawing.Size(830, 303);
            this.plVarsView.TabIndex = 0;
            this.plVarsView.ZoomButtons = System.Windows.Forms.MouseButtons.Left;
            this.plVarsView.ZoomButtons2 = System.Windows.Forms.MouseButtons.None;
            this.plVarsView.ZoomModifierKeys = System.Windows.Forms.Keys.None;
            this.plVarsView.ZoomModifierKeys2 = System.Windows.Forms.Keys.None;
            this.plVarsView.ZoomStepFraction = 0.1;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.nudVisibleTime);
            this.groupBox1.Controls.Add(this.cbVar_5);
            this.groupBox1.Controls.Add(this.cbVar_4);
            this.groupBox1.Controls.Add(this.cbVar_3);
            this.groupBox1.Controls.Add(this.cbVar_2);
            this.groupBox1.Controls.Add(this.cbVar_1);
            this.groupBox1.Controls.Add(this.cbVar_0);
            this.groupBox1.Controls.Add(this.cbXScale);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(5, 331);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(840, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры графиков";
            // 
            // nudVisibleTime
            // 
            this.nudVisibleTime.Location = new System.Drawing.Point(333, 17);
            this.nudVisibleTime.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudVisibleTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudVisibleTime.Name = "nudVisibleTime";
            this.nudVisibleTime.Size = new System.Drawing.Size(86, 20);
            this.nudVisibleTime.TabIndex = 18;
            this.nudVisibleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudVisibleTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudVisibleTime.ValueChanged += new System.EventHandler(this.nudVisibleTime_ValueChanged);
            // 
            // cbVar_5
            // 
            this.cbVar_5.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_5.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_5.FormattingEnabled = true;
            this.cbVar_5.Location = new System.Drawing.Point(6, 129);
            this.cbVar_5.Name = "cbVar_5";
            this.cbVar_5.Size = new System.Drawing.Size(121, 21);
            this.cbVar_5.Sorted = true;
            this.cbVar_5.TabIndex = 17;
            // 
            // cbVar_4
            // 
            this.cbVar_4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_4.FormattingEnabled = true;
            this.cbVar_4.Location = new System.Drawing.Point(6, 107);
            this.cbVar_4.Name = "cbVar_4";
            this.cbVar_4.Size = new System.Drawing.Size(121, 21);
            this.cbVar_4.Sorted = true;
            this.cbVar_4.TabIndex = 16;
            // 
            // cbVar_3
            // 
            this.cbVar_3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_3.FormattingEnabled = true;
            this.cbVar_3.Location = new System.Drawing.Point(6, 84);
            this.cbVar_3.Name = "cbVar_3";
            this.cbVar_3.Size = new System.Drawing.Size(121, 21);
            this.cbVar_3.Sorted = true;
            this.cbVar_3.TabIndex = 15;
            // 
            // cbVar_2
            // 
            this.cbVar_2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_2.FormattingEnabled = true;
            this.cbVar_2.Location = new System.Drawing.Point(6, 61);
            this.cbVar_2.Name = "cbVar_2";
            this.cbVar_2.Size = new System.Drawing.Size(121, 21);
            this.cbVar_2.Sorted = true;
            this.cbVar_2.TabIndex = 14;
            // 
            // cbVar_1
            // 
            this.cbVar_1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_1.FormattingEnabled = true;
            this.cbVar_1.Location = new System.Drawing.Point(6, 38);
            this.cbVar_1.Name = "cbVar_1";
            this.cbVar_1.Size = new System.Drawing.Size(121, 21);
            this.cbVar_1.Sorted = true;
            this.cbVar_1.TabIndex = 13;
            // 
            // cbVar_0
            // 
            this.cbVar_0.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbVar_0.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbVar_0.FormattingEnabled = true;
            this.cbVar_0.Location = new System.Drawing.Point(6, 16);
            this.cbVar_0.Name = "cbVar_0";
            this.cbVar_0.Size = new System.Drawing.Size(121, 21);
            this.cbVar_0.Sorted = true;
            this.cbVar_0.TabIndex = 12;
            // 
            // cbXScale
            // 
            this.cbXScale.AutoSize = true;
            this.cbXScale.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cbXScale.Checked = true;
            this.cbXScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbXScale.Location = new System.Drawing.Point(133, 42);
            this.cbXScale.Name = "cbXScale";
            this.cbXScale.Size = new System.Drawing.Size(175, 17);
            this.cbXScale.TabIndex = 11;
            this.cbXScale.Text = "Сдвигать график по времени";
            this.cbXScale.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(133, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Интервал времени просмотра (сек.):";
            // 
            // ViewGraphicsTabbedDocument
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.borderPanel1);
            this.Name = "ViewGraphicsTabbedDocument";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(860, 499);
            this.Closed += new System.EventHandler(this.ViewGraphicsTabbedDocument_Closed);
            this.borderPanel1.ResumeLayout(false);
            this.gbGraphics.ResumeLayout(false);
            this.gbGraphics.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVisibleTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Kontel.Relkon.BorderPanel borderPanel1;
        private Kontel.Relkon.Plotter plVarsView;
        private System.Windows.Forms.GroupBox gbGraphics;
        private System.Windows.Forms.GroupBox groupBox1;
        //private System.Windows.Forms.ComboBox[] cbVar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbXScale;
        private System.Windows.Forms.ComboBox cbVar_5;
        private System.Windows.Forms.ComboBox cbVar_4;
        private System.Windows.Forms.ComboBox cbVar_3;
        private System.Windows.Forms.ComboBox cbVar_2;
        private System.Windows.Forms.ComboBox cbVar_1;
        private System.Windows.Forms.ComboBox cbVar_0;
        private System.Windows.Forms.NumericUpDown nudVisibleTime;
    }
}
