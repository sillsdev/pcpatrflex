// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.PcPatrBrowser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIL.PcPatrBrowserTest
{
    [TestFixture]
    class TreeTranSaverTest
    {
        String TestDataDir { get; set; }
        String AndFile { get; set; }
        int[] ParsesChosen { get; set; }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
            int i = rootdir.LastIndexOf("PcPatrBrowserDllTest");
            String basedir = rootdir.Substring(0, i);
            TestDataDir = Path.Combine(basedir, "PcPatrBrowserDllTest", "TestData");
        }

        [Test]
        public void CreateAnaOutputTest()
        {
            string grammarFile;
            AndFile = Path.Combine(TestDataDir, "InvokerMany.and");
            string antFile = Path.Combine(TestDataDir, "InvokerMany.xml");
            string antFileContents = File.ReadAllText(antFile, Encoding.UTF8).Replace("\r", "");
            PcPatrDocument doc = new PcPatrDocument(AndFile, out grammarFile);
            Assert.AreEqual(10, doc.NumberOfSentences);
            TreeTranSaver saver = new TreeTranSaver(doc);
            ParsesChosen = new int[doc.NumberOfSentences];
            ParsesChosen[0] = 1;
            ParsesChosen[1] = 1;
            ParsesChosen[2] = 1;
            ParsesChosen[3] = 1;
            ParsesChosen[4] = 1;
            ParsesChosen[5] = 1;
            ParsesChosen[6] = 1;
            ParsesChosen[7] = 1;
            ParsesChosen[8] = 1;
            ParsesChosen[9] = 1;
            string ana = saver.CreateANAFromParsesChosen(ParsesChosen);
            //Console.Out.WriteLine(ana);
            Assert.AreEqual(antFileContents, ana);
        }
    }
}
