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
        Image<Bgr, Byte> backup;
        OpenFileDialog ImagePath;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            ImagePath=openFile;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = ImageProcessClass.ShowImage(ImagePath.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = ImageProcessClass.GrayScaleProcess(ImagePath.FileName);

            // HistogramViewer v = new HistogramViewer();
            // v.HistogramCtrl.GenerateHistograms(image, 255);
            // v.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox3.Image = ImageProcessClass.AlfaBetaImgConv((float)Convert.ToDouble(textBox1.Text), (float)Convert.ToDouble(textBox2.Text), ImagePath.FileName);
            }
            catch (Exception ex) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox4.Image = ImageProcessClass.GammaCorrectFunc((float)Convert.ToDouble(textBox3.Text), ImagePath.FileName);
            }
            catch (Exception ex) { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox5.Image = null;
            try
            {   
                pictureBox5.Image = ImageProcessClass.ResizeFunc(Convert.ToDouble(textBox4.Text), ImagePath.FileName);
            }
            catch (Exception ex) { }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox6.Image = ImageProcessClass.RotateFunc(Convert.ToDouble(textBox5.Text), ImagePath.FileName);
            }
            catch (Exception ex) { }
        }
        Rectangle rect; Point StartROI; bool MouseDown;

    
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
            backup=image.Clone();
            await ImageProcessClass.ImageBlendAsync(backup, pictureBox8);
        }
    }
}
