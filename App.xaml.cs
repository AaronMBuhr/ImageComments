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
                Shutdown(0);
                return;
            }
            
            // Continue with normal WPF startup
            base.OnStartup(e);
        }
        
        private void RunConsoleMode(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                Console.WriteLine("Error: No image file specified.");
                Console.WriteLine("Usage: ImageComments.exe [-c|--console] <image_file>");
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
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
