/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics;
using Utilities.DotNet.Types;

namespace Utilities.DotNet
{
    /// <summary>
    /// Base class for objects that implement the <see cref="IDisposable"/> interface."/>
    /// </summary>
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
            Debug.Print( $"Finalizing {GetType().GetPrettyName()} [{TraceInfo}]" );

            Dispose( false );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Dispose()
        {
            Debug.Print( $"Disposing {GetType().GetPrettyName()} [{TraceInfo}]" );

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
}
