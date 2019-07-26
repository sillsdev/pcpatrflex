namespace SIL.ToneParsFLEx
{
	partial class ToneParsFLExForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToneParsFLExForm));
			this.btnParseSegment = new System.Windows.Forms.Button();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lbTexts = new System.Windows.Forms.ListBox();
			this.lbSegments = new System.Windows.Forms.ListBox();
			this.lblTexts = new System.Windows.Forms.Label();
			this.lblSegments = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbGrammarFile = new System.Windows.Forms.TextBox();
			this.btnBrowseToneRule = new System.Windows.Forms.Button();
			this.btnParseText = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.btnBrowseIntxCtl = new System.Windows.Forms.Button();
			this.tbIntxCtlFile = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbRuleTrace = new System.Windows.Forms.CheckBox();
			this.cbTierAssignmentTrace = new System.Windows.Forms.CheckBox();
			this.cbDomainAssignmentTrace = new System.Windows.Forms.CheckBox();
			this.cbMorphemeToneAssignmentTrace = new System.Windows.Forms.CheckBox();
			this.cbTBUAssignmentTrace = new System.Windows.Forms.CheckBox();
			this.cbSyllableParsingTrace = new System.Windows.Forms.CheckBox();
			this.cbMoraParsingTrace = new System.Windows.Forms.CheckBox();
			this.cbMorphemeLinkingTrace = new System.Windows.Forms.CheckBox();
			this.cbSegmentParsingTrace = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnParseSegment
			// 
			this.btnParseSegment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnParseSegment.Location = new System.Drawing.Point(519, 289);
			this.btnParseSegment.Name = "btnParseSegment";
			this.btnParseSegment.Size = new System.Drawing.Size(267, 38);
			this.btnParseSegment.TabIndex = 10;
			this.btnParseSegment.Text = "&Parse this segment";
			this.btnParseSegment.UseVisualStyleBackColor = true;
			this.btnParseSegment.Click += new System.EventHandler(this.ParseSegment_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 359);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lbTexts);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lbSegments);
			this.splitContainer1.Size = new System.Drawing.Size(796, 790);
			this.splitContainer1.SplitterDistance = 356;
			this.splitContainer1.TabIndex = 4;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// lbTexts
			// 
			this.lbTexts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTexts.FormattingEnabled = true;
			this.lbTexts.ItemHeight = 20;
			this.lbTexts.Location = new System.Drawing.Point(0, 0);
			this.lbTexts.Name = "lbTexts";
			this.lbTexts.Size = new System.Drawing.Size(356, 790);
			this.lbTexts.TabIndex = 0;
			this.lbTexts.SelectedIndexChanged += new System.EventHandler(this.Texts_SelectedIndexChanged);
			// 
			// lbSegments
			// 
			this.lbSegments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSegments.FormattingEnabled = true;
			this.lbSegments.ItemHeight = 20;
			this.lbSegments.Location = new System.Drawing.Point(0, 0);
			this.lbSegments.Name = "lbSegments";
			this.lbSegments.Size = new System.Drawing.Size(436, 790);
			this.lbSegments.TabIndex = 0;
			this.lbSegments.SelectedIndexChanged += new System.EventHandler(this.Segments_SelectedIndexChanged);
			// 
			// lblTexts
			// 
			this.lblTexts.AutoSize = true;
			this.lblTexts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTexts.Location = new System.Drawing.Point(9, 299);
			this.lblTexts.Name = "lblTexts";
			this.lblTexts.Size = new System.Drawing.Size(61, 25);
			this.lblTexts.TabIndex = 7;
			this.lblTexts.Text = "Texts";
			// 
			// lblSegments
			// 
			this.lblSegments.AutoSize = true;
			this.lblSegments.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSegments.Location = new System.Drawing.Point(367, 299);
			this.lblSegments.Name = "lblSegments";
			this.lblSegments.Size = new System.Drawing.Size(101, 25);
			this.lblSegments.TabIndex = 9;
			this.lblSegments.Text = "Segments";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "TonePars rule file: ";
			// 
			// tbGrammarFile
			// 
			this.tbGrammarFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGrammarFile.Location = new System.Drawing.Point(216, 28);
			this.tbGrammarFile.Name = "tbGrammarFile";
			this.tbGrammarFile.Size = new System.Drawing.Size(885, 26);
			this.tbGrammarFile.TabIndex = 1;
			// 
			// btnBrowseToneRule
			// 
			this.btnBrowseToneRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseToneRule.Location = new System.Drawing.Point(1128, 28);
			this.btnBrowseToneRule.Name = "btnBrowseToneRule";
			this.btnBrowseToneRule.Size = new System.Drawing.Size(94, 38);
			this.btnBrowseToneRule.TabIndex = 2;
			this.btnBrowseToneRule.Text = "&Browse";
			this.btnBrowseToneRule.UseVisualStyleBackColor = true;
			this.btnBrowseToneRule.Click += new System.EventHandler(this.Browse_Click);
			// 
			// btnParseText
			// 
			this.btnParseText.Location = new System.Drawing.Point(77, 289);
			this.btnParseText.Name = "btnParseText";
			this.btnParseText.Size = new System.Drawing.Size(282, 38);
			this.btnParseText.TabIndex = 8;
			this.btnParseText.Text = "&Parse this text";
			this.btnParseText.UseVisualStyleBackColor = true;
			this.btnParseText.Click += new System.EventHandler(this.ParseText_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHelp.Location = new System.Drawing.Point(1146, 134);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(76, 40);
			this.btnHelp.TabIndex = 6;
			this.btnHelp.Text = "Help...";
			this.btnHelp.UseVisualStyleBackColor = true;
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// btnBrowseIntxCtl
			// 
			this.btnBrowseIntxCtl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseIntxCtl.Location = new System.Drawing.Point(1128, 79);
			this.btnBrowseIntxCtl.Name = "btnBrowseIntxCtl";
			this.btnBrowseIntxCtl.Size = new System.Drawing.Size(94, 38);
			this.btnBrowseIntxCtl.TabIndex = 14;
			this.btnBrowseIntxCtl.Text = "&Browse";
			this.btnBrowseIntxCtl.UseVisualStyleBackColor = true;
			this.btnBrowseIntxCtl.Click += new System.EventHandler(this.btnBrowseIntxCtl_Click);
			// 
			// tbIntxCtlFile
			// 
			this.tbIntxCtlFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbIntxCtlFile.Location = new System.Drawing.Point(216, 79);
			this.tbIntxCtlFile.Name = "tbIntxCtlFile";
			this.tbIntxCtlFile.Size = new System.Drawing.Size(885, 26);
			this.tbIntxCtlFile.TabIndex = 13;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(143, 20);
			this.label2.TabIndex = 12;
			this.label2.Text = "AMPLE intx.ctl file: ";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbRuleTrace);
			this.groupBox1.Controls.Add(this.cbTierAssignmentTrace);
			this.groupBox1.Controls.Add(this.cbDomainAssignmentTrace);
			this.groupBox1.Controls.Add(this.cbMorphemeToneAssignmentTrace);
			this.groupBox1.Controls.Add(this.cbTBUAssignmentTrace);
			this.groupBox1.Controls.Add(this.cbSyllableParsingTrace);
			this.groupBox1.Controls.Add(this.cbMoraParsingTrace);
			this.groupBox1.Controls.Add(this.cbMorphemeLinkingTrace);
			this.groupBox1.Controls.Add(this.cbSegmentParsingTrace);
			this.groupBox1.Location = new System.Drawing.Point(48, 120);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(1053, 147);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Tone Processing Tracing Options";
			// 
			// cbRuleTrace
			// 
			this.cbRuleTrace.AutoSize = true;
			this.cbRuleTrace.Location = new System.Drawing.Point(648, 114);
			this.cbRuleTrace.Name = "cbRuleTrace";
			this.cbRuleTrace.Size = new System.Drawing.Size(250, 24);
			this.cbRuleTrace.TabIndex = 8;
			this.cbRuleTrace.Text = "Rule application (normal trace)";
			this.cbRuleTrace.UseVisualStyleBackColor = true;
			// 
			// cbTierAssignmentTrace
			// 
			this.cbTierAssignmentTrace.AutoSize = true;
			this.cbTierAssignmentTrace.Location = new System.Drawing.Point(648, 72);
			this.cbTierAssignmentTrace.Name = "cbTierAssignmentTrace";
			this.cbTierAssignmentTrace.Size = new System.Drawing.Size(298, 24);
			this.cbTierAssignmentTrace.TabIndex = 7;
			this.cbTierAssignmentTrace.Text = "Primary and Register Tier assignment";
			this.cbTierAssignmentTrace.UseVisualStyleBackColor = true;
			// 
			// cbDomainAssignmentTrace
			// 
			this.cbDomainAssignmentTrace.AutoSize = true;
			this.cbDomainAssignmentTrace.Location = new System.Drawing.Point(648, 30);
			this.cbDomainAssignmentTrace.Name = "cbDomainAssignmentTrace";
			this.cbDomainAssignmentTrace.Size = new System.Drawing.Size(176, 24);
			this.cbDomainAssignmentTrace.TabIndex = 6;
			this.cbDomainAssignmentTrace.Text = "Domain assignment";
			this.cbDomainAssignmentTrace.UseVisualStyleBackColor = true;
			// 
			// cbMorphemeToneAssignmentTrace
			// 
			this.cbMorphemeToneAssignmentTrace.AutoSize = true;
			this.cbMorphemeToneAssignmentTrace.Location = new System.Drawing.Point(353, 115);
			this.cbMorphemeToneAssignmentTrace.Name = "cbMorphemeToneAssignmentTrace";
			this.cbMorphemeToneAssignmentTrace.Size = new System.Drawing.Size(233, 24);
			this.cbMorphemeToneAssignmentTrace.TabIndex = 5;
			this.cbMorphemeToneAssignmentTrace.Text = "Morpheme tone assignment";
			this.cbMorphemeToneAssignmentTrace.UseVisualStyleBackColor = true;
			// 
			// cbTBUAssignmentTrace
			// 
			this.cbTBUAssignmentTrace.AutoSize = true;
			this.cbTBUAssignmentTrace.Location = new System.Drawing.Point(353, 73);
			this.cbTBUAssignmentTrace.Name = "cbTBUAssignmentTrace";
			this.cbTBUAssignmentTrace.Size = new System.Drawing.Size(153, 24);
			this.cbTBUAssignmentTrace.TabIndex = 4;
			this.cbTBUAssignmentTrace.Text = "TBU assignment";
			this.cbTBUAssignmentTrace.UseVisualStyleBackColor = true;
			// 
			// cbSyllableParsingTrace
			// 
			this.cbSyllableParsingTrace.AutoSize = true;
			this.cbSyllableParsingTrace.Location = new System.Drawing.Point(353, 31);
			this.cbSyllableParsingTrace.Name = "cbSyllableParsingTrace";
			this.cbSyllableParsingTrace.Size = new System.Drawing.Size(145, 24);
			this.cbSyllableParsingTrace.TabIndex = 3;
			this.cbSyllableParsingTrace.Text = "Syllable parsing";
			this.cbSyllableParsingTrace.UseVisualStyleBackColor = true;
			// 
			// cbMoraParsingTrace
			// 
			this.cbMoraParsingTrace.AutoSize = true;
			this.cbMoraParsingTrace.Location = new System.Drawing.Point(7, 115);
			this.cbMoraParsingTrace.Name = "cbMoraParsingTrace";
			this.cbMoraParsingTrace.Size = new System.Drawing.Size(127, 24);
			this.cbMoraParsingTrace.TabIndex = 2;
			this.cbMoraParsingTrace.Text = "Mora parsing";
			this.cbMoraParsingTrace.UseVisualStyleBackColor = true;
			// 
			// cbMorphemeLinkingTrace
			// 
			this.cbMorphemeLinkingTrace.AutoSize = true;
			this.cbMorphemeLinkingTrace.Location = new System.Drawing.Point(7, 73);
			this.cbMorphemeLinkingTrace.Name = "cbMorphemeLinkingTrace";
			this.cbMorphemeLinkingTrace.Size = new System.Drawing.Size(289, 24);
			this.cbMorphemeLinkingTrace.TabIndex = 1;
			this.cbMorphemeLinkingTrace.Text = "Linking of morphemes to root nodes";
			this.cbMorphemeLinkingTrace.UseVisualStyleBackColor = true;
			// 
			// cbSegmentParsingTrace
			// 
			this.cbSegmentParsingTrace.AutoSize = true;
			this.cbSegmentParsingTrace.Location = new System.Drawing.Point(7, 31);
			this.cbSegmentParsingTrace.Name = "cbSegmentParsingTrace";
			this.cbSegmentParsingTrace.Size = new System.Drawing.Size(266, 24);
			this.cbSegmentParsingTrace.TabIndex = 0;
			this.cbSegmentParsingTrace.Text = "Segment parsing into root nodes";
			this.cbSegmentParsingTrace.UseVisualStyleBackColor = true;
			// 
			// ToneParsFLExForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1228, 1146);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnBrowseIntxCtl);
			this.Controls.Add(this.tbIntxCtlFile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnParseText);
			this.Controls.Add(this.btnBrowseToneRule);
			this.Controls.Add(this.tbGrammarFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSegments);
			this.Controls.Add(this.lblTexts);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.btnParseSegment);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ToneParsFLExForm";
			this.Text = "Use TonePars with FLEx";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnParseSegment;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lblTexts;
		private System.Windows.Forms.ListBox lbTexts;
		private System.Windows.Forms.Label lblSegments;
		private System.Windows.Forms.ListBox lbSegments;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbGrammarFile;
		private System.Windows.Forms.Button btnBrowseToneRule;
		private System.Windows.Forms.Button btnParseText;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button btnBrowseIntxCtl;
		private System.Windows.Forms.TextBox tbIntxCtlFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbRuleTrace;
		private System.Windows.Forms.CheckBox cbTierAssignmentTrace;
		private System.Windows.Forms.CheckBox cbDomainAssignmentTrace;
		private System.Windows.Forms.CheckBox cbMorphemeToneAssignmentTrace;
		private System.Windows.Forms.CheckBox cbTBUAssignmentTrace;
		private System.Windows.Forms.CheckBox cbSyllableParsingTrace;
		private System.Windows.Forms.CheckBox cbMoraParsingTrace;
		private System.Windows.Forms.CheckBox cbMorphemeLinkingTrace;
		private System.Windows.Forms.CheckBox cbSegmentParsingTrace;
	}
}

