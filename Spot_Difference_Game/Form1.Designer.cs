using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Spot_Difference_Game
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private Bitmap bitmap1;
        private Bitmap bitmap2;

        // قائمة تخزن مناطق الفروقات كمستطيلات (Rectangles)
        private List<Rectangle> differenceAreas = new List<Rectangle>();

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // pictureBox1
            pictureBox1.Location = new Point(59, 97);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(195, 190);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            // pictureBox2
            pictureBox2.Location = new Point(437, 85);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(190, 212);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.MouseClick += pictureBox2_MouseClick;
            // Form1
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Spot Difference Game";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            this.Load += new EventHandler(Form1_Load);
        }

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;

        private void Form1_Load(object sender, EventArgs e)
        {
            string path1 = Path.Combine(Application.StartupPath, "image1.jpg");
            string path2 = Path.Combine(Application.StartupPath, "image2.jpg");

            if (!File.Exists(path1) || !File.Exists(path2))
            {
                MessageBox.Show("الصور غير موجودة في مسار التطبيق.");
                return;
            }

            bitmap1 = new Bitmap(path1);
            bitmap2 = new Bitmap(path2);

            pictureBox1.Image = new Bitmap(bitmap1);
            pictureBox2.Image = new Bitmap(bitmap2);

            AdjustImagesLayout();

            // اكتشاف الفروق وتخزين مناطقها
            differenceAreas = DetectDifferences(bitmap1, bitmap2, 10, 30, 0.2f);
            MessageBox.Show($"عدد الفروقات المكتشفة: {differenceAreas.Count}", "نتيجة التحليل");

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustImagesLayout();
        }

        private void AdjustImagesLayout()
        {
            int padding = 20;
            int pictureWidth = (this.ClientSize.Width - (3 * padding)) / 2;
            int pictureHeight = this.ClientSize.Height - (2 * padding);

            pictureBox1.Location = new Point(padding, padding);
            pictureBox1.Size = new Size(pictureWidth, pictureHeight);

            pictureBox2.Location = new Point(padding * 2 + pictureWidth, padding);
            pictureBox2.Size = new Size(pictureWidth, pictureHeight);
        }

        // الخوارزمية: تعيد قائمة مستطيلات تمثل مناطق الفروقات
        // blockSize: حجم المربع للمقارنة (مثلاً 10)
        // colorThreshold: فرق اللون المقبول (مثلاً 30)
        // diffRatioThreshold: النسبة الأدنى لبكسلات مختلفة لتعتبر المربع مختلف (مثلاً 0.2 = 20%)
        private List<Rectangle> DetectDifferences(Bitmap bmp1, Bitmap bmp2, int blockSize, int colorThreshold, float diffRatioThreshold)
        {
            List<Rectangle> differences = new List<Rectangle>();

            int width = Math.Min(bmp1.Width, bmp2.Width);
            int height = Math.Min(bmp1.Height, bmp2.Height);

            for (int y = 0; y < height; y += blockSize)
            {
                for (int x = 0; x < width; x += blockSize)
                {
                    if (IsBlockDifferent(bmp1, bmp2, x, y, blockSize, colorThreshold, diffRatioThreshold))
                    {
                        differences.Add(new Rectangle(x, y, blockSize, blockSize));
                    }
                }
            }
            return differences;
        }

        private bool IsBlockDifferent(Bitmap bmp1, Bitmap bmp2, int startX, int startY, int blockSize, int colorThreshold, float diffRatioThreshold)
        {
            int width = bmp1.Width;
            int height = bmp1.Height;

            int diffCount = 0;
            int totalPixels = 0;

            for (int y = startY; y < startY + blockSize && y < height; y++)
            {
                for (int x = startX; x < startX + blockSize && x < width; x++)
                {
                    totalPixels++;
                    Color c1 = bmp1.GetPixel(x, y);
                    Color c2 = bmp2.GetPixel(x, y);
                    if (IsColorDifferent(c1, c2, colorThreshold))
                    {
                        diffCount++;
                    }
                }
            }

            float diffRatio = (float)diffCount / totalPixels;
            return diffRatio >= diffRatioThreshold;
        }

        private bool IsColorDifferent(Color c1, Color c2, int threshold)
        {
            return Math.Abs(c1.R - c2.R) > threshold ||
                   Math.Abs(c1.G - c2.G) > threshold ||
                   Math.Abs(c1.B - c2.B) > threshold;
        }

        // دالة تحويل إحداثيات النقر إلى إحداثيات داخل الصورة (بحسب Zoom/Aspect Ratio)
        private PointF GetImageCoordinates(PictureBox pictureBox, MouseEventArgs e)
        {
            if (pictureBox.Image == null) return PointF.Empty;

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

            float x = (e.X - offsetX) / scaleFactor;
            float y = (e.Y - offsetY) / scaleFactor;

            return new PointF(x, y);
        }

        private void DrawClickCircle(PictureBox pictureBox, MouseEventArgs e)
        {
            if (pictureBox.Image == null) return;

            PointF imgPoint = GetImageCoordinates(pictureBox, e);

            if (imgPoint.X < 0 || imgPoint.Y < 0 ||
                imgPoint.X >= bitmap1.Width || imgPoint.Y >= bitmap1.Height)
                return;

            // نتحقق هل النقرة داخل أحد مناطق الاختلاف
            bool isOnDifference = false;
            foreach (var rect in differenceAreas)
            {
                if (rect.Contains((int)imgPoint.X, (int)imgPoint.Y))
                {
                    isOnDifference = true;
                    break;
                }
            }

            if (!isOnDifference)
            {
                // النقرة خارج مناطق الفروق، لا نسمح برسم إشارة
                MessageBox.Show("يجب وضع الإشارة فقط على الفروقات المكتشفة.");
                return;
            }

            // رسم دائرة على نسخة من الصورة الأصلية (لعدم تعديل الصورة الأصلية)
            Bitmap original = new Bitmap(pictureBox.Image);
            using (Graphics g = Graphics.FromImage(original))
            {
                int radius = 10;
                Pen pen = new Pen(Color.Blue, 3);
                g.DrawEllipse(pen, imgPoint.X - radius, imgPoint.Y - radius, radius * 2, radius * 2);
            }

            pictureBox.Image = original;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            DrawClickCircle(pictureBox1, e);
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            DrawClickCircle(pictureBox2, e);
        }
    }
}
