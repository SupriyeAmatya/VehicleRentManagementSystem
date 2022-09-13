using Microsoft.AspNetCore.Mvc;
using NiceApp.Data;
using NiceApp.Models.DataModel;

namespace NiceApp.Controllers
{
    public class ExtraController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExtraController(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UploadImage()
        {
            foreach (var file in Request.Form.Files)
            {
                Images img = new Images();
                img.ImageTitle = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.ImageData = ms.ToArray();

                ms.Close();
                ms.Dispose();

                _dbContext.Images.Add(img);
                _dbContext.SaveChanges();
            }
            ViewBag.Message = "Image(s) stored in database!";
            return View("Index");
        }
        [HttpPost]
        public ActionResult RetrieveImage()
        {
            Images img = _dbContext.Images.OrderByDescending
        (i => i.Id).FirstOrDefault();
            string imageBase64Data =
        Convert.ToBase64String(img.ImageData);
            string imageDataURL =
        string.Format("data:image/jpg;base64,{0}",
        imageBase64Data);
            ViewBag.ImageTitle = img.ImageTitle;
            ViewBag.ImageDataUrl = imageDataURL;
            return View("Index");
        }
    }
}
