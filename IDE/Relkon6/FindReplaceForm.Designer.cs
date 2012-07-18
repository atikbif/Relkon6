namespace Kontel.Relkon
{
    partial class FindReplaceForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReplaceForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReplace = new System.Windows.Forms.ToolStripButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.ddlSearchingList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pReplaceWithContainer = new System.Windows.Forms.Panel();
            this.ddlReplacingList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.bFindNext = new System.Windows.Forms.Button();
            this.pReplaceContainer = new System.Windows.Forms.Panel();
            this.bReplace = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSearchUp = new System.Windows.Forms.CheckBox();
            this.cbWholeWord = new System.Windows.Forms.CheckBox();
            this.cbMatchCase = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.pReplaceAllContainer = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pReplaceWithContainer.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.pReplaceContainer.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pReplaceAllContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 269);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(1, 268);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(299, 1);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(299, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1, 268);
            this.panel3.TabIndex = 3;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFind,
            this.toolStripSeparator1,
            this.tsbReplace});
            this.toolStrip1.Location = new System.Drawing.Point(1, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(298, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // tsbFind
            // 
            this.tsbFind.Image = ((System.Drawing.Image)(resources.GetObject("tsbFind.Image")));
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(62, 22);
            this.tsbFind.Text = "Поиск";
            this.tsbFind.ToolTipText = "Переключение к режиму поиска";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbReplace
            // 
            this.tsbReplace.Image = ((System.Drawing.Image)(resources.GetObject("tsbReplace.Image")));
            this.tsbReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReplace.Name = "tsbReplace";
            this.tsbReplace.Size = new System.Drawing.Size(68, 22);
            this.tsbReplace.Text = "Замена";
            this.tsbReplace.ToolTipText = "Переключение к режиму замены";
            this.tsbReplace.Click += new System.EventHandler(this.tsbReplace_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.ddlSearchingList);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(1, 25);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(298, 45);
            this.panel4.TabIndex = 5;
            // 
            // ddlSearchingList
            // 
            this.ddlSearchingList.FormattingEnabled = true;
            this.ddlSearchingList.Location = new System.Drawing.Point(7, 22);
            this.ddlSearchingList.MaxLength = 30;
            this.ddlSearchingList.Name = "ddlSearchingList";
            this.ddlSearchingList.Size = new System.Drawing.Size(285, 21);
            this.ddlSearchingList.TabIndex = 1;
            this.ddlSearchingList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            this.ddlSearchingList.TextChanged += new System.EventHandler(this.ddlSearchingList_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Найти:";
            // 
            // pReplaceWithContainer
            // 
            this.pReplaceWithContainer.Controls.Add(this.ddlReplacingList);
            this.pReplaceWithContainer.Controls.Add(this.label2);
            this.pReplaceWithContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pReplaceWithContainer.Location = new System.Drawing.Point(1, 70);
            this.pReplaceWithContainer.Name = "pReplaceWithContainer";
            this.pReplaceWithContainer.Size = new System.Drawing.Size(298, 42);
            this.pReplaceWithContainer.TabIndex = 6;
            // 
            // ddlReplacingList
            // 
            this.ddlReplacingList.FormattingEnabled = true;
            this.ddlReplacingList.Location = new System.Drawing.Point(7, 19);
            this.ddlReplacingList.MaxLength = 30;
            this.ddlReplacingList.Name = "ddlReplacingList";
            this.ddlReplacingList.Size = new System.Drawing.Size(285, 21);
            this.ddlReplacingList.TabIndex = 2;
            this.ddlReplacingList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            this.ddlReplacingList.TextChanged += new System.EventHandler(this.ddlReplacingList_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Заменить на:";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.pReplaceContainer);
            this.panel6.Controls.Add(this.panel7);
            this.panel6.Controls.Add(this.panel5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(1, 112);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(298, 128);
            this.panel6.TabIndex = 7;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.bFindNext);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel9.Location = new System.Drawing.Point(38, 96);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(141, 32);
            this.panel9.TabIndex = 5;
            // 
            // bFindNext
            // 
            this.bFindNext.Location = new System.Drawing.Point(36, 5);
            this.bFindNext.Name = "bFindNext";
            this.bFindNext.Size = new System.Drawing.Size(106, 23);
            this.bFindNext.TabIndex = 6;
            this.bFindNext.Text = "Найти далее";
            this.bFindNext.UseVisualStyleBackColor = true;
            this.bFindNext.Click += new System.EventHandler(this.bFindNext_Click);
            this.bFindNext.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // pReplaceContainer
            // 
            this.pReplaceContainer.Controls.Add(this.bReplace);
            this.pReplaceContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pReplaceContainer.Location = new System.Drawing.Point(179, 96);
            this.pReplaceContainer.Name = "pReplaceContainer";
            this.pReplaceContainer.Size = new System.Drawing.Size(113, 32);
            this.pReplaceContainer.TabIndex = 3;
            // 
            // bReplace
            // 
            this.bReplace.Location = new System.Drawing.Point(6, 5);
            this.bReplace.Name = "bReplace";
            this.bReplace.Size = new System.Drawing.Size(105, 23);
            this.bReplace.TabIndex = 7;
            this.bReplace.Text = "Заменить";
            this.bReplace.UseVisualStyleBackColor = true;
            this.bReplace.Click += new System.EventHandler(this.bReplace_Click);
            this.bReplace.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.groupBox1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(292, 96);
            this.panel7.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSearchUp);
            this.groupBox1.Controls.Add(this.cbWholeWord);
            this.groupBox1.Controls.Add(this.cbMatchCase);
            this.groupBox1.Location = new System.Drawing.Point(7, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 85);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры поиска";
            // 
            // cbSearchUp
            // 
            this.cbSearchUp.AutoSize = true;
            this.cbSearchUp.Location = new System.Drawing.Point(13, 62);
            this.cbSearchUp.Name = "cbSearchUp";
            this.cbSearchUp.Size = new System.Drawing.Size(113, 17);
            this.cbSearchUp.TabIndex = 5;
            this.cbSearchUp.Text = "Двигаться вверх";
            this.cbSearchUp.UseVisualStyleBackColor = true;
            this.cbSearchUp.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.cbSearchUp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // cbWholeWord
            // 
            this.cbWholeWord.AutoSize = true;
            this.cbWholeWord.Location = new System.Drawing.Point(13, 42);
            this.cbWholeWord.Name = "cbWholeWord";
            this.cbWholeWord.Size = new System.Drawing.Size(104, 17);
            this.cbWholeWord.TabIndex = 4;
            this.cbWholeWord.Text = "Слово целиком";
            this.cbWholeWord.UseVisualStyleBackColor = true;
            this.cbWholeWord.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.cbWholeWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // cbMatchCase
            // 
            this.cbMatchCase.AutoSize = true;
            this.cbMatchCase.Location = new System.Drawing.Point(13, 22);
            this.cbMatchCase.Name = "cbMatchCase";
            this.cbMatchCase.Size = new System.Drawing.Size(120, 17);
            this.cbMatchCase.TabIndex = 3;
            this.cbMatchCase.Text = "С учетом регистра";
            this.cbMatchCase.UseVisualStyleBackColor = true;
            this.cbMatchCase.CheckedChanged += new System.EventHandler(this.cbMatchCase_CheckedChanged);
            this.cbMatchCase.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(292, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(6, 128);
            this.panel5.TabIndex = 1;
            // 
            // pReplaceAllContainer
            // 
            this.pReplaceAllContainer.Controls.Add(this.button1);
            this.pReplaceAllContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pReplaceAllContainer.Location = new System.Drawing.Point(1, 240);
            this.pReplaceAllContainer.Name = "pReplaceAllContainer";
            this.pReplaceAllContainer.Size = new System.Drawing.Size(298, 24);
            this.pReplaceAllContainer.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Заменить все";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            // 
            // FindReplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 269);
            this.Controls.Add(this.pReplaceAllContainer);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.pReplaceWithContainer);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(10000, 3290);
            this.MinimizeBox = false;
            this.Name = "FindReplaceForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск и замена";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindReplaceForm_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pReplaceWithContainer.ResumeLayout(false);
            this.pReplaceWithContainer.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.pReplaceContainer.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pReplaceAllContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbReplace;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ComboBox ddlSearchingList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pReplaceWithContainer;
        private System.Windows.Forms.ComboBox ddlReplacingList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pReplaceContainer;
        private System.Windows.Forms.Button bReplace;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cbSearchUp;
        private System.Windows.Forms.CheckBox cbWholeWord;
        private System.Windows.Forms.CheckBox cbMatchCase;
        private System.Windows.Forms.Panel pReplaceAllContainer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button bFindNext;
    }
}