using KwazyPixels.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KwazyPixels.Controllers;

/// <summary>
/// Gets information about the gallery of images.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class GalleryController(IGalleryCollection galleryCollection, IImageProcessor imageProcessor) : ControllerBase
{
    private readonly IGalleryCollection GalleryCollection = galleryCollection ?? throw new ArgumentNullException(nameof(galleryCollection));

    /// <summary>
    /// Gets the information about all the galleries.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<List<GalleryInfo>>(StatusCodes.Status200OK)]
    public IActionResult GetGalleries()
    {
        var galleries = GalleryCollection.Galleries.Select((g, index) => new GalleryInfo(g.Name, index)).ToList();

        return Ok(galleries);
    }

    /// <summary>
    /// Gets the next image from the specified gallery, resizing it to the specified dimensions.
    /// </summary>
    /// <param name="galleryIndex">The gallery index to access.</param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="resizeMode"></param>
    /// <returns></returns>
    [HttpGet("{galleryIndex}")]
    [ProducesErrorResponseType(typeof(string))]
    public IActionResult GetNextImage([FromRoute] int galleryIndex, [FromQuery] int width = 1920, [FromQuery] int height = 1080,
        [FromQuery] ResizeMode resizeMode = ResizeMode.Fill)
    {
        if (galleryIndex < 0 || galleryIndex >= GalleryCollection.Galleries.Count)
        {
            return NotFound($"Gallery with index {galleryIndex} not found.");
        }

        if(width <= 0 || height <= 0)
        {
            return BadRequest("Width and height must be positive integers.");
        }

        var gallery = GalleryCollection.Galleries[galleryIndex];
        using (var stream = imageProcessor.ProcessImage(gallery.GetNextImage(), width, height, resizeMode, out var mimeType))
        {
            return File(stream, mimeType);
        }
    }
}
