using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace Spot_Difference_Game

{
    partial class Form1
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(59, 97);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(195, 190);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(437, 85);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(190, 212);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            this.Load += new System.EventHandler(this.Form1_Load);
        }
        private void AdjustImagesLayout()
        {
            int padding = 20;
            int pictureWidth = (this.ClientSize.Width - (3 * padding)) / 2;
            int pictureHeight = this.ClientSize.Height - (2 * padding);

            // PictureBox1
            pictureBox1.Location = new Point(padding, padding);
            pictureBox1.Size = new Size(pictureWidth, pictureHeight);

            // PictureBox2
            pictureBox2.Location = new Point(padding * 2 + pictureWidth, padding);
            pictureBox2.Size = new Size(pictureWidth, pictureHeight);
        }
       
        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;

        private void Form1_Load(object sender, EventArgs e)
        {
            string path1 = Path.Combine(Application.StartupPath, "image1.jpg");
            string path2 = Path.Combine(Application.StartupPath, "image2.jpg");

            pictureBox1.Image = Image.FromFile(path1);
            pictureBox2.Image = Image.FromFile(path2);

            AdjustImagesLayout(); // تخصيص الحجم والموقع تلقائياً
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustImagesLayout(); // إعادة ضبط الصور عند تغيير حجم الفورم
        }

    }
}
