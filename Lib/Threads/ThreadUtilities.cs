/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Utilities.DotNet.Threads
{
    /// <summary>
    /// Thread utilities.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ThreadUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Prevents the computer from entering sleep mode.
        /// </summary>
        /// <remarks>
        /// This function must be called periodically to prevent the computer from entering sleep mode.
        /// </remarks>
        public static void PreventComputerSleep()
        {
            SetThreadExecutionState( EXECUTION_STATE.ES_SYSTEM_REQUIRED );
        }

        /// <summary>
        /// Prevents the display from powering off.
        /// </summary>
        /// <remarks>
        /// This function must be called periodically to prevent the display from powering off.
        /// </remarks>
        public static void PreventDisplayPowerOff()
        {
            SetThreadExecutionState( EXECUTION_STATE.ES_DISPLAY_REQUIRED );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        [DllImport( "kernel32.dll", CharSet = CharSet.Auto, SetLastError = true )]
        private static extern EXECUTION_STATE SetThreadExecutionState( EXECUTION_STATE esFlags );

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        [Flags]
        private enum EXECUTION_STATE : uint
        {
            ES_SYSTEM_REQUIRED = 0x00000001,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
        }
    }
}
