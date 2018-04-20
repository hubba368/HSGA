using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HSGA
{
    public class GeneFunctions
    {
        public float optimumWinRate = 90f;
        public float optimumLegality = 0f;
        public float optimumStandardDeviation = 0f;

        public float winRateFitness { get; set; }
        public float legalityFitness { get; set; }
        public float standardDeviationFitness { get; set; }

        private Dictionary<int, float> totalVictories;


        float winRateSum = 0f;
        Random rand;

        List<float> standardDeviationValues = new List<float>();
        public List<HSGAIndividual> population;

        public GeneFunctions()
        {
            winRateFitness = 0f;
            legalityFitness = 0f;
            standardDeviationFitness = 0f;
            rand = new Random();
            totalVictories = new Dictionary<int, float>();
        }

        public void CalculateFitness(bool legality)
        {
            // calc deck legality fitness value
            // if not legal stop here as no need to eval further.
            if (legality != true)
            {
                legalityFitness = 1f;
                winRateFitness = 0f;
                standardDeviationFitness = 0f;
                return;
            }
            else if (legality)
            {
                legalityFitness = -1f;
            }
            // calc winrate fitness value
            winRateFitness = winRateSum;

            //calc standard deviation fitness value
            float variance = 0f;
            float mean = 0f;
            for(int i = 0; i < totalVictories.Count; i++)
            {
                // get total victories across all opponents
                mean += totalVictories[i];
            }
            // get mean victories
            mean = mean / 9f;

            for (int i = 0; i < totalVictories.Count; i++)
            {
                //get variance value
                float currentDifference = (totalVictories[i] - mean);
                variance += (float)Math.Pow(currentDifference, 2);
            }

            variance = variance / 9f;
            standardDeviationFitness = (float)Math.Sqrt(variance);
        }

        //warrior - 0
        //priest - 1
        //shaman - 2
        // paladin - 3
        // rogue - 4
        // mage - 5
        // druid - 6
        // hunter - 7
        // warlock -8

        public void CalculatePerGameStats(Dictionary<string, float> values, int index)
        {
            //this is called for each game an individual plays
            float winRate = values["Games Won"];
            float gamesWon = values["Games Won"];
            winRateSum += winRate;
            standardDeviationValues.Add(gamesWon);
            totalVictories.Add(index, standardDeviationValues[index]);
        }

        public HSGAIndividual SelectIndividual(List<HSGAIndividual> pop, GeneLogger l)
        {
            /*float winRateSum = 0f;
            float legalitySum = 0f;
            float deviationSum = 0f;

            for(int i = 0; i < pop.Count; i++)
            {
                winRateSum += pop[i].winRateFitness;
                legalitySum += pop[i].legalFitness;
                deviationSum += pop[i].standardDeviationFitness;
            }*/

            // check legality fitness first
            // retrieve a new list of individuals which are legal
            // we have no need of using illegal individuals 
            // because their overall fitness will be too low.
            List<HSGAIndividual> editedList = new List<HSGAIndividual>();
            
            for(int i = 0; i < pop.Count; i++)
            {
                if(pop[i].legalFitness < 0)
                {
                    editedList.Add(pop[i]);
                }
            }

            // K tournament selection
            // select 2 to 4 members from current population at random
            List<HSGAIndividual> tourneyList = new List<HSGAIndividual>();
            HSGAIndividual currentIndToCheck = new HSGAIndividual();

            int tourneyCount = 4;//rand.Next(1, 5);

            for(int j = 0; j < tourneyCount; j++)
            {
                int index = rand.Next(0, editedList.Count);
                tourneyList.Add(editedList[index]);
            }

            HSGAIndividual selectedParent = new HSGAIndividual();

            for (int p = 0; p < tourneyList.Count; p++)
            {
                currentIndToCheck = tourneyList[p];
                if(selectedParent.cardList == null)
                {
                    selectedParent = currentIndToCheck;
                }

                if(currentIndToCheck.winRateFitness > selectedParent.winRateFitness
                    && currentIndToCheck.standardDeviationFitness < selectedParent.standardDeviationFitness)
                {
                    selectedParent = currentIndToCheck;
                }
            }

            l.AddToLog("Parent Selected:" + selectedParent.winRateFitness);
            return selectedParent;

        }


        public List<HSGAIndividual> Crossover(List<HSGAIndividual> parents, float crossoverProb, GeneLogger l)
        {
            // child 1 left = parent 1, right = parent 2
            // child 2 left = parent 2, right = parent 1
            HSGAIndividual child1 = new HSGAIndividual();
            child1.cardList = new List<Card>();
            HSGAIndividual child2 = new HSGAIndividual();
            child2.cardList = new List<Card>();

            List<HSGAIndividual> children = new List<HSGAIndividual>();

            

            /* for(int i = 0; i < 30; i++)
             {
                 Card c = new Card("", "", "", "", "", "", "");
                 child1.cardList.Add(c);
                 child2.cardList.Add(c);
             }*/

            if (rand.NextDouble() <= crossoverProb)
            {
                l.AddToLog("Performing Crossover.");
                int point1 = rand.Next(0, parents[0].cardList.Count);

                if (point1 == 0)
                {
                    point1 = rand.Next(0, parents[0].cardList.Count);
                }

                // need to replace item in list            
                for (int i = 0; i <= point1; i++)
                {
                    child1.cardList.Add(parents[0].cardList[i]);
                    child2.cardList.Add(parents[1].cardList[i]);
                }
                for (int i = point1 + 1; i < parents[0].cardList.Count; i++)
                {
                    // set index to 1 above crossover point so we dont replace
                    // that card which is set in the previous for loop
                    child1.cardList.Add(parents[1].cardList[i]);
                    child2.cardList.Add(parents[0].cardList[i]);
                }

                children.Add(child1);
                children.Add(child2);
            }
            else
            {
                // coin flip to determine which parent to clone if 
                // crossover is not chosen.
                l.AddToLog("Not Performing Crossover.");
                //Console.WriteLine("Not Performing Crossover.");
                if (rand.NextDouble() > 0.5f)
                {
                    child1.cardList = parents[0].cardList;
                    child2.cardList = parents[0].cardList;
                }
                else
                {
                    child1.cardList = parents[1].cardList;
                    child2.cardList = parents[1].cardList;
                }

                children.Add(child1);
                children.Add(child2);

            }

            

            return children;
        }
    }
}
