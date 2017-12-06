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
            this.GenSpecificDeckButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GetAllCards
            // 
            this.GetAllCards.Location = new System.Drawing.Point(214, 46);
            this.GetAllCards.Name = "GetAllCards";
            this.GetAllCards.Size = new System.Drawing.Size(131, 40);
            this.GetAllCards.TabIndex = 0;
            this.GetAllCards.Text = "Get All Cards";
            this.GetAllCards.UseVisualStyleBackColor = true;
            this.GetAllCards.Click += new System.EventHandler(this.GetAllCards_Click);
            // 
            // NeutralPathLabel
            // 
            this.NeutralPathLabel.AutoSize = true;
            this.NeutralPathLabel.Location = new System.Drawing.Point(690, 20);
            this.NeutralPathLabel.Name = "NeutralPathLabel";
            this.NeutralPathLabel.Size = new System.Drawing.Size(88, 17);
            this.NeutralPathLabel.TabIndex = 1;
            this.NeutralPathLabel.Text = "Current Path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(543, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Num Of Cards:";
            // 
            // TestValidationButton
            // 
            this.TestValidationButton.Enabled = false;
            this.TestValidationButton.Location = new System.Drawing.Point(12, 20);
            this.TestValidationButton.Name = "TestValidationButton";
            this.TestValidationButton.Size = new System.Drawing.Size(131, 55);
            this.TestValidationButton.TabIndex = 4;
            this.TestValidationButton.Text = "Test Deck Validation";
            this.TestValidationButton.UseVisualStyleBackColor = true;
            this.TestValidationButton.Click += new System.EventHandler(this.TestValidationButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Num Of Cards:";
            // 
            // GenDeckButton
            // 
            this.GenDeckButton.Location = new System.Drawing.Point(381, 46);
            this.GenDeckButton.Name = "GenDeckButton";
            this.GenDeckButton.Size = new System.Drawing.Size(131, 61);
            this.GenDeckButton.TabIndex = 6;
            this.GenDeckButton.Text = "Generate Random Deck";
            this.GenDeckButton.UseVisualStyleBackColor = true;
            this.GenDeckButton.Click += new System.EventHandler(this.GenDeckButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Druid",
            "Hunter",
            "Mage",
            "Paladin",
            "Priest",
            "Rogue",
            "Shaman",
            "Warlock",
            "Warrior"});
            this.comboBox1.Location = new System.Drawing.Point(45, 312);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(193, 24);
            this.comboBox1.TabIndex = 7;
            // 
            // GetAllSelectedCards
            // 
            this.GetAllSelectedCards.Location = new System.Drawing.Point(82, 234);
            this.GetAllSelectedCards.Name = "GetAllSelectedCards";
            this.GetAllSelectedCards.Size = new System.Drawing.Size(131, 50);
            this.GetAllSelectedCards.TabIndex = 8;
            this.GetAllSelectedCards.Text = "Get All Selected Cards\r\n";
            this.GetAllSelectedCards.UseVisualStyleBackColor = true;
            this.GetAllSelectedCards.Click += new System.EventHandler(this.GetAllSelectedCards_Click);
            // 
            // GenSpecificDeckButton
            // 
            this.GenSpecificDeckButton.Location = new System.Drawing.Point(381, 130);
            this.GenSpecificDeckButton.Name = "GenSpecificDeckButton";
            this.GenSpecificDeckButton.Size = new System.Drawing.Size(131, 61);
            this.GenSpecificDeckButton.TabIndex = 9;
            this.GenSpecificDeckButton.Text = "Generate Deck of Specific Class\r\n";
            this.GenSpecificDeckButton.UseVisualStyleBackColor = true;
            this.GenSpecificDeckButton.Click += new System.EventHandler(this.GenSpecificDeckButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 621);
            this.Controls.Add(this.GenSpecificDeckButton);
            this.Controls.Add(this.GetAllSelectedCards);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.GenDeckButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TestValidationButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NeutralPathLabel);
            this.Controls.Add(this.GetAllCards);
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
        private System.Windows.Forms.Button GenSpecificDeckButton;
    }
}

