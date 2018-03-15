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
            this.GetAllCards = new System.Windows.Forms.Button();
            this.NeutralPathLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.TestValidationButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.GenDeckButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.GetAllSelectedCards = new System.Windows.Forms.Button();
            this.GenInitialPopulationButton = new System.Windows.Forms.Button();
            this.GenButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetAllCards
            // 
            this.GetAllCards.Location = new System.Drawing.Point(160, 37);
            this.GetAllCards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GetAllCards.Name = "GetAllCards";
            this.GetAllCards.Size = new System.Drawing.Size(98, 32);
            this.GetAllCards.TabIndex = 0;
            this.GetAllCards.Text = "Get All Cards";
            this.GetAllCards.UseVisualStyleBackColor = true;
            this.GetAllCards.Click += new System.EventHandler(this.GetAllCards_Click);
            // 
            // NeutralPathLabel
            // 
            this.NeutralPathLabel.AutoSize = true;
            this.NeutralPathLabel.Location = new System.Drawing.Point(518, 16);
            this.NeutralPathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.NeutralPathLabel.Name = "NeutralPathLabel";
            this.NeutralPathLabel.Size = new System.Drawing.Size(66, 13);
            this.NeutralPathLabel.TabIndex = 1;
            this.NeutralPathLabel.Text = "Current Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(407, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Num Of Cards:";
            // 
            // TestValidationButton
            // 
            this.TestValidationButton.Enabled = false;
            this.TestValidationButton.Location = new System.Drawing.Point(9, 16);
            this.TestValidationButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TestValidationButton.Name = "TestValidationButton";
            this.TestValidationButton.Size = new System.Drawing.Size(98, 45);
            this.TestValidationButton.TabIndex = 4;
            this.TestValidationButton.Text = "Test Deck Validation";
            this.TestValidationButton.UseVisualStyleBackColor = true;
            this.TestValidationButton.Click += new System.EventHandler(this.TestValidationButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Num Of Cards:";
            // 
            // GenDeckButton
            // 
            this.GenDeckButton.Location = new System.Drawing.Point(286, 37);
            this.GenDeckButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GenDeckButton.Name = "GenDeckButton";
            this.GenDeckButton.Size = new System.Drawing.Size(98, 50);
            this.GenDeckButton.TabIndex = 6;
            this.GenDeckButton.Text = "Generate Random Deck";
            this.GenDeckButton.UseVisualStyleBackColor = true;
            this.GenDeckButton.Click += new System.EventHandler(this.GenDeckButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Warrior",
            "Priest",
            "Shaman",
            "Paladin",
            "Rogue",
            "Mage",
            "Druid",
            "Hunter",
            "Warlock"});
            this.comboBox1.Location = new System.Drawing.Point(47, 364);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(146, 21);
            this.comboBox1.TabIndex = 7;
            // 
            // GetAllSelectedCards
            // 
            this.GetAllSelectedCards.Location = new System.Drawing.Point(75, 299);
            this.GetAllSelectedCards.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GetAllSelectedCards.Name = "GetAllSelectedCards";
            this.GetAllSelectedCards.Size = new System.Drawing.Size(98, 41);
            this.GetAllSelectedCards.TabIndex = 8;
            this.GetAllSelectedCards.Text = "Get All Selected Cards\r\n";
            this.GetAllSelectedCards.UseVisualStyleBackColor = true;
            this.GetAllSelectedCards.Click += new System.EventHandler(this.GetAllSelectedCards_Click);
            // 
            // GenInitialPopulationButton
            // 
            this.GenInitialPopulationButton.Location = new System.Drawing.Point(286, 106);
            this.GenInitialPopulationButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.GenInitialPopulationButton.Name = "GenInitialPopulationButton";
            this.GenInitialPopulationButton.Size = new System.Drawing.Size(98, 50);
            this.GenInitialPopulationButton.TabIndex = 9;
            this.GenInitialPopulationButton.Text = "Generate Initial Population";
            this.GenInitialPopulationButton.UseVisualStyleBackColor = true;
            this.GenInitialPopulationButton.Click += new System.EventHandler(this.GenInitialPopulationButton_Click);
            // 
            // GenButton
            // 
            this.GenButton.Location = new System.Drawing.Point(286, 242);
            this.GenButton.Name = "GenButton";
            this.GenButton.Size = new System.Drawing.Size(75, 47);
            this.GenButton.TabIndex = 10;
            this.GenButton.Text = "Begin Generation";
            this.GenButton.UseVisualStyleBackColor = true;
            this.GenButton.Click += new System.EventHandler(this.GenButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 505);
            this.Controls.Add(this.GenButton);
            this.Controls.Add(this.GenInitialPopulationButton);
            this.Controls.Add(this.GetAllSelectedCards);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GenDeckButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TestValidationButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NeutralPathLabel);
            this.Controls.Add(this.GetAllCards);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetAllCards;
        private System.Windows.Forms.Label NeutralPathLabel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TestValidationButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GenDeckButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button GetAllSelectedCards;
        private System.Windows.Forms.Button GenInitialPopulationButton;
        private System.Windows.Forms.Button GenButton;
    }
}

