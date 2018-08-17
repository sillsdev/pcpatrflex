// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LcmLoaderUI;
using SIL.LCModel;
using SIL.PrepFLExDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDB
{
    public class FLExDBExtractor
	{
		public LcmCache Cache { get; set; }

		ICmPossibilityList PcpatrList { get; set; }

		FieldDescription CustomField { get; set; }

		public FLExDBExtractor(LcmCache cache)
		{
			Cache = cache;

			var possListRepository = Cache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			PcpatrList = possListRepository.AllInstances().FirstOrDefault(list => list.Name.BestAnalysisAlternative.Text == Constants.PcPatrFeatureDescriptorList);

			var customFields = GetListOfCustomFields();
			CustomField = customFields.Find(fd => fd.Name == Constants.PcPatrFeatureDescriptorCustomField);

		}

		public string ExtractPcPatrLexicon()
		{
			var sb = new StringBuilder();
			var lexEntries = Cache.LanguageProject.LexDbOA.Entries;
			foreach (ILexEntry entry in lexEntries.OrderBy(e => e.ShortName))
			{
				formatEntry(entry, sb);
			}
			return sb.ToString();
		}

		protected void formatEntry(ILexEntry entry, StringBuilder sb)
		{
			sb.Append("\\w ");
			sb.Append(entry.LexemeFormOA.Form.BestVernacularAlternative.Text);
			sb.Append("\n\\c ");
			var sense = entry.SensesOS.FirstOrDefault<ILexSense>();
			var msa = (IMoStemMsa)sense.MorphoSyntaxAnalysisRA;
			var pos = msa.PartOfSpeechRA;
			sb.Append(pos.Abbreviation.BestAnalysisAlternative.Text);
			sb.Append("\n\\g ");
			sb.Append(sense.Gloss.BestAnalysisAlternative.Text);
			sb.Append("\n\\f");
			sb.Append(GetFeatureDescriptorsFromSense(sense));
			sb.Append("\n\n");
		}

		private string GetFeatureDescriptorsFromSense(ILexSense sense)
		{
			var sb = new StringBuilder();
			if (CustomField != null)
			{
				IList<string> fds = new List<string>() { };
				var size = Cache.MainCacheAccessor.get_VecSize(sense.Hvo, CustomField.Id);
				for (int i = 0; i < size; i++)
				{
					var hvo = Cache.MainCacheAccessor.get_VecItem(sense.Hvo, CustomField.Id, i);
					if (PcpatrList != null)
					{
						var item = PcpatrList.PossibilitiesOS.Where(ps => ps.Hvo == hvo);
						var fd = item.ElementAt(0).Name.BestAnalysisAlternative.Text;
						fds.Add(fd);
					}
				}
				fds = fds.OrderBy(fd => fd).ToList();
				foreach (string fd in fds)
				{
					sb.Append(" " + fd);
				}
			}
			return sb.ToString();
		}

		public List<FieldDescription> GetListOfCustomFields()
		{
			return (from fd in FieldDescription.FieldDescriptors(Cache)
					where fd.IsCustomField //&& GetItem(m_locationComboBox, fd.Class) != null
					select fd).ToList();
		}

		public string ExtractTextSegmentAsANA(ISegment segment)
		{
			var sb = new StringBuilder();
			var sbA = new StringBuilder();
			var sbD = new StringBuilder();
			var sbC = new StringBuilder();
			var sbFD = new StringBuilder();
			var sbP = new StringBuilder();
			var sbW = new StringBuilder();
			foreach (IAnalysis analysis in segment.AnalysesRS)
			{
				var wordform = analysis.Wordform;
				if (wordform == null)
				{
					continue;
				}
				sbA.Clear();
				sbA.Append("\\a ");
				sbD.Clear();
				sbD.Append("\\d ");
				sbC.Clear();
				sbC.Append("\\cat ");
				sbFD.Clear();
				sbFD.Append("\\fd ");
				sbP.Clear();
				sbP.Append("\\p ");
				sbW.Clear();
				sbW.Append("\\w ");
				var shape = wordform.Form.VernacularDefaultWritingSystem.Text;
				sbW.Append(shape + "\n");
				int ambiguities = wordform.AnalysesOC.Count;
				if (ambiguities > 1)
				{
					String ambigs = "%" + ambiguities + "%";
					sbA.Append(ambigs);
					sbD.Append(ambigs);
					sbC.Append(ambigs);
					sbFD.Append(ambigs);
					sbP.Append(ambigs);
				}
				foreach (IWfiAnalysis wfiAnalysis in wordform.AnalysesOC)
				{
					sbA.Append("< ");
					int i = 1;
					foreach (IWfiMorphBundle bundle in wfiAnalysis.MorphBundlesOS)
					{
						var msa = bundle.MsaRA;
						if (msa == null)
							continue;
						var cat = msa.PartOfSpeechForWsTSS(Cache.DefaultAnalWs).Text;
						sbA.Append(cat + " ");
						sbC.Append(cat);
						var morph = bundle.MorphRA;
						if (morph != null)
						{
							sbD.Append(morph.Form.VernacularDefaultWritingSystem.Text);
						}
						var sense = bundle.SenseRA;
						if (sense == null)
						{
							if (morph != null)
							{
								var entry = (ILexEntry)morph.Owner;
								var sense2 = entry.SensesOS.FirstOrDefault();
								if (sense2 == null)
								{
									sbA.Append("missing_sense >");
								}
								else
								{
									HandleSense(sbA, sbFD, sense2);
								}
							}
						}
						else
						{
							HandleSense(sbA, sbFD, sense);
						}
						sbP.Append(bundle.Guid.ToString());
						if (i < wfiAnalysis.MorphBundlesOS.Count)
						{
							sbD.Append("-");
							sbC.Append("=");
							sbFD.Append("=");
							sbP.Append("=");
						}
						if (ambiguities > 1)
						{
							sbA.Append("%");
							sbD.Append("%");
							sbC.Append("%");
							sbFD.Append("%");
							sbP.Append("%");
						}
					}
				}
				sbA.Append("\n");
				sbD.Append("\n");
				sbC.Append("\n");
				sbFD.Append("\n");
				sbP.Append("\n");
				sbW.Append("\n");
				sb.Append(sbA.ToString());
				sb.Append(sbD.ToString());
				sb.Append(sbC.ToString());
				sb.Append(sbP.ToString());
				sb.Append(sbFD.ToString());
				sb.Append(sbW.ToString());
			}
			return sb.ToString();
		}

		private void HandleSense(StringBuilder sbA, StringBuilder sbFD, ILexSense sense)
		{
			var gloss = sense.Gloss.BestAnalysisAlternative.Text;
			sbA.Append(gloss + " >");
			var fds = GetFeatureDescriptorsFromSense(sense);
			fds = (fds.Length > 1) ? fds.Substring(1) : fds;
			sbFD.Append(fds);
		}
	}
}
