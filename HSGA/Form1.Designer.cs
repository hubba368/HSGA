namespace HSGA
{
    partial class Form1
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
            this.GetNeutrals = new System.Windows.Forms.Button();
            this.NeutralPathLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.decktest1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // GetNeutrals
            // 
            this.GetNeutrals.Location = new System.Drawing.Point(244, 305);
            this.GetNeutrals.Name = "GetNeutrals";
            this.GetNeutrals.Size = new System.Drawing.Size(131, 40);
            this.GetNeutrals.TabIndex = 0;
            this.GetNeutrals.Text = "Get All Neutrals";
            this.GetNeutrals.UseVisualStyleBackColor = true;
            this.GetNeutrals.Click += new System.EventHandler(this.GetNeutrals_Click);
            // 
            // NeutralPathLabel
            // 
            this.NeutralPathLabel.AutoSize = true;
            this.NeutralPathLabel.Location = new System.Drawing.Point(555, 20);
            this.NeutralPathLabel.Name = "NeutralPathLabel";
            this.NeutralPathLabel.Size = new System.Drawing.Size(88, 17);
            this.NeutralPathLabel.TabIndex = 1;
            this.NeutralPathLabel.Text = "Current Path";
            // 
            // decktest1
            // 
            this.decktest1.Enabled = false;
            this.decktest1.Location = new System.Drawing.Point(244, 211);
            this.decktest1.Name = "decktest1";
            this.decktest1.Size = new System.Drawing.Size(131, 40);
            this.decktest1.TabIndex = 2;
            this.decktest1.Text = "Gen neutral deck";
            this.decktest1.UseVisualStyleBackColor = true;
            this.decktest1.Click += new System.EventHandler(this.decktest1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(443, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Num Of Cards:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 621);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.decktest1);
            this.Controls.Add(this.NeutralPathLabel);
            this.Controls.Add(this.GetNeutrals);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetNeutrals;
        private System.Windows.Forms.Label NeutralPathLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button decktest1;
        private System.Windows.Forms.Label label1;
    }
}

