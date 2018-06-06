// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.LCModel;
using SIL.LCModel.Core;
using SIL.LCModel.Core.Text;
using SIL.LCModel.DomainServices;
using SIL.LCModel.Utils;
using SIL.PrepFLExDB;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrepFLExDBTests
{
	/// <summary>
	/// Tests related to Preparer.
	/// </summary>
	/// 
	[TestFixture]
	class PreparerTests : MemoryOnlyBackendProviderTestBase
	{
		LcmCache cache;
		public LcmLoader Loader { get; set; }
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
			int i = rootdir.LastIndexOf("PrepFLExDBTests");
			String basedir = rootdir.Substring(0, i);
			String testfile = Path.Combine(basedir, "PrepFLExDBTests", "TestData", "PCPATRTestingEmpty.fwdata");
			ProjId = new ProjectId(testfile);
			Loader = new LcmLoader(ProjId, new Label());
			cache = Loader.CreateCache();
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			//Directory.Delete(m_projectsDirectory, true);
			base.FixtureTeardown();
			cache.Dispose();
		}

		/// <summary>
		/// Test we get the expected results for the preparer service.
		/// </summary>
		[Test]
		public void PreparerTest()
		{
			Assert.IsNotNull(cache);
			Assert.AreEqual(5, cache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(0, cache.LangProject.LexDbOA.Entries.Count());
			var possListRepository = cache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			Assert.AreEqual(34, possListRepository.AllInstances().Count());
			var preparer = new Preparer(cache);
			preparer.AddPCPATRList();
			CheckPossibilityList(possListRepository);
			// invoke it again.  Only one copy should exist.
			preparer.AddPCPATRList();
			CheckPossibilityList(possListRepository);
		}

		private void CheckPossibilityList(ICmPossibilityListRepository possListRepository)
		{
			Assert.AreEqual(35, possListRepository.AllInstances().Count());
			var pcPatrList = possListRepository.AllInstances().Last();
			Assert.AreEqual(Constants.PcPatrFeatureDescriptorList, pcPatrList.Name.BestAnalysisAlternative.Text);
			Assert.AreEqual(665, pcPatrList.PossibilitiesOS.Count);
			CheckMatch(pcPatrList, "+root"); // first
			CheckMatch(pcPatrList, "causative_syntax"); // early middle
			CheckMatch(pcPatrList, "indefinite"); // late middle
			CheckMatch(pcPatrList, "witness"); // last
		}

		private void CheckMatch(ICmPossibilityList last, string sToMatch)
		{
			Assert.AreEqual(sToMatch, last.FindPossibilityByName(last.PossibilitiesOS, sToMatch, Cache.DefaultAnalWs).Name.BestAnalysisAlternative.Text);
		}
	}
}
