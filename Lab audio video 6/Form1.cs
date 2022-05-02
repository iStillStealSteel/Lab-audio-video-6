using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonEditareTools;

namespace Lab_audio_video_6
{
    public partial class Form1 : Form
    {

        Image<Bgr, Byte> image;
        Image<Bgr, Byte> image2;
        Image<Bgr, Byte> image3;
        Image<Bgr, Byte> image4;
        Image<Bgr, Byte> image5;
        Image<Gray, byte> gray_image;
        Image<Bgr, Byte> backup;
        Image<Bgr, Byte> imageBlend;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                image = new Image<Bgr, byte>(openFile.FileName);
                pictureBox1.Image = image.AsBitmap();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backup = image.Clone();
            pictureBox2.Image = ImageProcessClass.GrayScaleProcess(backup).AsBitmap();

            // HistogramViewer v = new HistogramViewer();
            // v.HistogramCtrl.GenerateHistograms(image, 255);
            // v.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                backup = image.Clone();
                pictureBox3.Image = ImageProcessClass.AlfaBetaImgConv((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text),backup).AsBitmap();
            }
            catch (Exception ex) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                image3 = image.Clone();
                pictureBox4.Image = ImageProcessClass.GammaCorrectFunc((float)Convert.ToDouble(textBox3.Text),backup).AsBitmap();
            }
            catch (Exception ex) { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox5.Image = null;
            try
            {   
                backup= image.Clone();
                var r = Convert.ToDouble(textBox4.Text);
                image4 = backup.Resize(r, Emgu.CV.CvEnum.Inter.Linear);
                pictureBox5.Image = image4.AsBitmap();
            }
            catch (Exception ex) { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                backup = image.Clone();
                var r = (float)Convert.ToDouble(textBox5.Text);
                image5 = backup.Rotate(r, new Bgr(), false);
                pictureBox6.Image = image5.AsBitmap();
            }
            catch (Exception ex) { }
        }
        Rectangle rect; Point StartROI; bool MouseDown;

        private void pictureBox7_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pictureBox7.Image = image.AsBitmap();
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
            if (pictureBox1.Image == null || rect == Rectangle.Empty)
            { return; }
            var img = new Bitmap(pictureBox1.Image).ToImage<Bgr, byte>();
            img.ROI = rect;
            var imgROI = img.Copy();
            pictureBox7.Image = imgROI.ToBitmap();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown = true;
            StartROI = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (pictureBox1.Image == null)
            {
                return;
            }
            int width = Math.Max(StartROI.X, e.X) - Math.Min(StartROI.X, e.X);
            int height = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
            rect = new Rectangle(Math.Min(StartROI.X, e.X),
            Math.Min(StartROI.Y, e.Y),
            width,
            height);
            Refresh();
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (MouseDown)
            {
                using (Pen pen = new Pen(Color.Red, 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            List<Image<Bgr, byte>> listImages = new List<Image<Bgr, byte>>();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imageBlend = new Image<Bgr, byte>(openFile.FileName);
                listImages.Add(image);
                listImages.Add(imageBlend);
                for (int i = 0; i < listImages.Count - 1; i++)
                {
                    for (double alpha = 0.0; alpha <= 1.0; alpha += 0.01)
                    {
                        pictureBox8.Image = listImages[i + 1].AddWeighted(listImages[i], alpha, 1 - alpha, 0).AsBitmap();
                        await Task.Delay(10);
                    }
                }

            }

        }
    }
}
