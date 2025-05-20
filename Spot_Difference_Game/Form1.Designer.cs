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
        private Label labelStatus;
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
            labelStatus = new Label();
            labelStatus.AutoSize = true;
            labelStatus.Font = new Font("Segoe UI", 12);
            labelStatus.Location = new Point(10, 10);
            labelStatus.Text = $"الفروقات: {foundDifferences} / {totalDifferences}";
            Controls.Add(labelStatus);
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(59, 97);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(195, 190);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(437, 85);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(190, 212);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.MouseClick += pictureBox2_MouseClick;
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
        private void DrawCircle(PictureBox pictureBox, float x, float y)
{
    Bitmap bmp = new Bitmap(pictureBox.Image);
    using (Graphics g = Graphics.FromImage(bmp))
    {
        Pen pen = new Pen(Color.Red, 3);
        int radius = 10;
        g.DrawEllipse(pen, x - radius, y - radius, radius * 2, radius * 2);
    }
    pictureBox.Image = bmp;
}
        //private void DrawClickCircle(PictureBox pictureBox, MouseEventArgs e)
        //{
        //    if (pictureBox.Image == null) return;

        //    // احصل على أبعاد الصورة الأصلية
        //    Image img = pictureBox.Image;
        //    float imageRatio = (float)img.Width / img.Height;
        //    float boxRatio = (float)pictureBox.Width / pictureBox.Height;

        //    float scaleFactor;
        //    float offsetX = 0, offsetY = 0;

        //    if (imageRatio > boxRatio)
        //    {
        //        // الصورة أعرض من الـ PictureBox
        //        scaleFactor = (float)pictureBox.Width / img.Width;
        //        offsetY = (pictureBox.Height - img.Height * scaleFactor) / 2;
        //    }
        //    else
        //    {
        //        // الصورة أطول من الـ PictureBox
        //        scaleFactor = (float)pictureBox.Height / img.Height;
        //        offsetX = (pictureBox.Width - img.Width * scaleFactor) / 2;
        //    }

        //    // تحويل إحداثيات النقر إلى داخل الصورة
        //    float x = (e.X - offsetX) / scaleFactor;
        //    float y = (e.Y - offsetY) / scaleFactor;

        //    if (x < 0 || y < 0 || x >= img.Width || y >= img.Height) return; // تم النقر خارج الصورة

        //    // نرسم على نسخة من الصورة
        //    Bitmap original = new Bitmap(img);
        //    using (Graphics g = Graphics.FromImage(original))
        //    {
        //        int radius = 10;
        //        Pen pen = new Pen(Color.Red, 3);
        //        g.DrawEllipse(pen, x - radius, y - radius, radius * 2, radius * 2);
        //    }

        //    pictureBox.Image = original;
        //}
        private PointF? GetImageCoordinates(PictureBox pictureBox, Point mouseLocation)
        {
            if (pictureBox.Image == null) return null;

            Image img = pictureBox.Image;
            float imageRatio = (float)img.Width / img.Height;
            float boxRatio = (float)pictureBox.Width / pictureBox.Height;

            float scaleFactor;
            float offsetX = 0, offsetY = 0;

            if (imageRatio > boxRatio)
            {
                scaleFactor = (float)pictureBox.Width / img.Width;
                offsetY = (pictureBox.Height - img.Height * scaleFactor) / 2;
            }
            else
            {
                scaleFactor = (float)pictureBox.Height / img.Height;
                offsetX = (pictureBox.Width - img.Width * scaleFactor) / 2;
            }

            float x = (mouseLocation.X - offsetX) / scaleFactor;
            float y = (mouseLocation.Y - offsetY) / scaleFactor;

            if (x < 0 || y < 0 || x >= img.Width || y >= img.Height) return null;
            return new PointF(x, y);
        }
        private void CheckAndMarkDifference(PictureBox pictureBox, MouseEventArgs e)
        {
            PointF? imagePoint = GetImageCoordinates(pictureBox, e.Location);
            if (imagePoint == null) return;

            float x = imagePoint.Value.X;
            float y = imagePoint.Value.Y;

            // فحص إن كانت النقطة قريبة من أحد الفروقات
            foreach (var diff in differences.ToList())
            {
                double distance = Math.Sqrt(Math.Pow(diff.X - x, 2) + Math.Pow(diff.Y - y, 2));
                if (distance <= 15) // شعاع النقر
                {
                    differences.Remove(diff); // تم العثور عليه
                    foundDifferences++;
                    labelStatus.Text = $"الفروقات: {foundDifferences} / {totalDifferences}";
                    DrawCircle(pictureBox, x, y); // رسم دائرة

                    if (differences.Count == 0)
                    {
                        MessageBox.Show("أحسنت! لقد وجدت كل الفروقات 🎉", "انتهت اللعبة");
                    }
                    return;
                }
            }

            // نقر خاطئ (اختياري: يمكنك عرض رسالة أو تجاهل)
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            CheckAndMarkDifference(pictureBox1, e);
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            CheckAndMarkDifference(pictureBox2, e);
        }
        //private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        //{
        //    // نحصل على نسخة من الصورة
        //    Bitmap original = new Bitmap(pictureBox2.Image);
        //    using (Graphics g = Graphics.FromImage(original))
        //    {
        //        // نرسم دائرة حمراء مكان النقر
        //        int radius = 10;
        //        Pen pen = new Pen(Color.Red, 3);
        //        g.DrawEllipse(pen, e.X - radius, e.Y - radius, radius * 2, radius * 2);
        //    }

        //    pictureBox2.Image = original; // نحدث الصورة
        //}
        // كل فرق يمثله نقطة في الصورة (مثلاً مركز الفرق)
        private List<Point> differences = new List<Point>()
{
    new Point(80, 60),
    new Point(150, 100),
    new Point(200, 150)
};

        private int foundDifferences = 0;
        private int totalDifferences => differences.Count;
    }
}
