namespace SIL.PrepFLExDB
{
	partial class PrepFLExDBForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrepFLExDBForm));
			this.lblSelectFWDB = new System.Windows.Forms.Label();
			this.btnOpenChooser = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnProcess = new System.Windows.Forms.Button();
			this.lblDatabaseToUse = new System.Windows.Forms.Label();
			this.lblStatus = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblSelectFWDB
			// 
			this.lblSelectFWDB.AutoSize = true;
			this.lblSelectFWDB.Location = new System.Drawing.Point(30, 49);
			this.lblSelectFWDB.Name = "lblSelectFWDB";
			this.lblSelectFWDB.Size = new System.Drawing.Size(212, 20);
			this.lblSelectFWDB.TabIndex = 0;
			this.lblSelectFWDB.Text = "Select FieldWorks database:";
			// 
			// btnOpenChooser
			// 
			this.btnOpenChooser.Location = new System.Drawing.Point(364, 40);
			this.btnOpenChooser.Name = "btnOpenChooser";
			this.btnOpenChooser.Size = new System.Drawing.Size(125, 41);
			this.btnOpenChooser.TabIndex = 1;
			this.btnOpenChooser.Text = "Choose";
			this.btnOpenChooser.UseVisualStyleBackColor = true;
			this.btnOpenChooser.Click += new System.EventHandler(this.btnOpenChooser_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(449, 226);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(107, 41);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnProcess
			// 
			this.btnProcess.Location = new System.Drawing.Point(319, 227);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.Size = new System.Drawing.Size(108, 41);
			this.btnProcess.TabIndex = 4;
			this.btnProcess.Text = "Process";
			this.btnProcess.UseVisualStyleBackColor = true;
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// lblDatabaseToUse
			// 
			this.lblDatabaseToUse.AutoSize = true;
			this.lblDatabaseToUse.Location = new System.Drawing.Point(30, 114);
			this.lblDatabaseToUse.Name = "lblDatabaseToUse";
			this.lblDatabaseToUse.Size = new System.Drawing.Size(135, 20);
			this.lblDatabaseToUse.TabIndex = 5;
			this.lblDatabaseToUse.Text = "Database chosen";
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(34, 152);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(56, 20);
			this.lblStatus.TabIndex = 6;
			this.lblStatus.Text = "Status";
			// 
			// PrepFLExDBForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(598, 298);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.lblDatabaseToUse);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOpenChooser);
			this.Controls.Add(this.lblSelectFWDB);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PrepFLExDBForm";
			this.Text = "Prepare FLEx database for PC-PATR";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSelectFWDB;
		private System.Windows.Forms.Button btnOpenChooser;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnProcess;
		private System.Windows.Forms.Label lblDatabaseToUse;
		private System.Windows.Forms.Label lblStatus;
	}
}

