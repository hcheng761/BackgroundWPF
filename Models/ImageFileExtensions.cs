using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWPF.Models
{
    public static class ImageFileExtensions
    {
        public static List<string> Extensions = new List<string>()
        {
            ".jpeg",
            ".JPEG",
            ".jpg",
            ".JPG",
            ".png",
            ".PNG",
            ".jfif", 
            ".pjpeg", 
            ".pjp"
        };
    }
}
