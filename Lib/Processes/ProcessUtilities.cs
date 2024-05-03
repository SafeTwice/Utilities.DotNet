/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Utilities.DotNet.Processes
{
    /// <summary>
    /// Process utilities.
    /// </summary>
    public static class ProcessUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets information about the modules loaded in the current process.
        /// </summary>
        /// <returns>A collection with the name and version of each loaded module.</returns>
        public static IEnumerable<(string name, string version)> GetModulesInfo()
        {
            foreach( ProcessModule module in Process.GetCurrentProcess().Modules )
            {
                yield return (module.ModuleName!, module.FileVersionInfo.FileVersion!);
            }
        }

        /// <summary>
        /// Gets information about the assemblies loaded in the current application domain.
        /// </summary>
        /// <returns>A collection with the name and version of each loaded assembly.</returns>
        public static IEnumerable<(string name, string version)> GetAssembliesInfo()
        {
            foreach( Assembly assemblyInfo in AppDomain.CurrentDomain.GetAssemblies() )
            {
                AssemblyName assemblyName = assemblyInfo.GetName();
                yield return (assemblyName.Name!, assemblyName.Version!.ToString());
            }
        }
    }
}
