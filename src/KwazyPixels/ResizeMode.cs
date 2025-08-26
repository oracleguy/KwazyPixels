namespace KwazyPixels;

/// <summary>
/// The supported image reize modes.
/// </summary>
public enum ResizeMode
{
    /// <summary>
    /// No resizing is done on the image, the original dimensions are returned ignoring any specified dimensions.
    /// </summary>
    None,
    /// <summary>
    /// Scales the image to the specified dimensions without maintaining its aspect ratio.
    /// </summary>
    Scale,
    /// <summary>
    /// The image is resized if either the height or the width is larger than the specified dimensions. This is essentially combining <see cref="FitWidth"/> and <see cref="FitHeight"/>.
    /// This will result in the image fitting within the specified dimensions while maintaining its aspect ratio.
    /// </summary>
    Fit,
    /// <summary>
    /// The image is resized based on the center point to completely fill the specified dimensions while maintaining its aspect ratio. This may result in some parts of the image being cropped.
    /// </summary>
    Fill,
    /// <summary>
    /// The image is resized to fit the width of the specified dimensions while maintaining its aspect ratio. The height will be whatever is necessary to maintain the aspect ratio.
    /// </summary>
    FitWidth,
    /// <summary>
    /// The image is resized to fit the height of the specified dimensions while maintaining its aspect ratio. The width will be whatever is necessary to maintain the aspect ratio.
    /// </summary>
    FitHeight
}
