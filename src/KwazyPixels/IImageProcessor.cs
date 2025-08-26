namespace KwazyPixels;

/// <summary>
/// Handles processing the image based on the specified mode, height, and width.
/// </summary>
public interface IImageProcessor
{
    MemoryStream ProcessImage(string imageFile, int width, int height, ResizeMode resizeMode, out string mimeType);
}
