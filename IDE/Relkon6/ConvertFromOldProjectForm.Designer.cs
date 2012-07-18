namespace Kontel.Relkon
{
    partial class ConvertFromOldProjectForm
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
            this.tbProgramFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bBrowseProgram = new System.Windows.Forms.Button();
            this.bAddPultFiles = new System.Windows.Forms.Button();
            this.bCreateProject = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lbPultFiles = new System.Windows.Forms.ListBox();
            this.bRemovePultFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbProgramFileName
            // 
            this.tbProgramFileName.Location = new System.Drawing.Point(12, 25);
            this.tbProgramFileName.Name = "tbProgramFileName";
            this.tbProgramFileName.Size = new System.Drawing.Size(303, 20);
            this.tbProgramFileName.TabIndex = 0;
            this.tbProgramFileName.TextChanged += new System.EventHandler(this.tbProgramFileName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Файл программы:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Файлы пультов:";
            // 
            // bBrowseProgram
            // 
            this.bBrowseProgram.Location = new System.Drawing.Point(321, 23);
            this.bBrowseProgram.Name = "bBrowseProgram";
            this.bBrowseProgram.Size = new System.Drawing.Size(78, 23);
            this.bBrowseProgram.TabIndex = 4;
            this.bBrowseProgram.Text = "Обзор...";
            this.bBrowseProgram.UseVisualStyleBackColor = true;
            this.bBrowseProgram.Click += new System.EventHandler(this.bBrowseProgram_Click);
            // 
            // bAddPultFiles
            // 
            this.bAddPultFiles.Location = new System.Drawing.Point(321, 68);
            this.bAddPultFiles.Name = "bAddPultFiles";
            this.bAddPultFiles.Size = new System.Drawing.Size(78, 23);
            this.bAddPultFiles.TabIndex = 5;
            this.bAddPultFiles.Text = "Добавить";
            this.bAddPultFiles.UseVisualStyleBackColor = true;
            this.bAddPultFiles.Click += new System.EventHandler(this.bBrowsePult_Click);
            // 
            // bCreateProject
            // 
            this.bCreateProject.Enabled = false;
            this.bCreateProject.Location = new System.Drawing.Point(12, 201);
            this.bCreateProject.Name = "bCreateProject";
            this.bCreateProject.Size = new System.Drawing.Size(97, 23);
            this.bCreateProject.TabIndex = 6;
            this.bCreateProject.Text = "Создать проект";
            this.bCreateProject.UseVisualStyleBackColor = true;
            this.bCreateProject.Click += new System.EventHandler(this.bCreateProject_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Файлы Relkon (*.kon)|*.kon|Файлы пультов (*.fpr, *.plt)|*.fpr;*.plt|Все файлы|*.*" +
                "";
            // 
            // lbPultFiles
            // 
            this.lbPultFiles.FormattingEnabled = true;
            this.lbPultFiles.Location = new System.Drawing.Point(13, 70);
            this.lbPultFiles.Name = "lbPultFiles";
            this.lbPultFiles.Size = new System.Drawing.Size(302, 121);
            this.lbPultFiles.TabIndex = 7;
            // 
            // bRemovePultFile
            // 
            this.bRemovePultFile.Enabled = false;
            this.bRemovePultFile.Location = new System.Drawing.Point(321, 97);
            this.bRemovePultFile.Name = "bRemovePultFile";
            this.bRemovePultFile.Size = new System.Drawing.Size(78, 23);
            this.bRemovePultFile.TabIndex = 8;
            this.bRemovePultFile.Text = "Удалить";
            this.bRemovePultFile.UseVisualStyleBackColor = true;
            this.bRemovePultFile.Click += new System.EventHandler(this.bRemovePultFile_Click);
            // 
            // ConvertFromOldProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 232);
            this.Controls.Add(this.bRemovePultFile);
            this.Controls.Add(this.lbPultFiles);
            this.Controls.Add(this.bCreateProject);
            this.Controls.Add(this.bAddPultFiles);
            this.Controls.Add(this.bBrowseProgram);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbProgramFileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConvertFromOldProjectForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конвертация проекта";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbProgramFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bBrowseProgram;
        private System.Windows.Forms.Button bAddPultFiles;
        private System.Windows.Forms.Button bCreateProject;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ListBox lbPultFiles;
        private System.Windows.Forms.Button bRemovePultFile;
    }
}