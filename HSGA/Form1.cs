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
using System.Diagnostics;

namespace HSGA
{
    public partial class Form1 : Form
    {
        public string initialDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\CardsToBeDeserialized";
        public string deckDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\MetaStone Source\\metastone-master\\cards\\src\\main\\resources\\decks";

        public CardJsonManager JSONHandler;

        public HSGAIndividual GeneIndividual;
        public List<HSGAIndividual> GenePopulation;

        private int _MaxPopulation = 20;

        private string selectedClass;

        public Form1()
        {
            InitializeComponent();
            //should init json manager here
            JSONHandler = new CardJsonManager();
            GenePopulation = new List<HSGAIndividual>();
            GeneIndividual = new HSGAIndividual();

            // deserialize all cards on startup
            int numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;
            //deserialize all in directory
            JSONHandler.GetAllCards(numOfFiles, initialDirectory);
            //enable/disable the other buttons
            TestValidationButton.Enabled = true;
            GenButton.Enabled = false;
        }

        private void GetAllCards_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Generates a single, validated and randomised deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenDeckButton_Click(object sender, EventArgs e)
        {
            JSONHandler.filePath = deckDirectory;
            JSONHandler.GenerateRandomDeck();

            NeutralPathLabel.Text = JSONHandler.finalGeneratedDeck;
            label1.Text = JSONHandler.cardCount.ToString();
        }

        /// <summary>
        /// Generates the initial population, which is given a specified hero class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenInitialPopulationButton_Click(object sender, EventArgs e)
        {
            selectedClass = comboBox1.GetItemText(comboBox1.SelectedItem);
            JSONHandler.filePath = deckDirectory;

            // assemble the initial population
            // test each individual in the population
            for(int i = 0; i < _MaxPopulation; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    GeneIndividual.deck = JSONHandler.GenerateSpecificDeck(selectedClass);
                    // calculate the fitness value of the current individual by testing it in Metastone
                    GenerateAndValidatePopulation(GeneIndividual.deck);

                    //TODO:
                    // retreive the sim stats from text file.
                    // calc fitness - fitness function

                    // Add the individual to the population
                    GenePopulation.Add(GeneIndividual);
                }
            }
        }

        private void TestValidationButton_Click(object sender, EventArgs e)
        {
           label2.Text =  JSONHandler.TestValidation();
        }

        private void GetAllSelectedCards_Click(object sender, EventArgs e)
        {
            //CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //dialog.InitialDirectory = initialDirectory;
            //dialog.IsFolderPicker = true;

            int numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;

            selectedClass = comboBox1.GetItemText(comboBox1.SelectedItem);
            string currentPath = initialDirectory + "\\" + selectedClass;

            //deserialize all in directory
            JSONHandler.GetAllSelectedCards(currentPath, selectedClass);
        }


        private void GenerateAndValidatePopulation(string deck)
        {
            //Using a process object to allow for multiple commands to be 
            //input into the command line process.
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "CMD.exe";
            info.RedirectStandardInput = true;
            info.UseShellExecute = false;

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\MetaStone Source\\metastone-master");
                    sw.WriteLine("gradlew run");
                }
            }
            // Wait until the current console window is closed,
            // otherwise we get 30 instances running and cannot fully test each
            // deck in metastone.
            p.WaitForExit();
        }

        private void GenButton_Click(object sender, EventArgs e)
        {
           // GenerateAndValidatePopulation();
        }
    }
}
