# Utilities.DotNet.Services

## About

The _Utilities.DotNet.Services_ package provides utilities to work with service components, as a simple and light-weight alternative to [`System.IServiceProvider`](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider).

## Definitions

A service is a software component that provides functionality to other software components.

>**Note**: In this context, do not confuse this with an operating system service or daemon (i.e., a process running in the background generally launched when the system boots).

A service is represented by an interface (or by an abstract class, but using an interface is recommended).

A service implementation is an instance of a class that implements a service interface (or extends a service abstract class).

A service provider is a class that implements the `Utilities.DotNet.Services.IServiceProvider` interface, which allows to obtain a service implementation for a service identified by its interface / abstract class.

A service implementation must be registered with a service provider before it can be obtained from that service provider.

## Getting Started

### Global Service Registration

The most convenient way of publishing services and then obtaining them is to use the global service provider, which is available as a static property of the `Utilities.DotNet.Services.ServiceProvider` class.

To register a global service implementation first create an instance of the service implementation class, then call the `RegisterService<T>` method of the global service provider, using the service interface as the generic argument and passing the service implementation instance.

Service registration is usually done at the beginning of the program, before any service is obtained.

> **Note**: Only a single service implementation can be registered for a given service interface in a given service provider. If a service implementation is registered more than once, a `System.InvalidOperationException` will be thrown.

##### Example

``` CS
using Utilities.DotNet.Services;

public interface IUserLogin
{
  IUser? ShowUserLogin();
}

public class UserLogin : IUserLogin
{
  public IUser? ShowUserLogin()
  {
    ... // Authenticate user

    if( isUserAuthenticated )
    {
      return new User( userId );
    }
    else
    {
      return null;
    }
  }
}

public static class Program
{
  public static void Main()
  {
    ServiceProvider.GlobalServices.RegisterService<IUserLogin>( new UserLogin() );
  }
}
```

### Obtaining a Global Service

To obtain a global service implementation, call `ServiceProvider.GetGlobalService<T>` (or the `GetService<T>` method of the global service provider), using the service interface as the generic argument, which will return the service implementation instance.

##### Example

``` CS
public void LoginUser()
{
  IUserLogin userLogin = ServiceProvider.GetGlobalService<IUserLogin>();

  IUser? user = userLogin.ShowUserLogin();

  if( user != null )
  {
    ... // User is authenticated
  }
  else
  {
    ... // User is not authenticated
  }
}
```

### Auto-Registered Global Service

Auto-registered global service implementations are service implementations that are registered automatically as a global service and available when their associated service is first obtained from the global service provider.

To implement an auto-registered global service, just derive the service implementation class from `AutoRegisteredGlobalService<T>`, where the generic parameter is the service interface (that must also be explicitly implemented by the service implementation class). The auto-registered global service implementation class must have a parameterless constructor.

> **Note**: If the auto-registered service implementation is implemented in a separate assembly (e.g., in a class library), the assembly may not be loaded before the service is obtained from the global service provider (specially when the service interface is in a different assembly than the service implementation), and therefore it will not be found and instantiated. In such cases, either ensure that the assembly is loaded before the service is obtained, or use the manual service registration method.

##### Example

``` CS
using Utilities.DotNet.Services;

public interface IUserLogin
{
  IUser? ShowUserLogin();
}

public class UserLogin : AutoRegisteredGlobalService<IUserLogin>, IUserLogin
{
  public IUser? ShowUserLogin()
  {
    ... // Authenticate user

    if( isUserAuthenticated )
    {
      return new User( userId );
    }
    else
    {
      return null;
    }
  }
}
```

## API Documentation

#### IServiceProvider interface

The `IServiceProvider` interface represents classes that provide functionality to register and obtain services:
* `RegisterService<T>` method to register a service instance for the service represented by the interface (or abstract class) specified in the generic type parameter.
* `RegisterService` method to register a service instance for the service represented by the interface (or abstract class) given in a `System.Type` parameter.
* `RegisterServiceByInterface` method to register a service instance for the service implicitly represented by the interface of the service instance (for very simple services defined and implemented with a single interface).
* `GetService<T>` method to retrieve a (previously registered) service instance represented by the generic type parameter.

#### IAutoRegisteredGlobalServiceCreator&lt;T&gt; interface

The `IAutoRegisteredGlobalServiceCreator<T>` interface represents classes that provide functionality to create auto-registered global service instances:
* `CreateServiceInstance<T>` method to create a service instance for the service represented by the interface (or abstract class) specified in the generic type parameter.

#### ServiceProvider class

The `ServiceProvider` class implements the [`IServiceProvider`](#iserviceprovider-interface), and provides some additional functionality:
* `GlobalServices` static property to access the global service provider.
* `GetGlobalService<T>` method to retrieve a service from the global service provider (shortcut for `ServiceProvider.GlobalServices.GetService<T>`).

The global service provider `GetService<T>` method, when a service implementation is not found, will try to find a class that implements the interface [`IAutoRegisteredGlobalServiceCreator<T>`](#iautoregisteredglobalservicecreatort-interface) for that service, and if found, will use it to create the service instance.

#### AutoRegisteredGlobalService&lt;T&gt; class

The `AutoRegisteredGlobalService<T>` class is a base class for auto-registered global service implementations, which implements the [`IAutoRegisteredGlobalServiceCreator<T>`](#iautoregisteredglobalservicecreatort-interface) interface.

## Full API Documentation

You can browse the full API documentation for:
 - [The last release (stable)](https://safetwice.github.io/Utilities.DotNet/stable/namespace_utilities_1_1_dot_net_1_1_services)
 - [Main branch (unstable)](https://safetwice.github.io/Utilities.DotNet/main/namespace_utilities_1_1_dot_net_1_1_services)
