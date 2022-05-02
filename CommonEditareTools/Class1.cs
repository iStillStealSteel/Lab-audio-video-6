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
        public static Image<Bgr, Byte> GammaCorrectFunc(float gama, Image<Bgr, Byte> backup)
        {
            return backup._GammaCorrect(gama);
        }
    }

}
