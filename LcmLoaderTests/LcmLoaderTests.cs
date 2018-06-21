// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LcmLoader;
using SIL.WritingSystems;

namespace SIL.LcmLoaderTests
{
	/// <summary>
	/// Tests related to LcmLoader.
	/// </summary>
	/// 
	[TestFixture]
	class LcmLoaderTests : MemoryOnlyBackendProviderTestBase
	{
		public SIL.LcmLoader.LcmLoader Loader { get; set; }

		public ProjectId ProjId { get; set; }

		int step = 1;

		public override void FixtureSetup()
		{
			Icu.InitIcuDataDir();
			if (!Sldr.IsInitialized)
			{
				Sldr.Initialize();
			}

			base.FixtureSetup();
		}

		private void SetUpTestFile(string testingFile)
		{
			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			int i = rootdir.LastIndexOf("LcmLoaderTests");
			String basedir = rootdir.Substring(0, i);
			String testfile = Path.Combine(basedir, "LcmLoaderTests", "TestData", testingFile);
			ProjId = new ProjectId(testfile);
			Loader = new SIL.LcmLoader.LcmLoader(ProjId);
			Loader.RaiseLcmLoaderEvent += HandleLcmLoaderEvent;
		}

		void HandleLcmLoaderEvent(object sender, LcmLoaderEventArgs e)
		{
			Console.WriteLine(" received this message: {0}", e.Message);
			switch (step)
			{
				case 1:
					Assert.AreEqual(LcmLoader.LcmLoader.kLoading, e.Message);
					step = 2;
					break;
				case 2:
					Assert.AreEqual(LcmLoader.LcmLoader.kCompleted, e.Message);
					break;
				case 3:
					Assert.AreEqual(LcmLoader.LcmLoader.kLoading, e.Message);
					step = 4;
					break;
				case 4:
					string result = LcmLoader.LcmLoader.kFailed + "\n" + LcmLoader.LcmLoader.kProjectOlder + "\n" + LcmLoader.LcmLoader.kProjectMigrate;
					Assert.AreEqual(result, e.Message);
					break;
			}
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			//Directory.Delete(m_projectsDirectory, true);
			base.FixtureTeardown();
		}

		/// <summary>
		/// Test we get the expected results.
		/// </summary>
		[Test]
		public void CreateCacheTest()
		{
			SetUpTestFile("PCPATRTestingEmpty.fwdata");
			step = 1;
			LcmCache cache = Loader.CreateCache();
			Assert.IsNotNull(cache);
			Assert.AreEqual(ProjId.UiName, cache.ProjectId.UiName);
			Assert.AreEqual(5, cache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(0, cache.LangProject.LexDbOA.Entries.Count());
		}

		/// <summary>
		/// Test we get the expected results.
		/// </summary>
		[Test]
		public void CreateOldProjectTest()
		{
			SetUpTestFile("PCPATRTestingEmptyOld.fwdata");
			step = 3;
			LcmCache cache = Loader.CreateCache();
			Assert.IsNull(cache);
		}
	}
}
