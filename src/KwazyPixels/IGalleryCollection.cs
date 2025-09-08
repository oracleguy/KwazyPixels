namespace KwazyPixels;

/// <summary>
/// The collection of galleries available to the application.
/// </summary>
public interface IGalleryCollection
{
    IReadOnlyList<Gallery> Galleries { get; }
}
