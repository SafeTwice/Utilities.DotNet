/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#if DEBUG_I18N || RELEASE_I18N
#define USE_I18N_LIB
using I18N.DotNet;
using System;
#endif


namespace Utilities.DotNet.I18N
{
    /// <summary>
    /// Global localizer for the library.
    /// </summary>
#if USE_I18N_LIB
    public static class LibraryLocalizer
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static void SetLocalizer( ILocalizer localizer )
        {
            g_localizer = localizer;
        }

        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        internal static string Localize( PlainString text ) => g_localizer?.Localize( text ) ?? text.Value;
        internal static string Localize( FormattableString text ) => g_localizer?.Localize( text ) ?? text.ToString();

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private static ILocalizer? g_localizer;
    }
#else
    internal static class LibraryLocalizer
    {
        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        internal static string Localize( string text ) => text;
    }
#endif
}
