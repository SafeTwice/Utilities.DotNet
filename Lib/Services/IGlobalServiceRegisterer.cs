/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Represents a class that registers services automatically with the <see cref="ServiceProvider.GlobalServices">global service provider</see>.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement a default constructor.
    /// </remarks>
    public interface IGlobalServiceRegisterer : IServiceRegisterer
    {
    }
}

