using SIL.LcmLoader;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.DomainServices;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIL.PcPatrFLEx
{
	public partial class PcPatrFLExForm : Form
	{
		public LcmCache Cache { get; set; }
		public ProjectId ProjId { get; set; }

		public PcPatrFLExForm()
		{
			InitializeComponent();
			btnParse.Enabled = false;
			lblDatabaseToUse.Text = "";
			Icu.InitIcuDataDir();
			Sldr.Initialize();
		}

		private void btnProject_Click(object sender, EventArgs e)
		{
			btnParse.Enabled = false;
			ProjId = ChooseLangProject(this);
			if (ProjId != null)
			{
				lblDatabaseToUse.Text = ProjId.Name;
				var loader = new LcmLoader.LcmLoader(ProjId);
				loader.RaiseLcmLoaderEvent += HandleLcmLoaderEvent;
				Cache = loader.CreateCache();
				if (Cache != null)
				{
					btnParse.Enabled = true;
				}
			}

		}

		private void HandleLcmLoaderEvent(object sender, LcmLoaderEventArgs a)
		{
			lblDatabaseToUse.Text = a.Message;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Lets the user select an existing language project.
		/// </summary>
		/// <param name="dialogOwner">The owner of the dialog.</param>
		/// <returns>The chosen project, or null if no project was chosen</returns>
		/// ------------------------------------------------------------------------------------
		internal static ProjectId ChooseLangProject(PcPatrFLExForm dialogOwner)
		{
			using (var dlg = new SIL.LcmLoaderUI.ChooseLangProjectDialog(false))
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

		private void UnlockDatabaseIfNeeded()
		{
			if (Cache != null)
			{
				ProjectLockingService.UnlockCurrentProject(Cache);
			}
		}
		private void PcPatrFLExForm_FormClosing(object sender, EventArgs e)
		{
			UnlockDatabaseIfNeeded();
		}

	}
}
