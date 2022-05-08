using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace CommonEditareTools
{

    public class ImageProcessClass
    {
        public static Image<Gray, byte> GrayScaleProcess(Image<Bgr, Byte> backup)
        {
            return backup.Convert<Gray, byte>();
        }
        public static Image<Bgr, Byte> AlfaBetaImgConv(float alpha, float beta, Image<Bgr, Byte> backup)
        {
            return backup.Mul(alpha) + beta; 
        }
        public static Image<Bgr, byte> GammaCorrectFunc(float gama, Image<Bgr, Byte> backup)
        {
            // return backup._GammaCorrect(gama);
            return backup;
        }
        public static Image<Bgr, Byte> ResizeFunc(double r, Image<Bgr, Byte> backup)
        {
            return backup.Resize(r, Emgu.CV.CvEnum.Inter.Linear);
        }
        public static Image<Bgr, Byte> RotateFunc(double r, Image<Bgr, Byte> backup)
        {
            return backup.Rotate(r, new Bgr(), false);
        }
        public static async Task ImageBlendAsync(Image<Bgr, Byte> image,PictureBox pb)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            List<Image<Bgr, byte>> listImages = new List<Image<Bgr, byte>>();
            Image<Bgr, Byte> imageBlend;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                imageBlend = new Image<Bgr, byte>(openFile.FileName);
                listImages.Add(image);
                listImages.Add(imageBlend);
                for (int i = 0; i < listImages.Count - 1; i++)
                {
                    for (double alpha = 0.0; alpha <= 1.0; alpha += 0.01)
                    {
                        pb.Image = listImages[i + 1].AddWeighted(listImages[i], alpha, 1 - alpha, 0).AsBitmap();
                        await Task.Delay(10);
                    }
                }

            }
        }

    }

}
