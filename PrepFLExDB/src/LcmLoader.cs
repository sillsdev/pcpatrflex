using PrepFLExDB;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.Utils;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIL.PrepFLExDB
{
	public class LcmLoader
	{
		public LcmLoader(ProjectId projId, Label label)
		{
			this.ProjectId = projId;
			this.Label = label;
		}

		public Label Label { get; set; }
		public ProjectId ProjectId { get; set; }

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
			Console.WriteLine("Loading FieldWorks project...");
			Label.Text = "Loading FieldWorks project...";
			Label.Refresh();
			LcmCache cache = null;
			try
			{
				cache = LcmCache.CreateCacheFromExistingData(ProjectId, "en", logger, dirs, settings, progress);
				Console.WriteLine("Loading completed.");
				Label.Text = "Loading completed.";
				return cache;
			}
			catch (LcmFileLockedException)
			{
				Console.WriteLine("Loading failed.");
				Console.WriteLine("The FieldWorks project is currently open in another application.");
				Console.WriteLine("Close the application and try to run this command again.");
				Label.Text = "Loading failed.\nThe FieldWorks project is currently open in another application.\nClose the application and try to run this program again.";
				return null;
			}
			catch (LcmDataMigrationForbiddenException)
			{
				Console.WriteLine("Loading failed.");
				Console.WriteLine("The FieldWorks project was created with an older version of FLEx.");
				Console.WriteLine("Migrate the project to the latest version by opening it in FLEx.");
				Label.Text = "Loading failed.\nThe FieldWorks project was created with an older version of FLEx.\nMigrate the project to the latest version by opening it in FLEx.";
				return null;
			}

		}
	}
}
