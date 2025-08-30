using Microsoft.IO;
using PhotoSauce.MagicScaler;

namespace KwazyPixels.Services;

/// <summary>
/// The primary internal image processor for the application.
/// </summary>
internal class ImageProcessor : IImageProcessor
{
    private readonly RecyclableMemoryStreamManager MemoryStreamManager;
    private const string TargetMimeType = "image/webp";

    public ImageProcessor(RecyclableMemoryStreamManager memoryStreamManager)
    {
        MemoryStreamManager = memoryStreamManager;
    }

    /// <inheritdoc/>
    public MemoryStream ProcessImage(string imageFile, int width, int height, ResizeMode resizeMode, out string mimeType)
    {
        mimeType = TargetMimeType;

        var imageSettings = new ProcessImageSettings();
        if(imageSettings.TrySetEncoderFormat(TargetMimeType) == false)
        {
            throw new InvalidOperationException($"Could not set encoder format to {TargetMimeType}");
        }

        var outputStream = new RecyclableMemoryStream(MemoryStreamManager);
        switch(resizeMode)
        {
            case ResizeMode.None:
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            case ResizeMode.Scale:
                imageSettings.ResizeMode = CropScaleMode.Stretch;
                imageSettings.Width = width;
                imageSettings.Height = height;
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            case ResizeMode.Fit:
                imageSettings.ResizeMode = CropScaleMode.Max;
                imageSettings.Width = width;
                imageSettings.Height = height;
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            case ResizeMode.Fill:
                imageSettings.ResizeMode = CropScaleMode.Crop;
                imageSettings.Width = width;
                imageSettings.Height = height;
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            case ResizeMode.FitWidth:
                imageSettings.ResizeMode = CropScaleMode.Contain;
                imageSettings.Width = width;
                imageSettings.Height = 0;
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            case ResizeMode.FitHeight:
                imageSettings.ResizeMode = CropScaleMode.Contain;
                imageSettings.Width = 0;
                imageSettings.Height = height;
                MagicImageProcessor.ProcessImage(imageFile, outputStream, imageSettings);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resizeMode), resizeMode, null);
        }

        return outputStream;
    }
}
