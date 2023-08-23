/// @file
/// @copyright  Copyright (c) 2019-2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Exceptions
{
    /// <summary>
    /// Represents a "controlled" error raised during processing.
    /// </summary>
    /// <remarks>
    /// This type of exceptions are useful to differenciate application-defined "controlled" exceptions, which are expected to be
    /// handled by the application without crashing or stopping the application (e.g., by showing an error dialog to the user,
    /// logging the error), from other unexpected exceptions (i.e., derived from <c>System.Exception</c>) that cannot be handled
    /// without aborting the execution of the application.
    /// </remarks>
    public class RuntimeException : Exception
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Description of the error.</param>
        public RuntimeException( string message ) : base( message )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Description of the error</param>
        /// <param name="innerException">Exception that caused the current exception</param>
        public RuntimeException( string message, Exception innerException ) : base( message, innerException )
        {
        }
    }
}
