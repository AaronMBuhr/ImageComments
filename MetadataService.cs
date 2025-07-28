using System.Text;
using MetadataExtractor;
using MetadataExtractor.Formats.Png;

namespace ImageComments
{
    public class MetadataService
    {
        public Dictionary<string, string> ExtractMetadata(string filePath)
        {
            var metadataFields = new Dictionary<string, string>();
            var allMetadata = new StringBuilder();
            var directoryEntries = new Dictionary<string, StringBuilder>();
            
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
                                    metadataFields[keyword] = value;
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
                    metadataFields[entry.Key] = entry.Value.ToString();
                }
                
                // Add the combined data
                if (allMetadata.Length > 0)
                {
                    metadataFields["All Metadata"] = allMetadata.ToString();
                }
                else
                {
                    metadataFields["Info"] = "No metadata found in this image.";
                }
            }
            catch (Exception ex)
            {
                metadataFields["Error"] = $"Error extracting metadata: {ex.Message}";
            }
            
            return metadataFields;
        }
    }
} 