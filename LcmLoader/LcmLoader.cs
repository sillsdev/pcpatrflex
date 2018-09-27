// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SI.LcmLoader;
using SIL.LCModel;
using SIL.LCModel.Utils;
using System;

namespace SIL.LcmLoader
{
	public class LcmLoader
	{

		public delegate void LcmLoaderEventHandler(object sender, LcmLoaderEventArgs a);
		public event LcmLoaderEventHandler RaiseLcmLoaderEvent;

		protected virtual void OnRaiseLcmLoaderEvent(LcmLoaderEventArgs e)
		{
			RaiseLcmLoaderEvent?.Invoke(this, e);
		}

		public LcmLoader(ProjectId projId)
		{
			this.ProjectId = projId;
		}

		public ProjectId ProjectId { get; set; }

		public static readonly string kLoading = "Loading FieldWorks project...";
		public const string kCompleted = "Loading completed.";
		public const string kFailed = "Loading failed.";
		public const string kProjectOpen = "The FieldWorks project is currently open in another application.";
		public const string kCloseApp = "Close the application and try to run this command again.";
		public const string kProjectOlder = "The FieldWorks project was created with an older version of FLEx.";
		public const string kProjectMigrate = "Migrate the project to the latest version by opening it in a newer version of FLEx.";

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Creates a cache used for accessing the specified project.
		/// </summary>
		/// <param name="projectId">The project id.</param>
		/// <returns>
		/// A new LcmCache used for accessing the specified project, or null, if a
		/// cache could not be created.
		/// </returns>
		/// ------------------------------------------------------------------------------------
		public LcmCache CreateCache()
		{
			var synchronizeInvoke = new SingleThreadedSynchronizeInvoke();

			var logger = new ConsoleLogger(synchronizeInvoke);
			var dirs = new NullLcmDirectories();
			var settings = new LcmSettings { DisableDataMigration = true };
			var progress = new NullThreadedProgress(synchronizeInvoke);
			Console.WriteLine(kLoading);
			OnRaiseLcmLoaderEvent(new LcmLoaderEventArgs(kLoading));
			LcmCache cache = null;
			try
			{
				cache = LcmCache.CreateCacheFromExistingData(ProjectId, "en", logger, dirs, settings, progress);
				Console.WriteLine(kCompleted);
				OnRaiseLcmLoaderEvent(new LcmLoaderEventArgs(kCompleted));
				return cache;
			}
			catch (LcmFileLockedException)
			{
				Console.WriteLine(kFailed);
				Console.WriteLine(kProjectOpen);
				Console.WriteLine(kCloseApp);
				OnRaiseLcmLoaderEvent(new LcmLoaderEventArgs(kFailed + "\n" + kProjectOpen + "\n" + kCloseApp));
				return null;
			}
			catch (LcmDataMigrationForbiddenException)
			{
				Console.WriteLine(kFailed);
				Console.WriteLine(kProjectOlder);
				Console.WriteLine(kProjectMigrate);
				OnRaiseLcmLoaderEvent(new LcmLoaderEventArgs(kFailed + "\n" + kProjectOlder + "\n" + kProjectMigrate));
				return null;
			}

		}
	}
}
