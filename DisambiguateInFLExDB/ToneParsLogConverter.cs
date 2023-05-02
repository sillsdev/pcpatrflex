// Copyright (c) 2019-2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SIL.DisambiguateInFLExDB
{
    public class ToneParsLogConverter
    {
        LcmCache Cache { get; set; }
        string LogFileContents { get; set; }
        string LogFileMiddle { get; set; }
        string LogFilePrologue { get; set; }
        string LogFileEpilogue { get; set; }

        public ToneParsLogConverter(LcmCache cache, string logfile)
        {
            Cache = cache;
            if (File.Exists(logfile))
            {
                LogFileContents = File.ReadAllText(logfile);
                int iEndOfPreamble = LogFileContents.IndexOf("Output file: ");
                if (iEndOfPreamble > -1)
                {
                    LogFilePrologue = LogFileContents.Substring(0, iEndOfPreamble);
                    int iBegOfStatistics = LogFileContents.IndexOf("TONEPARS STATISTICS: ");
                    if (iBegOfStatistics > -1)
                    {
                        LogFileEpilogue = LogFileContents.Substring(iBegOfStatistics);
                        LogFileMiddle = LogFileContents.Substring(
                            iEndOfPreamble,
                            iBegOfStatistics - iEndOfPreamble
                        );
                        return;
                    }
                }
            }
            CleanLogFileContent();
        }

        private void CleanLogFileContent()
        {
            LogFileContents = "";
            LogFilePrologue = "";
            LogFileMiddle = "";
            LogFileEpilogue = "";
        }

        // TonePars log file shows hvos of MSAs instead of a morphname.
        // We need to convert all of these from MSA hvo to gloss.
        public string ConvertHvosToMorphnames()
        {
            if (LogFileMiddle == "")
                return "";
            var sb = new StringBuilder();
            var hvoGlossMapper = new Dictionary<string, string> { };
            BuildHvoGlossMapper(hvoGlossMapper);
            // Replace all instances of the hvos with the glosses.
            var logMiddleWithGlosses = hvoGlossMapper.Aggregate(
                LogFileMiddle,
                (current, replacement) => current.Replace(replacement.Key, replacement.Value)
            );
            // Recreate the log file with glosses instead of hvos.
            sb.Append(LogFilePrologue);
            sb.Append(logMiddleWithGlosses);
            sb.Append(LogFileEpilogue);
            return sb.ToString();
        }

        private void BuildHvoGlossMapper(Dictionary<string, string> hvoGlossMapper)
        {
            // Find all instances of 'Working on:' which has the MSA hvos.
            // Create a dictionary mapping from how the hvos appear to glosses.
            var matches = Regex.Matches(
                LogFileContents,
                " Working on: ([^\r\n]+)",
                RegexOptions.Multiline
            );
            foreach (Match match in matches)
            {
                var hvos = match.Value.Split(' ');
                foreach (string sHvo in hvos)
                {
                    // There are three different ways we need to match to avoid splitting an hvo:
                    // space hvo space
                    // space hvo \r
                    // space hvo :
                    var spaceHvoSpace = " " + sHvo + " ";
                    var spaceHvoCR = " " + sHvo + "\r";
                    var spaceHvoColon = " " + sHvo + ":";
                    if (hvoGlossMapper.ContainsKey(spaceHvoSpace))
                        continue;
                    int hvo = 0;
                    if (Int32.TryParse(sHvo, out hvo))
                    {
                        var msa = Cache.ServiceLocator.GetObject(hvo) as IMoMorphSynAnalysis;
                        if (msa != null)
                        {
                            var sense =
                                msa.ReferringObjects.FirstOrDefault(item => item is ILexSense)
                                as ILexSense;
                            if (sense != null)
                            {
                                var gloss = sense.Gloss.BestAnalysisAlternative.Text;
                                hvoGlossMapper.Add(spaceHvoSpace, " " + gloss + " ");
                                hvoGlossMapper.Add(spaceHvoCR, " " + gloss + "\r");
                                hvoGlossMapper.Add(spaceHvoColon, " " + gloss + ":");
                            }
                        }
                    }
                }
            }
        }
    }
}
