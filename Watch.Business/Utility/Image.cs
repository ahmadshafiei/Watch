using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch.Business.Utility
{
    public static class Image
    {
        public static string Save(byte[] image)
        {
            if (image == null || image.Count() == 0)
                return null;
            MemoryStream ms = new MemoryStream(image);
            var mainImg = System.Drawing.Image.FromStream(ms);

            string relPath = "Images/" + Guid.NewGuid().ToString() + ".jpg";
            string absPath = "c:/inetpub/wwwroot" + relPath;

            mainImg.Save(absPath, mainImg.RawFormat);

            return relPath;
        }
    }
}
