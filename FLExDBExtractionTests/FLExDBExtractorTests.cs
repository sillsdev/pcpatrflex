// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LcmLoader;
using SIL.WritingSystems;
using System.Reflection;
using System.IO;
using SIL.FLExDBExtraction;
using SIL.LCModel.DomainServices;

namespace SIL.FLExDBExtractionTest
{
	[TestFixture]
	public class FLExDBExtractorTests : MemoryOnlyBackendProviderTestBase
	{
		string Lexicon { get; set; }

		LcmCache myCache { get; set; }

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
			int i = rootdir.LastIndexOf("FLExDBExtractionTests");
			String basedir = rootdir.Substring(0, i);
			String testdata = Path.Combine(basedir, "FLExDBExtractionTests", "TestData");
			String testfile = Path.Combine(testdata, "PCPATRTesting.fwdata");
			ProjId = new ProjectId(testfile);
			Loader = new SIL.LcmLoader.LcmLoader(ProjId);

			using (var streamReader = new StreamReader(Path.Combine(testdata, "Lexicon.lex"), Encoding.UTF8))
			{
				Lexicon = streamReader.ReadToEnd().Replace("\r", "");
			}
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			base.FixtureTeardown();
			if (myCache != null)
			{
				ProjectLockingService.UnlockCurrentProject(myCache);
			}
		}

		/// <summary>
		/// Test extractions.
		/// </summary>
		[Test]
		public void ExtractLexiconTest()
		{
			myCache = Loader.CreateCache();
			Assert.IsNotNull(myCache);
			Assert.AreEqual(ProjId.UiName, myCache.ProjectId.UiName);
			Assert.AreEqual(26, myCache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(323, myCache.LangProject.LexDbOA.Entries.Count());
			var extractor = new FLExDBExtractor(myCache);
			string lexicon = extractor.ExtractPcPatrLexicon();
			Assert.AreEqual(Lexicon, lexicon);
		}

	}
}
