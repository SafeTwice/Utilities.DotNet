# Utilities.DotNet.Files

## About

The _Utilities.DotNet.Files_ package provides utilities to work with files and directories.

## Usage

Theis package provides a single static class `FileUtilities` that contains methods to work with files and directories.

### FileUtilities

The `FileUtilities` class provides the following static methods:

| Method                    | Description                                                             |
|---------------------------|-------------------------------------------------------------------------|
| EnsureFilePathIsAvailable | Creates the directory and subdirectories where a file would be stored   |
| SanitizeFilename | Sanitizes a filename, replacing or deleting charactars invalid for file names    |

#### Example

``` CS
void SaveXML( XDocument doc, string projectName, var outputDir )
{
	var filename = SanitizeFilename( projectName );

    var filepath = $"{outputDir}/{filename}"

    EnsureFilePathIsAvailable( filepath );

    doc.Save( filepath );
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_files)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_files)
