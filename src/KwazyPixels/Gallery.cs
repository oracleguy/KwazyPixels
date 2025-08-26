namespace KwazyPixels;

/// <summary>
/// Represents a gallery of images.
/// </summary>
public class Gallery(string name, string path)
{

    /// <summary>
    /// Gets the name of the gallery.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the path to the gallery on disk.
    /// </summary>
    public string Path { get; } = path;

    public string GetNextImage()
    {
        return string.Empty;
    }
}
