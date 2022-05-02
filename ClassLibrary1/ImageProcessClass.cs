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

namespace ClassLibrary1
{
    class ImageProcessClass
    {
        static Image<Gray, byte> GrayScaleProcess(Image<Bgr, Byte> backup) {
            Image<Gray, byte> gray_image;
            gray_image = backup.Convert<Gray, byte>();
            return gray_image;
        }
    }
}
