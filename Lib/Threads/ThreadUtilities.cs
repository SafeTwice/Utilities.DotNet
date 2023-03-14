/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Runtime.InteropServices;

namespace Utilities.Net.Threads
{
    /// <summary>
    /// Thread utilities.
    /// </summary>
    public static class ThreadUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static void PreventComputerSleep()
        {
            SetThreadExecutionState( EXECUTION_STATE.ES_SYSTEM_REQUIRED );
        }

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
            ES_SYSTEM_REQUIRED =    0x00000001,
            ES_DISPLAY_REQUIRED =   0x00000002,
            ES_AWAYMODE_REQUIRED =  0x00000040,
            ES_CONTINUOUS =         0x80000000,
        }
    }
}
