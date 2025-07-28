using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using MetadataExtractor;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.Exif;

namespace ImageComments
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<string, string> _metadataFields = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            
            // Process command line arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string filePath = args[1];
                ProcessImageFile(filePath);
            }
            else
            {
                FileNameTextBlock.Text = "No file specified";
                CommentTextBox.Text = "Please provide an image file as a command line argument.";
                FieldsComboBox.IsEnabled = false;
            }
        }

        private void ProcessImageFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                FileNameTextBlock.Text = "File not found: " + filePath;
                CommentTextBox.Text = "The specified file could not be found.";
                FieldsComboBox.IsEnabled = false;
                return;
            }

            FileNameTextBlock.Text = filePath;
            _metadataFields.Clear();
            
            try
            {
                // Extract metadata using MetadataExtractor
                ExtractImageMetadata(filePath);
                
                // Populate dropdown
                FieldsComboBox.Items.Clear();
                foreach (var key in _metadataFields.Keys)
                {
                    FieldsComboBox.Items.Add(key);
                }
                
                if (FieldsComboBox.Items.Count > 0)
                {
                    // Try to select "All Metadata" by default
                    int allMetadataIndex = FieldsComboBox.Items.IndexOf("All Metadata");
                    if (allMetadataIndex >= 0)
                    {
                        FieldsComboBox.SelectedIndex = allMetadataIndex;
                    }
                    else
                    {
                        // Fall back to first item if "All Metadata" not found
                        FieldsComboBox.SelectedIndex = 0;
                    }
                    
                    FieldsComboBox.IsEnabled = true;
                }
                else
                {
                    CommentTextBox.Text = "No metadata found in this image.";
                    FieldsComboBox.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                CommentTextBox.Text = $"Error processing file: {ex.Message}";
                FieldsComboBox.IsEnabled = false;
            }
        }

        private void ExtractImageMetadata(string filePath)
        {
            StringBuilder allMetadata = new StringBuilder();
            Dictionary<string, StringBuilder> directoryEntries = new Dictionary<string, StringBuilder>();
            
            try
            {
                // Use MetadataExtractor to get comprehensive metadata
                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
                
                // Iterate through all directories and tags
                foreach (var directory in directories)
                {
                    StringBuilder directoryContent = new StringBuilder();
                    
                    foreach (var tag in directory.Tags)
                    {
                        // Add each tag to the appropriate directory entry
                        directoryContent.AppendLine($"{tag.Name}: {tag.Description}");
                        
                        // Special handling for PNG text data which might contain Parameters
                        if (directory is PngDirectory && tag.Name.Contains("Text"))
                        {
                            string[] parts = tag.Description.Split(new[] { "Keyword=\"", "\" Value=\"" }, StringSplitOptions.None);
                            if (parts.Length >= 3)
                            {
                                string keyword = parts[1].Trim();
                                string value = parts[2].TrimEnd('"');
                                
                                // Store Parameters and other text entries as separate fields
                                if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrEmpty(value))
                                {
                                    _metadataFields[keyword] = value;
                                }
                            }
                        }
                        
                        // Add to all metadata
                        allMetadata.AppendLine($"{directory.Name} - {tag.Name}: {tag.Description}");
                    }
                    
                    // Store each directory's content
                    if (directoryContent.Length > 0)
                    {
                        directoryEntries[directory.Name] = directoryContent;
                    }
                    
                    // Check for errors
                    foreach (var error in directory.Errors)
                    {
                        allMetadata.AppendLine($"Error: {error}");
                    }
                }
                
                // Add each directory as a separate field in the dropdown
                foreach (var entry in directoryEntries)
                {
                    _metadataFields[entry.Key] = entry.Value.ToString();
                }
                
                // Add the combined data
                if (allMetadata.Length > 0)
                {
                    _metadataFields["All Metadata"] = allMetadata.ToString();
                }
                else
                {
                    _metadataFields["Info"] = "No metadata found in this image.";
                }
            }
            catch (Exception ex)
            {
                _metadataFields["Error"] = $"Error extracting metadata: {ex.Message}";
            }
        }

        private void FieldsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FieldsComboBox.SelectedItem != null)
            {
                string selectedField = FieldsComboBox.SelectedItem.ToString();
                
                if (_metadataFields.ContainsKey(selectedField))
                {
                    CommentTextBox.Text = _metadataFields[selectedField];
                }
                else
                {
                    CommentTextBox.Text = "No data available for this field.";
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (CommentTextBox.SelectionLength > 0)
            {
                // If text is selected, copy only the selection
                Clipboard.SetText(CommentTextBox.SelectedText);
                MessageBox.Show("Selected text copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (!string.IsNullOrEmpty(CommentTextBox.Text))
            {
                // If no selection but there is text, copy all text
                Clipboard.SetText(CommentTextBox.Text);
                MessageBox.Show("All text copied to clipboard!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}