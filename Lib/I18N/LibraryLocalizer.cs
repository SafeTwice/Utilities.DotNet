/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#if DEBUG || RELEASE
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
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Library localizer.
        /// </summary>
        public static ILocalizer Localizer { get; } = new AutoLoadLocalizer();

        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        /// <inheritdoc cref="GlobalLocalizer.Localize(FormattableString)"/>
        internal static string Localize( FormattableString text ) => Localizer.Localize( text );
    }
#else
    internal static class LibraryLocalizer
    {
        //===========================================================================
        //                            INTERNAL METHODS
        //===========================================================================

        /// <summary>
        /// Localizer stub.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <returns>Same <paramref name="text"/>.</returns>
        internal static string Localize( string text ) => text;
    }
#endif
}
