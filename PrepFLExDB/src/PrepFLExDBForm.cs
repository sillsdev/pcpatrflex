using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrepFLExDB;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.Utils;
using SIL.PrepFLExDB;
using SIL.WritingSystems;

namespace SIL.PrepFLExDB
{
	public partial class PrepFLExDBForm : Form
	{
		public ProjectId projId { get; set;  }

		public PrepFLExDBForm()
		{
			InitializeComponent();
			btnProcess.Enabled = false;
			lblDatabaseToUse.Text = "";
			lblStatus.Text = "";
			Icu.InitIcuDataDir();
			Sldr.Initialize();
		}

		private void btnOpenChooser_Click(object sender, EventArgs e)
		{
			projId = ChooseLangProject(this);
			if (projId != null)
			{
				lblDatabaseToUse.Text = projId.Name;
				btnProcess.Enabled = true;
				LcmCache cache = CreateCache(projId, lblStatus);
			}
			else
			{
				btnProcess.Enabled = false;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Lets the user select an existing language project.
		/// </summary>
		/// <param name="dialogOwner">The owner of the dialog.</param>
		/// <returns>The chosen project, or null if no project was chosen</returns>
		/// ------------------------------------------------------------------------------------
		internal static ProjectId ChooseLangProject(PrepFLExDBForm dialogOwner)
		{
			using (var dlg = new ChooseLangProjectDialog(false))
			{
				FwRegistryHelper.Initialize();
				dlg.ShowDialog(dialogOwner);
				if (dlg.DialogResult == DialogResult.OK)
				{
					var projId = new ProjectId(dlg.Project);
					return projId;
				}

				return null;
			}
		}

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
		private static LcmCache CreateCache(ProjectId projectId, Label status)
		{
			//Icu.InitIcuDataDir();
			//Sldr.Initialize();
			var synchronizeInvoke = new SingleThreadedSynchronizeInvoke();

			//var projectId = new ProjectIdentifier(args[0]);
			var logger = new ConsoleLogger(synchronizeInvoke);
			var dirs = new NullLcmDirectories();
			var settings = new LcmSettings { DisableDataMigration = true };
			var progress = new NullThreadedProgress(synchronizeInvoke);
			Console.WriteLine("Loading FieldWorks project...");
			status.Text = "Loading FieldWorks project...";
			status.Refresh();
			LcmCache cache = null;
			try
			{
				cache = LcmCache.CreateCacheFromExistingData(projectId, "en", logger, dirs, settings, progress);
				Console.WriteLine("Loading completed.");
				status.Text = "Loading completed.";
				return  cache;
			}
			catch (LcmFileLockedException)
			{
				Console.WriteLine("Loading failed.");
				Console.WriteLine("The FieldWorks project is currently open in another application.");
				Console.WriteLine("Close the application and try to run this command again.");
				status.Text = "Loading failed.\nThe FieldWorks project is currently open in another application.\nClose the application and try to run this program again.";
				return null;
			}
			catch (LcmDataMigrationForbiddenException)
			{
				Console.WriteLine("Loading failed.");
				Console.WriteLine("The FieldWorks project was created with an older version of FLEx.");
				Console.WriteLine("Migrate the project to the latest version by opening it in FLEx.");
				status.Text = "Loading failed.\nThe FieldWorks project was created with an older version of FLEx.\nMigrate the project to the latest version by opening it in FLEx.";
				return null;
			}
		}
		
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
