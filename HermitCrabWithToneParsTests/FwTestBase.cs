// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.LCModel;
using SIL.LCModel.Core.WritingSystems;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SIL.HermitCrabWithToneParsTests
{
    public class FwTestBase : MemoryOnlyBackendProviderTestBase
    {
        protected string TestDataDir { get; set; }
        protected string FieldWorksTestFile { get; set; }
        protected LcmCache myCache { get; set; }

        // Following three needed to get cache
        protected ILcmUI m_ui;
        protected string m_projectsDirectory;
        protected ILcmDirectories m_lcmDirectories;

        public override void FixtureSetup()
        {
            if (!Sldr.IsInitialized)
                Sldr.Initialize();
            base.FixtureSetup();
            m_projectsDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(m_projectsDirectory);
            m_ui = new DummyLcmUI();
            m_lcmDirectories = new TestLcmDirectories(m_projectsDirectory);
        }

        [SetUp]
        virtual public void Setup()
        {
            Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
            int i = rootdir.LastIndexOf("HermitCrabWithToneParsTests");
            string basedir = rootdir.Substring(0, i);
            TestDataDir = Path.Combine(basedir, "HermitCrabWithToneParsTests", "TestData");
            FieldWorksTestFile = Path.Combine(TestDataDir, "KuniToneParsTest.fwdata");

            var projectId = new TestProjectId(BackendProviderType.kXML, FieldWorksTestFile);
            // following came from LcmCacheTests.cs
            myCache = LcmCache.CreateCacheFromExistingData(
                projectId,
                "en",
                m_ui,
                m_lcmDirectories,
                new LcmSettings(),
                new DummyProgressDlg()
            );
        }
    }
}
