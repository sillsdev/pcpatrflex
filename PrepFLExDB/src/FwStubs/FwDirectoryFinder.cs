// Copyright (c) 2003-2017 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)
//
// <remarks>
// To find the current user's "My Documents" folder, use something like:
//		string sMyDocs = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
// See the MSDN documentation for the System.Environment.SpecialFolder enumeration for details.
// </remarks>

using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using Microsoft.Win32;
using SIL.LCModel;
using SIL.LCModel.Utils;

namespace SIL.PrepFLExDB
{
	/// <summary>
	/// This class is used to find files and directories for FW apps.
	/// </summary>
	public static class FwDirectoryFinder
	{
		/// <summary>
		/// The name of the Language Explorer folder (Even though this is the same as
		/// FwSubKey.LexText, PLEASE do not use it interchangeably.
		/// Use the one that is correct for your context, in case they need to be changed later.)
		/// </summary>
		public const string ksFlexFolderName = "Language Explorer";


		/// <summary>The Scripture-specific stylesheet (ideally, this would be in a TE-specific place, but FDO needs it)</summary>
		public const string kTeStylesFilename = "TeStyles.xml";
		private const string ksProjectsDir = "ProjectsDir";

		private const string CompanyName = "SIL";

		private static readonly ILcmDirectories s_lcmDirs = new FwLcmDirectories();

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the Scripture-specific stylesheet.
		/// This should NOT be in the TE folder, because it is used by the SE edition, when
		/// doing minimal scripture initialization in order to include Paratext texts.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string TeStylesPath
		{
			get { return Path.Combine(CodeDirectory, ksFlexFolderName, kTeStylesFilename); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the folder where FLEx-specific files are installed.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FlexFolder
		{
			get { return GetCodeSubDirectory(ksFlexFolderName); }
		}

		/// <summary>
		/// Return the folder in which FlexBridge resides, or empty string if it is not installed.
		/// </summary>
		public static string FlexBridgeFolder
		{
			get { return GetFLExBridgeFolderPath(); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the FW Language Explorer executable.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FlexExe
		{
			get { return ExeOrDllPath("Flex.exe"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the FW Language Explorer dynamic load library.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string FlexDll
		{
			get { return ExeOrDllPath("LexTextDll.dll"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the Migrate SQL databases executable.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string MigrateSqlDbsExe
		{
			get { return ExeOrDllPath("MigrateSqlDbs.exe"); }
		}

		/// <summary>
		/// Gets the path to MSSQLMigration\Db.exe.
		/// </summary>
		public static string DbExe
		{
			get { return Path.Combine(GetCodeSubDirectory("MSSQLMigration"), "db.exe"); }
		}

		/// <summary>
		/// Gets the converter console executable.
		/// </summary>
		public static string ConverterConsoleExe
		{
			get { return ExeOrDllPath("ConverterConsole.exe"); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path of the requested executable/dll file in the folder from which FW
		/// is being executed.
		/// </summary>
		/// <param name="file">Name of the file (case-sensistive, with the extension)</param>
		/// ------------------------------------------------------------------------------------
		private static string ExeOrDllPath(string file)
		{
			if (file == null)
				throw new ArgumentNullException("file");

			if (Assembly.GetEntryAssembly() == null)
			{
				// This seems to happen when tests call this method when run from NUnit
				// for some reason.

				// The following code should only run by unittests.
#if DEBUG
				const string arch = "Debug";
#else
				const string arch = "Release";
#endif
				return Path.Combine(Path.Combine(Path.Combine(Path.GetDirectoryName(SourceDirectory), "Output"), arch), file);
			}

			return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), file);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get a sub directory of the given <paramref name="directory"/>,
		/// or return a tidied up version of the original path,
		/// if it is not in the FW code folder structure.
		/// </summary>
		/// <param name="directory">Base directory.</param>
		/// <param name="subDirectory">examples: "WW\XAMPLE or \WW\XAMPLE"</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		private static string GetSubDirectory(string directory, string subDirectory)
		{
			Debug.Assert(subDirectory != null);

			string retval = subDirectory.Trim();
			if (retval.Length > 0 && (retval[0] == Path.DirectorySeparatorChar
				|| retval[0] == Path.AltDirectorySeparatorChar))
			{
				// remove leading directory separator from subdirectory
				retval = retval.Substring(1);
			}
			string possiblePath = Path.Combine(directory, retval);
			if (Directory.Exists(possiblePath))
				retval = possiblePath;
			// Implicit 'else' assumes it to be a full path,
			// but not in the code folder structure.
			// Sure hope the caller can handle it.

#if __MonoCS__
			else if (!Directory.Exists(retval)) // previous Substring(1) causes problem for 'full path' in Linux
				return subDirectory;
#endif

			return retval;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get a sub directory of the FW code directory,
		/// or return a tidied up version of the original path,
		/// if it is not in the FW code folder structure.
		/// </summary>
		/// <param name="subDirectory">examples: "WW\XAMPLE or \WW\XAMPLE"</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static string GetCodeSubDirectory(string subDirectory)
		{
			return GetSubDirectory(CodeDirectory, subDirectory);
		}

		private static string GetFLExBridgeFolderPath()
		{
			// Setting a Local Machine registry value is problematic for Linux/Mono.  (FWNX-1180)
			// Try an alternative way of finding FLExBridge first.
			var dir = Environment.GetEnvironmentVariable("FLEXBRIDGEDIR");
			if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
				return dir;
			var key = FwRegistryHelper.FieldWorksBridgeRegistryKeyLocalMachine;
			if (key != null)
				return GetDirectory(key, "InstallationDir", "");
			return "";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get a sub directory of the FW data directory,
		/// or return a tidied up version of the original path,
		/// if it is not in the FW data folder structure.
		/// </summary>
		/// <param name="subDirectory">examples: "Languages or \Languages"</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static string GetDataSubDirectory(string subDirectory)
		{
			return GetSubDirectory(DataDirectory, subDirectory);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Get a file in the FW code directory.
		/// </summary>
		/// <param name="filename">examples: "iso-8859-1.tec"</param>
		/// <returns></returns>
		/// ------------------------------------------------------------------------------------
		public static string GetCodeFile(string filename)
		{
			return Path.Combine(CodeDirectory, filename);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a subdirectory of FieldWorks either by reading the
		/// <paramref name="registryValue"/> or by getting <paramref name="defaultDir"/>.
		/// Will not return <c>null</c>.
		/// </summary>
		/// <param name="registryValue">The name of the registry value to read from the FW root
		/// key in HKLM.</param>
		/// <param name="defaultDir">The default directory to use if there is no value in the
		/// registry.</param>
		/// <returns>
		/// The desired subdirectory of FieldWorks (without trailing directory separator).
		/// </returns>
		/// ------------------------------------------------------------------------------------
		private static string GetDirectory(string registryValue, string defaultDir)
		{
			using (var userKey = FwRegistryHelper.FieldWorksRegistryKey)
			{
				if (userKey == null || userKey.GetValue(registryValue) == null)
					return GetDirectoryLocalMachine(registryValue, defaultDir);
				return GetDirectory(userKey, registryValue, defaultDir);
			}
		}

		/// <summary>
		/// Get a directory for a particular key ignoring current user settings.
		/// </summary>
		/// <param name="registryValue"></param>
		/// <param name="defaultDir"></param>
		/// <returns></returns>
		private static string GetDirectoryLocalMachine(string registryValue, string defaultDir)
		{
			using (RegistryKey machineKey = FwRegistryHelper.FieldWorksRegistryKeyLocalMachine)
			{
				return GetDirectory(machineKey, registryValue, defaultDir);
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a subdirectory of FieldWorks either by reading the
		/// <paramref name="registryValue"/> or by getting <paramref name="defaultDir"/>.
		/// Will not return <c>null</c>.
		/// </summary>
		/// <param name="registryKey">The registry key where the value is stored.</param>
		/// <param name="registryValue">The name of the registry value under the given key that
		/// contains the desired directory.</param>
		/// <param name="defaultDir">The default directory to use if there is no value in the
		/// registry.</param>
		/// <returns>
		/// The desired subdirectory of FieldWorks (without trailing directory separator).
		/// </returns>
		/// <exception cref="ApplicationException">If the desired directory could not be found.
		/// </exception>
		/// ------------------------------------------------------------------------------------
		private static string GetDirectory(RegistryKey registryKey, string registryValue,
			string defaultDir)
		{
			string rootDir = (registryKey == null) ? null : registryKey.GetValue(registryValue, null) as string;

			if (string.IsNullOrEmpty(rootDir) && !string.IsNullOrEmpty(defaultDir))
				rootDir = defaultDir;
			if (string.IsNullOrEmpty(rootDir))
			{
				throw new ApplicationException(
					"Invalid Installation");////ResourceHelper.GetResourceString("kstidInvalidInstallation"));
			}
			// Hundreds of callers of this method are using Path.Combine with the results.
			// Combine only works with a root directory if it is followed by \ (e.g., c:\)
			// so we don't want to trim the \ in this situation.
			string dir = rootDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			return dir.Length > 2 ? dir : dir + Path.DirectorySeparatorChar;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the directory where FieldWorks code was installed (usually
		/// C:\Program Files\SIL\FieldWorks n).
		/// Will not return <c>null</c>.
		/// </summary>
		/// <exception cref="ApplicationException">If an installation directory could not be
		/// found.</exception>
		/// ------------------------------------------------------------------------------------
		public static string CodeDirectory
		{
			get
			{
				////string defaultDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), CompanyName,
				////	$"FieldWorks {FwUtils.SuiteVersion}");
				return "code directory"; //// GetDirectory("RootCodeDir", defaultDir);
			}
		}

		private const string ksRootDataDir = "RootDataDir";
		private const string ksFieldWorks = "FieldWorks";
		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the directory where FieldWorks data was installed (i.e. under AppData).
		/// </summary>
		/// <exception cref="ApplicationException">If an installation directory could not be
		/// found.</exception>
		/// ------------------------------------------------------------------------------------
		public static string DataDirectory => GetDirectory(ksRootDataDir,
			Path.Combine(LcmFileHelper.CommonApplicationData, CompanyName, ksFieldWorks));

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the directory where FieldWorks data was installed (i.e. under AppData),
		/// as it would be determined ignoring current user registry settings.
		/// </summary>
		/// <exception cref="ApplicationException">If an installation directory could not be
		/// found.</exception>
		/// ------------------------------------------------------------------------------------
		public static string DataDirectoryLocalMachine => GetDirectoryLocalMachine(ksRootDataDir,
			Path.Combine(LcmFileHelper.CommonApplicationData, CompanyName, ksFieldWorks));

		private static string m_srcdir;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the src dir (for running tests)
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string SourceDirectory
		{
			get
			{
				if (!string.IsNullOrEmpty(m_srcdir))
					return m_srcdir;
				if (MiscUtils.IsUnix)
				{
					// Linux doesn't have the registry setting, at least while running tests,
					// so we'll assume the executing assembly is $FW/Output/Debug/FwUtils.dll,
					// and the source dir is $FW/Src.
					Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
					var dir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
					dir = Path.GetDirectoryName(dir);       // strip the parent directory name (Debug)
					dir = Path.GetDirectoryName(dir);       // strip the parent directory again (Output)
					dir = Path.Combine(dir, "Src");
					if (!Directory.Exists(dir))
						throw new ApplicationException("Could not find the Src directory.  Was expecting it at: " + dir);
					m_srcdir = dir;
				}
				else
				{
					string rootDir = null;
					if (FwRegistryHelper.FieldWorksRegistryKey != null)
					{
						rootDir = FwRegistryHelper.FieldWorksRegistryKey.GetValue("RootCodeDir") as string;
					}
					else if (FwRegistryHelper.FieldWorksRegistryKeyLocalMachine != null)
					{
						rootDir = FwRegistryHelper.FieldWorksRegistryKeyLocalMachine.GetValue("RootCodeDir") as string;
					}
					if (string.IsNullOrEmpty(rootDir))
					{
						throw new ApplicationException(
							string.Format(@"You need to have the registry key {0}\RootCodeDir pointing at your DistFiles dir.",
							FwRegistryHelper.FieldWorksRegistryKeyLocalMachine?.Name));
					}
					string fw = Directory.GetParent(rootDir).FullName;
					string src = Path.Combine(fw, "Src");
					if (!Directory.Exists(src))
						throw new ApplicationException(@"Could not find the Src directory.  Was expecting it at: " + src);
					m_srcdir = src;
				}
				return m_srcdir;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the full path name of the editorial checks directory.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string EditorialChecksDirectory
		{
			get
			{
				string directory = GetCodeSubDirectory(@"Editorial Checks");
				if (!Directory.Exists(directory))
				{
					string msg = "Unable To Find Editorial Checks"; //// ResourceHelper.GetResourceString("kstidUnableToFindEditorialChecks");
					throw new ApplicationException(string.Format(msg, directory));
				}
				return directory;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the basic editorial checks DLL. Note that this is currently the ScrChecks DLL,
		/// but if we ever split this DLL to separate Scripture-specific checks from more
		/// generic checks that are really based on the WS and could be used to check any text,
		/// then this property should be made to return the DLL containing the punctuation
		/// patterns and characters checks.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string BasicEditorialChecksDll
		{
			get
			{
#if RELEASE
				try
				{
#endif
				string directory = EditorialChecksDirectory;
				string checksDll = Path.Combine(directory, "ScrChecks.dll");
				if (!File.Exists(checksDll))
				{
					string msg = "Unable To Find Editorial Checks";//// ResourceHelper.GetResourceString("kstidUnableToFindEditorialChecks");
					throw new ApplicationException(string.Format(msg, directory));
				}
				return checksDll;
#if RELEASE
				}
				catch (ApplicationException e)
				{
					throw new InstallationException(e);
				}
#endif
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the dir where templates are installed
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static string TemplateDirectory
		{
			get { return GetCodeSubDirectory("Templates"); }
		}

		private const string ksProjects = "Projects";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the dir where projects are stored. Setting to null will delete the HKCU
		/// key, so that the HKLM key (system default) will be used for this user.
		/// </summary>
		/// <exception cref="SecurityException">If user does not have permission to write to HKLM
		/// </exception>
		/// ------------------------------------------------------------------------------------
		public static string ProjectsDirectory
		{
			get { return GetDirectory(ksProjectsDir, Path.Combine(DataDirectory, ksProjects)); }
			set
			{
				if (ProjectsDirectory == value)
					return; // no change.

				using (var registryKey = FwRegistryHelper.FieldWorksRegistryKey)
				{
					if (value == null)
					{
						registryKey.DeleteValue(ksProjectsDir);
					}
					else
					{
						registryKey.SetValue(ksProjectsDir, value);
					}
				}
			}
		}

		/// <summary>
		/// The project directory that would be identified if we didn't have any current user registry settings.
		/// </summary>
		public static string ProjectsDirectoryLocalMachine
		{
			get { return GetDirectoryLocalMachine(ksProjectsDir, Path.Combine(DataDirectoryLocalMachine, ksProjects)); }
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether the given path is a direct sub folder of the projects directory.
		/// (This is typically true for the a project-specific folder.)
		/// </summary>
		/// <param name="path">The path.</param>
		/// ------------------------------------------------------------------------------------
		public static bool IsSubFolderOfProjectsDirectory(string path)
		{
			return !string.IsNullOrEmpty(path) && Path.GetDirectoryName(path) == ProjectsDirectory;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path for storing user-specific application data.
		/// </summary>
		/// <param name="appName">Name of the application.</param>
		/// ------------------------------------------------------------------------------------
		public static string UserAppDataFolder(string appName)
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			return Path.Combine(Path.Combine(path, CompanyName), appName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the path for storing common application data that might be shared between
		/// multiple applications and multiple users on the same machine.
		///
		/// On Windows this returns a subdirectory of
		/// Environment.SpecialFolder.CommonApplicationData (C:\ProgramData),on Linux
		/// /var/lib/fieldworks.
		/// </summary>
		/// <param name="appName">Name of the application.</param>
		/// ------------------------------------------------------------------------------------
		public static string CommonAppDataFolder(string appName)
		{
			return Path.Combine(Path.Combine(LcmFileHelper.CommonApplicationData, CompanyName), appName);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the default directory for Backup files. This is per-user.
		/// </summary>
		/// <exception cref="SecurityException">If setting this value and the user does not have
		/// permission to write to HKCU (probably can never happen)</exception>
		/// ------------------------------------------------------------------------------------
		public static string DefaultBackupDirectory
		{
			get
			{
				// NOTE: SpecialFolder.MyDocuments returns $HOME on Linux
				string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
				// FWNX-501: use slightly different default path on Linux
				string defaultDir = MiscUtils.IsUnix ?
					Path.Combine(myDocs, "Documents/fieldworks/backups") :
					Path.Combine(Path.Combine(myDocs, "My FieldWorks"), "Backups");

				using (RegistryKey registryKey = FwRegistryHelper.FieldWorksRegistryKey.OpenSubKey("ProjectBackup"))
					return GetDirectory(registryKey, "DefaultBackupDirectory", defaultDir);
			}
			set
			{
				using (RegistryKey key = FwRegistryHelper.FieldWorksRegistryKey.CreateSubKey("ProjectBackup"))
				{
					if (key != null)
						key.SetValue("DefaultBackupDirectory", value);
				}
			}
		}

		/// <summary>
		/// Location of the DefaultConfigurations folder (root for  Dictionary, Reversal, and other default configurations)
		/// </summary>
		public static string DefaultConfigurations
		{
			get { return Path.Combine(FlexFolder, "DefaultConfigurations"); }
		}

		/// <summary>
		/// Gets the LCM directories service.
		/// </summary>
		public static ILcmDirectories LcmDirectories
		{
			get { return s_lcmDirs; }
		}

		private class FwLcmDirectories : ILcmDirectories
		{
			/// <summary>
			/// Gets the projects directory.
			/// </summary>
			string ILcmDirectories.ProjectsDirectory
			{
				get { return ProjectsDirectory; }
			}

			string ILcmDirectories.TemplateDirectory
			{
				get { return TemplateDirectory; }
			}
		}
	}
}
