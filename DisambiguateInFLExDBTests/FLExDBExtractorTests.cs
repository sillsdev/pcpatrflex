﻿// Copyright (c) 2018 SIL International
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
			IcuInit();
			TestDirInit();
			TestFile = Path.Combine(TestDataDir, "PCPATRTestingMultiMorphemic.fwdata");
			SavedTestFile = Path.Combine(TestDataDir, "PCPATRTestingMultiMorphemicB4.fwdata");

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
			Assert.AreEqual(328, myCache.LangProject.LexDbOA.Entries.Count());
			var extractor = new FLExDBExtractor(myCache);
			String lexicon = extractor.ExtractPcPatrLexicon();
			//Console.Write(lexicon);
			Assert.AreEqual(Lexicon, lexicon);
		}

		[Test]
		public void IsAttachedCliticTest()
		{
			myCache = Loader.CreateCache();
			var extractor = new FLExDBExtractor(myCache);
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphBoundRoot, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphBoundStem, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphCircumfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphDiscontiguousPhrase, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphInfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphInfixingInterfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphParticle, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphPhrase, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphPrefix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphPrefixingInterfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphRoot, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphSimulfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphStem, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphSuffix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphSuffixingInterfix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphSuprafix, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphClitic, 1));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphClitic, 2));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphEnclitic, 1));
			Assert.IsTrue(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphEnclitic, 2));
			Assert.IsFalse(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphProclitic, 1));
			Assert.IsTrue(extractor.IsAttachedClitic(MoMorphTypeTags.kguidMorphProclitic, 2));
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
			Assert.AreEqual(328, myCache.LangProject.LexDbOA.Entries.Count());
			Assert.AreEqual(5, myCache.LangProject.InterlinearTexts.Count);
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
			text = myCache.LangProject.InterlinearTexts.Where(t => t.Title.BestAnalysisAlternative.Text == "Mulit-morphemic").First();
			paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(0);
			segment = paragraph.SegmentsOS.First();
			segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			expectedANA = ExpectedSegmentAsANA("ISeeTwoTrees.ana");
			//Console.WriteLine("ana='" + segmentAsANA + "'");
			Assert.AreEqual(expectedANA, segmentAsANA);
			paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(1);
			segment = paragraph.SegmentsOS.First();
			segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			expectedANA = ExpectedSegmentAsANA("ISeeTheTreesColor.ana");
			//Console.WriteLine("ana='" + segmentAsANA + "'");
			Assert.AreEqual(expectedANA, segmentAsANA);
			paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(2);
			segment = paragraph.SegmentsOS.First();
			segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			expectedANA = ExpectedSegmentAsANA("ThePreturntablesAreBetterThanTheProturntables.ana");
			//Console.WriteLine("ana='" + segmentAsANA + "'");
			Assert.AreEqual(expectedANA, segmentAsANA);
			paragraph = (IStTxtPara)text.ParagraphsOS.ElementAt(3);
			segment = paragraph.SegmentsOS.First();
			segmentAsANA = extractor.ExtractTextSegmentAsANA(segment);
			expectedANA = ExpectedSegmentAsANA("SiPro.ana");
			//Console.WriteLine("ana='" + segmentAsANA + "'");
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
