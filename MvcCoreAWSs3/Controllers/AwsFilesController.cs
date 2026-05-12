using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSs3.Services;
using System.Threading.Tasks;

namespace MvcCoreAWSs3.Controllers
{
    public class AwsFilesController : Controller
    {
        private ServiceStorageS3 service;

        public AwsFilesController(ServiceStorageS3 service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> files = await this.service.GetFilesAsync();
            ViewBag.Mnesaje = TempData["Mensaje"];
            return View(files);
        }

        public async Task<IActionResult> UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            int codigo = 0;
            using(Stream stream = file.OpenReadStream())
            {
                codigo = await this.service.UploadFileAsync(file.FileName, stream);
            }
            TempData["Mensaje"] = "Status Code: " + codigo;
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrivateFile(string filename)
        {
            Stream stream = await this.service.GetPrivateFileAsync(filename);
            return File(stream, "image/png");
        }
    }
}
