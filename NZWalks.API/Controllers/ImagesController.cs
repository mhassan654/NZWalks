using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IImageRepository _imageRepository;

    public ImagesController(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }
   
    // POST: /api/Images/Upload
    [HttpPost]
    [Route("Upload")]
    public  async Task<IActionResult> Upload([FromForm] ImageUploadRequestDTo imageUploadRequestDTo)
    {
        ValidateFileUpload(imageUploadRequestDTo);
        if (ModelState.IsValid)
        {
            // convert to dto to domain model
            var imageDomainModel = new Image
            {
                File = imageUploadRequestDTo.File,
                FileExtension = Path.GetExtension(imageUploadRequestDTo.File.FileName),
                FileSizeInBytes = imageUploadRequestDTo.File.Length,
                FileName = imageUploadRequestDTo.FileName,
                FileDescription = imageUploadRequestDTo.FileDescription,
            };
            
            // user repository to upload image
            await _imageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }

        return BadRequest(ModelState);
    }

    private void ValidateFileUpload(ImageUploadRequestDTo requestDTo)
    {
        var allowedExtensions = new string[] { ".jpg", "jpeg", ".png" };

        if (!allowedExtensions.Contains(Path.GetExtension(requestDTo.File.FileName)))
        {
            ModelState.AddModelError("file","Unsupported file type");
        }

        if (requestDTo.File.Length > 1048760)
        {
            ModelState.AddModelError("file","Files size more than 10mb, please upload a smaller size file");
        }
    }
}