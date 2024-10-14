namespace SachkovTech.SharedKernel;

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
        
        public static readonly string[] FORBIDDEN_FILE_EXTENSIONS = new[] 
        {
            // Video
            ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv",
    
            // Sound
            ".mp3", ".wav", ".flac", ".aac", ".ogg", ".wma",
    
            // Software
            ".exe", ".dll", ".msi", ".bat", ".sh", ".jar"
        };
    }

    public static class Issues
    {
        public const int LIFETIME_AFTER_DELETION = 30;
    }
}