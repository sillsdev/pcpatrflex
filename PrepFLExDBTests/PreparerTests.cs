// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.LcmLoader;
using SIL.LcmLoaderUI;
using SIL.LCModel;
using SIL.LCModel.Core.Cellar;
using SIL.LCModel.Core.Text;
using SIL.LCModel.DomainServices;
using SIL.PrepFLExDB;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SIL.PrepFLExDBTests
{
	/// <summary>
	/// Tests related to Preparer.
	/// </summary>
	/// 
	[TestFixture]
	class PreparerTests : MemoryOnlyBackendProviderTestBase
	{
		LcmCache MyCache { get; set; }
		public SIL.LcmLoader.LcmLoader Loader { get; set; }
		public ProjectId ProjId { get; set; }
		private List<FieldDescription> customFields;

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
			Loader = new SIL.LcmLoader.LcmLoader(ProjId);
			MyCache = Loader.CreateCache();
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			//Directory.Delete(m_projectsDirectory, true);
			base.FixtureTeardown();
			MyCache.Dispose();
		}

		/// <summary>
		/// Test we get the expected results for the preparer service.
		/// </summary>
		[Test]
		public void PreparerTest()
		{
			Assert.IsNotNull(MyCache);
			Assert.AreEqual(5, MyCache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(0, MyCache.LangProject.LexDbOA.Entries.Count());
			var preparer = new Preparer(MyCache, false);

			// If we try to create the custom field before the master possibility list, the field is not created.
			customFields = preparer.GetListOfCustomFields();
			Assert.AreEqual(0, customFields.Count);
			preparer.AddPCPATRSenseCustomField();
			customFields = preparer.GetListOfCustomFields();
			Assert.AreEqual(0, customFields.Count);

			var possListRepository = MyCache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			Assert.AreEqual(34, possListRepository.AllInstances().Count());
			preparer.AddPCPATRList();
			CheckPossibilityList(possListRepository);
			// Invoke it again.  Only one copy should exist.
			preparer.AddPCPATRList();
			CheckPossibilityList(possListRepository);

			// Add custom field
			customFields = preparer.GetListOfCustomFields();
			Assert.AreEqual(0, customFields.Count);
			preparer.AddPCPATRSenseCustomField();
			customFields = preparer.GetListOfCustomFields();
			Assert.AreEqual(1, customFields.Count);
			var cf = customFields.Find(fd => fd.Name == Constants.PcPatrFeatureDescriptorCustomField);
			Assert.IsNotNull(cf);
			Assert.IsTrue(cf.IsCustomField);
			Assert.AreEqual(Constants.PcPatrFeatureDescriptorCustomField, cf.Name);
			Assert.AreEqual(Constants.PcPatrFeatureDescriptorCustomField, cf.Userlabel);
			Assert.AreEqual(CellarPropertyType.ReferenceCollection, cf.Type);
			Assert.AreEqual(LexSenseTags.kClassId, cf.Class);
			Assert.AreEqual(CmCustomItemTags.kClassId, cf.DstCls);
			Assert.AreEqual(WritingSystemServices.kwsAnal, cf.WsSelector);
			// Invoke it again.  Only one should exist.
			preparer.AddPCPATRSenseCustomField();
			customFields = preparer.GetListOfCustomFields();
			Assert.AreEqual(1, customFields.Count);
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
			Assert.AreEqual(sToMatch, last.FindPossibilityByName(last.PossibilitiesOS, sToMatch, base.Cache.DefaultAnalWs).Name.BestAnalysisAlternative.Text);
		}
	}
}
