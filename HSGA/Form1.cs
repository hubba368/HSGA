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

        public List<HSGAIndividual> GenePopulation;
        public List<HSGAIndividual> newPopulation;

        private int _MaxPopulation = 2;
        private float mutationProbability = 0.15f;
        private float similarCostProbability = 0.25f;

        private string selectedClass;

        private Random rand;
        private int numOfFiles;

        public Form1()
        {
            InitializeComponent();
            JSONHandler = new CardJsonManager();
            GenePopulation = new List<HSGAIndividual>();
            newPopulation = new List<HSGAIndividual>();
            rand = new Random();

            // deserialize all cards on startup
            numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;
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
                JSONHandler.GetAllCards(numOfFiles, initialDirectory);

                HSGAIndividual GeneIndividual = new HSGAIndividual();
                // Create the deck for the current individual.
                Tuple<string, List<Card>> t = new Tuple<string, List<Card>>("", null);
                t = JSONHandler.GenerateSpecificDeck(selectedClass);
                GeneIndividual.deck = t.Item1;
               // cList = JSONHandler.GetFinalDeckList();
                GeneIndividual.cardList = t.Item2;

                GeneFunctions gene = new GeneFunctions();

                // check if deck is legal - dont really need to do it here
                // because initial population will be legal regardless
                bool isLegal = JSONHandler.ValidateDeck(GeneIndividual.cardList);


                //if deck isnt legal, no need to test it in metastone
                if(isLegal == true)
                {
                    // test each individual against each hero class type.
                    for (int opponentNum = 0; opponentNum < 9; opponentNum++)
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

        //TODO: check if everything works
        // fix card at crossover point being replaced with card from second parent
        // remember to grab all cards after mutating
        // change selection to get one with lowest win rate prob

        private void ProduceNextGeneration()
        {
            List<string> cards = new List<string>();

            while (newPopulation.Count != 10)
            {
                GeneFunctions gene = new GeneFunctions();
                List<HSGAIndividual> parents = new List<HSGAIndividual>();
                List<HSGAIndividual> children = new List<HSGAIndividual>();

                // select parents
                parents.Add(gene.SelectIndividual(GenePopulation));
                parents.Add(gene.SelectIndividual(GenePopulation));
                //crossover
                children = gene.Crossover(parents);
                //mutation
                children[0].cardList = MutateIndividual(children[0].cardList);

                cards.Clear();
                for (int i = 0; i < children[0].cardList.Count; i++)
                {
                    cards.Add(children[0].cardList[i]._CardID);
                }

                children[1].cardList = MutateIndividual(children[1].cardList);

                cards.Clear();
                for (int i = 0; i < children[1].cardList.Count; i++)
                {
                    cards.Add(children[1].cardList[i]._CardID);
                }
                // add children to new population
                newPopulation.Add(children[0]);
                newPopulation.Add(children[1]);
            }
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

        // TODO add ability to mutate either neutral or class cards
        private List<Card> MutateIndividual(List<Card> ind)
        {
            float sameClassProbability = 0.5f;
            float chooseUpperOrLowerProb = 0.5f;

            GetCurrentDeckDuplicates(ind, comboBox1.SelectedText);

            for (int i = 0; i < ind.Count; i++)
            {
                if (rand.NextDouble() <= mutationProbability)
                {
                    // select card of similar mana cost
                    if(rand.NextDouble() <= similarCostProbability)
                    {
                        // select card from same class - either specific class or neutral
                        if(rand.NextDouble() <= sameClassProbability)
                        {
                            List<Card> tempClassList = JSONHandler.allCardsList.GetDeckClassType(ind[i]._CardClassType);
                            List<Card> tempNeutralList = JSONHandler.allCardsList.NeutralCardList;
                            Card temp = new Card("","","","","","","");
                            // get card upper and lower costs compared to actual cost
                            int currentCardCost = int.Parse(ind[i]._CardCost);
                            int lowerCardCost = currentCardCost - 1;
                            int higherCardCost = currentCardCost + 1;

                            if(currentCardCost == 1 || currentCardCost == 0)
                            {
                                lowerCardCost = 1;
                            }
                            if(currentCardCost >= 10)
                            {
                                // the tiny amount of cards that cost above 10 means
                                // we can just set the upper cost to 10
                                higherCardCost = 10;
                            }
                            // choose upper or lower cost
                            if(rand.NextDouble() <= chooseUpperOrLowerProb)
                            {
                                // choose either class or neutral cards
                                if(rand.NextDouble() <= 0.5)
                                {
                                    List<Card> t = tempClassList.FindAll(Card => int.Parse(Card._CardCost) == lowerCardCost);
                                    temp = t[rand.Next(0, t.Count)];
                                }
                                else
                                {
                                    List<Card> t = tempNeutralList.FindAll(Card => int.Parse(Card._CardCost) == lowerCardCost);
                                    temp = t[rand.Next(0, t.Count)];
                                }
                            }
                            else
                            {
                                if (rand.NextDouble() <= 0.5)
                                {
                                    List<Card> t = tempClassList.FindAll(Card => int.Parse(Card._CardCost) == higherCardCost);
                                    temp = t[rand.Next(0, t.Count)];
                                }
                                else
                                {
                                    List<Card> t = tempNeutralList.FindAll(Card => int.Parse(Card._CardCost) == higherCardCost);
                                    temp = t[rand.Next(0, t.Count)];
                                }
                            }
                            // replace current card with newly mutated card
                            ind[i] = temp;
                        }                    
                    }
                    // select class at random
                    else
                    {
                        if (rand.NextDouble() <= 0.5)
                        {
                            List<Card> tempList = JSONHandler.allCardsList.GetDeckClassType(ind[i]._CardClassType);
                            Card temp = new Card("", "", "", "", "", "", "");
                            temp = tempList[rand.Next(0, tempList.Count)];
                            ind[i] = temp;
                        }
                        else
                        {
                            List<Card> tempList = JSONHandler.allCardsList.NeutralCardList;
                            Card temp = new Card("", "", "", "", "", "", "");
                            temp = tempList[rand.Next(0, tempList.Count)];
                            ind[i] = temp;
                        }
                    }
                }
            }
            return ind;
        }

        /// <summary>
        /// Get the potential duplicates cards from the current deck
        /// We need to remove them from the card pool so we dont accidentally mutate
        /// and create an invalid deck.
        /// </summary>
        /// <param name="deckToCheck"></param>
        /// <param name="classType"></param>
        private void GetCurrentDeckDuplicates(List<Card> deckToCheck, string classType)
        {
            List<string> duplicatesList = new List<string>();
            int prevCardCount = 0;

            List<Card> test = deckToCheck.OrderBy(Card => Card._CardID.Split('_')[1].ToCharArray()[0])
                .ThenBy(Card => Card._CardID.Split('_').Count() >= 3 ? Card._CardID.Split('_')[2].ToCharArray()[0] : Card._CardID.Split('_')[1].ToCharArray()[0]).ToList();
            deckToCheck = test;

            for (int j = 0; j < deckToCheck.Count - 1; j++)
            {
                string cardToCompare = deckToCheck[j]._CardID;
                string cardToCompareRarity = deckToCheck[j]._CardRarity;

                for (int l = j+1; l < deckToCheck.Count - 1; l++)
                {
                    // get the next card and rarity in the list
                    string nextCard = deckToCheck[l]._CardID;
                    string nextCardRarity = deckToCheck[l]._CardRarity;

                    bool compareCheck = nextCard.Equals(cardToCompare);

                    if (compareCheck == true)
                    {
                        prevCardCount += 2;
                        duplicatesList.Add(deckToCheck[l]._CardID);
                    }

                    // remove all cards from both lists that are included in the current deck at least
                    // 2+ times.
                    for (int i = 0; i < duplicatesList.Count; i++)
                    {
                        JSONHandler.allCardsList.GetDeckClassType(classType).RemoveAll(Card => Card._CardID == duplicatesList[i]);
                        JSONHandler.allCardsList.NeutralCardList.RemoveAll(Card => Card._CardID == duplicatesList[i]);
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
