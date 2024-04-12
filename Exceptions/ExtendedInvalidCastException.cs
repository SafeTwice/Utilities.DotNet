/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using static Utilities.DotNet.Exceptions.I18N.LibraryLocalizer;

namespace Utilities.DotNet.Exceptions
{
    /// <summary>
    /// Extension of <see cref="InvalidCastException"/>.
    /// </summary>
    public class ExtendedInvalidCastException : InvalidCastException
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the object that was being casted.
        /// </summary>
        public object Object { get; }

        /// <summary>
        /// Gets the target type of the cast operation.
        /// </summary>
        public Type TargetType { get; }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedInvalidCastException"/> class with the specified object and target type.
        /// </summary>
        /// <param name="obj">The object that was being casted.</param>
        /// <param name="targetType">The target type of the cast operation.</param>
        public ExtendedInvalidCastException( object obj, Type targetType )
            : base( Localize( $"Cannot cast object '{obj}' of type {obj.GetType().FullName} to type {targetType.FullName}." ) )
        {
            Object = obj;
            TargetType = targetType;
        }
    }
}
