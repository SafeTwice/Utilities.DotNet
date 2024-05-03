/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Logs
{
    /// <summary>
    /// Represents a log.
    /// </summary>
    public interface ILog<TLogEntryType> where TLogEntryType : struct, Enum
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <summary>
        /// Enabled entry types.
        /// </summary>
        TLogEntryType EnabledEntryTypes { get; set; }

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Indicates if an entry type is enabled.
        /// </summary>
        /// <param name="entryType">Type of entry to check.</param>
        /// <returns><c>true</c> if the entry type is enabled, <c>false</c> otherwise.</returns>
        bool IsEntryTypeEnabled( TLogEntryType entryType );

        /// <summary>
        /// Adds an entry to the log if the entry type is enabled.
        /// </summary>
        /// <param name="entryType">Type of the entry.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="message">Message for the entry.</param>
        void AddLogEntry( TLogEntryType entryType, string category, string message );

        /// <summary>
        /// Adds an entry to the log if the entry type is enabled.
        /// </summary>
        /// <remarks>
        /// The message generation function is only called if the entry type is enabled. 
        /// </remarks>
        /// <param name="entryType">Type of the entry.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="messageGenerator">Function that generates the message for the entry.</param>
        void AddLogEntry( TLogEntryType entryType, string category, Func<string> messageGenerator );
    }
}
