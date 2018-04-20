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

        public List<HSGAIndividual> currentPopulation;
        public List<HSGAIndividual> newPopulation;

        private int _MaxGenerations = 50;
        private int _MaxPopulation = 10;
        private float mutationProbability = 0.9f;
        private float similarCostProbability = 0.25f;
        private float crossoverProbability = 0.75f;

        private string selectedClass;
        string generationDirectory = "C:\\Users\\Elliott\\Documents\\Visual Studio 2017\\Projects\\HSGA\\Assets\\Generations";

        private Random rand;
        private int numOfFiles;

        public GeneLogger logger;



        public Form1()
        {
            InitializeComponent();
            JSONHandler = new CardJsonManager();
            currentPopulation = new List<HSGAIndividual>();
            newPopulation = new List<HSGAIndividual>();
            rand = new Random();
            logger = new GeneLogger(listBox1);

            // deserialize all cards on startup
            numOfFiles = Directory.GetDirectories(initialDirectory, "*", SearchOption.TopDirectoryOnly).Length;
            //deserialize all in directory
            JSONHandler.GetAllCards(numOfFiles, initialDirectory);
            JSONHandler.filePath = deckDirectory;
            //enable/disable the other buttons
            TestValidationButton.Enabled = true;
            GenButton.Enabled = true;
        }


        private void GetAllCards_Click(object sender, EventArgs e)
        {
            /* Directory.CreateDirectory(generationDirectory + "\\Generation" + 1);
             ProduceNextGeneration();
             TestNewPopulation();*/
          //  ProduceGenerationStats(0);
        }


        /// <summary>
        /// Generates a single, validated and randomised deck.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenDeckButton_Click(object sender, EventArgs e)
        {
            GeneFunctions g = new GeneFunctions();
            List<HSGAIndividual> l = new List<HSGAIndividual>();
            l.Add(new HSGAIndividual());
            l.Add(new HSGAIndividual());
            l.Add(new HSGAIndividual());
            l.Add(new HSGAIndividual());
            l[0].winRateFitness = 25f;
            l[0].standardDeviationFitness = 1.2f;
            l[0].legalFitness = -1f;
            l[1].winRateFitness = 43f;
            l[1].standardDeviationFitness = 1.1f;
            l[1].legalFitness = -1f;
            l[2].winRateFitness = 50f;
            l[2].standardDeviationFitness = 2.0f;
            l[2].legalFitness = -1f;
            l[3].winRateFitness = 5f;
            l[3].standardDeviationFitness = 0.11f;
            l[3].legalFitness = -1f;
            g.SelectIndividual(l, logger);
        }


        /// <summary>
        /// Generates the initial population, which is given a specified hero class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenInitialPopulationButton_Click(object sender, EventArgs e)
        {
            // remove all previous generation files
            DirectoryInfo di = new DirectoryInfo(generationDirectory);
            foreach(DirectoryInfo file in di.EnumerateDirectories())
            {
                file.Delete(true);
            }

            logger.AddToLog("Initialising Logger.");
            logger.AddToLog("Creating Initial Generation.");

            int currentGenerationNum = 0;
            List<HSGAIndividual> currentPop = new List<HSGAIndividual>();

            Directory.CreateDirectory(generationDirectory + "\\Generation" + currentGenerationNum);
            currentPop = ProduceInitialPopulation(currentGenerationNum);
            ProduceGenerationStats(0, currentPop);

            for (currentGenerationNum = 1; currentGenerationNum < _MaxGenerations; currentGenerationNum++)
            {
                mutationProbability -= 0.01f;
                newPopulation.Clear();
                Directory.CreateDirectory(generationDirectory + "\\Generation" + currentGenerationNum);
                newPopulation = ProduceNextGeneration(currentPop);
                currentPop.Clear();
                currentPop.AddRange(newPopulation);

                logger.AddToLog("Beginning testing of Generation " + currentGenerationNum);

                TestNewPopulation(currentPop, currentGenerationNum);
                ProduceGenerationStats(currentGenerationNum, currentPop);
            }
            if(currentGenerationNum == 50)
            {
                MessageBox.Show("Generation Complete.");
            }
        }

        //TODO
        // check if it works
        // may be errors if both pops are illegal due to low pop count
        // may not occur when full population numbers
        // MAYBE RUN 2 AT A TIME TO COMPARE SINGLE POINT OVER K POINT CROSSOVER


        private void ProduceGenerationStats(int generationNum, List<HSGAIndividual> pop)
        {
            string path = generationDirectory + "\\Generation" + generationNum;
            int filesNum = Directory.GetDirectories(generationDirectory, "*", SearchOption.TopDirectoryOnly).Length;
            float winRateSum = 0f;
            //float legalSum = 0f;
            float stdSum = 0f;

            // calculate avg generation stats
            for (int i = 0; i < 9; i++)
            {
                string currentPath = generationDirectory + "\\Generation" + generationNum + "\\Individual" + i + ".txt";
                string[] data = File.ReadAllLines(currentPath);
                string wr = data[0].Split(':')[1].ToString();
                string lg = data[1].Split(':')[1].ToString();
                string sd = data[2].Split(':')[1].ToString();
                winRateSum += float.Parse(wr);
                stdSum += float.Parse(sd);
            }

            float winRateAvg = winRateSum / 9;
            float stdAvg = stdSum / 9;

            // get the best and worst individuals
            HSGAIndividual bestInd = null;
            HSGAIndividual worstInd = null;

            for(int j = 0; j < pop.Count; j++)
            {
                float bestWr = pop.Max(HSGAIndividual => HSGAIndividual.winRateFitness);
                //float bestSd = pop.Max(HSGAIndividual => HSGAIndividual.standardDeviationFitness);
                float worstWr = pop.Min(HSGAIndividual => HSGAIndividual.winRateFitness);
                //float worstSd = pop.Min(HSGAIndividual => HSGAIndividual.standardDeviationFitness);

                bestInd = pop.First(HSGAIndividual => HSGAIndividual.winRateFitness == bestWr);
                worstInd = pop.First(HSGAIndividual => HSGAIndividual.winRateFitness == worstWr);
            }

            using (StreamWriter w = File.CreateText(generationDirectory + "\\Generation" + generationNum + "\\_GenerationAvgStats.txt"))
            {
                w.WriteLine("Total Wins:" + winRateAvg);
                w.WriteLine("Standard Deviation Average:" + stdAvg);
                w.WriteLine();

                w.WriteLine("Best Individual:");
                w.WriteLine("Total Wins:" + bestInd.winRateFitness);
                w.WriteLine("Standard Deviation:" + bestInd.standardDeviationFitness);
                w.WriteLine("Decklist:");
                for (int j = 0; j < bestInd.cardList.Count; j++)
                {
                    string s = bestInd.cardList[j]._CardID;
                    w.WriteLine(s);
                }
                w.WriteLine();

                w.WriteLine("Worst Individual:");
                w.WriteLine("Total Wins:" + worstInd.winRateFitness);
                w.WriteLine("Standard Deviation:" + worstInd.standardDeviationFitness);
                w.WriteLine("Decklist:");
                for (int j = 0; j < worstInd.cardList.Count; j++)
                {
                    string s = worstInd.cardList[j]._CardID;
                    w.WriteLine(s);
                }
            }

        }

        private void GenButton_Click(object sender, EventArgs e)
        {
            mutationProbability = 0.78f;
            RecreatePreviousPopulation();
            ContinueGenerations(currentPopulation);
        }


        /// <summary>
        /// Recreates the population from the previously created population
        /// Needed this in case program crashed and wanted to continue where 
        /// it left off.
        /// </summary>
        private void RecreatePreviousPopulation()
        {
            for(int i = 0; i < 10; i++)
            {
                string currentPath = generationDirectory + "\\Generation" + 11 + "\\Individual" + i + ".txt";
                string ctype = "Shaman";
                List<string> deck = new List<string>();
                HSGAIndividual temp = new HSGAIndividual();

                //get numerical stats from file
                List<float> l = new List<float>();

                using (StreamReader sr = File.OpenText(currentPath))
                {
                    string s = sr.ReadToEnd();
                    string[] numbers = Regex.Split(s, @"[^-?\d+\.]");
                    foreach (string value in numbers)
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            if(l.Count == 3)
                            {
                                break;
                            }
                            float h = float.Parse(value);
                            l.Add(h);
                        }
                    }
                }

                temp.winRateFitness = l[0];
                temp.legalFitness = l[1];
                temp.standardDeviationFitness = l[2];

                // get deck of card IDs
                deck = File.ReadLines(currentPath).ToList();
                deck.RemoveRange(0, 5);
                // recreate current population
                Tuple<string, List<Card>> t = new Tuple<string, List<Card>>("", null);
                t = JSONHandler.RecreateIndividualFromFile(deck, ctype);

                temp.deck = t.Item1;
                temp.cardList = t.Item2;
                currentPopulation.Add(temp);
            }
            
        }

        private void ContinueGenerations(List<HSGAIndividual> current)
        {
            string filePath = deckDirectory;

            int currentGenerationNum = 21;
            logger.AddToLog("Resuming from Generation" + currentGenerationNum +".");


            List<HSGAIndividual> currentPop = new List<HSGAIndividual>();
            currentPop.AddRange(current);

            for (currentGenerationNum = 22; currentGenerationNum < _MaxGenerations; currentGenerationNum++)
            {
                //mutationProbability -= 0.01f;
                newPopulation.Clear();
                Directory.CreateDirectory(generationDirectory + "\\Generation" + currentGenerationNum);
                newPopulation = ProduceNextGeneration(currentPop);
                currentPop.Clear();
                currentPop.AddRange(newPopulation);

                logger.AddToLog("Beginning testing of Generation " + currentGenerationNum);

                TestNewPopulation(currentPop, currentGenerationNum);
                ProduceGenerationStats(currentGenerationNum, currentPop);
            }
            if (currentGenerationNum == 50)
            {
                MessageBox.Show("Generation Complete.");
            }



        }


        private List<HSGAIndividual> ProduceInitialPopulation(int initialGenNum)
        {
            selectedClass = comboBox1.GetItemText(comboBox1.SelectedItem);
            JSONHandler.filePath = deckDirectory;
            List<HSGAIndividual> currentPop = new List<HSGAIndividual>();

            // assemble the initial population
            // test each individual in the population
            for (int i = 0; i < _MaxPopulation; i++)
            {
                JSONHandler.RemoveAllCards();
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
                if (isLegal == true)
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
                        gene.CalculatePerGameStats(currentStats, opponentNum);
                    }
                }
                // calc fitness - fitness function
                // calc legality
                gene.CalculateFitness(isLegal);
                GeneIndividual.winRateFitness = gene.winRateFitness;
                GeneIndividual.legalFitness = gene.legalityFitness;
                GeneIndividual.standardDeviationFitness = gene.standardDeviationFitness;

                // Add the individual to the population
                currentPop.Add(GeneIndividual);

                // send individual stats to its correspondent folder.
                using (StreamWriter w = File.CreateText(generationDirectory + "\\Generation" + initialGenNum +"\\Individual" + i + ".txt"))
                {
                    w.WriteLine("Total Wins: " + GeneIndividual.winRateFitness);
                    w.WriteLine("Legality: " + GeneIndividual.legalFitness);
                    w.WriteLine("Standard Deviation: " + GeneIndividual.standardDeviationFitness);
                    w.WriteLine();
                    w.WriteLine("Decklist:");
                    for(int j = 0; j < GeneIndividual.cardList.Count; j++)
                    {
                        string s = GeneIndividual.cardList[j]._CardID;
                        w.WriteLine(s);
                    }
                }
            }

            return currentPop;
        }


        /// <summary>
        /// Produces offspring decks from the current population.
        /// </summary>
        private List<HSGAIndividual> ProduceNextGeneration(List<HSGAIndividual> localCurrentPop)
        {
            List<string> cards = new List<string>();
            List<HSGAIndividual> localNewPop = new List<HSGAIndividual>();


            while (localNewPop.Count != 10)
            {
                GeneFunctions gene = new GeneFunctions();
                List<HSGAIndividual> parents = new List<HSGAIndividual>();
                List<HSGAIndividual> children = new List<HSGAIndividual>();

                // select parents
                parents.Add(gene.SelectIndividual(localCurrentPop, logger));
                parents.Add(gene.SelectIndividual(localCurrentPop, logger));



                //crossover
                children = gene.Crossover(parents, crossoverProbability, logger);
                //mutation
                children[0].cardList = MutateIndividual(children[0].cardList);

                // sort the newly edited list
                List<Card> test = new List<Card>();
                test.AddRange(children[0].cardList.OrderBy(Card => Card._CardID.Split('_')[1].ToCharArray()[0])
    .ThenBy(Card => Card._CardID.Split('_').Count() >= 3 ? Card._CardID.Split('_')[2].ToCharArray()[0] : Card._CardID.Split('_')[1].ToCharArray()[0]).ToList());
                children[0].cardList.Clear();
                children[0].cardList.AddRange(test);

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
                localNewPop.Add(children[0]);
                localNewPop.Add(children[1]);
            }
            return localNewPop;
        }


        /// <summary>
        /// Tests the newly generated population in Metastone.
        /// </summary>
        private void TestNewPopulation(List<HSGAIndividual> currentPop, int currentGenerationNum)
        {
            for (int i = 0; i < _MaxPopulation; i++)
            {
                // get back all the cards we removed from the lists during
                // previous mutations/validations
                JSONHandler.RemoveAllCards();
                JSONHandler.GetAllCards(numOfFiles, initialDirectory);

                // Reassemble the deck string for the current individual.
                currentPop[i].deck = JSONHandler.GenerateDeckString(currentPop[i], comboBox1.Text);

                GeneFunctions gene = new GeneFunctions();

                // check if deck is legal
                bool isLegal = JSONHandler.ValidateDeck(currentPop[i].cardList);

                //if deck isnt legal, no need to test it in metastone
                if (isLegal == true)
                {
                    // test each individual against each hero class type.
                    for (int opponentNum = 0; opponentNum < 9; opponentNum++)
                    {
                        // send over individual and opponent class numbers
                        GenerateMetastoneValues(comboBox1.SelectedIndex, opponentNum);
                        // run gradlew run command in cmd in metastone directory
                        GenerateAndValidatePopulation(currentPop[i].deck);


                        // retreive the sim stats from text file.
                        Dictionary<string, float> currentStats = new Dictionary<string, float>();
                        currentStats = ParseMetastoneResults();
                        // accumulate current win rate per game.
                        gene.CalculatePerGameStats(currentStats, opponentNum);
                    }
                }
                // calc fitness - fitness function
                // calc legality
                gene.CalculateFitness(isLegal);
                currentPop[i].winRateFitness = gene.winRateFitness;
                currentPop[i].legalFitness = gene.legalityFitness;
                currentPop[i].standardDeviationFitness = gene.standardDeviationFitness;


                // send individual stats to its correspondent folder.
                using (StreamWriter w = File.CreateText(generationDirectory + "\\Generation" + currentGenerationNum + "\\Individual" + i + ".txt"))
                {
                    w.WriteLine("Total Wins: " + currentPop[i].winRateFitness);
                    w.WriteLine("Legality: " + currentPop[i].legalFitness);
                    w.WriteLine("Standard Deviation: " + currentPop[i].standardDeviationFitness);
                    w.WriteLine();
                    w.WriteLine("Decklist:");
                    for (int j = 0; j < currentPop[i].cardList.Count; j++)
                    {
                        string s = currentPop[i].cardList[j]._CardID;
                        w.WriteLine(s);
                    }
                }
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
                    string[] numbers = Regex.Split(s, @"[^-?\d+\.]");
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
            bool mutate = true;
            float sameClassProbability = 0.5f;
            float chooseUpperOrLowerProb = 0.5f;
            int mutationCounter = 0;

            GetCurrentDeckDuplicates(ind, comboBox1.SelectedText);
            
            while(mutate == true)
            {
                if (rand.NextDouble() <= mutationProbability)
                {
                    int i = rand.Next(0, ind.Count);
                    // select card of similar mana cost
                    if (rand.NextDouble() <= similarCostProbability)
                    {
                        // select card from same class - either specific class or neutral
                        if (rand.NextDouble() <= sameClassProbability)
                        {
                            List<Card> tempClassList = JSONHandler.allCardsList.GetDeckClassType(ind[i]._CardClassType);
                            List<Card> tempNeutralList = JSONHandler.allCardsList.NeutralCardList;
                            Card temp = new Card("", "", "", "", "", "", "");
                            // get card upper and lower costs compared to actual cost
                            int currentCardCost = int.Parse(ind[i]._CardCost);
                            int lowerCardCost = currentCardCost - 1;
                            int higherCardCost = currentCardCost + 1;

                            if (currentCardCost == 1 || currentCardCost == 0)
                            {
                                lowerCardCost = 1;
                            }
                            if (currentCardCost >= 10)
                            {
                                // the tiny amount of cards that cost above 10 means
                                // we can just set the upper cost to 10
                                higherCardCost = 10;
                            }
                            // choose upper or lower cost
                            if (rand.NextDouble() <= chooseUpperOrLowerProb)
                            {
                                // choose either class or neutral cards
                                if (rand.NextDouble() <= 0.5)
                                {
                                    List<Card> t = tempClassList.FindAll(Card => int.Parse(Card._CardCost) == lowerCardCost);
                                    temp = t[rand.Next(0, t.Count)];
                                    // if our temp list has no cards of required cost,
                                    // choose at random
                                    if (t.Count == 0)
                                    {
                                        temp = tempClassList[rand.Next(0, tempClassList.Count)];
                                    }
                                    else
                                    {
                                        temp = t[rand.Next(0, t.Count)];
                                    }
                                }
                                else
                                {
                                    List<Card> t = tempNeutralList.FindAll(Card => int.Parse(Card._CardCost) == lowerCardCost);

                                    if (t.Count == 0)
                                    {
                                        temp = tempNeutralList[rand.Next(0, tempNeutralList.Count)];
                                    }
                                    else
                                    {
                                        temp = t[rand.Next(0, t.Count)];
                                    }
                                }
                            }
                            else
                            {
                                if (rand.NextDouble() <= 0.5)
                                {
                                    List<Card> t = tempClassList.FindAll(Card => int.Parse(Card._CardCost) == higherCardCost);
                                    if (t.Count == 0)
                                    {
                                        temp = tempClassList[rand.Next(0, tempClassList.Count)];
                                    }
                                    else
                                    {
                                        temp = t[rand.Next(0, t.Count)];
                                    }
                                }
                                else
                                {
                                    List<Card> t = tempNeutralList.FindAll(Card => int.Parse(Card._CardCost) == higherCardCost);

                                    if (t.Count == 0)
                                    {
                                        temp = tempNeutralList[rand.Next(0, tempNeutralList.Count)];
                                    }
                                    else
                                    {
                                        temp = t[rand.Next(0, t.Count)];
                                    }
                                }
                            }
                            // replace current card with newly mutated card
                            ind[i] = temp;
                        }
                    }
                    // select class and cost at random
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

                    if(rand.NextDouble() <= mutationProbability)
                    {
                        mutate = true;
                        mutationCounter++;
                    }
                    else
                    {
                        mutate = false;
                    }
                }
            }

            logger.AddToLog("Mutation Count: " + mutationCounter);

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

        /// <summary>
        /// Runs Metastone from the Command Line.
        /// </summary>
        /// <param name="deck"></param>
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
    }
}
