using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_core.Utilities
{

    public static class StringExtensions
    {
        /// <summary>
        /// Borrowed From https://www.codegrepper.com/code-examples/csharp/convert+iformfile+to+base64+string+c%23
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string FileToBase64String(this IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string base64 = Convert.ToBase64String(fileBytes);
                    return base64;
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        //public static object Base64ToPng (this string base64Png)
        //{
        //    string base64String = File.ReadAllText(@"C:\samples\base64Image.txt");
        //    byte[] imgBytes = Convert.FromBase64String(base64String);

        //    using (var imageFile = new FileStream(@"C:\samples\sample.png", FileMode.Create))
        //    {
        //        imageFile.Write(imgBytes, 0, imgBytes.Length);
        //        imageFile.Flush();
        //    }
        //}
    }
}
