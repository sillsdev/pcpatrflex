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
using SIL.DisambiguateInFLExDB;
using SIL.LCModel.DomainServices;

namespace SIL.DisambiguateInFLExDBTests
{
	[TestFixture]
	class FLExDBExtractorTests : DisambiguateTests
	{
		String Lexicon { get; set; }

		public override void FixtureSetup()
		{
			base.FixtureSetup();

			using (var streamReader = new StreamReader(Path.Combine(TestDataDir, "Lexicon.lex"), Encoding.UTF8))
			{
				Lexicon = streamReader.ReadToEnd().Replace("\r", "");
			}
		}

		/// <summary></summary>
		public override void FixtureTeardown()
		{
			base.FixtureTeardown();
		}

		/// <summary>
		/// Test extracting of lexicon.
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
			String lexicon = extractor.ExtractPcPatrLexicon();
			Assert.AreEqual(Lexicon, lexicon);
		}

		/// <summary>
		/// Test extracting of text segments in ANA format.
		/// </summary>
		[Test]
		public void ExtractTextSegmentAsANATest()
		{
			myCache = Loader.CreateCache();
			Assert.IsNotNull(myCache);
			Assert.AreEqual(ProjId.UiName, myCache.ProjectId.UiName);
			Assert.AreEqual(26, myCache.LangProject.AllPartsOfSpeech.Count);
			Assert.AreEqual(323, myCache.LangProject.LexDbOA.Entries.Count());
			Assert.AreEqual(4, myCache.LangProject.InterlinearTexts.Count);
			var extractor = new FLExDBExtractor(myCache);
			var text = myCache.LangProject.InterlinearTexts.Where(t => t.Title.BestAnalysisAlternative.Text == "Part 4").First();
			var paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(3);
			var segment = paragraph.SegmentsOS.First();
			String segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			String expectedANA = ExpectedSegmentAsANA("WeWantToGetMarriedAndBeHappy.ana");
			Assert.AreEqual(expectedANA, segmentAsANA);
			paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(7);
			segment = paragraph.SegmentsOS.First();
			segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			expectedANA = ExpectedSegmentAsANA("ItIsHardToPickUpTheDullBrokenGlass.ana");
			Assert.AreEqual(expectedANA, segmentAsANA);
		}

		private String ExpectedSegmentAsANA(String segmentFileName)
		{
			String result;
			using (var streamReader = new StreamReader(Path.Combine(TestDataDir, segmentFileName), Encoding.UTF8))
			{
				result = streamReader.ReadToEnd().Replace("\r", "");
			}
			return result;
		}
	}
}
