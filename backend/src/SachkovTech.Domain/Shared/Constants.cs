namespace SachkovTech.Domain.Shared;

public class Constants
{
    public static class Default
    {
        public const int MAX_LOW_TEXT_LENGTH = 100;

        public const int MAX_HIGH_TEXT_LENGTH = 2000;
    }

    public static class Files
    {
        //Bytes
        public const int MAX_FILE_SIZE = 10 * 1024 * 1024; 
        
        public static readonly string[] ALLOWED_PHOTO_EXTENSIONS = new[] 
            { ".png", ".jpg", ".bmp", ".tiff", ".raw", ".jfif" };
        
        public static readonly string[] ALLOWED_TEXT_EXTENSIONS = new[] 
            { ".txt", ".rtf"};
    }
}