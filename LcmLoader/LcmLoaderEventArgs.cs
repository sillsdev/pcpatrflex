// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;

namespace SIL.LcmLoader
{
	public delegate void LcmLoaderEventHandler(object sender, LcmLoaderEventArgs e);

	public class LcmLoaderEventArgs : EventArgs
	{
		private readonly string message;

		public LcmLoaderEventArgs(string message)
		{
			this.message = message;
		}

		public string Message => message;
	}
}
