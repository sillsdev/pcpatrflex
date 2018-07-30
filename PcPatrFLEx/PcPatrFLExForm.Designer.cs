namespace SIL.PcPatrFLEx
{
	partial class PcPatrFLExForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PcPatrFLExForm));
			this.btnParse = new System.Windows.Forms.Button();
			this.textsPanel = new System.Windows.Forms.Panel();
			this.btnProject = new System.Windows.Forms.Button();
			this.lblDatabaseToUse = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnParse
			// 
			this.btnParse.Location = new System.Drawing.Point(697, 12);
			this.btnParse.Name = "btnParse";
			this.btnParse.Size = new System.Drawing.Size(94, 38);
			this.btnParse.TabIndex = 0;
			this.btnParse.Text = "Parse";
			this.btnParse.UseVisualStyleBackColor = true;
			// 
			// textsPanel
			// 
			this.textsPanel.Location = new System.Drawing.Point(1, 63);
			this.textsPanel.Name = "textsPanel";
			this.textsPanel.Size = new System.Drawing.Size(799, 385);
			this.textsPanel.TabIndex = 1;
			// 
			// btnProject
			// 
			this.btnProject.Location = new System.Drawing.Point(13, 12);
			this.btnProject.Name = "btnProject";
			this.btnProject.Size = new System.Drawing.Size(191, 38);
			this.btnProject.TabIndex = 2;
			this.btnProject.Text = "Choose FLEx Project";
			this.btnProject.UseVisualStyleBackColor = true;
			this.btnProject.Click += new System.EventHandler(this.btnProject_Click);
			// 
			// lblDatabaseToUse
			// 
			this.lblDatabaseToUse.AutoSize = true;
			this.lblDatabaseToUse.Location = new System.Drawing.Point(282, 30);
			this.lblDatabaseToUse.Name = "lblDatabaseToUse";
			this.lblDatabaseToUse.Size = new System.Drawing.Size(158, 20);
			this.lblDatabaseToUse.TabIndex = 3;
			this.lblDatabaseToUse.Text = "Chosen FLEx Project";
			// 
			// PcPatrFLExForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.lblDatabaseToUse);
			this.Controls.Add(this.btnProject);
			this.Controls.Add(this.textsPanel);
			this.Controls.Add(this.btnParse);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PcPatrFLExForm";
			this.Text = "Use PC-PATR with FLEx";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnParse;
		private System.Windows.Forms.Panel textsPanel;
		private System.Windows.Forms.Button btnProject;
		private System.Windows.Forms.Label lblDatabaseToUse;
	}
}

