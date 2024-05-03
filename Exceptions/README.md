# Utilities.DotNet.Converters

## About

The _Utilities.DotNet.Exceptions_ package provides subclasses of `System.Exception` intended for general usage.

## Usage

The following exception classes are available:

| Class                    | Description                                                              |
|--------------------------|--------------------------------------------------------------------------|
| RuntimeException         | Represents a "controlled" error raised during processing.                |
| FileProcessingException  | Represents an error raised while processing a file.                      |

### RuntimeException

The `RuntimeException` class, which extends `System.Exception`, is useful to differentiate application-defined "controlled" exceptions, which are expected to be handled by the application without crashing or stopping the application (e.g., by showing an error dialog to the user, logging the error, etc.), from other unexpected exceptions (i.e., derived from <c>System.Exception</c>) that might not be handled without aborting the execution of the application.

Any exceptions (i.e., not derived from `RuntimeException`) handled by the application but which cause aborting a processing activity can be wrapped as the inner exception of a `RuntimeException` that can be graciously handled in upper layers.

##### Example

``` CS
async Task DoSomeProcessing()
{
  try
  {
    // Do some asynchronous processing
    ...
  }
  catch( RuntimeException e )
  {
    MessageBox.Show( e.Message, "Error" );
  }
  catch( Exception e )
  {
    Log.AddLogEntry( LogEntryType.GENERAL, "Unexpected Error", e.ToString() );
    throw;
  }
}
```

### FileProcessingException

The `FileProcessingException` class extends [`RuntimeException`](#runtimeexception) by adding file name and line information to the exception to indicate in which line of which file the processing of a file failed.

> Note: Indicating the file name is optional and it can be omitted when the affected file is implicit.

##### Example

``` CS
void LoadXML( XElement element )
{
  if( element.Name.LocalName != TAGNAME )
  {
    throw new FileProcessingException( $"Invalid element {element.Name.LocalName}", filename, line );
  }
  ...
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_exceptions)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_exceptions)
