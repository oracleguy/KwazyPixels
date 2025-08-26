using Microsoft.IO;

namespace KwazyPixels.Services;

internal class ImageProcessor : IImageProcessor
{
    private readonly RecyclableMemoryStreamManager MemoryStreamManager;

    public ImageProcessor(RecyclableMemoryStreamManager memoryStreamManager)
    {
        MemoryStreamManager = memoryStreamManager;
    }

    public MemoryStream ProcessImage(string imageFile, int width, int height, ResizeMode resizeMode, out string mimeType)
    {
        mimeType = "image/jpeg";
        return new RecyclableMemoryStream(MemoryStreamManager);
    }
}
