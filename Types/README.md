# Utilities.DotNet.Types

## About

The _Utilities.DotNet.Types_ package provides utilities to work with [Type](https://learn.microsoft.com/es-es/dotnet/api/system.type)s.

## Usage

The following methods are provided in the `Utilities.DotNet.Types.TypeUtilities` static class:

| Method                   | Description                                              |
|--------------------------|----------------------------------------------------------|
| FindSubclasses           | Finds the subclasses of a type                           |
| RunClassConstructors     | Runs the static constructors of subclasses of a type     |

### FindSubclasses

The `FindSubclasses` methods are used to obtain the subclasses of a class or interface. The subclasses can be searched in all loaded assemblies, in a collection of assemblies or in a single assembly.

##### Example

``` CS
public interface IPlugin
{
  ...
}

public class PluginManager
{
  public void LoadPlugins()
  {
  	var pluginTypes = TypeUtilities.FindSubclasses( typeof( IPlugin ) );

    foreach( var pluginType in pluginTypes )
    {
      var plugin = (IPlugin) Activator.CreateInstance( pluginType );
      m_plugins.Add( plugin );
    }
  }

  private List<IPlugin> m_plugins;
}
```

### RunClassConstructors

The `RunClassConstructors` methods are used to execute the static class constructors for subclasses of a class or interface. The subclasses can be searched in all loaded assemblies, in a collection of assemblies or in a single assembly.

##### Example

``` CS
public interface IModule
{
  ...
}

public class ModuleManager
{
  public static void Register( IModule module )
  {
    ...
  }
}

public partial class App : Application
{
  private void OnStartup( object sender, StartupEventArgs e )
  {
    TypeUtilities.RunClassConstructors( typeof( IModule ) );
  }
}

public class AModule : IModule
{
  static AModule()
  {
    ModuleManager.Register( new AModule() );
  }
  ...
}
```

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_types)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_types)
