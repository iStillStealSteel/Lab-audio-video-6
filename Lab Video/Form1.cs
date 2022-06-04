using Emgu.CV;
using Emgu.CV.CvEnum;
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
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Lab_Video
{

    public partial class Form1 : Form
    {
         WaveOutEvent outputDevice;
         AudioFileReader audioFile;

        int TotalFrame, FrameNo;
        double Fps;
        bool IsReadingFrame;
        VideoCapture capture;
        private static VideoCapture cameraCapture;
        private Image<Bgr, Byte> newBackgroundImage;
        private static IBackgroundSubtractor fgDetector;
        OpenFileDialog ofdv = new OpenFileDialog();


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofd.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();
                TotalFrame = (int)capture.Get(CapProp.FrameCount);
                Fps = 60;
                FrameNo = 1;
            }
            if (capture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //OpenFileDialog openFile = new OpenFileDialog();
            //if (openFile.ShowDialog() == DialogResult.OK)
            //{
            //    newBackgroundImage = new Image<Bgr, byte>(openFile.FileName);
            //}
            try
            {
                cameraCapture = new VideoCapture();
                fgDetector = new BackgroundSubtractorMOG2();
                Application.Idle += ProcessFrames;
            }
            catch (Exception)
            {
                MessageBox.Show("pusca");
                return;
            }

        }

        private void ProcessFrames(object sender, EventArgs e)
        {
            Mat mm = new Mat();
            capture.Read(mm);
            Mat frame = cameraCapture.QueryFrame();
            Image<Bgr, byte> frameImage = frame.ToImage<Bgr, Byte>();
            newBackgroundImage = mm.ToImage<Bgr, byte>(); 
            Mat foregroundMask = new Mat();
            fgDetector.Apply(frame, foregroundMask);
            var foregroundMaskImage = foregroundMask.ToImage<Gray, Byte>();
            foregroundMaskImage = foregroundMaskImage.Not();

            var copyOfNewBackgroundImage = newBackgroundImage.Resize(foregroundMaskImage.Width, foregroundMaskImage.Height, Inter.Lanczos4);
            copyOfNewBackgroundImage = copyOfNewBackgroundImage.Copy(foregroundMaskImage);

            foregroundMaskImage = foregroundMaskImage.Not();
            frameImage = frameImage.Copy(foregroundMaskImage);
            frameImage = frameImage.Or(copyOfNewBackgroundImage);
            
            pictureBox2.Image = frameImage.ToBitmap();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            VideoCapture capture = new VideoCapture(@"D:\Programe_facultate\Do. Or do not. There is no try.mpg");

            int Fourcc = Convert.ToInt32(capture.Get(CapProp.FourCC));
            int Width = Convert.ToInt32(capture.Get(CapProp.FrameWidth));
            int Height = Convert.ToInt32(capture.Get(CapProp.FrameHeight));
            var Fps = capture.Get(CapProp.Fps);
            var TotalFrame = capture.Get(CapProp.FrameCount);


            string destionpath = @"D:\Programe_facultate\Lab Editare audio video\capture.mpg";
            using (VideoWriter writer = new VideoWriter(destionpath, Fourcc, Fps, new Size(Width, Height), true))
            {
                Image<Bgr, byte> logo = new Image<Bgr, byte>(@"C:\Users\boo_b\Pictures\pic.jpg");
                Mat m = new Mat();

                var FrameNo = 1;
                while (FrameNo < TotalFrame)
                {
                    capture.Read(m);
                    Image<Bgr, byte> img = m.ToImage<Bgr, byte>();
                    img.ROI = new Rectangle(Width - logo.Width - 30, 10, logo.Width, logo.Height);
                    logo.CopyTo(img);

                    img.ROI = Rectangle.Empty;

                    writer.Write(img.Mat);
                    FrameNo++;
                }
            }

        }

        //private async void ReadAllFrames()
        //{
        //    Mat m = new Mat();
        //    while (IsReadingFrame == true && FrameNo < TotalFrame)
        //    {
        //        FrameNo += 1;
        //        var mat = capture.QueryFrame();
        //        pictureBox1.Image = mat.ToBitmap();
        //        await Task.Delay(1000 / Convert.ToInt16(Fps));
        //        label1.Text = FrameNo.ToString() + "/" + TotalFrame.ToString();
        //    }
        //}
        private void button4_Click(object sender, EventArgs e)
        {
            ofdv = new OpenFileDialog();
            if (ofdv.ShowDialog() == DialogResult.OK)
            {
                capture = new VideoCapture(ofdv.FileName);
                Mat m = new Mat();
                capture.Read(m);
                pictureBox1.Image = m.ToBitmap();

                TotalFrame = (int)capture.Get(CapProp.FrameCount);
                Fps = capture.Get(CapProp.Fps);
                FrameNo = 1;
            }
            if (capture == null)
            {
                return;
            }
            IsReadingFrame = true;
            ReadAllFrames();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button1.Show();
            button2.Show();
            button3.Show();
            button4.Show();
            button5.Show();
            button6.Hide();
            pictureBox1.Show();
            pictureBox2.Show();
            button7.Hide();
            button8.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Hide();
            button2.Hide();
            button3.Hide();
            button4.Hide();
            button5.Hide();
            pictureBox1.Hide();
            pictureBox2.Hide();
            button6.Show();
            numericUpDown1.Hide();
            button7.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                outputDevice.Dispose();
            }
            if(audioFile != null)
            {
                outputDevice.Stop();
                audioFile = null;
            }
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += button7_Click;
            }
            if (audioFile == null)
            {
                audioFile = new AudioFileReader(@"C:\Users\boo_b\Music\ELUVEITIE - Inis Mona (OFFICIAL MUSIC VIDEO).mp3");
            }
            outputDevice.Init(audioFile);
            outputDevice.Play();
            button8.Show();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            outputDevice.Dispose();
            audioFile.Dispose();
            audioFile = null;
            //using (var reader1 = new AudioFileReader("file1.wav"))
            using (audioFile = new AudioFileReader(@"C:\Users\boo_b\Music\ELUVEITIE - Inis Mona (OFFICIAL MUSIC VIDEO).mp3"))
            using (var reader2 = new AudioFileReader(@"C:\Users\boo_b\Music\Foolish Samurai Warrior.mp3"))
            {
                audioFile.Volume = 0.40f;
                var mixer = new MixingSampleProvider(new[] { audioFile, reader2 });
                WaveFileWriter.CreateWaveFile16("mixed.wav", mixer);
                var red = new AudioFileReader("mixed.wav");
                outputDevice.Init(red);
            }
            outputDevice.Play();

        }

        private async void ReadAllFrames()
        {

            Mat m = new Mat();
            while (IsReadingFrame == true && FrameNo < TotalFrame)
            {
                FrameNo += 1;
                var mat = capture.QueryFrame();
                if (mat != null)
                {
                    newBackgroundImage = mat.ToImage<Bgr, byte>();
                    var mod = mat.ToBitmap();
                    if (numericUpDown1.Value == 2 && FrameNo > TotalFrame - 11)
                    {
                        mod = (mat.ToImage<Bgr, byte>().Mul(3.0 * (10 - (TotalFrame - FrameNo))) + 2.0 * (10 - (TotalFrame - FrameNo))).AsBitmap();
                    }
                    if (numericUpDown1.Value == 2 && FrameNo < 11)
                    {
                        mod = (mat.ToImage<Bgr, byte>().Mul(3.0 * (10 - FrameNo)) + 2.0 * (10 - FrameNo)).AsBitmap();
                    }
                    pictureBox1.Image = mod;
                }
                await Task.Delay(1000 / Convert.ToInt16(Fps));
                label1.Text = FrameNo.ToString() + "/" + TotalFrame.ToString();
            }

            if (numericUpDown1.Value == 0)
                numericUpDown1.Value = 1;
            if (numericUpDown1.Value > 3)
                numericUpDown1.Value = 1;

            capture.Read(m);
            pictureBox1.Image = m.ToBitmap();

            TotalFrame = (int)capture.Get(CapProp.FrameCount);
            Fps = capture.Get(CapProp.Fps);
            FrameNo = 1;
            if (capture == null)
            {
                return;
            }
            IsReadingFrame = true;
            capture = new VideoCapture(ofdv.FileName);
            ReadAllFrames();


        }
    }
}
