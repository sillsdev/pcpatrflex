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
            Assert.AreEqual(10, stText.ParagraphsOS.Count);
            var stPara = stText.ParagraphsOS.ElementAtOrDefault(0) as IStTxtPara;
            Assert.NotNull(stPara);
            Assert.AreEqual(1, stPara.SegmentsOS.Count);
            foreach (ISegment segment in stPara.SegmentsOS)
            {
                string ana = anaBuilder.ExtractTextSegmentAndParseWordAsANA(segment);
                //Console.WriteLine(ana);
                string[] records = ana.Split(
                    new[] { "\\a " },
                    StringSplitOptions.RemoveEmptyEntries
                );
                Assert.AreEqual(3, records.Length);
                Assert.AreEqual("%26%", records[0].Substring(0, 4));
                // the results do not always occur in the same order, so we are just checking for some
                Assert.AreEqual(true, records[0].Contains("%< W 4310 > 9713 10957%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4310 > 9713 9553%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4310 > 9713 3%"));
                Assert.AreEqual(true, records[0].Contains("%< W 4310 > 9713 5625%"));

                Assert.AreEqual("%13%", records[1].Substring(0, 4));
                Assert.AreEqual(true, records[1].Contains("%< W 7548 > 5284%"));
                Assert.AreEqual(true, records[1].Contains("%< W 7548 > 9553%"));
                Assert.AreEqual(true, records[1].Contains("%< W 7548 > 3%"));
                Assert.AreEqual(true, records[1].Contains("%< W 7548 >%"));

                Assert.AreEqual("%125%", records[2].Substring(0, 5));
                Assert.AreEqual(true, records[2].Contains("%7905 < W 3345 > 3651%"));
                Assert.AreEqual(true, records[2].Contains("%7905 < W 3345 > 308%"));
                Assert.AreEqual(true, records[2].Contains("%383 < W 3345 > 528%"));
                Assert.AreEqual(true, records[2].Contains("%383 < W 3345 > 5695%"));

                Assert.AreEqual(7264, ana.Length);
            }

            stPara = stText.ParagraphsOS.ElementAtOrDefault(8) as IStTxtPara;
            Assert.NotNull(stPara);
            Assert.AreEqual(1, stPara.SegmentsOS.Count);
            foreach (ISegment segment in stPara.SegmentsOS)
            {
                string ana = anaBuilder.ExtractTextSegmentAndParseWordAsANA(segment);
                //Console.WriteLine(ana);
                string[] records = ana.Split(
                    new[] { "\\a " },
                    StringSplitOptions.RemoveEmptyEntries
                );
                Assert.AreEqual(2, records.Length);
                Assert.AreEqual("%25%", records[0].Substring(0, 4));
                // the results do not always occur in the same order, so we are just checking for some
                Assert.AreEqual(true, records[0].Contains("%10918 8145 < W 1711 > 7227%"));
                Assert.AreEqual(true, records[0].Contains("%10918 8145 < W 1711 > 859%"));
                Assert.AreEqual(true, records[0].Contains("%10918 8145 < W 1711 > 4880%"));
                Assert.AreEqual(true, records[0].Contains("%10918 8145 < W 1711 > 308%"));

                Assert.AreEqual("%25%", records[1].Substring(0, 4));
                Assert.AreEqual(true, records[1].Contains("%10918 8145 < W 1711 > 6570%"));
                Assert.AreEqual(true, records[1].Contains("%10918 8145 < W 1711 > 5205%"));
                Assert.AreEqual(true, records[1].Contains("%10918 8145 < W 1711 > 7028%"));
                Assert.AreEqual(true, records[1].Contains("%10918 8145 < W 1711 > 65%"));

                records = ana.Split(new[] { "\\p " }, StringSplitOptions.RemoveEmptyEntries);
                Assert.AreEqual(true, records[1].Contains("Ac2"));
                Assert.AreEqual(false, records[1].Contains("sampleToneParsAllomorphProperty"));
                Assert.AreEqual(true, records[2].Contains("Ac2"));
                Assert.AreEqual(true, records[2].Contains("sampleToneParsAllomorphProperty"));

                Assert.AreEqual(3686, ana.Length);
            }
        }
    }
}
