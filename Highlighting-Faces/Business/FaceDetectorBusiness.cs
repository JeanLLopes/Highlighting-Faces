using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Emgu.CV;

namespace Highlighting_Faces.Business
{
    public class FaceDetectorBusiness
    {
        public static List<Rectangle> DetectFaces(Mat image)
        {
            var faces = new List<Rectangle>();
            var facesCascade = HttpContext.Current.Server.MapPath("~/haarcascade_frontalface_default.xml");
            using (var face = new CascadeClassifier(facesCascade))
            {
                using (var ugray = new UMat())
                {
                    CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    //normalizes brightness and increases contrast of the image
                    CvInvoke.EqualizeHist(ugray, ugray);

                    //Detect the faces from the gray scale image and store the locations as rectangle
                    //The first dimensional is the channel
                    //The second dimension is the index of the rectangle in the specific channel
                    Rectangle[] facesDetected = face.DetectMultiScale(
                                                    ugray,
                                                    1.1,
                                                    10,
                                                    new Size(20, 20));

                    faces.AddRange(facesDetected);
                }
            }
            return faces;
        }
    }
}