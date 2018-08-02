using SIL.DisambiguateSegmentInFLExDB;
using SIL.LcmLoader;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.Core.WritingSystems;
using SIL.LCModel.DomainServices;
using SIL.PrepFLExDB;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIL.PcPatrFLEx
{
	public partial class PcPatrFLExForm : Form
	{
		public LcmCache Cache { get; set; }
		public ProjectId ProjId { get; set; }

		private IList<IText> Texts { get; set; }
		private FLExDBExtractor Extractor { get; set; }
		private String GrammarFile { get; set; }

		public PcPatrFLExForm()
		{
			InitializeComponent();
			btnParse.Enabled = false;
			lblDatabaseToUse.Text = "";
			Icu.InitIcuDataDir();
			Sldr.Initialize();

			splitContainer1.Dock = DockStyle.Bottom;
			splitContainer1.Size = new System.Drawing.Size(796, 475);

			lbSegments.DisplayMember = "Baseline";
			lbSegments.ValueMember = "Segment";
		}

		private void btnProject_Click(object sender, EventArgs e)
		{
			btnParse.Enabled = false;
			ProjId = ChooseLangProject(this);
			if (ProjId != null)
			{
				lblDatabaseToUse.Text = ProjId.Name;
				var loader = new LcmLoader.LcmLoader(ProjId);
				loader.RaiseLcmLoaderEvent += HandleLcmLoaderEvent;
				Cache = loader.CreateCache();
				if (Cache != null)
				{
					EnsureDatabaseHasBeenPrepped();
					Extractor = new FLExDBExtractor(Cache);
					lbTexts.Font = CreateFont(Cache.LanguageProject.DefaultAnalysisWritingSystem);
					lbSegments.Font = CreateFont(Cache.LanguageProject.DefaultVernacularWritingSystem);
					FillTextsListBox();
				}
			}
		}

		private static Font CreateFont(CoreWritingSystemDefinition wsDef)
		{
			float fontSize = (wsDef.DefaultFontSize == 0) ? 10 : wsDef.DefaultFontSize;
			var fStyle = FontStyle.Regular;
			if (wsDef.DefaultFontFeatures.Contains("Bold"))
			{
				fStyle |= FontStyle.Bold;
			}
			if (wsDef.DefaultFontFeatures.Contains("Italic"))
			{
				fStyle |= FontStyle.Italic;
			}
			return new Font(wsDef.DefaultFontName, fontSize, fStyle);
		}

		private void EnsureDatabaseHasBeenPrepped()
		{
			var preparer = new Preparer(Cache, false);
			preparer.AddPCPATRList();
			preparer.AddPCPATRSenseCustomField();
			preparer.AddPCPATRSyntacticParserAgent();
		}

		private void FillTextsListBox()
		{
			Texts = Cache.LanguageProject.Texts.Where(t => t.ContentsOA != null).Cast<IText>().
				OrderBy(t => t.ShortName).ToList();
			lbTexts.DataSource = Texts;
		}

		private void HandleLcmLoaderEvent(object sender, LcmLoaderEventArgs a)
		{
			lblDatabaseToUse.Text = a.Message;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Lets the user select an existing language project.
		/// </summary>
		/// <param name="dialogOwner">The owner of the dialog.</param>
		/// <returns>The chosen project, or null if no project was chosen</returns>
		/// ------------------------------------------------------------------------------------
		internal static ProjectId ChooseLangProject(PcPatrFLExForm dialogOwner)
		{
			using (var dlg = new SIL.LcmLoaderUI.ChooseLangProjectDialog(false))
			{
				FwRegistryHelper.Initialize();
				dlg.ShowDialog(dialogOwner);
				if (dlg.DialogResult == DialogResult.OK)
				{
					var projId = new ProjectId(dlg.Project);
					return projId;
				}

				return null;
			}
		}

		private void UnlockDatabaseIfNeeded()
		{
			if (Cache != null)
			{
				ProjectLockingService.UnlockCurrentProject(Cache);
			}
		}
		private void PcPatrFLExForm_FormClosing(object sender, EventArgs e)
		{
			UnlockDatabaseIfNeeded();
		}

		private void lbTexts_SelectedIndexChanged_1(object sender, EventArgs e)
		{
			IList<SegmentToShow> segmentsInListBox = new List<SegmentToShow>();
			IText text = lbTexts.SelectedItem as IText;
			var contents = text.ContentsOA;
			IList<IStPara> paragraphs = contents.ParagraphsOS;
			foreach (IStPara para in paragraphs)
			{
				var paraUse = para as IStTxtPara;
				if (paraUse != null)
				{
					foreach (var segment in paraUse.SegmentsOS)
					{
						segmentsInListBox.Add(new SegmentToShow(segment, segment.BaselineText.Text));
					}
				}
			}
			lbSegments.DataSource = segmentsInListBox;
		}

		private void btnParse_Click(object sender, EventArgs e)
		{
			string ana = GetAnaForm();
			Console.WriteLine("ana='" + ana + "'");
		}

		private string GetAnaForm()
		{
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			var segment = selectedSegmentToShow.Segment;
			var ana = Extractor.ExtractTextSegmentAsANA(segment);
			return ana;
		}

		private void lbSegments_SelectedIndexChanged(object sender, EventArgs e)
		{
			var ana = GetAnaForm();
			if (ana.Contains("\\a \n"))
			{
				btnParse.Enabled = false;
			}
			else
			{
				btnParse.Enabled = true;
			}
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "PC-PATR Grammar File (*.grm)|*.grm|" +
			"All Files (*.*)|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				GrammarFile = dlg.FileName;
				tbGrammarFile.Text = GrammarFile;
			}
		}
	}

	public class SegmentToShow
	{
		public ISegment Segment { get; set; }
		public String Baseline { get; set; }

		public SegmentToShow(ISegment segment, string baseline)
		{
			Segment = segment;
			Baseline = baseline;
		}
	}
}
