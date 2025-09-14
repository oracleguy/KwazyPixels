namespace KwazyPixels;

/// <summary>
/// Representation of an image file.
/// </summary>
public interface IImage
{
    /// <summary>
    /// Gets the path to the image on disk.
    /// </summary>
    string Path { get; }

    /// <summary>
    /// Gets the date and time the image was taken, if available from metadata.
    /// </summary>
    DateTime? DateTaken { get; }

    /// <summary>
    /// Gets the date and time the image file was created on disk.
    /// </summary>
    DateTime DateCreated { get; }
}
