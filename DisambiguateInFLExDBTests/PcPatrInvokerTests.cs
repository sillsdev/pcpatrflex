// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.DisambiguateInFLExDB;
using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDBTests
{
	[TestFixture]
	class PcPatrInvokerTests
	{
		String TestDataDir { get; set; }
		String AnaString { get; set; }
		String AndString { get; set; }

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			int i = rootdir.LastIndexOf("DisambiguateInFLExDBTests");
			String basedir = rootdir.Substring(0, i);
			TestDataDir = Path.Combine(basedir, "DisambiguateInFLExDBTests", "TestData");
		}

		/// <summary></summary>
		[TestFixtureTearDown]
		public void FixtureTeardown()
		{
		}

		/// <summary>
		/// Test extracting of lexicon.
		/// </summary>
		[Test]
		public void PcPatrInvokerTest()
		{
			String grammarFile = Path.Combine(TestDataDir, "Invoker.grm");
			String anaFile = Path.Combine(TestDataDir, "Invoker.ana");
			String andFile = Path.Combine(TestDataDir, "InvokerB4.and");
			using (var streamReader = new StreamReader(andFile, Encoding.UTF8))
			{
				AndString = streamReader.ReadToEnd().Replace("\r", "");
			}
			var invoker = new PCPatrInvoker(grammarFile, anaFile);
			invoker.Invoke();
			String andResult = "";
			using (var streamReader = new StreamReader(invoker.AndFile, Encoding.UTF8))
			{
				andResult = streamReader.ReadToEnd().Replace("\r", "");
			}
			// The \id line has the location of the Invoker.grm file which will vary by machine.
			// So we just check the firset 23 characters (which are always the same)
			// and what starts at "Invoker.grm".
			int iIDBeginning = 23;
			int iExpected = AndString.IndexOf("Invoker.grm");
			int iResult = andResult.IndexOf("Invoker.grm");
			Assert.AreEqual(AndString.Substring(0, iIDBeginning), andResult.Substring(0, iIDBeginning));
			Assert.AreEqual(AndString.Substring(iExpected), andResult.Substring(iResult));
		}
	}
}
