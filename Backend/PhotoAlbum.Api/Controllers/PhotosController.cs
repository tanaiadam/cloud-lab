using Microsoft.AspNetCore.Mvc;
using PhotoAlbum.Bll.Interfaces;
using PhotoAlbum.Bll.Models;

namespace PhotoAlbum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotosController(IPhotoService photoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPhotos([FromQuery] PhotoFilterRequest request)
    {
        var photos = await photoService.GetPhotosAsync(request);
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

    [HttpPut("{id}/name")]
    public async Task<IActionResult> Rename(Guid id, [FromBody] RenamePhotoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Name cannot be empty.");

        var updatedPhoto = await photoService.RenamePhotoAsync(id, request.Name);

        if (updatedPhoto == null)
            return NotFound();

        return Ok(updatedPhoto);
    }
}
