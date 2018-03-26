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
using System.Text.RegularExpressions;

namespace HSGA
{
    public partial class Form1 : Form
    {
        public string initialDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\CardsToBeDeserialized";
        public string deckDirectory = "C:\\Users\\Elliott\\Desktop\\DissertationProjects2017_18\\metastone-master\\cards\\src\\main\\resources\\decks";

        public CardJsonManager JSONHandler;

        public HSGAIndividual GeneIndividual;
        public List<HSGAIndividual> GenePopulation;
        public List<HSGAIndividual> newPopulation;

        private int _MaxPopulation = 2;
        private float mutationProbability = 0.015f;
        private float similarCostProbability = 0.25f;

        private string selectedClass;

        private Random rand;

        public Form1()
        {
            InitializeComponent();
            JSONHandler = new CardJsonManager();
            GenePopulation = new List<HSGAIndividual>();
            newPopulation = new List<HSGAIndividual>();
            GeneIndividual = new HSGAIndividual();
            rand = new Random();

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
            ProduceNextGeneration();
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
                // Create the deck for the current individual.
                GeneIndividual.deck = JSONHandler.GenerateSpecificDeck(selectedClass);
                GeneIndividual.cardList = JSONHandler.finalGeneratedCardList;

                GeneFunctions gene = new GeneFunctions();

                // check if deck is legal - dont really need to do it here
                // because initial population will be legal regardless
                bool isLegal = JSONHandler.ValidateDeck(GeneIndividual.cardList);


                //if deck isnt legal, no need to test it in metastone
                if(isLegal == true)
                {
                    // test each individual against each hero class type.
                    for (int opponentNum = 0; opponentNum < 8; opponentNum++)
                    {
                        // send over individual and opponent class numbers
                        GenerateMetastoneValues(comboBox1.SelectedIndex, opponentNum);
                        // run gradlew run command in cmd in metastone directory
                        GenerateAndValidatePopulation(GeneIndividual.deck);


                        // retreive the sim stats from text file.
                        Dictionary<string, float> currentStats = new Dictionary<string, float>();
                        currentStats = ParseMetastoneResults();
                        // accumulate current win rate per game.
                        gene.CalculateAvgWinRate(currentStats);
                    }
                }
                // calc fitness - fitness function
                // calc legality
                gene.CalculateFitness(isLegal);
                GeneIndividual.winRateFitness = gene.winRateFitness;
                GeneIndividual.legalFitness = gene.legalityFitness;
                GeneIndividual.standardDeviationFitness = gene.standardDeviationFitness;

                // Add the individual to the population
                GenePopulation.Add(GeneIndividual);
            }
        }


        private void ProduceNextGeneration()
        {
            GeneFunctions gene = new GeneFunctions();
            List<HSGAIndividual> parents = new List<HSGAIndividual>();
            // select parents
            parents.Add(gene.SelectIndividual(GenePopulation));
            parents.Add(gene.SelectIndividual(GenePopulation));
            //crossover
            gene.Crossover(parents);
            //mutation
        }


        /// <summary>
        /// Retrieves post game stats which are generated as a text file
        /// </summary>
        /// <returns></returns>
        private Dictionary<string,float> ParseMetastoneResults()
        {
            //parse file
            string path = @"c:\Users\Elliott\Desktop\DissertationProjects2017_18\metastone-master\app\test.txt";

            if (File.Exists(path))
            {

                //get numerical stats from testing results file
                List<float> l = new List<float>();

                using (StreamReader sr = File.OpenText(path))
                {
                    string s = sr.ReadToEnd();
                    string[] numbers = Regex.Split(s, @"\D+");
                    foreach(string value in numbers)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            float i = float.Parse(value);
                            l.Add(i);
                        }
                    }
                }

                // add results to dictionary for easy formatting
                Dictionary<string, float> currentStats = new Dictionary<string, float>();
                currentStats.Add("Winrate", l[0]);
                currentStats.Add("Games Won", l[1]);
                currentStats.Add("Games Lost", l[2]);
                currentStats.Add("Damage Dealt", l[3]);
                currentStats.Add("Healing Done", l[4]);
                currentStats.Add("Mana Spent", l[5]);
                currentStats.Add("Cards Played", l[6]);
                currentStats.Add("Turns Taken", l[7]);
                currentStats.Add("Armour Gained", l[8]);
                currentStats.Add("Cards Drawn", l[9]);
                currentStats.Add("Fatigue Damage", l[10]);
                currentStats.Add("Minions Played", l[11]);
                currentStats.Add("Permanents Played", l[12]);
                currentStats.Add("Spells Cast", l[13]);
                currentStats.Add("Hero Power Used", l[14]);
                currentStats.Add("Weapons Equipped", l[15]);
                currentStats.Add("Weapons Played", l[16]);

                return currentStats;
            }
            else
            {
                return null;
            }
        }


        public HSGAIndividual MutateIndividual(HSGAIndividual ind)
        {
            // TODO: get new card, swap it with index in list
            for (int i = 0; i < ind.cardList.Count; i++)
            {
                if (rand.NextDouble() <= mutationProbability)
                {
                    if(rand.NextDouble() <= similarCostProbability)
                    {
                      //  Card temp = JSONHandler.allCardsList.

                    }
                    else
                    {

                    }
                }
            }
        }


        /// <summary>
        /// Generates hero class numbers for gene deck and for opponent as text file,
        /// This is then parsed within metastone to set up pre-game settings correctly.
        /// </summary>
        /// <param name="selectedClassNum"></param>
        /// <param name="opponentClassNum"></param>
        private void GenerateMetastoneValues(int selectedClassNum, int opponentClassNum)
        {
            string path = @"c:\Users\Elliott\Desktop\DissertationProjects2017_18\metastone-master\CurrentIndividual.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(selectedClassNum.ToString());
                    sw.WriteLine(opponentClassNum.ToString());
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
                    // this currently makes use of my directory, need to change to be user agnostic??
                    sw.WriteLine("cd C:\\Users\\Elliott\\Desktop\\DissertationProjects2017_18\\metastone-master");
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
