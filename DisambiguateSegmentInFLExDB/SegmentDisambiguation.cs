// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LcmLoaderUI;
using SIL.LCModel;
using SIL.LCModel.Infrastructure;
using SIL.PrepFLExDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateSegmentInFLExDB
{
	public class SegmentDisambiguation
	{
		public SegmentDisambiguation(ISegment segment, List<Guid> disambiguatedMorphBundleGuids)
		{
			Segment = segment;
			DisambiguatedMorphBundles = disambiguatedMorphBundleGuids;
		}

		public ISegment Segment { get; set; }
		public List<Guid> DisambiguatedMorphBundles { get; set; }

		public void Disambiguate(LcmCache cache)
		{
			var pcpatrAgent = GetPCPATRSyntacticParsingAgent(cache);
			NonUndoableUnitOfWorkHelper.Do(cache.ActionHandlerAccessor, () =>
			{
				int i = 0;
				foreach (IAnalysis analysis in Segment.AnalysesRS)
				{
					if (analysis.ClassID == WfiWordformTags.kClassId)
					{
						var wfiWordform = analysis as IWfiWordform;
						var wfiMorphBundelGuidToUse = DisambiguatedMorphBundles.ElementAt(i);
						var wfiMorphBundle = cache.ServiceLocator.ObjectRepository.GetObject(wfiMorphBundelGuidToUse);
						var wfiAnalysisToUse = wfiMorphBundle.Owner as IWfiAnalysis;
						wfiAnalysisToUse.Analysis.SetAgentOpinion(pcpatrAgent, Opinions.approves);
						Segment.AnalysesRS.RemoveAt(i);
						Segment.AnalysesRS.Insert(i, wfiAnalysisToUse);
					}
					else if (analysis.ClassID != PunctuationFormTags.kClassId)
					{
						analysis.Analysis.SetAgentOpinion(pcpatrAgent, Opinions.approves);
					}
					i++;
				}
			});
		}

		public ICmAgent GetPCPATRSyntacticParsingAgent(LcmCache cache)
		{
			var agents = cache.LangProject.AnalyzingAgentsOC;
			var pcpatrAgents = agents.Where(a => a.Name.BestAnalysisAlternative.Text == Constants.PcPatrSyntacticParser);
			if (pcpatrAgents.Count() > 0)
			{
				return pcpatrAgents.First();
			}
			else
			{
				return null;
			}
		}

	}
}
