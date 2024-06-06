/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Threading;

namespace Utilities.DotNet.Tasks
{
    /// <summary>
    /// {Enter brief class description here...}
    /// </summary>
    public class TaskState
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Cancellation token for the task.
        /// </summary>
        public CancellationToken CancellationToken { get; }

        /// <summary>
        /// Indicates if the task has been cancelled.
        /// </summary>
        public bool IsCancellationRequested => CancellationToken.IsCancellationRequested;

        /// <summary>
        /// Sets the status text of the task.
        /// </summary>
        public string StatusText
        {
            set => m_onStatusUpdated?.Invoke( value );
        }

        /// <summary>
        /// Sets the progress of the task (0.0 = 0%, 1.0 = 100%).
        /// </summary>
        public double Progress
        {
            set => m_onProgressUpdated?.Invoke( value );
        }

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        internal TaskState( CancellationToken cancellationToken, Action<string>? onStatusUpdated, Action<double>? onProgressUpdated )
        {
            CancellationToken = cancellationToken;
            m_onStatusUpdated = onStatusUpdated;
            m_onProgressUpdated = onProgressUpdated;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Action<string>? m_onStatusUpdated;
        private readonly Action<double>? m_onProgressUpdated;
    }
}
