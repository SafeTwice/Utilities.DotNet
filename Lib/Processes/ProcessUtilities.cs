﻿/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Utilities.Net.Processes
{
    /// <summary>
    /// Process utilities.
    /// </summary>
    public static class ProcessUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static IEnumerable<(string name, string version)> GetModulesInfo()
        {
            foreach( ProcessModule module in Process.GetCurrentProcess().Modules )
            {
                yield return (module.ModuleName!, module.FileVersionInfo.FileVersion!);
            }
        }

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
