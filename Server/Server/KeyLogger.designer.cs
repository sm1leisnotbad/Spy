using System.Windows.Forms;

namespace Server
{
    partial class KeyLogger
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
            this.LogBx = new System.Windows.Forms.TextBox();
            this.ExportBtn = new System.Windows.Forms.Button();
            this.IPBx = new System.Windows.Forms.TextBox();
            this.IPLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LogBx
            // 
            this.LogBx.Location = new System.Drawing.Point(12, 100);
            this.LogBx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LogBx.Multiline = true;
            this.LogBx.Name = "LogBx";
            this.LogBx.ReadOnly = true;
            this.LogBx.Size = new System.Drawing.Size(568, 290);
            this.LogBx.TabIndex = 0;
            // 
            // ExportBtn
            // 
            this.ExportBtn.Location = new System.Drawing.Point(628, 358);
            this.ExportBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExportBtn.Name = "ExportBtn";
            this.ExportBtn.Size = new System.Drawing.Size(113, 32);
            this.ExportBtn.TabIndex = 1;
            this.ExportBtn.Text = "button1";
            this.ExportBtn.UseVisualStyleBackColor = true;
            // 
            // IPBx
            // 
            this.IPBx.Location = new System.Drawing.Point(101, 39);
            this.IPBx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.IPBx.Name = "IPBx";
            this.IPBx.ReadOnly = true;
            this.IPBx.Size = new System.Drawing.Size(245, 22);
            this.IPBx.TabIndex = 2;
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(63, 45);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(19, 16);
            this.IPLabel.TabIndex = 3;
            this.IPLabel.Text = "IP";
            // 
            // KeyLogger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.IPBx);
            this.Controls.Add(this.ExportBtn);
            this.Controls.Add(this.LogBx);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "KeyLogger";
            this.Text = "KeyLogger";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox LogBx;
        private Button ExportBtn;
        private TextBox IPBx;
        private Label IPLabel;
    }
}