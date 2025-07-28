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
        private readonly MetadataService _metadataService = new MetadataService();

        public MainWindow()
        {
            InitializeComponent();
            
            // Process command line arguments
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                // Find the image path (skip flags)
                string? filePath = null;
                for (int i = 1; i < args.Length; i++)
                {
                    if (!args[i].StartsWith("-"))
                    {
                        filePath = args[i];
                        break;
                    }
                }
                
                if (!string.IsNullOrEmpty(filePath))
                {
                    ProcessImageFile(filePath);
                }
                else
                {
                    FileNameTextBlock.Text = "No file specified";
                    CommentTextBox.Text = "Please provide an image file as a command line argument.";
                    FieldsComboBox.IsEnabled = false;
                }
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
                // Extract metadata using the service
                _metadataFields = _metadataService.ExtractMetadata(filePath);
                
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