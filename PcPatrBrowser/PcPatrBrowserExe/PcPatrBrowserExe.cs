// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2003-2021, SIL International. All Rights Reserved.
// <copyright from='2003' to='2021' company='SIL International'>
//		Copyright (c) 2003, SIL International. All Rights Reserved.
//
//		Distributable under the terms of either the Common Public License or the
//		GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
#endregion
//
// File: PcPatrBrowser.cs
// Responsibility:
// Last reviewed:
//
// <remarks>
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using SIL.PcPatrBrowser;

namespace SIL.PcPatrBrowser
{
    /// <summary>
    /// Summary description for PcPatrBrowser.
    /// </summary>
    public class PcPatrBrowserExe
    {
        /// -----------------------------------------------------------------------------------
        /// <summary>
        /// Application entry point. If PcPatrBrowser isn't already running,
        /// an instance of the app is created.
        /// </summary>
        /// <param name="rgArgs">Command-line arguments</param>
        /// <returns>0</returns>
        /// -----------------------------------------------------------------------------------
        [STAThread]
        public static void Main(string[] rgArgs)
        {
            PcPatrBrowserApp.Main(rgArgs);
        }
    }
}
