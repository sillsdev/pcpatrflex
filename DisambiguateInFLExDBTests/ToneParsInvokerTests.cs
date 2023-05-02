﻿// Copyright (c) 2019 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.DisambiguateInFLExDB;
using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDBTests
{
    [TestFixture]
    class ToneParsInvokerTests : DisambiguateTests
    {
        String AnaExpectedString { get; set; }
        String AntExpectedString { get; set; }
        String LexExpectedString { get; set; }
        String ParserFilerXMLString { get; set; }
        String ToneParsBatchExpectedString { get; set; }
        String ToneParsCmdExpectedString { get; set; }
        String Word1ExpectedString { get; set; }
        String Word2ExpectedString { get; set; }
        String Word3ExpectedString { get; set; }
        String Word24ExpectedString { get; set; }
        String XAmpleBatchExpectedString { get; set; }
        String XAmpleCmdExpectedString { get; set; }
        ToneParsInvoker invoker { get; set; }
        const String kADCtlFile = "KuniToneParsTestadctl.txt";
        const String kGrammarFile = "KuniToneParsTestgram.txt";
        const String kLexiconFile = "KuniToneParsTestlex.txt";
        const String kLexiconTPFile = "KuniToneParsTestTPlex.txt";

        [TestFixtureSetUp]
        public override void FixtureSetup()
        {
            IcuInit();
            TestDirInit();
            TestFile = Path.Combine(TestDataDir, "KuniToneParsTest.fwdata");
            SavedTestFile = Path.Combine(TestDataDir, "KuniToneParsTestB4.fwdata");
            ToneParsInvokerOptions.Instance.ResetAllOptions();

            base.FixtureSetup();
        }

        /// <summary></summary>
        [TestFixtureTearDown]
        public override void FixtureTeardown()
        {
            base.FixtureTeardown();
        }

        /// <summary>
        /// Test conversion of log file using hvos to using glosses.
        /// </summary>
        [Test]
        public void ToneParsHvoToGlossInLogTest()
        {
            var logFileWithHvos = Path.Combine(TestDataDir, "ToneParsInvokerWithHvos.log");
            var logFileWithMorphnames = Path.Combine(
                TestDataDir,
                "ToneParsInvokerWithMorphnames.log"
            );
            var converter = new ToneParsLogConverter(myCache, logFileWithHvos);
            var expected = File.ReadAllText(logFileWithMorphnames);
            var result = converter.ConvertHvosToMorphnames();
            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// Test invoking of XAmple followed by TonePars.
        /// </summary>
        [Test]
        public void ToneParsInvokerTest()
        {
            File.Copy(
                Path.Combine(TestDataDir, kADCtlFile),
                Path.Combine(Path.GetTempPath(), kADCtlFile),
                true
            );
            File.Copy(
                Path.Combine(TestDataDir, kGrammarFile),
                Path.Combine(Path.GetTempPath(), kGrammarFile),
                true
            );
            File.Copy(
                Path.Combine(TestDataDir, kLexiconFile),
                Path.Combine(Path.GetTempPath(), kLexiconFile),
                true
            );
            String toneParsLexFile = Path.Combine(TestDataDir, "KvgTP.ctl");
            String toneParsRuleFile = Path.Combine(TestDataDir, "KvgTP.ctl");
            String intxCtlFile = Path.Combine(TestDataDir, "KVGintx.ctl");
            String inputFile = Path.Combine(TestDataDir, "KVGinput.txt");
            invoker = new ToneParsInvoker(toneParsRuleFile, intxCtlFile, inputFile, '+', myCache);
            CreateExpectedFileStrings();
            ToneParsInvokerOptions.Instance.VerifyInformation = true;
            invoker.Invoke();
            CompareResultToExpectedFile(XAmpleBatchExpectedString, invoker.XAmpleBatchFile);
            CompareResultToExpectedFile(XAmpleCmdExpectedString, invoker.XAmpleCmdFile);
            CompareResultToExpectedFile(ToneParsBatchExpectedString, invoker.ToneParsBatchFile);
            CompareResultToExpectedFile(ToneParsCmdExpectedString, invoker.ToneParsCmdFile);
            CompareResultToExpectedFile(AnaExpectedString, invoker.AnaFile);
            CompareResultToExpectedFile(AntExpectedString, invoker.AntFile);
            CompareResultToExpectedFile(
                LexExpectedString,
                Path.Combine(Path.GetTempPath(), kLexiconTPFile)
            );

            Boolean found = invoker.ConvertAntToParserFilerXML(0);
            Assert.AreEqual(false, found);
            found = invoker.ConvertAntToParserFilerXML(1);
            Assert.AreEqual(true, found);
            Assert.AreEqual(Word1ExpectedString, invoker.ParserFilerXMLString);
            found = invoker.ConvertAntToParserFilerXML(2);
            Assert.AreEqual(true, found);
            Assert.AreEqual(Word2ExpectedString, invoker.ParserFilerXMLString);
            found = invoker.ConvertAntToParserFilerXML(3);
            Assert.AreEqual(true, found);
            Assert.AreEqual(Word3ExpectedString, invoker.ParserFilerXMLString);
            // find last one and then look for one beyond it
            found = invoker.ConvertAntToParserFilerXML(24);
            Assert.AreEqual(true, found);
            Assert.AreEqual(Word24ExpectedString, invoker.ParserFilerXMLString);
            found = invoker.ConvertAntToParserFilerXML(25);
            Assert.AreEqual(false, found);

            var wf﻿Mbumbukiam = invoker.GetWordformFromString("Mbumbukiam");
            Assert.NotNull(wfMbumbukiam);
            Assert.AreEqual(0, wfMbumbukiam.ParserCount);
            var wffia = invoker.GetWordformFromString("fia");
            Assert.NotNull(wffia);
            Assert.AreEqual(0, wffia.ParserCount);
            var wfndot = invoker.GetWordformFromString("ndø-tá");
            Assert.NotNull(wfndot);
            Assert.AreEqual(0, wfndot.ParserCount);
            var wfndot2 = invoker.GetWordformFromString("ndǿ-ta");
            Assert.NotNull(wfndot2);
            Assert.AreEqual(0, wfndot2.ParserCount);

            invoker.SaveResultsInDatabase();
            Assert.AreEqual(1, wfMbumbukiam.ParserCount);
            Assert.AreEqual(1, wffia.ParserCount);
            Assert.AreEqual(3, wfndot.ParserCount);
            Assert.AreEqual(9, wfndot2.ParserCount);
        }

        private void CompareResultToExpectedFile(String expectedFileString, String actualFile)
        {
            String result = CreateFileString(actualFile);
            Assert.AreEqual(expectedFileString, result);
        }

        private void CreateExpectedFileStrings()
        {
            AnaExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.AnaFile))
            );
            AntExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.AntFile))
            );
            LexExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(kLexiconTPFile))
            );
            Word1ExpectedString = CreateFileString(Path.Combine(TestDataDir, "Word1Expected.xml"));
            Word2ExpectedString = CreateFileString(Path.Combine(TestDataDir, "Word2Expected.xml"));
            Word3ExpectedString = CreateFileString(Path.Combine(TestDataDir, "Word3Expected.xml"));
            Word24ExpectedString = CreateFileString(
                Path.Combine(TestDataDir, "Word24Expected.xml")
            );
            XAmpleBatchExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.XAmpleBatchFile))
            );
            XAmpleCmdExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.XAmpleCmdFile))
            );
            ToneParsBatchExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.ToneParsBatchFile))
            );
            ToneParsCmdExpectedString = CreateFileString(
                Path.Combine(TestDataDir, Path.GetFileName(invoker.ToneParsCmdFile))
            );
        }

        private String CreateFileString(String fileName)
        {
            String expectedFile = Path.Combine(TestDataDir, fileName);
            String result = "";
            using (var streamReader = new StreamReader(expectedFile, Encoding.UTF8))
            {
                result = streamReader.ReadToEnd().Replace("\r", "");
            }
            return result;
        }
    }
}
