using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Highlighting_Faces.Business
{
    public class FilePathBusiness
    {
        public bool VerifiedPathImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            return false;
        }


        public void CreatePath(string path)
        {
            Directory.CreateDirectory(path);
        }

    }
}