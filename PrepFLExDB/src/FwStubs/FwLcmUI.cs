// Copyright (c) 2013-2014 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using SIL.LCModel;
using SIL.LCModel.Utils;

namespace SIL.PrepFLExDB
{
	/// <summary>
	/// The implementation of ILcmUI for FieldWorks apps.
	/// </summary>
	public class FwLcmUI : ILcmUI
	{
		private readonly ISynchronizeInvoke m_synchronizeInvoke;

		public FwLcmUI(ISynchronizeInvoke synchronizeInvoke)
		{
			m_synchronizeInvoke = synchronizeInvoke;
		}

		/// <summary>
		/// Gets the object that is used to invoke methods on the main UI thread.
		/// </summary>
		public ISynchronizeInvoke SynchronizeInvoke
		{
			get { return m_synchronizeInvoke; }
		}

		/// <summary>
		/// Check with user regarding conflicting changes
		/// </summary>
		/// <returns>True if user wishes to revert to saved state. False otherwise.</returns>
		public bool ConflictingSave()
		{
			//using (var dlg = new ConflictingSaveDlg())
			//{
			//	DialogResult result = dlg.ShowDialog();
			//	return result != DialogResult.OK;
			//}
			return false;
		}

		/// <summary>
		/// Check with user regarding which files to use
		/// </summary>
		/// <returns></returns>
		public FileSelection ChooseFilesToUse()
		{
			//using (var dlg = new FilesToRestoreAreOlder(m_helpTopicProvider))
			//{
			//	if (dlg.ShowDialog() == DialogResult.OK)
			//	{
			//		if (dlg.fKeepFilesThatAreNewer)
			//		{
			//			return FileSelection.OkKeepNewer;
			//		}
			//		if (dlg.fOverWriteThatAreNewer)
			//		{
			//			return FileSelection.OkUseOlder;
			//		}
			//	}
			//	return FileSelection.Cancel;
			//}
			return new FileSelection();
		}

		/// <summary>
		/// Check with user regarding restoring linked files in the project folder or original path
		/// </summary>
		/// <returns>True if user wishes to restore linked files in project folder. False to leave them in the original location.</returns>
		public bool RestoreLinkedFilesInProjectFolder()
		{
			//using (var dlg = new RestoreLinkedFilesToProjectsFolder(m_helpTopicProvider))
			//{
			//	return dlg.ShowDialog() == DialogResult.OK && dlg.fRestoreLinkedFilesToProjectFolder;
			//}
			return false;
		}

		/// <summary>
		/// Cannot restore linked files to original path.
		/// Check with user regarding restoring linked files in the project folder or not at all
		/// </summary>
		/// <returns>OkYes to restore to project folder, OkNo to skip restoring linked files, Cancel otherwise</returns>
		public YesNoCancel CannotRestoreLinkedFilesToOriginalLocation()
		{
			//using (var dlgCantWriteFiles = new CantRestoreLinkedFilesToOriginalLocation(m_helpTopicProvider))
			//{
			//	if (dlgCantWriteFiles.ShowDialog() == DialogResult.OK)
			//	{
			//		if (dlgCantWriteFiles.fRestoreLinkedFilesToProjectFolder)
			//		{
			//			return YesNoCancel.OkYes;
			//		}
			//		if (dlgCantWriteFiles.fDoNotRestoreLinkedFiles)
			//		{
			//			return YesNoCancel.OkNo;
			//		}
			//	}
			//	return YesNoCancel.Cancel;
			//}
			return YesNoCancel.Cancel;
		}

		/// <summary>
		/// Displays the message.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="message">The message.</param>
		/// <param name="caption">The caption.</param>
		/// <param name="helpTopic">The help topic.</param>
		public void DisplayMessage(MessageType type, string message, string caption, string helpTopic)
		{
			var icon = MessageBoxIcon.Information;
			switch (type)
			{
				case MessageType.Error:
					icon = MessageBoxIcon.Error;
					break;
				case MessageType.Info:
					icon = MessageBoxIcon.Information;
					break;
				case MessageType.Warning:
					icon = MessageBoxIcon.Warning;
					break;
			}
			m_synchronizeInvoke.Invoke(() =>
				{
#if __MonoCS__
					// Mono doesn't support Help
					MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
#else
				if (string.IsNullOrEmpty(helpTopic))
					MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
				else
					MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);////, MessageBoxDefaultButton.Button1,
							////0, m_helpTopicProvider.HelpFile, HelpNavigator.Topic, helpTopic);
#endif
				});
		}

		/// <summary>
		/// Displays the circular reference breaker report.
		/// </summary>
		public void DisplayCircularRefBreakerReport(string report, string caption)
		{
			var icon = MessageBoxIcon.Information;
			m_synchronizeInvoke.Invoke(() => MessageBox.Show(report, caption, MessageBoxButtons.OK, icon));
		}

		/// <summary>
		/// show a dialog or output to the error log, as appropriate.
		/// </summary>
		/// <param name="error">the exception you want to report</param>
		/// <param name="isLethal">set to <c>true</c> if the error is lethal, otherwise
		/// <c>false</c>.</param>
		public void ReportException(Exception error, bool isLethal)
		{
			////m_synchronizeInvoke.Invoke(() => ErrorReporter.ReportException(error, null, null, null, isLethal));
		}

		public DateTime LastActivityTime
		{
			get { return DateTime.Now /*(m_activityMonitor.LastActivityTime*/; }
		}

		/// <summary>
		/// Reports duplicate guids to the user
		/// </summary>
		/// <param name="errorText">The error text.</param>
		public void ReportDuplicateGuids(string errorText)
		{
			////m_synchronizeInvoke.Invoke(() => ErrorReporter.ReportDuplicateGuids(FwRegistryHelper.FieldWorksRegistryKey, "FLExErrors@sil.org", null, errorText));
		}

		/// <summary>
		/// Ask user if they wish to restore an XML project from a backup project file.
		/// </summary>
		/// <param name="projectPath">The project path.</param>
		/// <param name="backupPath">The backup path.</param>
		/// <returns></returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool OfferToRestore(string projectPath, string backupPath)
		{
			//return m_synchronizeInvoke.Invoke(() => MessageBox.Show(
			//	String.Format(FdoUiStrings.kstidOfferToRestore, projectPath, File.GetLastWriteTime(projectPath),
			//	backupPath, File.GetLastWriteTime(backupPath)),
			//	FdoUiStrings.kstidProblemOpeningFile, MessageBoxButtons.YesNo,
			//	MessageBoxIcon.Error) == DialogResult.Yes);
			return false;
		}

		/// <summary>
		/// Present a message to the user and allow the options to Retry or Cancel
		/// </summary>
		/// <param name="msg">The message.</param>
		/// <param name="caption">The caption.</param>
		/// <returns>True to retry.  False otherwise</returns>
		public bool Retry(string msg, string caption)
		{
			return m_synchronizeInvoke.Invoke(() => MessageBox.Show(msg, caption,
				MessageBoxButtons.RetryCancel, MessageBoxIcon.None) == DialogResult.Retry);
		}
	}
}
