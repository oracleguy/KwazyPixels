using MetadataExtractor;

namespace KwazyPixels.Images;

internal class ImageFactory : IImageFactory
{
    private readonly ILogger<ImageFactory> Logger;

    public ImageFactory(ILogger<ImageFactory> logger)
    {
        Logger = logger;
    }

    public IImage Create(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"The file at path '{path}' does not exist.");
        }

        var dateCreated = File.GetCreationTimeUtc(path);
        var dateTaken = GetDateTaken(path);

        return new Image(path, dateTaken, dateCreated);
    }

    private DateTime? GetDateTaken(string path)
    {
        try
        {
            var data = ImageMetadataReader.ReadMetadata(path);
        }
        catch(ImageProcessingException ex)
        {
            Logger.LogWarning(ex, "Failed to read metadata from image at path '{Path}'", path);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An unexpected error occurred while reading metadata from image at path '{Path}'", path);
        }

        return null;
    }
}
