// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

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
			ProjId = ChooseLangProject(this);
			if (ProjId != null)
			{
				lblDatabaseToUse.Text = ProjId.Name;
				btnProcess.Enabled = true;
				LcmLoader loader = new LcmLoader(ProjId, lblStatus);
				LcmCache cache = loader.CreateCache();
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

		
		private void btnCancel_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
