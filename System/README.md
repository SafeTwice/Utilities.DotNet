# Utilities.DotNet.System

## About

The _Utilities.DotNet.System_ package provides utilities to work with processes and threads.

## Usage

### Processes

The following methods are provided in the `Utilities.DotNet.System.ProcessUtilities` static class:

| Method            | Description                                  |
|-------------------|----------------------------------------------|
| GetModulesInfo    | Gets information about the loaded modules    |
| GetAssembliesInfo | Gets information about the loaded assemblies |

### Threads

The following methods are provided in the `Utilities.DotNet.System.ThreadUtilities` static class:

| Method                 | Description                                    |
|------------------------|------------------------------------------------|
| PreventComputerSleep   | Prevents the computer from entering sleep mode |
| PreventDisplayPowerOff | Prevents the display from powering off         |

Call these functions periodically to prevent the computer from entering sleep mode or the display from powering off.

##### Example

``` CS
bool workFinished = false;

while( !workFinished )
{
  ThreadUtilities.PreventComputerSleep();
  ThreadUtilities.PreventDisplayPowerOff();

  // Do some "quick" incremental/spliced work and set workFinished to true when done
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_system)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_system)
