// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.DisambiguateInFLExDB;
using SIL.HermitCrabWithTonePars;
using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIL.HermitCrabWithToneParsTests
{
    [TestFixture]
    public class ANABuilderTest : FwTestBase
    {
        [Test]
        public void ExtractTextSegmentAndParseWordAsANATest()
        {
            Assert.NotNull(myCache);
            var extractor = new FLExDBExtractor(myCache);
            var changer = new OrthoChanger();
            string inputOrthoFile = Path.Combine(TestDataDir, "KVGintx.ctl");
            changer.LoadOrthoChangesFile(inputOrthoFile);
            Assert.AreEqual(true, changer.ChangesExist);
            changer.CreateOrthoChanges();

            var anaBuilder = new ANABuilder(myCache, extractor, changer);
            var texts = myCache.LangProject.Texts;
            Assert.AreEqual(3, texts.Count());
            var text1 = texts.ElementAtOrDefault(2);
            Assert.NotNull(text1);
            var textNameMU = text1.Name;
            string textName = textNameMU.BestAnalysisVernacularAlternative.Text;
            Assert.AreEqual("Roland's sample, one line", textName);
            var stText = text1.ContentsOA;
            var stPara = stText.ParagraphsOS.ElementAtOrDefault(0) as IStTxtPara;
            Assert.NotNull(stPara);
            Assert.AreEqual(1, stPara.SegmentsOS.Count);
            foreach (ISegment segment in stPara.SegmentsOS)
            {
                string ana = anaBuilder.ExtractTextSegmentAndParseWordAsANA(segment);
                string[] records = ana.Split(
                    new[] { "\\a " },
                    StringSplitOptions.RemoveEmptyEntries
                );
                Assert.AreEqual(3, records.Length);
                Assert.AreEqual("%26%", records[0].Substring(0, 4));
                // the results do not always occur in the same order, so we are just checking for some
                Assert.AreEqual(true, records[0].Contains("%< n God > SGᴺ «X'NP'H»%"));
                Assert.AreEqual(true, records[0].Contains("%< n God > SGᴺ «X'NP'L»%"));
                Assert.AreEqual(true, records[0].Contains("%< n God > SGᴺ «ATTDR'NV'Q»%"));
                Assert.AreEqual(true, records[0].Contains("%< n God > SGᴺ «X'POSS'H»%"));

                Assert.AreEqual("%13%", records[1].Substring(0, 4));
                Assert.AreEqual(true, records[1].Contains("%< n value > «X'NP'H»%"));
                Assert.AreEqual(true, records[1].Contains("%< n value > «EQ'EO'D»%"));
                Assert.AreEqual(true, records[1].Contains("%< n value > «CMPL\"EQ'D»%"));
                Assert.AreEqual(true, records[1].Contains("%< n value >%"));

                Assert.AreEqual("%125%", records[2].Substring(0, 5));
                Assert.AreEqual(
                    true,
                    records[2].Contains("%S.B&L.GEN.3.FN < v cook > «INT'FN'Q»%")
                );
                Assert.AreEqual(
                    true,
                    records[2].Contains("%S.B&L.GEN.3.FN < v cook > «NINT'FV'Q»%")
                );
                Assert.AreEqual(
                    true,
                    records[2].Contains("%S.B&L.GEN.IMP.2.FN < v cook > «INT'EO'D»%")
                );
                Assert.AreEqual(
                    true,
                    records[2].Contains("%S.B&L.DX'F.3SGN < v cook > «NINT'EO'Q»%")
                );

                Assert.AreEqual(10432, ana.Length);
            }
        }
    }
}
