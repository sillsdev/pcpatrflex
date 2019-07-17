// Copyright (c) 2019 SIL International
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
		String ToneParsBatchExpectedString { get; set; }
		String ToneParsCmdExpectedString { get; set; }
		String XAmpleBatchExpectedString { get; set; }
		String XAmpleCmdExpectedString { get; set; }
		ToneParsInvoker invoker { get; set; }
		const String kADCtlFile = "KuniToneParsTestadctl.txt";
		const String kGrammarFile = "KuniToneParsTestgram.txt";
		const String kLexiconFile = "KuniToneParsTestlex.txt";

		[TestFixtureSetUp]
		public override void FixtureSetup()
		{
			IcuInit();
			TestDirInit();
			TestFile = Path.Combine(TestDataDir, "KuniToneParsTest.fwdata");
			SavedTestFile = Path.Combine(TestDataDir, "KuniToneParsTestB4.fwdata");

			base.FixtureSetup();
	}

	/// <summary></summary>
	[TestFixtureTearDown]
		public override void FixtureTeardown()
		{
			base.FixtureTeardown();
		}

		/// <summary>
		/// Test extracting of lexicon.
		/// </summary>
		[Test]
		public void ToneParsInvokerTest()
		{
			File.Copy(Path.Combine(TestDataDir, kADCtlFile), Path.Combine(Path.GetTempPath(), kADCtlFile), true);
			File.Copy(Path.Combine(TestDataDir, kGrammarFile), Path.Combine(Path.GetTempPath(), kGrammarFile), true);
			File.Copy(Path.Combine(TestDataDir, kLexiconFile), Path.Combine(Path.GetTempPath(), kLexiconFile), true);
			String toneParsRuleFile = Path.Combine(TestDataDir, "KvgTP.ctl");
			String intxCtlFile = Path.Combine(TestDataDir, "KVGintx.ctl");
			String inputFile = Path.Combine(TestDataDir, "KVGinput.txt");
			invoker = new ToneParsInvoker(toneParsRuleFile, intxCtlFile, inputFile, "", myCache);
			CreateExpectedFileStrings();
			invoker.Invoke();
			CompareResultToExpectedFile(XAmpleBatchExpectedString, invoker.XAmpleBatchFile);
			CompareResultToExpectedFile(XAmpleCmdExpectedString, invoker.XAmpleCmdFile);
			CompareResultToExpectedFile(ToneParsBatchExpectedString, invoker.ToneParsBatchFile);
			CompareResultToExpectedFile(ToneParsCmdExpectedString, invoker.ToneParsCmdFile);
			CompareResultToExpectedFile(AnaExpectedString, invoker.AnaFile);
			CompareResultToExpectedFile(AntExpectedString, invoker.AntFile);

			//checkRootGlossState(invoker, null);
			//checkRootGlossState(invoker, "off");
			//checkRootGlossState(invoker, "leftheaded");
			//checkRootGlossState(invoker, "rightheaded");
			//checkRootGlossState(invoker, "all");
			//checkRootGlossStateValue(invoker, null, null);
			//checkRootGlossStateValue(invoker, "Off", "off");
			//checkRootGlossStateValue(invoker, "Leftheaded", "leftheaded");
			//checkRootGlossStateValue(invoker, "Rightheaded", "rightheaded");
			//checkRootGlossStateValue(invoker, "All", "all");
			//checkRootGlossStateValue(invoker, "Of course", "off");
			//checkRootGlossStateValue(invoker, "Luis", "leftheaded");
			//checkRootGlossStateValue(invoker, "Rival", "rightheaded");
			//checkRootGlossStateValue(invoker, "Alone", "all");
		}

		private void CompareResultToExpectedFile(String expectedFileString, String actualFile)
		{
			String result = CreateFileString(actualFile);
			Assert.AreEqual(expectedFileString, result);
		}

		private void CreateExpectedFileStrings()
		{
			AnaExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.AnaFile)));
			AntExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.AntFile)));
			XAmpleBatchExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.XAmpleBatchFile)));
			XAmpleCmdExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.XAmpleCmdFile)));
			ToneParsBatchExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.ToneParsBatchFile)));
			ToneParsCmdExpectedString = CreateFileString(Path.Combine(TestDataDir, Path.GetFileName(invoker.ToneParsCmdFile)));
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

		//private void checkRootGlossState(PCPatrInvoker invoker, string state)
		//{
		//	String takeFile = Path.Combine(Path.GetTempPath(), "PcPatrFLEx.tak");

		//	invoker.RootGlossState = state;
		//	invoker.Invoke();
		//	using (var streamReader = new StreamReader(takeFile, Encoding.UTF8))
		//	{
		//		TakeString = streamReader.ReadToEnd().Replace("\r", "");
		//	}
		//	if (String.IsNullOrEmpty(state))
		//	{
		//		Assert.IsFalse(TakeString.Contains("set rootgloss "));
		//	}
		//	else
		//	{
		//		Assert.IsTrue(TakeString.Contains("set rootgloss " + state + "\n"));
		//	}
		//}
		//private void checkRootGlossStateValue(PCPatrInvoker invoker, string state, string expectedValue)
		//{
		//	String takeFile = Path.Combine(Path.GetTempPath(), "PcPatrFLEx.tak");

		//	invoker.RootGlossState = state;
		//	invoker.Invoke();
		//	using (var streamReader = new StreamReader(takeFile, Encoding.UTF8))
		//	{
		//		TakeString = streamReader.ReadToEnd().Replace("\r", "");
		//	}
		//	if (String.IsNullOrEmpty(state))
		//	{
		//		Assert.IsFalse(TakeString.Contains("set rootgloss "));
		//	}
		//	else
		//	{
		//		Assert.IsTrue(TakeString.Contains("set rootgloss " + expectedValue + "\n"));
		//	}
		//}
	}
}
