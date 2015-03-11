namespace DotNetThumbor
{
    public interface IThumborImage
    {
        IThumborImage Resize(int? newWidth, int? newHeight);

        /// <summary>
        /// The smart.
        /// </summary>
        /// <param name="doSmartImage">
        /// The smart image.
        /// </param>
        /// <returns>
        /// The <see cref="Thumbor"/>.
        /// </returns>
        IThumborImage Smart(bool doSmartImage);

        IThumborImage Format(Thumbor.ImageFormat imageFormat);

        IThumborImage Crop(int topLeft, int topRight, int bottomLeft, int bottomRight);

        IThumborImage Quality(int? imageQuality);

        IThumborImage Grayscale(bool grayscaleImage);

        IThumborImage Watermark(string watermarkImageUrl, int right, int down, int transparency);

        IThumborImage Fill(string fillInColour);

        IThumborImage Trim(bool trimImage);

        IThumborImage FitIn(bool fitIn);

        IThumborImage FullFitIn(bool fullFitIn);

        IThumborImage HorizontalAlign(Thumbor.ImageHorizontalAlign align);

        IThumborImage VerticalAlign(Thumbor.ImageVerticalAlign align);

        IThumborImage HorizontalFlip(bool flipHorizontal);

        IThumborImage VerticalFlip(bool flipVertical);

        string ToUrl();

        string ToUnsafeUrl();

        IThumborImage Brightness(int brightness);

        IThumborImage Contrast(int contrast);
    }
}
