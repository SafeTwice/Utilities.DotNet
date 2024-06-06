/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Utilities.DotNet.Tasks
{
    /// <summary>
    /// Controls the execution of a cancellable task with status and progress feedback.
    /// </summary>
    public class TaskController
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Indicates if the task is running.
        /// </summary>
        public bool IsRunning => ( m_cancellationTokenSource != null );

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="onStatusUpdated">Action called when a running task status is updated.</param>
        /// <param name="onProgressUpdated">Action called when a running task progress is updated.</param>
        public TaskController( Action<string>? onStatusUpdated = null, Action<double>? onProgressUpdated = null )
        {
            m_onStatusUpdated = onStatusUpdated;
            m_onProgressUpdated = onProgressUpdated;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Runs a task that has no return value.
        /// </summary>
        /// <remarks>
        /// The action that runs the task receives a <see cref="TaskState"/> object that allows
        /// to update the task status and progress.
        /// </remarks>
        /// <param name="action">Action that will execute the task.</param>
        /// <exception cref="InvalidOperationException">Thrown when a task is already running.</exception>
        public void Run( Action<TaskState> action )
        {
            if( m_cancellationTokenSource != null )
            {
                throw new InvalidOperationException( "Task already running" );
            }

            m_cancellationTokenSource = new();

            var state = new TaskState( m_cancellationTokenSource.Token, m_onStatusUpdated, m_onProgressUpdated );

            try
            {
                action( state );
            }
            finally
            {
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Runs asynchronously a task that has no return value.
        /// </summary>
        /// <remarks>
        /// The action that runs the task receives a <see cref="TaskState"/> object that allows
        /// to update the task status and progress.
        /// </remarks>
        /// <param name="action">Asynchronous action that will execute the task.</param>
        /// <exception cref="InvalidOperationException">Thrown when a task is already running.</exception>
        public async Task RunAsync( Func<TaskState, Task> action )
        {
            if( m_cancellationTokenSource != null )
            {
                throw new InvalidOperationException( "Task already running" );
            }

            m_cancellationTokenSource = new();

            var state = new TaskState( m_cancellationTokenSource.Token, m_onStatusUpdated, m_onProgressUpdated );

            try
            {
                await action( state );
            }
            finally
            {
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Runs a task that returns a value.
        /// </summary>
        /// <remarks>
        /// The function that runs the task receives a <see cref="TaskState"/> object that allows
        /// to update the task status and progress.
        /// </remarks>
        /// <param name="function">Function that will execute the task.</param>
        /// <returns>Result of the task.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a task is already running.</exception>
        public TResult Run<TResult>( Func<TaskState, TResult> function )
        {
            if( m_cancellationTokenSource != null )
            {
                throw new InvalidOperationException( "Task already running" );
            }

            m_cancellationTokenSource = new();

            var state = new TaskState( m_cancellationTokenSource.Token, m_onStatusUpdated, m_onProgressUpdated );

            try
            {
                var result = function( state );
                return result;
            }
            finally
            {
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Runs asynchronously a task that returns a value.
        /// </summary>
        /// <remarks>
        /// The function that runs the task receives a <see cref="TaskState"/> object that allows
        /// to update the task status and progress.
        /// </remarks>
        /// <param name="function">Function that will execute the task.</param>
        /// <returns>Result of the task.</returns>
        /// <exception cref="InvalidOperationException">Thrown when a task is already running.</exception>
        public async Task<TResult> RunAsync<TResult>( Func<TaskState, Task<TResult>> function )
        {
            if( m_cancellationTokenSource != null )
            {
                throw new InvalidOperationException( "Task already running" );
            }

            m_cancellationTokenSource = new();

            var state = new TaskState( m_cancellationTokenSource.Token, m_onStatusUpdated, m_onProgressUpdated );

            try
            {
                var result = await function( state );
                return result;
            }
            finally
            {
                m_cancellationTokenSource.Dispose();
                m_cancellationTokenSource = null;
            }
        }

        /// <summary>
        /// Cancels the running task.
        /// </summary>
        public void Cancel()
        {
            m_cancellationTokenSource?.Cancel();
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly Action<string>? m_onStatusUpdated;
        private readonly Action<double>? m_onProgressUpdated;

        private CancellationTokenSource? m_cancellationTokenSource;
    }
}
