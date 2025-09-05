/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Utilities.DotNet.Types
{
    /// <summary>
    /// Type utilities.
    /// </summary>
    public static class TypeUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Finds in all loaded assemblies the types of subclasses of the specified type.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses<BaseType>( bool nonAbstractOnly = true )
        {
            return FindSubclasses( typeof( BaseType ), nonAbstractOnly );
        }

        /// <summary>
        /// Finds in all loaded assemblies the types for the subclasses of the specified type.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses( this Type baseType, bool nonAbstractOnly = true )
        {
            return FindSubclasses( baseType, AppDomain.CurrentDomain.GetAssemblies(), nonAbstractOnly );
        }

        /// <summary>
        /// Finds in a collection of assemblies the types for the subclasses of the specified type.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        /// <param name="assemblies">Assemblies to search.</param>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses<BaseType>( this IEnumerable<Assembly> assemblies, bool nonAbstractOnly = true )
        {
            return FindSubclasses( typeof( BaseType ), assemblies, nonAbstractOnly );
        }

        /// <summary>
        /// Finds in a collection of assemblies the types for the subclasses of the specified type.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="assemblies">Assemblies to search.</param>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses( this Type baseType, IEnumerable<Assembly> assemblies, bool nonAbstractOnly = true )
        {
            List<Type> types = new();

            foreach( var assembly in assemblies )
            {
               types.AddRange( FindSubclasses( baseType, assembly, nonAbstractOnly ) );
            }

            return types;
        }

        /// <summary>
        /// Finds in a single assembly the types for the subclasses of the specified type.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        /// <param name="assembly">Assembly to search.</param>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses<BaseType>( this Assembly assembly, bool nonAbstractOnly = true )
        {
            return FindSubclasses( typeof( BaseType ), assembly, nonAbstractOnly );
        }

        /// <summary>
        /// Finds in a single assembly the types for the subclasses of the specified type.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="assembly">Assembly to search.</param>
        /// <param name="nonAbstractOnly">If <c>true</c>, only non-abstract classes are returned.</param>
        /// <returns>Collection of types.</returns>
        public static IEnumerable<Type> FindSubclasses( this Type baseType, Assembly assembly, bool nonAbstractOnly = true )
        {
            return FindSubclasses( baseType, assembly, nonAbstractOnly, true );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in all the loaded assemblies.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        public static void RunClassConstructors<BaseType>()
        {
            RunClassConstructors( typeof( BaseType ) );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in all the loaded assemblies.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        public static void RunClassConstructors( this Type baseType )
        {
            RunClassConstructors( baseType, AppDomain.CurrentDomain.GetAssemblies() );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in the given assemblies.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        /// <param name="assemblies">Assemblies to search for classes.</param>
        public static void RunClassConstructors<BaseType>( this IEnumerable<Assembly> assemblies )
        {
            RunClassConstructors( typeof( BaseType ), assemblies );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in the given assemblies.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="assemblies">Assemblies to search for classes.</param>
        public static void RunClassConstructors( this Type baseType, IEnumerable<Assembly> assemblies )
        {
            foreach( var assembly in assemblies )
            {
                RunClassConstructors( baseType, assembly );
            }
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in the given assembly.
        /// </summary>
        /// <typeparam name="BaseType">Base type.</typeparam>
        /// <param name="assembly">Assembly to search for classes.</param>
        public static void RunClassConstructors<BaseType>( this Assembly assembly )
        {
            RunClassConstructors( typeof( BaseType ), assembly );
        }

        /// <summary>
        /// Runs the static constructors of all the classes that are subclasses of the specified type found
        /// in the given assembly.
        /// </summary>
        /// <param name="baseType">Base type.</param>
        /// <param name="assembly">Assembly to search for classes.</param>
        public static void RunClassConstructors( this Type baseType, Assembly assembly )
        {
            var subTypes = FindSubclasses( baseType, assembly, false, false );

            foreach( var subType in subTypes )
            {
                RuntimeHelpers.RunClassConstructor( subType.TypeHandle );
            }
        }

        /// <summary>
        /// Gets the pretty name of the specified type.
        /// </summary>
        /// <param name="type">A type.</param>
        /// <returns>The pretty name of the type.</returns>
        public static string GetPrettyName( this Type type )
        {
            string typeName = type.Name;

            if( type.IsGenericType )
            {
                var genericArguments = type.GetGenericArguments();
                var genericArgumentsNames = genericArguments.Select( t => t.GetPrettyName() );
                string genericArgumentsString = string.Join( ", ", genericArgumentsNames );

                var prettyTypeName = typeName.Substring( 0, typeName.IndexOf( '`' ) );

                typeName = $"{prettyTypeName}<{genericArgumentsString}>";
            }

            return typeName;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private static IEnumerable<Type> FindSubclasses( Type baseType, Assembly assembly, bool nonAbstractOnly, bool excludeBaseType )
        {
            List<Type> types = new();

            try
            {
                var allAssemblyTypes = assembly.GetTypes();

                foreach( var assemblyType in allAssemblyTypes )
                {
                    if( ( !excludeBaseType || ( assemblyType != baseType ) ) && assemblyType.IsClass && ( !nonAbstractOnly || !assemblyType.IsAbstract ) &&
                          baseType.IsAssignableFrom( assemblyType ) )
                    {
                        types.Add( assemblyType );
                    }
                }
            }
            catch( Exception )
            {
            }

            return types;
        }
    }
}
