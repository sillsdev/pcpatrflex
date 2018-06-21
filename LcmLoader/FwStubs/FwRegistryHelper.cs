// Copyright (c) 2010-2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Microsoft.Win32;
using SIL.LCModel.Utils;

namespace SIL.LcmLoader
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Helper class for accessing FieldWorks-specific registry settings
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public static class FwRegistryHelper
	{
		/// <summary/>
		public static readonly string TranslationEditor = "Translation Editor";
		private static IFwRegistryHelper RegistryHelperImpl = new FwRegistryHelperImpl();

		/// <summary/>
		public static class Manager
		{
			/// <summary>
			/// Resets the registry helper. NOTE: should only be used from unit tests!
			/// </summary>
			public static void Reset()
			{
				RegistryHelperImpl = new FwRegistryHelperImpl();
			}

			/// <summary>
			/// Sets the registry helper. NOTE: Should only be used from unit tests!
			/// </summary>
			public static void SetRegistryHelper(IFwRegistryHelper helper)
			{
				RegistryHelperImpl = helper;
			}
		}

		/// <summary>Default implementation of registry helper</summary>
		private class FwRegistryHelperImpl: IFwRegistryHelper
		{
			public FwRegistryHelperImpl()
			{
				// FWNX-1235 Mono's implementation of the "Windows Registry" on Unix uses XML files in separate folders for
				// each user and each software publisher.  We need to read Paratext's entries, so we copy theirs into ours.
				// We overwrite any existing Paratext keys in case they have changed.
				if (MiscUtils.IsUnix)
				{
#if DEBUG
					// On a developer Linux machine these are kept under output/registry. Since the program is running at output/{debug|release},
					// one level up should find the registry folder.
					var fwRegLoc = Path.Combine(
						Path.GetDirectoryName(FileUtils.StripFilePrefix(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)) ?? ".", "../registry");
#else
					var fwRegLoc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".config/fieldworks/registry");
#endif

					var ptRegKeys = new[]
					{
						"LocalMachine/software/scrchecks", // Paratext 7 and earlier
						"LocalMachine/software/paratext" // Paratext 8 (latest as of 2017.07)
					};

					foreach (var ptRegKey in ptRegKeys)
					{
						var ptRegKeyLoc = Path.Combine(
							Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), ".config/paratext/registry", ptRegKey);

						if (Directory.Exists(ptRegKeyLoc))
							DirectoryUtils.CopyDirectory(ptRegKeyLoc, Path.Combine(fwRegLoc, ptRegKey), true, true);
					}
				}
			}

			/// <inheritdoc />
			public RegistryKey FieldWorksRegistryKeyLocalMachine => RegistryHelper.SettingsKeyLocalMachine(FieldWorksRegistryKeyName);

			/// <inheritdoc />
			public RegistryKey LocalMachineHive => Registry.LocalMachine;

			/// <inheritdoc />
			/// <remarks>This key is not opened for write access because it will fail on
			/// non-administrator logins. 32bit registry section on a 64bit machine is checked first
			/// and then the 'normal' registry location.
			/// </remarks>
			public RegistryKey FieldWorksBridgeRegistryKeyLocalMachine
			{
				get
				{
					////var flexBridgeKey = $@"FLEx Bridge\{FwUtils.SuiteVersion}";
					return null;//// RegistryHelper.CompanyKeyLocalMachine?.OpenSubKey(flexBridgeKey)
						////?? RegistryHelper.CompanyKeyLocalMachineOld32Bit?.OpenSubKey(flexBridgeKey);
				}
			}

			/// <inheritdoc />
			public RegistryKey FieldWorksRegistryKeyLocalMachineForWriting => RegistryHelper.SettingsKeyLocalMachineForWriting(FieldWorksRegistryKeyName);

			/// <inheritdoc />
			public RegistryKey FieldWorksRegistryKey => RegistryHelper.SettingsKey(FieldWorksRegistryKeyName);

			/// <inheritdoc />
			public RegistryKey FieldWorksVersionlessRegistryKey => RegistryHelper.SettingsKey();

			/// <inheritdoc />
			public RegistryKey FieldWorksVersionlessOld32BitRegistryKey => RegistryHelper.SettingsKeyOld32Bit();

			/// <inheritdoc />
			public string UserLocaleValueName => "UserWs";

			/// <inheritdoc />
			public bool Paratext7Installed()
			{
				using (var ParatextKey = Registry.LocalMachine.OpenSubKey("Software\\ScrChecks\\1.0"))
				{
#if __MonoCS__
					// Unfortunately on Linux Paratext 7.5 does not produce all the same registry keys as it does on Windows
					// we can't actually tell the version of Paratext from these keys, so assume 7 if Settings_Directory is found
					return ParatextKey != null && RegistryHelper.KeyExists(ParatextKey, "Settings_Directory");
#else
					return ParatextKey != null && RegistryHelper.KeyExists(ParatextKey, "Program_Files_Directory_Ptw7");
#endif
				}
			}
		}

		/// <summary>
		/// Sets the company and product key names.
		/// </summary>
		public static void Initialize()
		{
			RegistryHelper.CompanyName = "SIL";
			RegistryHelper.ProductName = "FieldWorks";
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the read-only local machine Registry key for FieldWorks.
		/// NOTE: This key is not opened for write access because it will fail on
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RegistryKey FieldWorksRegistryKeyLocalMachine => RegistryHelperImpl.FieldWorksRegistryKeyLocalMachine;

		/// <summary>
		/// Get LocalMachine hive. (Overridable for unit tests.)
		/// </summary>
		public static RegistryKey LocalMachineHive => RegistryHelperImpl.LocalMachineHive;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the read-only local machine Registry key for FieldWorksBridge.
		/// NOTE: This key is not opened for write access because it will fail on
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RegistryKey FieldWorksBridgeRegistryKeyLocalMachine => RegistryHelperImpl.FieldWorksBridgeRegistryKeyLocalMachine;

		/// <summary>
		/// Returns a read-only subkey of HKLM\Software using the company name, the product name, and, if necessary, WOW6432Node.
		/// </summary>
		public static RegistryKey SettingsKeyLocalMachineForCurrentOr32BitApp(params string[] subkeys)
		{
			return RegistryHelper.SettingsKeyLocalMachine(subkeys) ??
				Registry.LocalMachine.OpenSubKey($@"Software\WOW6432Node\{RegistryHelper.CompanyName}\{RegistryHelper.ProductName}\{string.Join("\\", subkeys)}");
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the local machine Registry key for FieldWorks.
		/// NOTE: This will throw with non-administrative logons! Be ready for that.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static RegistryKey FieldWorksRegistryKeyLocalMachineForWriting => RegistryHelperImpl.FieldWorksRegistryKeyLocalMachineForWriting;

		/// <summary>
		/// Extension method to write a registry key to somewhere in HKLM hopfully with
		/// eleverating privileges. This method can cause the UAC dialog to be shown to the user
		/// (on Vista or later).
		/// Can throw SecurityException on permissions problems.
		/// </summary>
		public static void SetValueAsAdmin(this RegistryKey key, string name, string value)
		{
			Debug.Assert(key.Name.Substring(0, key.Name.IndexOf("\\", StringComparison.Ordinal)) == "HKEY_LOCAL_MACHINE",
				"SetValueAsAdmin should only be used for writing hklm values.");

			if (MiscUtils.IsUnix)
			{
				key.SetValue(name, value);
				return;
			}

			int startOfKey = key.Name.IndexOf("\\", StringComparison.Ordinal) + "\\".Length;
			string location = key.Name.Substring(startOfKey, key.Name.Length - startOfKey);
			location = location.Trim('\\');

			// .NET cmd processing treats \" as a single ", not part of a delimiter.
			// This can mess up closing " delimiters when the string ends with backslash.
			// To get around this, you need to add an extra \ to the end.  "D:\"  -> D:"	 "D:\\" -> D:\
			// Cmd line with 4 args: "Software\SIL\"8" "Projects\\Dir\" "I:\" "e:\\"
			// Interpreted as 3 args: 1)"Software\\SIL\\FieldWorks\"8"  2)"Projects\\\\Dir\" I:\""  3)"e:\\"
			// We'll hack the final value here to put in an extra \ for final \. "c:\\" will come through as c:\.
			string path = value;
			if (value.EndsWith("\\"))
				path = value + "\\";

			using (var process = new Process())
			{
				// Have to show window to get UAC message to allow admin action.
				//process.StartInfo.CreateNoWindow = true;
				process.StartInfo.FileName = "WriteKey.exe";
				process.StartInfo.Arguments = $"LM \"{location}\" \"{name}\" \"{path}\"";
				// NOTE: According to information I found, these last 2 values have to be set as they are
				// (Verb='runas' and UseShellExecute=true) in order to get the UAC dialog to show.
				// On Xp (Verb='runas' and UseShellExecute=true) causes crash.
				if (MiscUtils.IsWinVistaOrNewer)
				{
					process.StartInfo.Verb = "runas";
					process.StartInfo.UseShellExecute = true;
				}
				else
				{
					process.StartInfo.UseShellExecute = false;
				}
				// Make sure the shell window is not shown (FWR-3361)
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

				// Can throw a SecurityException.
				process.Start();
				process.WaitForExit();
			}
		}

		/// <summary>
		/// Gets the default (current user) Registry key for FieldWorks.
		/// </summary>
		public static RegistryKey FieldWorksRegistryKey => RegistryHelperImpl.FieldWorksRegistryKey;

		/// <summary>
		/// Gets the default (current user) Registry key for FieldWorks without the version number.
		/// </summary>
		public static RegistryKey FieldWorksVersionlessRegistryKey => RegistryHelperImpl.FieldWorksVersionlessRegistryKey;

		/// <summary>
		/// Gets the default (current user) Registry key for FieldWorks without the version number.
		/// </summary>
		public static RegistryKey FieldWorksVersionlessOld32BitRegistryKey => RegistryHelperImpl.FieldWorksVersionlessOld32BitRegistryKey;

		/// <summary>
		/// Gets the current SuiteVersion as a string
		/// </summary>
		public static string FieldWorksRegistryKeyName => "9"; //// FwUtils.SuiteVersion.ToString(CultureInfo.InvariantCulture);

		/// <summary>
		/// It's probably a good idea to keep around the name of the old versions' keys
		/// for upgrading purposes. See UpgradeUserSettingsIfNeeded().
		/// </summary>
		internal const string OldFieldWorksRegistryKeyNameVersion7 = "7.0";

		/// <summary>
		/// It's probably a good idea to keep around the name of the old versions' keys
		/// for upgrading purposes. See UpgradeUserSettingsIfNeeded().
		/// </summary>
		internal const string OldFieldWorksRegistryKeyNameVersion8 = "8";

		/// <summary>
		/// The value we look up in the FieldWorksRegistryKey to get(or set) the persisted user locale.
		/// </summary>
		public static string UserLocaleValueName => RegistryHelperImpl.UserLocaleValueName;

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Determines the installation or absence of version 7 of the Paratext program by checking for the
		/// existence of the registry key that that application uses to store its program files
		/// directory in the local machine settings.
		/// This is 'HKLM\Software\ScrChecks\1.0\Program_Files_Directory_Ptw7'
		/// NOTE: This key is not opened for write access because it will fail on
		/// non-administrator logins.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public static bool Paratext7Installed()
		{
			return RegistryHelperImpl.Paratext7Installed();
		}

		/// <summary>
		/// E.g. the first time the user runs FW9, we need to copy a bunch of registry keys
		/// from HKCU/Software/SIL/FieldWorks/7.0 -> FieldWorks/9 or
		/// from HKCU/Software/SIL/FieldWorks/8 -> FieldWorks/9 and
		/// from HKCU/Software/WOW6432Node/SIL/FieldWorks -> HKCU/Software/SIL/FieldWorks
		/// </summary>
		/// <returns>'true' if upgrade was done from any earlier version, otherwise, 'false'.</returns>
		public static bool UpgradeUserSettingsIfNeeded()
		{
			var migratedAnything = false;
			try
			{
				// We could be migrating historic keys from the current or an older architecture
				using (var fwCurrentArchKey = FieldWorksVersionlessRegistryKey)
				using (var fwOld32BitKey = FieldWorksVersionlessOld32BitRegistryKey)
				// Keys for versions 7 & 8 will be either 32-bit or 64-bit
				using (var version7Key = fwCurrentArchKey.OpenSubKey(OldFieldWorksRegistryKeyNameVersion7)
										?? fwOld32BitKey?.OpenSubKey(OldFieldWorksRegistryKeyNameVersion7))
				using (var version8Key = fwCurrentArchKey.OpenSubKey(OldFieldWorksRegistryKeyNameVersion8, true)
										?? fwOld32BitKey?.OpenSubKey(OldFieldWorksRegistryKeyNameVersion8, true))
				// Keys for version 9 could be both 32-bit and 64-bit; our final target is the current architecture
				using (var version9Old32Key = fwOld32BitKey?.OpenSubKey(FieldWorksRegistryKeyName, true))
				using (var currentKey = fwCurrentArchKey.CreateSubKey(FieldWorksRegistryKeyName))
				{
					var allKeys = new[] { version7Key, version8Key, version9Old32Key, currentKey };
					var oldestUnmigrated = allKeys.First(key => key != null);
					foreach (var nextKey in allKeys)
					{
						if (nextKey != null && nextKey != oldestUnmigrated)
						{
							CopyFilteredSubKeyTree(oldestUnmigrated, nextKey);
							DeleteRegistryKeyFromCurentUser(oldestUnmigrated);
							migratedAnything = true;
							oldestUnmigrated = nextKey;
						}
					}
					// Now that everything has been migrated, we are done with WOW6432Node\FieldWorks, if it exists
					if (fwOld32BitKey != null)
					{
						DeleteRegistryKeyFromCurentUser(fwOld32BitKey);
					}
					fwCurrentArchKey.DeleteSubKeyTree(TranslationEditor, false);
				}
			}
			catch (SecurityException se)
			{
				// What to do here? Punt!
			}
			return migratedAnything;
		}

		/// <summary>
		/// Copies filtered list of keys and values from src to dest subKey recursively. Does not overwrite existing values.
		/// </summary>
		private static void CopyFilteredSubKeyTree(RegistryKey srcSubKey, RegistryKey destSubKey)
		{
			CopyFilteredValues(srcSubKey, destSubKey);

			// Copy subkeys, except these that are not copied.
			var subkeysToRemove = new HashSet<string>
			{
				TranslationEditor.ToLowerInvariant()
			};
			foreach (var subKeyName in srcSubKey.GetSubKeyNames())
			{
				if (subkeysToRemove.Contains(subKeyName.ToLowerInvariant()))
					continue;

				using (var srcKey = srcSubKey.OpenSubKey(subKeyName))
				using (var newDestKey = destSubKey.CreateSubKey(subKeyName))
				{
					CopyFilteredSubKeyTree(srcKey, newDestKey);
				}
			}
		}

		/// <summary>
		/// Deletes the specified registry key and its subkeys from its parent key
		/// </summary>
		private static void DeleteRegistryKeyFromCurentUser(RegistryKey goner)
		{
			var oldKeyName = goner.Name;
			// +1 to first backslash so the backslash itself is excluded from the substring
			var oldKeyFirstBackslash = oldKeyName.IndexOf(@"\", StringComparison.Ordinal) + 1;
			var oldKeyLastBackslash = oldKeyName.LastIndexOf(@"\", StringComparison.Ordinal);
			var oldKeyParentName = oldKeyName.Substring(oldKeyFirstBackslash, oldKeyLastBackslash - oldKeyFirstBackslash);
			var oldKeyLastName = oldKeyName.Substring(oldKeyLastBackslash + 1);
			using (var oldKeyParent = Registry.CurrentUser.OpenSubKey(oldKeyParentName, true))
			{
				oldKeyParent?.DeleteSubKeyTree(oldKeyLastName, false);
			}
		}

		private static void CopyFilteredValues(RegistryKey srcSubKey, RegistryKey destSubKey)
		{
			// Copy all values, except these that are not copied.
			const string NumberPrefix = "numberof";
			var valuesToBeRemoved = new HashSet<string>
			{
				"launches",
				"projectshared"
			};
			foreach (var valueName in srcSubKey.GetValueNames())
			{
				var lcValueName = valueName.ToLowerInvariant();
				if (lcValueName.StartsWith(NumberPrefix) || valuesToBeRemoved.Contains(lcValueName))
					continue;

				// Don't overwrite the value if it exists already!
				object dummyValue;
				if (RegistryHelper.RegEntryValueExists(destSubKey, string.Empty, valueName, out dummyValue))
					continue;

				var valueObject = srcSubKey.GetValue(valueName);
				destSubKey.SetValue(valueName, valueObject);
			}
		}
		}
}
