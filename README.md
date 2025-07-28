# ImageComments

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![WPF](https://img.shields.io/badge/UI-WPF-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)

A lightweight WPF application for viewing and extracting metadata comments from image files. Perfect for photographers, digital artists, and AI image creators who need to quickly access embedded metadata, including AI generation parameters.

## 📸 Screenshots

*Screenshots coming soon - show the application interface with an image loaded*

## ✨ Features

- 🖼️ **Multi-format Support** - View metadata from JPG/JPEG and PNG files (and many other formats)
- 📝 **Organized Metadata** - Browse metadata organized by directory type with an intuitive dropdown
- 🤖 **AI Parameters** - Special support for AI generation parameters commonly found in PNG files
- 📋 **Easy Copying** - Copy metadata to clipboard with a single click
- 🎯 **Simple Interface** - Clean, user-friendly WPF interface
- ⚡ **Fast & Lightweight** - No external dependencies, works as a standalone application
- 🖱️ **Drag & Drop Ready** - Launch with command-line arguments or batch file

## 📦 Installation

### Option 1: Download Release (Recommended)
1. Go to the [Releases](../../releases) page
2. Download the latest `ImageComments.exe` 
3. Run directly - no installation required!

### Option 2: Build from Source
```bash
# Clone the repository
git clone https://github.com/AaronMBuhr/ImageComments.git
cd ImageComments

# Build the application
dotnet build --configuration Release

# Run the application
dotnet run "path/to/your/image.jpg"
```

## 🚀 Usage

### Command Line
```cmd
# Using dotnet (from source)
dotnet run "path/to/your/image.jpg"

# Using the executable
ImageComments.exe "path/to/your/image.jpg"
```

### File Association (Advanced)
You can set up file associations to right-click any image and "Open with ImageComments"

### Development/Testing
A `RunWithImage.bat` file is included for development purposes to handle process cleanup during testing.

## 🎯 Use Cases

- **Digital Photography** - View EXIF data, camera settings, and GPS coordinates
- **AI Art Creation** - Access generation parameters from AI-created images
- **Image Forensics** - Examine technical metadata for analysis
- **Asset Management** - Quick metadata inspection for image libraries
- **Quality Control** - Verify image properties and embedded information

## 🖼️ Supported Formats

- **JPEG/JPG** (.jpg, .jpeg) - Full EXIF support
- **PNG** (.png) - Text chunks, including AI parameters
- **TIFF** (.tif, .tiff) - Professional image metadata
- **And many more** - Thanks to the MetadataExtractor library

## 🔍 Metadata Types

### JPEG Files
- 📷 **EXIF Data** - Camera settings, exposure, ISO, GPS
- 🏷️ **IPTC Keywords** - Tags and categories
- 📝 **Comments** - User-added descriptions

### PNG Files
- 🤖 **Parameters** - AI generation settings (Stable Diffusion, DALL-E, etc.)
- 📏 **Image Properties** - Dimensions, color type, compression
- 📝 **Text Chunks** - Custom metadata fields

## ⚙️ Requirements

- **Operating System**: Windows 10/11
- **Framework**: .NET 8.0 Runtime
- **Architecture**: x64 or x86

## 🛠️ Dependencies

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) (2.8.1) - Image metadata extraction
- [System.Drawing.Common](https://www.nuget.org/packages/System.Drawing.Common) (8.0.0) - Image processing support

## 🏗️ Building from Source

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 or VS Code (optional)

### Build Steps
```bash
# Clone repository
git clone https://github.com/AaronMBuhr/ImageComments.git
cd ImageComments

# Restore dependencies
dotnet restore

# Build release version
dotnet build --configuration Release

# Publish standalone executable
dotnet publish --configuration Release --self-contained true --runtime win-x64
```

## 🤝 Contributing

We welcome contributions! Here's how you can help:

1. 🍴 Fork the repository
2. 🌟 Create a feature branch (`git checkout -b feature/amazing-feature`)
3. 💻 Make your changes
4. ✅ Test thoroughly
5. 📝 Commit your changes (`git commit -m 'Add amazing feature'`)
6. 🚀 Push to the branch (`git push origin feature/amazing-feature`)
7. 🎯 Open a Pull Request

### Areas for Contribution
- 🌐 Additional image format support
- 🎨 UI/UX improvements
- 📱 Cross-platform support (Avalonia?)
- 🐛 Bug fixes and optimizations
- 📖 Documentation improvements

## 🐛 Issues & Support

- 🐛 **Bug Reports**: [Create an issue](../../issues/new?template=bug_report.md)
- 💡 **Feature Requests**: [Request a feature](../../issues/new?template=feature_request.md)
- ❓ **Questions**: [Start a discussion](../../discussions)

## 📋 Roadmap

- [ ] 🖱️ Drag & drop support
- [ ] 🌓 Dark mode theme
- [ ] 📁 Batch processing multiple files
- [ ] 🔄 Auto-refresh on file changes
- [ ] 📤 Export metadata to JSON/CSV
- [ ] 🖼️ Image preview thumbnail
- [ ] 🌐 Cross-platform support

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- [MetadataExtractor](https://github.com/drewnoakes/metadata-extractor-dotnet) - Excellent library for image metadata extraction
- The .NET and WPF communities for great documentation and support

## 📊 Project Stats

![GitHub repo size](https://img.shields.io/github/repo-size/AaronMBuhr/ImageComments)
![GitHub last commit](https://img.shields.io/github/last-commit/AaronMBuhr/ImageComments)
![GitHub issues](https://img.shields.io/github/issues/AaronMBuhr/ImageComments)

---

<div align="center">
  <sub>Built with ❤️ for the digital imaging community</sub>
</div> 