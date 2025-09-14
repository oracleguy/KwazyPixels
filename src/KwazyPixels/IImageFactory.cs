namespace KwazyPixels;

/// <summary>
/// The factory interface for creating <see cref="IImage"/> instances.
/// </summary>
public interface IImageFactory
{
    /// <summary>
    /// Create an <see cref="IImage"/> instance from the specified path.
    /// </summary>
    /// <param name="path">The path to the image on disk.</param>
    /// <returns></returns>
    IImage Create(string path);
}
