﻿using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    partial class DashBoardForm
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
            this.ConnectBx = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ConnectBx
            // 
            this.ConnectBx.Location = new System.Drawing.Point(122, 28);
            this.ConnectBx.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConnectBx.Multiline = true;
            this.ConnectBx.Name = "ConnectBx";
            this.ConnectBx.ReadOnly = true;
            this.ConnectBx.Size = new System.Drawing.Size(263, 279);
            this.ConnectBx.TabIndex = 0;
            // 
            // DashBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 351);
            this.Controls.Add(this.ConnectBx);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DashBoardForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox ConnectBx;
    }
}