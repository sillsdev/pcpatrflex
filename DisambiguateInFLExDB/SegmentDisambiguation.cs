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
					if (analysis.ClassID == WfiWordformTags.kClassId)
					{
						var wfiWordform = analysis as IWfiWordform;
						var wfiMorphBundelGuidToUse = DisambiguatedMorphBundles.ElementAt(i);
						var wfiMorphBundle = cache.ServiceLocator.ObjectRepository.GetObject(wfiMorphBundelGuidToUse);
						if (wfiMorphBundle.Owner is IWfiAnalysis wfiAnalysisToUse)
						{
							// To disambiguate, we replace the WfiWordForm with a WfiGloss
							IWfiGloss gloss;
							if (wfiAnalysisToUse.MeaningsOC.Count == 0)
							{
								var wgFactory = cache.ServiceLocator.GetInstance<IWfiGlossFactory>();
								gloss = wgFactory.Create();
								wfiAnalysisToUse.MeaningsOC.Add(gloss);
							}
							else
							{
								gloss = wfiAnalysisToUse.MeaningsOC.First();
							}
							wfiAnalysisToUse.SetAgentOpinion(cache.LanguageProject.DefaultUserAgent, Opinions.approves);
							Segment.AnalysesRS[i] = gloss;
						}
					}
					i++;
				}
			});
		}

	}
}
