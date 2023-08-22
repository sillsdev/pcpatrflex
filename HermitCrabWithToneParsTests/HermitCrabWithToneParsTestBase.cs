// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.DisambiguateInFLExDB;
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
    public abstract class HermitCrabWithToneParsTestBase
    {
        protected string TestDataDir { get; set; }
        protected string InputOrthoFile { get; set; }
        protected OrthoChanger changer;
        protected FLExDBExtractor extractor;

        [SetUp]
        public void Setup()
        {
            Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
            int i = rootdir.LastIndexOf("HermitCrabWithToneParsTests");
            String basedir = rootdir.Substring(0, i);
            TestDataDir = Path.Combine(basedir, "HermitCrabWithToneParsTests", "TestData");
            InputOrthoFile = Path.Combine(TestDataDir, "KVGintx.ctl");
            changer = new OrthoChanger();
        }
    }
}
