// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NUnit.Framework;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.PrepFLExDB;
using SIL.WritingSystems;

namespace SIL.PrepFLExDBTests
{
	/// <summary>
	/// Tests related to LcmLoader.
	/// </summary>
	/// 
	[TestFixture]
	class LcmLoaderTests : MemoryOnlyBackendProviderTestBase
	{
		LcmLoader loader;
		ProjectId projId;

		public LcmLoader Loader { get => loader; set => loader = value; }

		public ProjectId ProjId { get => projId; set => projId = value; }

		public override void FixtureSetup()
		{
			Icu.InitIcuDataDir();
			Sldr.Initialize();

			base.FixtureSetup();

			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			int i = rootdir.LastIndexOf("PrepFLExDBTests");
			String basedir = rootdir.Substring(0, i);
			String testfile = Path.Combine(basedir, "PrepFLExDBTests", "TestData", "PCPATRTestingEmpty.fwdata");
			ProjId = new ProjectId(testfile);

			Label lbl = new Label();
			Loader = new LcmLoader(ProjId, lbl);
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			//Directory.Delete(m_projectsDirectory, true);
			base.FixtureTeardown();
		}

		/// <summary>
		/// Test we can make them in multiple ways and always get the same instance.
		/// </summary>
		[Test]
		public void CreateCacheTest()
		{
			LcmCache cache = Loader.CreateCache();
			Assert.IsNotNull(cache);
			Assert.AreEqual(ProjId.UiName, cache.ProjectId.UiName);
			Assert.AreEqual(5, cache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(0, cache.LangProject.LexDbOA.Entries.Count());
		}

	}
}
