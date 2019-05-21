// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using SIL.LCModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDB
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
			NonUndoableUnitOfWorkHelper.Do(cache.ActionHandlerAccessor, () =>
			{
				int i = 0;
				foreach (IAnalysis analysis in Segment.AnalysesRS)
				{
					if ((analysis.ClassID == WfiWordformTags.kClassId
						|| analysis.ClassID == WfiGlossTags.kClassId
						|| analysis.ClassID == WfiAnalysisTags.kClassId)
						&& i < DisambiguatedMorphBundles.Count)
					{
						var wfiWordform = analysis as IWfiWordform;
						var wfiMorphBundleGuidToUse = DisambiguatedMorphBundles.ElementAt(i);
						var wfiMorphBundle = cache.ServiceLocator.ObjectRepository.GetObject(wfiMorphBundleGuidToUse);
						if (wfiMorphBundle.Owner is IWfiAnalysis wfiAnalysisToUse)
						{
							wfiAnalysisToUse.SetAgentOpinion(cache.LanguageProject.DefaultUserAgent, Opinions.approves);
							Segment.AnalysesRS[i] = wfiAnalysisToUse;
						}
					}
					i++;
				}
			});
		}

	}
}
