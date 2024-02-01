namespace Generator
{
    partial class IncludeToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IncludeToolForm));
            this.GetPropertiesBtn = new System.Windows.Forms.Button();
            this.DtoName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GenerateCodeBtn = new System.Windows.Forms.Button();
            this.PropertycheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.IncludeListBox = new System.Windows.Forms.CheckedListBox();
            this.ResetPropertiesBtn = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SelectAllBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetPropertiesBtn
            // 
            this.GetPropertiesBtn.BackColor = System.Drawing.Color.Black;
            this.GetPropertiesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GetPropertiesBtn.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Bold);
            this.GetPropertiesBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.GetPropertiesBtn.Location = new System.Drawing.Point(16, 255);
            this.GetPropertiesBtn.Name = "GetPropertiesBtn";
            this.GetPropertiesBtn.Size = new System.Drawing.Size(133, 25);
            this.GetPropertiesBtn.TabIndex = 1;
            this.GetPropertiesBtn.Text = "Get Properties";
            this.GetPropertiesBtn.UseVisualStyleBackColor = false;
            this.GetPropertiesBtn.Click += new System.EventHandler(this.GetPropertiesBtn_Click);
            // 
            // DtoName
            // 
            this.DtoName.Location = new System.Drawing.Point(112, 44);
            this.DtoName.Name = "DtoName";
            this.DtoName.Size = new System.Drawing.Size(348, 20);
            this.DtoName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(14, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "DtoName";
            // 
            // GenerateCodeBtn
            // 
            this.GenerateCodeBtn.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.GenerateCodeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GenerateCodeBtn.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Bold);
            this.GenerateCodeBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.GenerateCodeBtn.Location = new System.Drawing.Point(327, 491);
            this.GenerateCodeBtn.Name = "GenerateCodeBtn";
            this.GenerateCodeBtn.Size = new System.Drawing.Size(133, 25);
            this.GenerateCodeBtn.TabIndex = 6;
            this.GenerateCodeBtn.Text = "Generate Code";
            this.GenerateCodeBtn.UseVisualStyleBackColor = false;
            this.GenerateCodeBtn.Click += new System.EventHandler(this.GenerateCodeBtn_Click);
            // 
            // PropertycheckedListBox
            // 
            this.PropertycheckedListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.PropertycheckedListBox.FormattingEnabled = true;
            this.PropertycheckedListBox.HorizontalScrollbar = true;
            this.PropertycheckedListBox.Location = new System.Drawing.Point(16, 286);
            this.PropertycheckedListBox.Name = "PropertycheckedListBox";
            this.PropertycheckedListBox.Size = new System.Drawing.Size(444, 199);
            this.PropertycheckedListBox.TabIndex = 7;
            this.PropertycheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.PropertycheckedListBox_ItemCheck);
            this.PropertycheckedListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PropertycheckedListBox_MouseClick);
            // 
            // IncludeListBox
            // 
            this.IncludeListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.IncludeListBox.FormattingEnabled = true;
            this.IncludeListBox.HorizontalScrollbar = true;
            this.IncludeListBox.Location = new System.Drawing.Point(16, 80);
            this.IncludeListBox.Name = "IncludeListBox";
            this.IncludeListBox.Size = new System.Drawing.Size(444, 169);
            this.IncludeListBox.TabIndex = 8;
            this.IncludeListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.IncludeListBox_ItemCheck);
            this.IncludeListBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IncludeListBox_MouseClick);
            // 
            // ResetPropertiesBtn
            // 
            this.ResetPropertiesBtn.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ResetPropertiesBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ResetPropertiesBtn.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Bold);
            this.ResetPropertiesBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.ResetPropertiesBtn.Location = new System.Drawing.Point(330, 255);
            this.ResetPropertiesBtn.Name = "ResetPropertiesBtn";
            this.ResetPropertiesBtn.Size = new System.Drawing.Size(133, 25);
            this.ResetPropertiesBtn.TabIndex = 9;
            this.ResetPropertiesBtn.Text = "Reset Properties";
            this.ResetPropertiesBtn.UseVisualStyleBackColor = false;
            this.ResetPropertiesBtn.Click += new System.EventHandler(this.ResetIncludePropertiesBtn_Click);
            // 
            // SelectAllBtn
            // 
            this.SelectAllBtn.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SelectAllBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.SelectAllBtn.Font = new System.Drawing.Font("Segoe Print", 9.75F, System.Drawing.FontStyle.Bold);
            this.SelectAllBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.SelectAllBtn.Location = new System.Drawing.Point(16, 491);
            this.SelectAllBtn.Name = "SelectAllBtn";
            this.SelectAllBtn.Size = new System.Drawing.Size(133, 25);
            this.SelectAllBtn.TabIndex = 10;
            this.SelectAllBtn.Text = "All Properties";
            this.SelectAllBtn.UseVisualStyleBackColor = false;
            this.SelectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
            // 
            // IncludeToolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(474, 539);
            this.Controls.Add(this.SelectAllBtn);
            this.Controls.Add(this.ResetPropertiesBtn);
            this.Controls.Add(this.IncludeListBox);
            this.Controls.Add(this.PropertycheckedListBox);
            this.Controls.Add(this.GenerateCodeBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DtoName);
            this.Controls.Add(this.GetPropertiesBtn);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IncludeToolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STemplate ";
            this.Load += new System.EventHandler(this.IncludeToolForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Button GetPropertiesBtn;
        private System.Windows.Forms.TextBox DtoName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GenerateCodeBtn;
        private System.Windows.Forms.CheckedListBox PropertycheckedListBox;
        private System.Windows.Forms.CheckedListBox IncludeListBox;
        private System.Windows.Forms.Button ResetPropertiesBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button SelectAllBtn;
    }
}