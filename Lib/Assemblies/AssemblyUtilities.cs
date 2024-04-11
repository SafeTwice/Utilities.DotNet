/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Utilities.Net.Assemblies
{
    /// <summary>
    /// Assembly utilities.
    /// </summary>
    public static class AssemblyUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        public static void RunClassConstructors<BaseType>()
        {
            RunClassConstructors( typeof( BaseType ) );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        public static void RunClassConstructors( Type baseType )
        {
            var subTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany( a => a.GetTypes() )
                .Where( t => t.IsClass && !t.IsAbstract && t.IsSubclassOf( baseType ) );

            foreach( var subType in subTypes )
            {
                RuntimeHelpers.RunClassConstructor( subType.TypeHandle );
            }
        }
    }
}
