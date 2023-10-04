using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;
namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider fileExtensionContentTypeProvider;
        public FilesController(FileExtensionContentTypeProvider _fileExtensionContentTypeProvider)
        {
            fileExtensionContentTypeProvider = _fileExtensionContentTypeProvider; 
        }

        [HttpGet("{fileId}")]
        public IActionResult GetFiles(string fileId)
        {
            var pathToFile = "sample.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }
            if(!fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName("x.pdf"));
        }
    }
}
