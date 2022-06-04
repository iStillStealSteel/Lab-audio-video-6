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
        public static Bitmap ShowImage(string path)
        {
            Image<Bgr, Byte> image = new Image<Bgr, byte>(path);
            return image.AsBitmap();
        }
        public static Bitmap GrayScaleProcess(string path)
        {
            Image<Bgr, Byte> backup = new Image<Bgr, byte>(path);
            return backup.Convert<Gray, byte>().AsBitmap();
        }
        public static Bitmap AlfaBetaImgConv(float alpha, float beta, string path)
        {
            Image<Bgr, Byte> backup = new Image<Bgr, byte>(path);
            return (backup.Mul(alpha) + beta).AsBitmap(); 
        }
        public static Bitmap GammaCorrectFunc(float gama, string path)
        {
            Image<Bgr, Byte> backup = new Image<Bgr, byte>(path);
            return backup.Mul(gama).AsBitmap();
        }
        public static Bitmap ResizeFunc(double r, string path)
        {
            Image<Bgr, Byte> backup = new Image<Bgr, byte>(path);
            return backup.Resize(r, Emgu.CV.CvEnum.Inter.Linear).AsBitmap();
        }
        public static Bitmap RotateFunc(double r, string path)
        {
            Image<Bgr, Byte> backup = new Image<Bgr, byte>(path);
            return backup.Rotate(r, new Bgr(), false).AsBitmap();
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
