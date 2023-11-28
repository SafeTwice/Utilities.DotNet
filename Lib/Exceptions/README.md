# Utilities.DotNet.Converters

## About

The _Utilities.DotNet.Exceptions_ package provides subclasses of `System.Exception` intented for general usage.

## Usage

The following exception classes are available:

| Class                    | Description                                                              |
|--------------------------|--------------------------------------------------------------------------|
| FileProcessingException  | Represents an error raised while processing a file                       |

### FileProcessingException

The `FileProcessingException` class extends `System.Exception` by adding file name and line information to the exception to indicate which the file and line where the exception was triggered.

#### Example

``` CS
void LoadXML( XElement element )
{
	if( element.Name.LocalName != TAGNAME )
    {
		throw new FileProcessingException( $"Invalid element {}", filename, line );
    }
    ...
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_exceptions)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_exceptions)
