﻿// Copyright (c) 2003-2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepFLExDB
{
	class ConsoleLogger : ILcmUI
	{
		private readonly ISynchronizeInvoke m_synchronizeInvoke;

		public ConsoleLogger(ISynchronizeInvoke synchronizeInvoke)
		{
			m_synchronizeInvoke = synchronizeInvoke;
		}

		public ISynchronizeInvoke SynchronizeInvoke
		{
			get { return m_synchronizeInvoke; }
		}

		public bool ConflictingSave()
		{
			throw new NotImplementedException();
		}

		public DateTime LastActivityTime
		{
			get { return DateTime.Now; }
		}

		public FileSelection ChooseFilesToUse()
		{
			throw new NotImplementedException();
		}

		public bool RestoreLinkedFilesInProjectFolder()
		{
			throw new NotImplementedException();
		}

		public YesNoCancel CannotRestoreLinkedFilesToOriginalLocation()
		{
			throw new NotImplementedException();
		}

		public void DisplayMessage(MessageType type, string message, string caption, string helpTopic)
		{
			Console.WriteLine(message);
		}

		public void ReportException(Exception error, bool isLethal)
		{
			Console.WriteLine(error.Message);
		}

		public void ReportDuplicateGuids(string errorText)
		{
			Console.WriteLine(errorText);
		}

		public void DisplayCircularRefBreakerReport(string msg, string caption)
		{
			Console.WriteLine("{0}: {1}", caption, msg);
		}

		public bool Retry(string msg, string caption)
		{
			throw new NotImplementedException();
		}

		public bool OfferToRestore(string projectPath, string backupPath)
		{
			throw new NotImplementedException();
		}
	}
}
