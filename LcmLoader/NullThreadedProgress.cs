// Copyright (c) 2003-2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIL.LCModel.Utils;

namespace SIL.PrepFLExDB
{
	class NullThreadedProgress : IThreadedProgress
	{
		private readonly ISynchronizeInvoke m_synchronizeInvoke;

		public NullThreadedProgress(ISynchronizeInvoke synchronizeInvoke)
		{
			m_synchronizeInvoke = synchronizeInvoke;
		}

		public void Step(int amount)
		{
			Position += amount * StepSize;
		}

		public string Title { get; set; }

		public string Message { get; set; }

		public int Position { get; set; }

		public int StepSize { get; set; }

		public int Minimum { get; set; }

		public int Maximum { get; set; }

		public ISynchronizeInvoke SynchronizeInvoke
		{
			get { return m_synchronizeInvoke; }
		}

		public bool IsIndeterminate { get; set; }

		public bool AllowCancel { get; set; }

		public bool IsCanceling
		{
			get { return false; }
		}

		public event CancelEventHandler Canceling;

		public object RunTask(Func<IThreadedProgress, object[], object> backgroundTask, params object[] parameters)
		{
			return RunTask(true, backgroundTask, parameters);
		}

		public object RunTask(bool fDisplayUi, Func<IThreadedProgress, object[], object> backgroundTask, params object[] parameters)
		{
			return backgroundTask(this, parameters);
		}

		public bool Canceled { get; set; }
	}
}