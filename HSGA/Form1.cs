using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace HSGA
{
    public partial class Form1 : Form
    {
        public string initialDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\CardsToBeDeserialized";
        public string deckDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\Decks"; //needs to be changed to metastone folder
        public CardJsonManager JSONHandler;


        public Form1()
        {
            InitializeComponent();
            //should init json manager here
            JSONHandler = new CardJsonManager();
        }

        private void GetNeutrals_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = initialDirectory;
            dialog.IsFolderPicker = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string currentPath = dialog.FileName;
                int numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;
                //MessageBox.Show("num of files in " + currentPath + ": " + numOfFiles);

                //deserialize all in directory
                JSONHandler.GetAllNeutrals(currentPath);

                //enable test
                decktest1.Enabled = true;
            }

        }

        private void decktest1_Click(object sender, EventArgs e)
        {
            JSONHandler.filePath = deckDirectory;
            JSONHandler.TestGenerateNewDeck();

            NeutralPathLabel.Text = JSONHandler.finalGeneratedDeck;
            label1.Text = JSONHandler.cardCount.ToString();
        }
    }
}
