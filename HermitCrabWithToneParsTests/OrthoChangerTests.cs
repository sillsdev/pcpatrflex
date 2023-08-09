// Copyright (c) 2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using NUnit.Framework;
using SIL.HermitCrabWithTonePars;
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
    public class OrthoChangerTests : HermitCrabWithToneParsTestBase
    {
        [Test]
        public void LoadOrthoChangesFileTest()
        {
            changer.LoadOrthoChangesFile("");
            Assert.AreEqual(false, changer.ChangesExist);

            InputOrthoFile = Path.Combine(TestDataDir, "KVGintx.ctl");
            changer.LoadOrthoChangesFile(InputOrthoFile);
            Assert.AreEqual(true, changer.ChangesExist);
        }

        [Test]
        public void CreateOrthoChangesTest()
        {
            string pathName;
            List<OrthoChangeMapping> mappings;
            changer.LoadOrthoChangesFile(InputOrthoFile);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(6, mappings.Count);
            CheckMapping(mappings, 0, "́", ""); // high tone
            CheckMapping(mappings, 1, "᷄", ""); // rising tone
            CheckMapping(mappings, 2, "̄", ""); // middle tone
            CheckMapping(mappings, 3, "̀", ""); // low tone
            CheckMapping(mappings, 4, "̂", ""); // falling tone
            CheckMapping(mappings, 5, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "ChangeAtFront.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(7, mappings.Count);
            CheckMapping(mappings, 0, "this", "that");
            CheckMapping(mappings, 1, "́", ""); // high tone
            CheckMapping(mappings, 2, "᷄", ""); // rising tone
            CheckMapping(mappings, 3, "̄", ""); // middle tone
            CheckMapping(mappings, 4, "̀", ""); // low tone
            CheckMapping(mappings, 5, "̂", ""); // falling tone
            CheckMapping(mappings, 6, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "CommentChangeAtFront.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(6, mappings.Count);
            CheckMapping(mappings, 0, "́", ""); // high tone
            CheckMapping(mappings, 1, "᷄", ""); // rising tone
            CheckMapping(mappings, 2, "̄", ""); // middle tone
            CheckMapping(mappings, 3, "̀", ""); // low tone
            CheckMapping(mappings, 4, "̂", ""); // falling tone
            CheckMapping(mappings, 5, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "CommentFirstChange.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(5, mappings.Count);
            //CheckMapping(mappings, 0, "́", "");  // high tone
            CheckMapping(mappings, 0, "᷄", ""); // rising tone
            CheckMapping(mappings, 1, "̄", ""); // middle tone
            CheckMapping(mappings, 2, "̀", ""); // low tone
            CheckMapping(mappings, 3, "̂", ""); // falling tone
            CheckMapping(mappings, 4, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "ChangeAtFront.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(7, mappings.Count);
            CheckMapping(mappings, 0, "this", "that");
            CheckMapping(mappings, 1, "́", ""); // high tone
            CheckMapping(mappings, 2, "᷄", ""); // rising tone
            CheckMapping(mappings, 3, "̄", ""); // middle tone
            CheckMapping(mappings, 4, "̀", ""); // low tone
            CheckMapping(mappings, 5, "̂", ""); // falling tone
            CheckMapping(mappings, 6, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "TwoChangesInARow.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(6, mappings.Count);
            CheckMapping(mappings, 0, "́", ""); // high tone
            CheckMapping(mappings, 1, "᷄", ""); // rising tone
            CheckMapping(mappings, 2, "̄", ""); // middle tone
            CheckMapping(mappings, 3, "̀", ""); // low tone
            CheckMapping(mappings, 4, "̂", ""); // falling tone
            CheckMapping(mappings, 5, "-", ""); // hyphen

            pathName = Path.Combine(TestDataDir, "NoChanges.ctl");
            changer.LoadOrthoChangesFile(pathName);
            mappings = changer.CreateOrthoChanges();
            Assert.NotNull(mappings);
            Assert.AreEqual(0, mappings.Count);
        }

        private void CheckMapping(
            List<OrthoChangeMapping> mappings,
            int index,
            string from,
            string to
        )
        {
            Assert.IsTrue(index < mappings.Count);
            OrthoChangeMapping mapping = mappings.ElementAt(index);
            Assert.AreEqual(mapping.From, from);
            Assert.AreEqual(mapping.To, to);
        }

        [Test]
        public void FindFirstChIndexTest()
        {
            CheckFirstChangeIndex("ChangeAtFront.ctl", 0);
            CheckFirstChangeIndex("CommentChangeAtFront.ctl", 1216);
            CheckFirstChangeIndex("KVGintx.ctl", 1196);
            CheckFirstChangeIndex("CommentFirstChange.ctl", 1233);
        }

        private void CheckFirstChangeIndex(string fileName, int expectedIndex)
        {
            string pathName = Path.Combine(TestDataDir, fileName);
            changer.LoadOrthoChangesFile(pathName);
            int chIndex = changer.FindFirstChIndex(changer.OrthoFileContents);
            Assert.AreEqual(expectedIndex, chIndex);
        }

        [Test]
        public void CreateOrthoChangeMappingTest()
        {
            // normal
            CheckOrthoChangeMapping("\\ch \"this\" \"that\"", "this", "that");
            CheckOrthoChangeMapping("\\ch \"this\" \"that\"\n", "this", "that");
            CheckOrthoChangeMapping("\n\\ch \"this\" \"that\"\n", "this", "that");
            CheckOrthoChangeMapping("\\ch \"this\"\"that\"", "this", "that");
            // comments
            CheckOrthoChangeMapping(
                "\\ch \"this\" | other stuff \"here\" and \"there\'\n\"that\"\n",
                "this",
                "that"
            );
            CheckOrthoChangeMapping(
                "|\\ch \"this\"\n | other stuff \"here\" and \"there\"\n\\ch \"hi\" \"there\"",
                "hi",
                "there"
            );
            // deletion
            CheckOrthoChangeMapping("\\ch \"X\" \"\" ", "X", "");
            // single quotes instead of double quotes
            CheckOrthoChangeMapping("\\ch 'this' \"that\"", "this", "that");
            CheckOrthoChangeMapping("\\ch \"this\" 'that'", "this", "that");
            CheckOrthoChangeMapping("\\ch 'this' 'that'", "this", "that");
            CheckOrthoChangeMapping("\\ch 'X' \"\" ", "X", "");
            CheckOrthoChangeMapping("\\ch \"X\" '' ", "X", "");
            CheckOrthoChangeMapping("\\ch 'X' ''", "X", "");
            CheckOrthoChangeMapping("\\ch 'X'''", "X", "");
        }

        private void CheckOrthoChangeMapping(string orthoChange, string sFrom, string sTo)
        {
            int finalIndex = -99;
            OrthoChangeMapping mapping = changer.CreateOrthoChangeMapping(
                orthoChange,
                0,
                out finalIndex
            );
            Assert.False(finalIndex == -1);
            Assert.AreEqual(sFrom, mapping.From);
            Assert.AreEqual(sTo, mapping.To);
        }
    }
}
