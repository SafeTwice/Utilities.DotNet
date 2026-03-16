/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;

#if DEBUG || PROFILE
using Utilities.DotNet.Types;
#endif

#pragma warning disable S6670

namespace Utilities.DotNet
{
#pragma warning disable S3881

    /// <summary>
    /// Base class for objects that implement the <see cref="IDisposable"/> interface."/>
    /// </summary>
    [DebuggerDisplay( "TraceInfo = {TraceInfo}" )]
    public abstract class DisposableObject : IDisposable
    {
        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~DisposableObject()
        {
#if DEBUG || PROFILE
            try
            {
                Trace.WriteLine( $"Finalizing {GetType().GetPrettyName()} [{TraceInfo}]" );
            }
            catch( Exception ex ) // TraceInfo may throw an exception if the object is in an inconsistent state during finalization.
            {
                try
                {
                    Trace.WriteLine( $"Exception while finalizing {GetType().GetPrettyName()}: {ex}" );
                }
                catch
                {
                    // Ignore any exception thrown while trying to log the exception during finalization.
                }
            }
#endif

            Dispose( false );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Dispose()
        {
#if DEBUG || PROFILE
            Trace.WriteLine( $"Disposing {GetType().GetPrettyName()} [{TraceInfo}]" );
#endif

            Dispose( true );
            GC.SuppressFinalize( this );
        }

        //===========================================================================
        //                           PROTECTED PROPERTIES
        //===========================================================================

        /// <summary>
        /// Gets the trace information for the object.
        /// </summary>
        protected virtual string TraceInfo => $"HashCode: {GetHashCode():X8}";

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Derived classes must override this method to release resources.
        /// </summary>
        /// <param name="disposing">Indicates if it is called from the <see cref="Dispose()">Dispose()</see> method (when <c>true</c>) or from the finalizer (when <c>false</c>).</param>
        /// <remarks>
        /// <para>Overriding implementations must only dispose other objects when <paramref name="disposing"/> is <c>true</c>.</para>
        /// <para>Overriding implementations must call its base class implementation for this method passing the <paramref name="disposing"/> parameter.</para>
        /// </remarks>
        protected virtual void Dispose( bool disposing )
        {
        }
    }

#pragma warning restore S3881
}
