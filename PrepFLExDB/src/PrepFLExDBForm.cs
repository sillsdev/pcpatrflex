// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Windows.Forms;
using SIL.LcmLoader;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.DomainServices;
using SIL.WritingSystems;

namespace SIL.PrepFLExDB
{
	public partial class PrepFLExDBForm : Form
	{
		public LcmCache Cache { get; set; }
		public ProjectId ProjId { get; set;  }

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
			btnProcess.Enabled = false;
			ProjId = ChooseLangProject(this);
			if (ProjId != null)
			{
				lblDatabaseToUse.Text = ProjId.Name;
				var loader = new LcmLoader.LcmLoader(ProjId);
				loader.RaiseLcmLoaderEvent += HandleLcmLoaderEvent;
				Cache = loader.CreateCache();
				if (Cache != null)
				{
					btnProcess.Enabled = true;
				}
			}
		}

		private void HandleLcmLoaderEvent(object sender, LcmLoaderEventArgs a)
		{
			lblStatus.Text = a.Message;
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

		
		private void btnCancel_Click(object sender, EventArgs e)
		{
			UnlockDatabaseIfNeeded();
			Application.Exit();
		}

		private void UnlockDatabaseIfNeeded()
		{
			if (Cache != null)
			{
				ProjectLockingService.UnlockCurrentProject(Cache);
			}
		}

		private void btnProcess_Click(object sender, EventArgs e)
		{
			Application.UseWaitCursor = true;
			var preparer = new Preparer(Cache);
			preparer.AddPCPATRList();
			preparer.AddPCPATRSenseCustomField();
			preparer.AddPCPATRSyntacticParserAgent();
			Cache.ActionHandlerAccessor.Commit();
			Application.UseWaitCursor = false;
			MessageBox.Show("Process is complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			btnProcess.Enabled = false;
			ProjectLockingService.UnlockCurrentProject(Cache);
			btnOpenChooser.Focus();
		}

		private void PrepFLExDBForm_FormClosing(object sender, EventArgs e)
		{
			UnlockDatabaseIfNeeded();
		}
	}
}
