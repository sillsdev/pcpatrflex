﻿using Microsoft.Win32;
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
		private IList<SegmentToShow> SegmentsInListBox { get; set; }
		private FLExDBExtractor Extractor { get; set; }
		private String GrammarFile { get; set; }

		public static string m_strRegKey = "Software\\SIL\\PcPatrFLEx";
		const string m_strLastDatabase = "LastDatabase";
		const string m_strLastGrammarFile = "LastGrammarFile";
		const string m_strLastText = "LastText";
		const string m_strLastSegment = "LastSegment";
		const string m_strLocationX = "LocationX";
		const string m_strLocationY = "LocationY";
		const string m_strSizeHeight = "SizeHeight";
		const string m_strSizeWidth = "SizeWidth";
		const string m_strWindowState = "WindowState";

		public Rectangle RectNormal { get; set; }

		public string LastDatabase { get; set; }
		public string LastGrammarFile { get; set; }
		public string LastText { get; set; }
		public string LastSegment { get; set; }

		private RegistryKey regkey;

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

			try
			{
				regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey);
				if (regkey != null)
				{
					retrieveRegistryInfo();
					regkey.Close();
					DesktopBounds = RectNormal;
					WindowState = WindowState;
					StartPosition = FormStartPosition.Manual;
					if (LastGrammarFile != null)
						tbGrammarFile.Text = LastGrammarFile;
					FwRegistryHelper.Initialize();
					ProjId = new ProjectId(LastDatabase);
					String lastText = LastText;
					String lastSegment = LastSegment;
					LoadProject();
					lblProjectName.Text = "";
					if (ProjId != null)
						lblProjectName.Text = ProjId.Name;
					// select last used text and segment, if any
					if (!String.IsNullOrEmpty(lastText))
					{
						var selectedText = Texts.Where(t => t.Guid.ToString() == lastText).FirstOrDefault();
						if (selectedText != null)
						{
							lbTexts.SelectedIndex = Texts.IndexOf(selectedText);
						}
						var selectedSegment = SegmentsInListBox.Where(s => s.Segment.Guid.ToString() == lastSegment).FirstOrDefault();
						if (selectedSegment != null)
						{
							lbSegments.SelectedIndex = SegmentsInListBox.IndexOf(selectedSegment);
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.InnerException);
				Console.WriteLine(e.StackTrace);
			}
		}

		void retrieveRegistryInfo()
		{
			// Window location
			int iX = (int)regkey.GetValue(m_strLocationX, 100);
			int iY = (int)regkey.GetValue(m_strLocationY, 100);
			int iWidth = (int)regkey.GetValue(m_strSizeWidth, 809); // 1228);
			int iHeight = (int)regkey.GetValue(m_strSizeHeight, 670); // 947);
			RectNormal = new Rectangle(iX, iY, iWidth, iHeight);
			// Set form properties
			WindowState = (FormWindowState)regkey.GetValue(m_strWindowState, 0);

			LastDatabase = (string)regkey.GetValue(m_strLastDatabase);
			LastGrammarFile = (string)regkey.GetValue(m_strLastGrammarFile);
			LastText = (string)regkey.GetValue(m_strLastText);
			LastSegment = (string)regkey.GetValue(m_strLastSegment);
		}
		public void saveRegistryInfo()
		{
			regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey, true);
			if (regkey == null)
			{
				regkey = Registry.CurrentUser.CreateSubKey(m_strRegKey);
			}

			if (LastDatabase != null)
				regkey.SetValue(m_strLastDatabase, LastDatabase);
			if (LastGrammarFile != null)
				regkey.SetValue(m_strLastGrammarFile, LastGrammarFile);
			if (LastText != null)
				regkey.SetValue(m_strLastText, LastText);
			if (LastSegment != null)
				regkey.SetValue(m_strLastSegment, LastSegment);
			// Window position and location
			regkey.SetValue(m_strWindowState, (int)WindowState);
			regkey.SetValue(m_strLocationX, RectNormal.X);
			regkey.SetValue(m_strLocationY, RectNormal.Y);
			regkey.SetValue(m_strSizeWidth, RectNormal.Width);
			regkey.SetValue(m_strSizeHeight, RectNormal.Height);

			regkey.Close();
		}

		private void ChooseProject_Click(object sender, EventArgs e)
		{
			btnParse.Enabled = false;
			ProjId = ChooseLangProject(this);
			LoadProject();
			lblProjectName.Text = "";
			if (ProjId != null)
				lblProjectName.Text = ProjId.Name;
		}

		private void LoadProject()
		{
			if (ProjId == null)
				return;
			LastDatabase = ProjId.Name;
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
		private void OnFormClosing(object sender, EventArgs e)
		{
			Console.WriteLine("form closing");
			saveRegistryInfo();
			UnlockDatabaseIfNeeded();
		}

		private void Texts_SelectedIndexChanged(object sender, EventArgs e)
		{
			SegmentsInListBox = new List<SegmentToShow>();
			IText text = lbTexts.SelectedItem as IText;
			LastText = text.Guid.ToString();
			var contents = text.ContentsOA;
			IList<IStPara> paragraphs = contents.ParagraphsOS;
			foreach (IStPara para in paragraphs)
			{
				var paraUse = para as IStTxtPara;
				if (paraUse != null)
				{
					foreach (var segment in paraUse.SegmentsOS)
					{
						SegmentsInListBox.Add(new SegmentToShow(segment, segment.BaselineText.Text));
					}
				}
			}
			lbSegments.DataSource = SegmentsInListBox;
		}

		private void Parse_Click(object sender, EventArgs e)
		{
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			string ana = GetAnaForm(selectedSegmentToShow);
			Console.WriteLine("ana='" + ana + "'");
		}

		private string GetAnaForm(SegmentToShow selectedSegmentToShow)
		{
			var segment = selectedSegmentToShow.Segment;
			var ana = Extractor.ExtractTextSegmentAsANA(segment);
			return ana;
		}

		private void Segments_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			LastSegment = selectedSegmentToShow.Segment.Guid.ToString();
			var ana = GetAnaForm(selectedSegmentToShow);
			if (ana.Contains("\\a \n"))
			{
				btnParse.Enabled = false;
			}
			else
			{
				btnParse.Enabled = true;
			}
		}

		private void Browse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "PC-PATR Grammar File (*.grm)|*.grm|" +
			"All Files (*.*)|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				GrammarFile = dlg.FileName;
				LastGrammarFile = GrammarFile;
				tbGrammarFile.Text = GrammarFile;
			}
		}

		protected override void OnMove(EventArgs ea)
		{
			base.OnMove(ea);

			if (WindowState == FormWindowState.Normal)
				RectNormal = DesktopBounds;
		}
		protected override void OnResize(EventArgs ea)
		{
			base.OnResize(ea);

			if (WindowState == FormWindowState.Normal)
				RectNormal = DesktopBounds;
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
