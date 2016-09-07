using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;
using Emgu.CV;
using Emgu.CV.Structure;
using Newtonsoft.Json;
using ZedGraph;

//MODEL CLASS
using Highlighting_Faces.Models;

//BUSINESS
using Highlighting_Faces.Business;

namespace Highlighting_Faces.Controllers
{
    public class DefaultController : Controller
    {
        private readonly FilePathBusiness _filePathBusiness = new FilePathBusiness();

        public ActionResult Index()
        {
            if (Request.HttpMethod == "POST")
            {
                ViewBag.ImageProcessed = true;
                // Try to process the image.
                if (Request.Files.Count > 0)
                {
                    // There will be just one file.
                    var file = Request.Files[0];

                    var fileName = Guid.NewGuid().ToString() + ".jpg";

                    //path save image
                    var pathSaveImage = Server.MapPath("~/Images/");

                    //VERIFIED AND CREATE PATH TO SAVE IMAGE UPLOAD
                    if (!(_filePathBusiness.VerifiedPathImage(pathSaveImage)))
                    {
                        //CREATE PATH FOR SAVE IMAGE
                        _filePathBusiness.CreatePath(pathSaveImage);
                    }

                    //SAVE IMAGE
                    file.SaveAs(pathSaveImage + fileName);

                    // Load the saved image, for native processing using Emgu CV.
                    var bitmap = new Bitmap(pathSaveImage+ fileName);

                    var faces = FaceDetectorBusiness.DetectFaces(new Image<Bgr, byte>(bitmap).Mat);

                    // If faces where found.
                    if (faces.Count > 0)
                    {
                        ViewBag.FacesDetected = true;
                        ViewBag.FaceCount = faces.Count;

                        var positions = new List<Location>();
                        foreach (var face in faces)
                        {
                            // Add the positions.
                            positions.Add(new Location
                            {
                                X = face.X,
                                Y = face.Y,
                                Width = face.Width,
                                Height = face.Height
                            });
                        }

                        ViewBag.FacePositions = JsonConvert.SerializeObject(positions);
                    }

                    ViewBag.ImageUrl = fileName;
                }
            }
            return View();
        }



	}
}