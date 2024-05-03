/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Filters
{
    /// <summary>
    /// Represents an status value that is debounced using a counter-based hysteresis filter.
    /// </summary>
    public class DebouncedStatus
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <value>
        /// Indicates the current filtered status.
        /// </value>
        public bool Active { get; private set; }

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <summary>
        /// Event triggered when the status changes from not active to active.
        /// </summary>
        public event Action? Activated;

        /// <summary>
        /// Event triggered when the status changes from active to not active.
        /// </summary>
        public event Action? Deactivated;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activationLimit">Number of immediate activations / deactivations to change the status.</param>
        public DebouncedStatus( int activationLimit )
        {
            Active = false;
            m_activationLimit = activationLimit;
            m_count = 0;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Updates the immediate (not filtered) status.
        /// </summary>
        /// <remarks>
        /// The user must call this method to update the immediate status. 
        /// When the accumulated activations reach the activation limit, the debounced status is set to active. 
        /// Likewise, when the accumulated activations reach the deactivation limit, the debounced status is set to not active.
        /// When the debounced status changes, the corresponding event is triggered.
        /// </remarks>
        /// <param name="active">Indicates if the immediate status is active.</param>
        public void UpdateStatus( bool active )
        {
            if( active )
            {
                if( m_count < m_activationLimit )
                {
                    m_count++;

                    if( !Active && ( m_count == m_activationLimit ) )
                    {
                        Active = true;
                        Activated?.Invoke();
                    }
                }
            }
            else
            {
                if( m_count > 0 )
                {
                    m_count--;

                    if( Active && ( m_count == 0 ) )
                    {
                        Active = false;
                        Deactivated?.Invoke();
                    }
                }
            }
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly int m_activationLimit;
        private int m_count;
    }
}
