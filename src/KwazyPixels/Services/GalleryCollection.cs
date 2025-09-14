namespace KwazyPixels.Services;

/// <summary>
/// Handles all the loaded galleries for the application.
/// </summary>
internal class GalleryCollection : IGalleryCollection
{
    private readonly List<Gallery> galleries = new();
    private readonly IImageFactory ImageFactory;

    public GalleryCollection(IConfiguration configuration, ILogger<GalleryCollection> logger, IImageFactory imageFactory)
    {
        ImageFactory = imageFactory;
        try
        {
            LoadGalleries(configuration, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to load galleries from configuration.");
        }
    }

    public IReadOnlyList<Gallery> Galleries => galleries;

    private void LoadGalleries(IConfiguration configuration, ILogger logger)
    {
        var galleryConfigs = configuration.GetSection("Galleries").Get<List<GalleryConfig>>();
        if (galleryConfigs == null || galleryConfigs.Count == 0)
        {
            throw new InvalidOperationException("No galleries configured.");
        }
        foreach (var config in galleryConfigs)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(config.Name))
                {
                    throw new InvalidOperationException("Gallery name cannot be empty.");
                }
                if (string.IsNullOrWhiteSpace(config.Path))
                {
                    throw new InvalidOperationException($"Gallery path for '{config.Name}' cannot be empty.");
                }
                if (!Directory.Exists(config.Path))
                {
                    throw new InvalidOperationException($"Gallery path '{config.Path}' for '{config.Name}' does not exist.");
                }
                var gallery = new Gallery(config, ImageFactory);
                if (gallery.TotalImageCount == 0)
                {
                    logger.LogWarning("Gallery '{Name}' contains no images.", config.Name);
                }
                galleries.Add(gallery);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to load gallery '{Name}' from path '{Path}'.", config.Name, config.Path);
            }
        }
    }
}
