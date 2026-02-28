using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Bll.Interfaces;

namespace PhotoAlbum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController(IPhotoService photoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPhotos([FromQuery] string sortBy = "name")
    {
        var photos = await photoService.GetPhotosAsync(sortBy);
        return Ok(photos);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected.");

        using var stream = file.OpenReadStream();
        var photo = await photoService.UploadPhotoAsync(file.FileName, stream);

        return Created(string.Empty, photo); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await photoService.DeletePhotoAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
