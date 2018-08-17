// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.LcmLoader;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.DomainServices;
using SIL.WritingSystems;
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
	abstract class DisambiguateTests : MemoryOnlyBackendProviderTestBase
	{
		protected String TestDataDir { get; set; }
		protected String SavedTestFile { get; set; }
		protected String TestFile { get; set; }
		protected LcmCache myCache { get; set; }
		public SIL.LcmLoader.LcmLoader Loader { get; set; }
		public ProjectId ProjId { get; set; }

		public override void FixtureSetup()
		{
			Icu.InitIcuDataDir();
			if (!Sldr.IsInitialized)
			{
				Sldr.Initialize();
			}

			base.FixtureSetup();
			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			int i = rootdir.LastIndexOf("DisambiguateInFLExDBTests");
			String basedir = rootdir.Substring(0, i);
			TestDataDir = Path.Combine(basedir, "DisambiguateInFLExDBTests", "TestData");
			if (String.IsNullOrEmpty(TestFile))
				TestFile = Path.Combine(TestDataDir, "PCPATRTesting.fwdata");
			if (String.IsNullOrEmpty(SavedTestFile))
				SavedTestFile = Path.Combine(TestDataDir, "PCPATRTestingB4.fwdata");
			File.Copy(SavedTestFile, TestFile, true);
			ProjId = new ProjectId(TestFile);
			Loader = new SIL.LcmLoader.LcmLoader(ProjId);
			myCache = Loader.CreateCache();
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			base.FixtureTeardown();
			if (myCache != null)
			{
				ProjectLockingService.UnlockCurrentProject(myCache);
				File.Copy(SavedTestFile, TestFile, true);
			}
		}
	}
}
