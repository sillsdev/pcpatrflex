// Copyright (c) 2019-2023 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using Microsoft.Win32;
using SIL.DisambiguateInFLExDB;
using SIL.FieldWorks.LexText.Controls;
using SIL.FieldWorks.WordWorks.Parser;
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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ToneParsFLExDll;
using XCore;
using XAmpleManagedWrapper;
using ToneParsTextPreparer;
using System.Text.RegularExpressions;

namespace SIL.ToneParsFLEx
{
    public partial class ToneParsFLExForm : Form
    {
        public LcmCache Cache { get; set; }

        protected IList<IText> Texts { get; set; }
        protected IList<SegmentToShow> SegmentsInListBox { get; set; }
        protected FLExDBExtractor Extractor { get; set; }
        protected Font AnalysisFont { get; set; }
        protected Font VernacularFont { get; set; }

        private String ToneRuleFile { get; set; }
        private String IntxCtlFile { get; set; }

        public static string m_strRegKey = "Software\\SIL\\ToneParsFLEx";
        const string m_strLastDatabase = "LastDatabase";
        const string m_strLastToneRuleFile = "LastToneRuleFile";
        const string m_strLastIntxCtlFile = "LastIntxCtlFile";
        const string m_strLastText = "LastText";
        const string m_strLastSegment = "LastSegment";
        const string m_strLastVerify = "LastVerify";
        const string m_strLastIgnoreContext = "LastIgnoreContext";
        const string m_strLastDoTracing = "LastDoTracing";
        const string m_strLastRuleTrace = "LastRuleTrace";
        const string m_strLastTierAssignmentTrace = "LastTierAssignmentTrace";
        const string m_strLastDomainAssignmentTrace = "LastDomainAssignmentTrace";
        const string m_strLastMorphemeToneAssignmentTrace = "LastMorphemeToneAssignmentTrace";
        const string m_strLastTBUAssignmentTrace = "LastTBUAssignmentTrace";
        const string m_strLastSyllableParsingTrace = "LastSyllableParsingTrace";
        const string m_strLastMoraParsingTrace = "LastMoraParsingTrace";
        const string m_strLastMorphemeLinkingTrace = "LastMorphemeLinkingTrace";
        const string m_strLastSegmentParsingTrace = "LastSegmentParsingTrace";
        const string m_strLocationX = "LocationX";
        const string m_strLocationY = "LocationY";
        const string m_strSizeHeight = "SizeHeight";
        const string m_strSizeWidth = "SizeWidth";
        const string m_strWindowState = "WindowState";
        const string m_strSplitterLocationX = "SplitterLocationX";
        private int SplitterLocationRetrieved { get; set; }

        const string m_strAll = "All";
        const string m_strLeftmost = "Leftmost";
        const string m_strOff = "Off";
        const string m_strRightmost = "Rightmos";

        public Rectangle RectNormal { get; set; }

        public string LastDatabase { get; set; }
        public string LastToneRuleFile { get; set; }
        public string LastIntxCtlFile { get; set; }
        public string LastText { get; set; }
        public string LastSegment { get; set; }
        public bool LastDoTracing { get; set; }
        public string RetrievedLastText { get; set; }
        public string RetrievedLastSegment { get; set; }

        private RegistryKey regkey;
        private ParserConnection m_parserConnection;

        private ContextMenuStrip helpContextMenu;
        const string UserDocumentation = "User Documentation";
        const string ToneParsDocumentation = "TonePars Documentation";
        const string ToneParsGrammarDocumentation = "TonePars Grammar Documentation";
        const string About = "About";

        public ToneParsFLExForm()
        {
            InitializeComponent();
            btnParseSegment.Enabled = false;

            splitContainer1.Dock = DockStyle.Bottom;
            splitContainer1.SplitterWidth = 3;

            lbSegments.DisplayMember = "Baseline";
            lbSegments.ValueMember = "Segment";

            BuildHelpContextMenu();

            try
            {
                regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey);
                if (regkey != null)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Application.DoEvents();
                    retrieveRegistryInfo();
                    regkey.Close();
                    DesktopBounds = RectNormal;
                    WindowState = WindowState;
                    StartPosition = FormStartPosition.Manual;
                    if (!String.IsNullOrEmpty(LastToneRuleFile))
                        tbToneRuleFile.Text = LastToneRuleFile;
                    if (!String.IsNullOrEmpty(LastIntxCtlFile))
                        tbIntxCtlFile.Text = LastIntxCtlFile;
                    if (Cache != null && Cache.LangProject.Texts.Count > 0)
                        btnParseText.Enabled = true;
                    else
                        btnParseText.Enabled = false;
                    Cursor.Current = Cursors.Default;
                }
                AdjustSplitterLocation();
                lblParsingStatus.Text = "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException);
                Console.WriteLine(e.StackTrace);
                MessageBox.Show(e.Message + e.InnerException + e.StackTrace);
            }
        }

        private void AdjustSplitterLocation()
        {
            int x = splitContainer1.Location.X;
            // Make the Y location be where the bottom of the parse text
            //  button is plus a gap of 15 pixels
            int y = this.ClientSize.Height - (btnParseText.Bottom + 15);
            splitContainer1.Size = new Size(x, y);
        }

        private void BuildHelpContextMenu()
        {
            helpContextMenu = new ContextMenuStrip();
            ToolStripMenuItem userDoc = new ToolStripMenuItem(UserDocumentation);
            userDoc.Click += new EventHandler(UserDoc_Click);
            userDoc.Name = UserDocumentation;
            ToolStripMenuItem toneParsDoc = new ToolStripMenuItem(ToneParsDocumentation);
            toneParsDoc.Click += new EventHandler(ToneParsDoc_Click);
            toneParsDoc.Name = ToneParsDocumentation;
            ToolStripMenuItem toneParsGrammarDoc = new ToolStripMenuItem(
                ToneParsGrammarDocumentation
            );
            toneParsGrammarDoc.Click += new EventHandler(ToneParsGrammarDoc_Click);
            toneParsGrammarDoc.Name = ToneParsGrammarDocumentation;
            ToolStripMenuItem about = new ToolStripMenuItem(About);
            about.Click += new EventHandler(About_Click);
            about.Name = About;
            helpContextMenu.Items.Add(userDoc);
            helpContextMenu.Items.Add(toneParsDoc);
            helpContextMenu.Items.Add(toneParsGrammarDoc);
            helpContextMenu.Items.Add("-");
            helpContextMenu.Items.Add(about);
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
            ToneRuleFile = LastToneRuleFile = (string)regkey.GetValue(m_strLastToneRuleFile);
            IntxCtlFile = LastIntxCtlFile = (string)regkey.GetValue(m_strLastIntxCtlFile);
            RetrievedLastText = LastText = (string)regkey.GetValue(m_strLastText);
            RetrievedLastSegment = LastSegment = (string)regkey.GetValue(m_strLastSegment);
            splitContainer1.SplitterDistance = (int)regkey.GetValue(m_strSplitterLocationX, 150);
            SplitterLocationRetrieved = splitContainer1.SplitterDistance;
            cbVerify.Checked = Convert.ToBoolean(regkey.GetValue(m_strLastVerify, false));
            cbIgnoreContext.Checked = Convert.ToBoolean(
                regkey.GetValue(m_strLastIgnoreContext, true)
            );
            cbTraceToneProcessing.Checked = Convert.ToBoolean(
                regkey.GetValue(m_strLastDoTracing, false)
            );
            ToneParsInvokerOptions.Instance.RuleTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastRuleTrace, false)
            );
            ToneParsInvokerOptions.Instance.TierAssignmentTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastTierAssignmentTrace, false)
            );
            ToneParsInvokerOptions.Instance.DomainAssignmentTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastDomainAssignmentTrace, false)
            );
            ToneParsInvokerOptions.Instance.MorphemeToneAssignmentTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastMorphemeToneAssignmentTrace, false)
            );
            ToneParsInvokerOptions.Instance.TBUAssignmentTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastTBUAssignmentTrace, false)
            );
            ToneParsInvokerOptions.Instance.SyllableParsingTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastSyllableParsingTrace, false)
            );
            ToneParsInvokerOptions.Instance.MoraParsingTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastMoraParsingTrace, false)
            );
            ToneParsInvokerOptions.Instance.MorphemeLinkingTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastMorphemeLinkingTrace, false)
            );
            ToneParsInvokerOptions.Instance.SegmentParsingTrace = Convert.ToBoolean(
                regkey.GetValue(m_strLastSegmentParsingTrace, false)
            );
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
            if (LastToneRuleFile != null)
                regkey.SetValue(m_strLastToneRuleFile, LastToneRuleFile);
            if (LastIntxCtlFile != null)
                regkey.SetValue(m_strLastIntxCtlFile, LastIntxCtlFile);
            if (LastText != null)
                regkey.SetValue(m_strLastText, LastText);
            if (LastSegment != null)
                regkey.SetValue(m_strLastSegment, LastSegment);
            regkey.SetValue(m_strLastVerify, cbVerify.Checked);
            regkey.SetValue(m_strLastIgnoreContext, cbIgnoreContext.Checked);
            regkey.SetValue(m_strLastDoTracing, cbTraceToneProcessing.Checked);
            regkey.SetValue(m_strLastRuleTrace, ToneParsInvokerOptions.Instance.RuleTrace);
            regkey.SetValue(
                m_strLastTierAssignmentTrace,
                ToneParsInvokerOptions.Instance.TierAssignmentTrace
            );
            regkey.SetValue(
                m_strLastDomainAssignmentTrace,
                ToneParsInvokerOptions.Instance.DomainAssignmentTrace
            );
            regkey.SetValue(
                m_strLastMorphemeToneAssignmentTrace,
                ToneParsInvokerOptions.Instance.MorphemeToneAssignmentTrace
            );
            regkey.SetValue(
                m_strLastTBUAssignmentTrace,
                ToneParsInvokerOptions.Instance.TBUAssignmentTrace
            );
            regkey.SetValue(
                m_strLastSyllableParsingTrace,
                ToneParsInvokerOptions.Instance.SyllableParsingTrace
            );
            regkey.SetValue(
                m_strLastMoraParsingTrace,
                ToneParsInvokerOptions.Instance.MoraParsingTrace
            );
            regkey.SetValue(
                m_strLastMorphemeLinkingTrace,
                ToneParsInvokerOptions.Instance.MorphemeLinkingTrace
            );
            regkey.SetValue(
                m_strLastSegmentParsingTrace,
                ToneParsInvokerOptions.Instance.SegmentParsingTrace
            );

            // Window position and location
            regkey.SetValue(m_strWindowState, (int)WindowState);
            regkey.SetValue(m_strLocationX, RectNormal.X);
            regkey.SetValue(m_strLocationY, RectNormal.Y);
            regkey.SetValue(m_strSizeWidth, RectNormal.Width);
            regkey.SetValue(m_strSizeHeight, RectNormal.Height);
            regkey.SetValue(m_strSplitterLocationX, splitContainer1.SplitterDistance);

            regkey.Close();
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
            preparer.AddToneParsList();
            preparer.AddToneParsFormCustomField();
            preparer.AddToneParsSenseCustomField();
        }

        public void PrepareForm()
        {
            splitContainer1.SplitterDistance = SplitterLocationRetrieved;
            if (Cache != null)
            {
                EnsureDatabaseHasBeenPrepped();
                Extractor = new FLExDBExtractor(Cache);
                AnalysisFont = CreateFont(Cache.LanguageProject.DefaultAnalysisWritingSystem);
                lbTexts.Font = AnalysisFont;
                VernacularFont = CreateFont(Cache.LanguageProject.DefaultVernacularWritingSystem);
                lbSegments.Font = VernacularFont;
                lbSegments.RightToLeft = Cache
                    .LanguageProject
                    .DefaultVernacularWritingSystem
                    .RightToLeftScript
                    ? RightToLeft.Yes
                    : RightToLeft.No;
                FillTextsListBox();
            }
        }

        public void FillTextsListBox()
        {
            Texts = Cache.LanguageProject.Texts
                .Where(t => t.ContentsOA != null)
                .Cast<IText>()
                .OrderBy(t => t.ShortName)
                .ToList();
            lbTexts.DataSource = Texts;
            if (Texts.Count > 0)
                btnParseText.Enabled = true;
            // select last used text and segment, if any
            if (!String.IsNullOrEmpty(LastText))
            {
                var selectedText = Texts
                    .Where(t => t.Guid.ToString() == RetrievedLastText)
                    .FirstOrDefault();
                if (selectedText != null)
                {
                    lbTexts.SelectedIndex = Texts.IndexOf(selectedText);
                }
                var selectedSegment = SegmentsInListBox
                    .Where(s => s.Segment.Guid.ToString() == RetrievedLastSegment)
                    .FirstOrDefault();
                if (selectedSegment != null)
                {
                    lbSegments.SelectedIndex = SegmentsInListBox.IndexOf(selectedSegment);
                }
            }
        }

        private void OnFormClosing(object sender, EventArgs e)
        {
            saveRegistryInfo();
            if (m_parserConnection != null && Cache != null && !Cache.IsDisposed)
            {
                m_parserConnection.Dispose();
            }
            m_parserConnection = null;
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
                        SegmentsInListBox.Add(
                            new SegmentToShow(segment, segment.BaselineText.Text)
                        );
                    }
                }
            }
            lbSegments.DataSource = SegmentsInListBox;
        }

        private bool CheckForValidFiles()
        {
            bool result = true;
            if (!File.Exists(IntxCtlFile))
            {
                MessageBox.Show(
                    "Could not find the "
                        + RemoveColon(lblAmpleIntxCtl.Text)
                        + " at: "
                        + IntxCtlFile
                );
                result = false;
            }
            if (!File.Exists(ToneRuleFile))
            {
                MessageBox.Show(
                    "Could not find the "
                        + RemoveColon(lblToneRuleFile.Text)
                        + " at: "
                        + ToneRuleFile
                );
                result = false;
            }
            else
            {
                string sContents = File.ReadAllText(ToneRuleFile);
                string pattern = "\r\n\\\\segments ";
                var ms = Regex.Matches(sContents, pattern);
                foreach (Match match in Regex.Matches(sContents, pattern))
                {
                    if (match.Success && match.Index > -1)
                    {
                        int indexBeg = match.Index + 12;
                        int end = sContents.Substring(indexBeg).IndexOf("\r\n");
                        string sSegFile = sContents.Substring(indexBeg, end);
                        sSegFile = sSegFile.Trim();
                        if (!File.Exists(sSegFile))
                        {
                            MessageBox.Show(
                                "Could not find the segments file in the "
                                    + RemoveColon(lblToneRuleFile.Text)
                                    + " at: "
                                    + sSegFile
                                    + "\nRemember that this file path may need to be in 8.3 format."
                            );
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        private static string RemoveColon(string sLabel)
        {
            int index = sLabel.LastIndexOf(":");
            if (index > -1)
            {
                sLabel = sLabel.Substring(0, index);
            }

            return sLabel;
        }

        private void ParseSegment_Click(object sender, EventArgs e)
        {
            if (!CheckForValidFiles())
                return;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
            string textToUse;
            if (cbIgnoreContext.Checked)
            {
                TextPreparer preparer = TextPreparer.Instance;
                textToUse = preparer.GetUniqueWordForms(selectedSegmentToShow.Segment);
            }
            else
            {
                textToUse = selectedSegmentToShow.Baseline;
            }
            var inputFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.txt");
            File.WriteAllText(inputFile, textToUse);
            InvokeToneParser(inputFile);
            Cursor.Current = Cursors.Default;
        }

        private void UpdateParsingStatus(string content)
        {
            lblParsingStatus.Text = content;
            lblParsingStatus.Invalidate();
            lblParsingStatus.Update();
        }

        private void InvokeToneParser(string inputFile)
        {
            lblParsingStatus.Text = "";
            btnParseText.Enabled = btnParseSegment.Enabled = false;
            var invoker = new ToneParsInvoker(
                tbToneRuleFile.Text,
                tbIntxCtlFile.Text,
                inputFile,
                GetDecompSeparationCharacter(),
                Cache
            );
            invoker.ParsingStatus = lblParsingStatus;
            UpdateParsingStatus("Updating Grammar and Lexicon");
            if (ConnectToParser(invoker.Queue))
            {
                m_parserConnection.ReloadGrammarAndLexicon();
                WaitForLoadToFinish();
            }
            invoker.Invoke();
            if (invoker.InvocationSucceeded)
            {
                Console.WriteLine("invocation succeeded!\n");
                UpdateParsingStatus("Updating results in texts");
                invoker.SaveResultsInDatabase();
                UpdateParsingStatus("");
                Console.Beep();
            }
            else
            {
                Console.WriteLine("invocation failed!\n");
                Console.Beep();
            }
            btnParseText.Enabled = btnParseSegment.Enabled = true;
        }

        private void WaitForLoadToFinish()
        {
            while (m_parserConnection.GetQueueSize(ParserPriority.ReloadGrammarAndLexicon) > 0)
            {
                Thread.Sleep(250);
            }
        }

        public bool ConnectToParser(IdleQueue idleQueue)
        {
            //CheckDisposed();

            if (m_parserConnection == null)
            {
                // Don't bother if the lexicon is empty.  See FWNX-1019.
                if (Cache.ServiceLocator.GetInstance<ILexEntryRepository>().Count == 0)
                {
                    return false;
                }
                m_parserConnection = new ParserConnection(Cache, idleQueue);
            }
            return true;
        }

        private char GetDecompSeparationCharacter()
        {
            String textInptControlFileContents = File.ReadAllText(tbIntxCtlFile.Text);
            string decompSeparationCharacter = ToneParsInvoker
                .GetFieldFromAntRecord(textInptControlFileContents, "\\dsc ")
                .Trim();
            return decompSeparationCharacter[0];
        }

        private string GetAnaForm(SegmentToShow selectedSegmentToShow)
        {
            var segment = selectedSegmentToShow.Segment;
            var ana = Extractor.ExtractTextSegmentAsANA(segment);
            return ana;
        }

        private string GetTextBaselines(IText selectedTextToShow)
        {
            var sb = new StringBuilder();
            var contents = selectedTextToShow.ContentsOA;
            IList<IStPara> paragraphs = contents.ParagraphsOS;
            foreach (IStPara para in paragraphs)
            {
                var paraUse = para as IStTxtPara;
                if (paraUse != null)
                {
                    foreach (var segment in paraUse.SegmentsOS)
                    {
                        sb.Append(segment.BaselineText);
                        sb.Append("\n");
                    }
                }
            }
            return sb.ToString();
        }

        private void Segments_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
            string sSelectedIndicies = FormatSelectedSegmentIndices();
            lblStatus.Text = sSelectedIndicies + "/" + lbSegments.Items.Count;
            LastSegment = selectedSegmentToShow.Segment.Guid.ToString();
            var ana = GetAnaForm(selectedSegmentToShow);
            if (ana.Contains("\\a \n"))
            {
                btnParseSegment.Enabled = false;
            }
            else
            {
                btnParseSegment.Enabled = true;
            }
            btnParseSegment.Enabled = true;
        }

        private String FormatSelectedSegmentIndices()
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var seg in lbSegments.SelectedItems)
            {
                var selectedSegment = seg as SegmentToShow;
                if (selectedSegment != null)
                {
                    sb.Append(SegmentsInListBox.IndexOf(selectedSegment) + 1);
                    if (++i < lbSegments.SelectedItems.Count)
                    {
                        sb.Append(",");
                    }
                }
            }

            return sb.ToString();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Tone Parse Rule File (*TP.ctl)|*.ctl|" + "All Files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ToneRuleFile = dlg.FileName;
                LastToneRuleFile = ToneRuleFile;
                tbToneRuleFile.Text = ToneRuleFile;
            }
        }

        private void btnBrowseIntxCtl_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "AMPLE Input Text Control File (*intx.ctl)|*.ctl|" + "All Files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                IntxCtlFile = dlg.FileName;
                LastIntxCtlFile = IntxCtlFile;
                tbIntxCtlFile.Text = IntxCtlFile;
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
            AdjustSplitterLocation();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            lblSegments.Left = e.SplitX + 10;
            btnParseSegment.Left = lblSegments.Right + 10;
        }

        private void ParseText_Click(object sender, EventArgs e)
        {
            if (!CheckForValidFiles())
                return;
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            var selectedTextToShow = lbTexts.SelectedItem as IText;
            string textToUse;
            if (cbIgnoreContext.Checked)
            {
                TextPreparer preparer = TextPreparer.Instance;
                textToUse = preparer.GetUniqueWordForms(selectedTextToShow);
            }
            else
            {
                textToUse = GetTextBaselines(selectedTextToShow);
            }
            var inputFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.txt");
            File.WriteAllText(inputFile, textToUse);
            InvokeToneParser(inputFile);
            Cursor.Current = Cursors.Default;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(0, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            helpContextMenu.Show(ptLowerLeft);
        }

        void UserDoc_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == UserDocumentation)
            {
                String basedir = GetAppBaseDir();
                Process.Start(Path.Combine(basedir, "doc", "ToneParsFLExUserDocumentation.pdf"));
            }
        }

        private static string GetAppBaseDir()
        {
            string basedir;
            string rootdir;
            int indexOfBinInPath;
            DetermineIndexOfBinInExecutablesPath(out rootdir, out indexOfBinInPath);
            if (indexOfBinInPath >= 0)
                basedir = rootdir.Substring(0, indexOfBinInPath);
            else
                basedir = rootdir;
            return basedir;
        }

        private static void DetermineIndexOfBinInExecutablesPath(
            out string rootdir,
            out int indexOfBinInPath
        )
        {
            Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
            indexOfBinInPath = rootdir.LastIndexOf("bin");
        }

        void ToneParsDoc_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == ToneParsDocumentation)
            {
                String basedir = GetAppBaseDir();
                Process.Start(Path.Combine(basedir, "doc", "silewp2007_002.pdf"));
            }
        }

        void ToneParsGrammarDoc_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == ToneParsGrammarDocumentation)
            {
                String basedir = GetAppBaseDir();
                Process.Start(Path.Combine(basedir, "doc", "ToneParsGrammar.txt"));
            }
        }

        void About_Click(object sender, EventArgs e)
        {
            ToolStripItem menuItem = (ToolStripItem)sender;
            if (menuItem.Name == About)
            {
                var dialog = new AboutBox();
                // for some reason the following is needed to keep the dialog within the form
                Point pt = dialog.PointToClient(System.Windows.Forms.Cursor.Position);
                dialog.Location = new Point(this.Location.X + 20, this.Location.Y + 20);
                dialog.Show();
            }
        }

        private void ShowLog_Click(object sender, EventArgs e)
        {
            var invoker = new ToneParsInvoker(
                tbToneRuleFile.Text,
                tbIntxCtlFile.Text,
                "",
                ' ',
                Cache
            );
            if (File.Exists(invoker.ToneParsLogFile))
            {
                if (ToneParsInvokerOptions.Instance.DoTracing)
                {
                    var converter = new ToneParsLogConverter(Cache, invoker.ToneParsLogFile);
                    var result = converter.ConvertHvosToMorphnames();
                    File.WriteAllText(invoker.ToneParsLogFile, result);
                }
                Process.Start(invoker.ToneParsLogFile);
            }
            else
                MessageBox.Show("Log file does not exist; please parse a segment or a text.");
        }

        private void cbVerify_CheckedChanged(object sender, EventArgs e)
        {
            ToneParsInvokerOptions.Instance.VerifyInformation = cbVerify.Checked;
        }

        private void cbTraceToneProcessing_CheckedChanged(object sender, EventArgs e)
        {
            ToneParsInvokerOptions.Instance.DoTracing = cbTraceToneProcessing.Checked;
            ToneParsInvokerOptions.Instance.RuleTrace = cbTraceToneProcessing.Checked;
        }

        private void btnTracingOptions_Click(object sender, EventArgs e)
        {
            var optionsDialog = new TracingOptionsDialog();
            optionsDialog.ShowDialog();
        }

        private void tbToneRuleFile_TextChanged(object sender, EventArgs e)
        {
            ToneRuleFile = tbToneRuleFile.Text;
        }

        private void tbIntxCtlFile_TextChanged(object sender, EventArgs e)
        {
            IntxCtlFile = tbIntxCtlFile.Text;
        }

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			Application.DoEvents();
			RetrievedLastText = LastText;
			FillTextsListBox();
			Cursor.Current = Cursors.Default;
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
