/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#if DEBUG_I18N || RELEASE_I18N
#define USE_I18N_LIB
using I18N.Net;
#endif

using System;
/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt
namespace Utilities.Net.I18N
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

#if USE_I18N_LIB
        public static void SetLocalizer( ILocalizer localizer )
        {
            m_localizer = localizer;
        }
#endif

        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        internal static string Localize( PlainString text ) => m_localizer?.Localize( text ) ?? text.Value;
        internal static string Localize( FormattableString text ) => m_localizer?.Localize( text ) ?? text.ToString();

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private static ILocalizer? m_localizer;
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
