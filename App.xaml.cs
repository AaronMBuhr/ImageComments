using System.Configuration;
using System.Data;
using System.Windows;
using System.IO;

namespace ImageComments
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string[] args = e.Args;
            
            // Check for help flags first - exit immediately without any WPF initialization
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-h" || args[i] == "--help")
                {
                    // Allocate console for output
                    AllocConsole();
                    ShowHelp();
                    // Exit immediately without any WPF startup
                    Environment.Exit(0);
                    return;
                }
            }
            
            // Check for console mode flag
            bool consoleMode = false;
            string? imagePath = null;
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-c" || args[i] == "--console")
                {
                    consoleMode = true;
                }
                else if (!args[i].StartsWith("-"))
                {
                    imagePath = args[i];
                }
            }
            
            if (consoleMode)
            {
                // Allocate console for output
                AllocConsole();
                
                // Run in console mode
                RunConsoleMode(imagePath);
                
                // Exit without showing GUI
                Environment.Exit(0);
                return;
            }
            
            // Continue with normal WPF startup (GUI mode)
            base.OnStartup(e);
        }
        
        private void RunConsoleMode(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                Console.WriteLine("Error: No image file specified.");
                Console.WriteLine();
                Console.WriteLine(GetHelpText());
                return;
            }
            
            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"Error: File not found: {imagePath}");
                return;
            }
            
            try
            {
                var metadataService = new MetadataService();
                var metadata = metadataService.ExtractMetadata(imagePath);
                
                Console.WriteLine($"Image: {imagePath}");
                Console.WriteLine(new string('=', 60));
                
                if (metadata.ContainsKey("All Metadata"))
                {
                    Console.WriteLine(metadata["All Metadata"]);
                }
                else
                {
                    Console.WriteLine("No metadata found in this image.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file: {ex.Message}");
            }
        }
        
        private void ShowHelp()
        {
            Console.WriteLine(GetHelpText());
        }
        
        public static string GetHelpText()
        {
            return "ImageComments - Extract and view metadata from image files\r\n" +
                   "\r\n" +
                   "Usage: ImageComments.exe [options] <image_file>\r\n" +
                   "\r\n" +
                   "Options:\r\n" +
                   "  -c, --console    Run in console mode (display metadata in console)\r\n" +
                   "  -h, --help       Show this help message\r\n" +
                   "\r\n" +
                   "Arguments:\r\n" +
                   "  <image_file>     Path to the image file to process\r\n" +
                   "\r\n" +
                   "Examples:\r\n" +
                   "  ImageComments.exe photo.jpg              # Open GUI with photo.jpg\r\n" +
                   "  ImageComments.exe -c photo.jpg           # Display metadata in console\r\n" +
                   "  ImageComments.exe --console photo.jpg    # Display metadata in console\r\n" +
                   "  ImageComments.exe -h                     # Show this help message";
        }
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
