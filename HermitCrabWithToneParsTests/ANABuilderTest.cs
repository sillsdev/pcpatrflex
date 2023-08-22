// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.DisambiguateInFLExDB;
using SIL.LCModel;
using SIL.ToneParsFLEx;
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
                Console.WriteLine(ana);
                string[] records = ana.Split(
                    new[] { "\\a " },
                    StringSplitOptions.RemoveEmptyEntries
                );
                Assert.AreEqual(3, records.Length);
                Assert.AreEqual("%26%", records[0].Substring(0, 4));
                // the results do not always occur in the same order, so we are just checking for some
                Assert.AreEqual(true, records[0].Contains("%< W 4183 > 9436 1892%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4183 > 9436 1257%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4183 > 9436 698%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4183 > 9436 9260%"));

                Assert.AreEqual("%13%", records[1].Substring(0, 4));
                Assert.AreEqual(true, records[1].Contains("%< W 7330 > 1892"));
                Assert.AreEqual(true, records[1].Contains("%< W 7330 > 6764%"));
                Assert.AreEqual(true, records[1].Contains("%< W 7330 > 9278%"));
                Assert.AreEqual(true, records[1].Contains("%< W 7330 >%"));

                Assert.AreEqual("%125%", records[2].Substring(0, 5));
                Assert.AreEqual(true, records[2].Contains("%2656 < W 3263 > 7588%"));
                Assert.AreEqual(true, records[2].Contains("%2656 < W 3263 > 6827%"));
                Assert.AreEqual(true, records[2].Contains("%7679 < W 3263 > 9105%"));
                Assert.AreEqual(true, records[2].Contains("%380 < W 3263 > 2088%"));

                Assert.AreEqual(7763, ana.Length);
            }
        }
    }
}
