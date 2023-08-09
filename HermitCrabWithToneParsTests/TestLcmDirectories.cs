﻿// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.HermitCrabWithToneParsTests
{
    // Following taken from LCM test code (TestLcmDiectories.cs)
    internal class TestLcmDirectories : ILcmDirectories
    {
        public TestLcmDirectories(string projectsDirectory)
        {
            ProjectsDirectory = projectsDirectory;
        }

        public string ProjectsDirectory { get; }

        public string TemplateDirectory => TestDirectoryFinder.TemplateDirectory;
    }
}
