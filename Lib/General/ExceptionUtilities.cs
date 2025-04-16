/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet
{
    /// <summary>
    /// Exception utilities.
    /// </summary>
    public static class ExceptionUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Execute an action and ignore any exception that may be thrown.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        /// <returns><see langword="true"/> if the action executed successfully, <see langword="false"/> if an exception was thrown.</returns>
        public static bool BypassExceptions( Action action )
        {
            return BypassExceptions<Exception>( action );
        }

        /// <summary>
        /// Execute an action and ignore any exception of a given type that may be thrown.
        /// </summary>
        /// <typeparam name="TException">Type of exception to ignore.</typeparam>
        /// <param name="action">Action to execute.</param>
        /// <returns><see langword="true"/> if the action executed successfully, <see langword="false"/> if an exception was thrown.</returns>
        public static bool BypassExceptions<TException>( Action action ) where TException : Exception
        {
            try
            {
                action();
                return true;
            }
            catch( TException )
            {
                return false;
            }
        }

        /// <summary>
        /// Execute a function and ignore any exception that may be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="function">Function to execute.</param>
        /// <param name="fallbackValue">Fallback value to return in case of exception.</param>
        /// <returns>Result of the function or the fallback value in case of exception.</returns>
        public static TResult BypassExceptions<TResult>( Func<TResult> function, TResult fallbackValue )
        {
            return BypassExceptions<Exception, TResult>( function, fallbackValue );
        }

        /// <summary>
        /// Execute a function and ignore any exception of a given type that may be thrown.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TException">Type of exception to ignore.</typeparam>
        /// <param name="function">Function to execute.</param>
        /// <param name="fallbackValue">Fallback value to return in case of exception.</param>
        /// <returns>Result of the function or the fallback value in case of exception.</returns>
        public static TResult BypassExceptions<TException, TResult>( Func<TResult> function, TResult fallbackValue )
             where TException : Exception
        {
            try
            {
                return function();
            }
            catch( TException )
            {
                return fallbackValue;
            }
        }
    }
}
