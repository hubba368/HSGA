using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HSGA
{
    public class GeneFunctions
    {
        public float optimumWinRate = 90f;
        public float optimumLegality = 0f;
        public float optimumStandardDeviation = 16f;

        public float winRateFitness { get; set; }
        public float legalityFitness { get; set; }
        public float standardDeviationFitness { get; set; }


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
            winRateFitness = winRateSum / 16f;

            //calc standard deviation fitness value
            float sum = 0f;
            for(int i = 0; i < standardDeviationValues.Count; i++)
            {
                //get variance value
                sum += (float)Math.Pow(standardDeviationValues[i], 2);
            }

            sum = sum / 16f;
            standardDeviationFitness = (float)Math.Sqrt(sum);
        }

        public void CalculatePerGameStats(Dictionary<string, float> values)
        {
            //this is called for each game an individual plays
            float winRate = values["Winrate"];
            winRateSum += winRate;
            standardDeviationValues.Add(winRate);
        }

        public HSGAIndividual SelectIndividual(List<HSGAIndividual> pop)
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

            //binary tournament selection
            // select 2 members from current population at random
            int index = rand.Next(0, editedList.Count);
            HSGAIndividual parent1 = editedList[index];
            index = rand.Next(0, editedList.Count);
            HSGAIndividual parent2 = editedList[index];

            // todo get fitness values and compare distance from optimum values per individual
            float p1WRProbability = 0 + parent1.winRateFitness;
            float p2WRProbability = 0 + parent2.winRateFitness;
            float p1SDProbability = 0 + parent1.standardDeviationFitness;
            float p2SDProbability = 0 + parent2.standardDeviationFitness;
            HSGAIndividual selectedParent = new HSGAIndividual();



            // check other fitness
            // we need the individual with the highest fitness
            if (p1WRProbability > p2WRProbability)
            {
                if (p1SDProbability > p2SDProbability)
                {
                    selectedParent = parent1;
                }
            }
            if (p2WRProbability > p1WRProbability)
            {
                if (p2SDProbability > p1SDProbability)
                {
                    selectedParent = parent2;
                }
            }

            if (p1WRProbability == p2WRProbability)
            {
                // if both values are the same, return either parent
                if (p1SDProbability == p2SDProbability)
                {
                    selectedParent = parent1;
                }

                if(p1SDProbability > p2SDProbability)
                {
                    selectedParent = parent1;
                }
                else
                {
                    selectedParent = parent2;
                }
            }

            return selectedParent;


        }

        public List<HSGAIndividual> Crossover(List<HSGAIndividual> parents)
        {
            // child 1 left = parent 1, right = parent 2
            // child 2 left = parent 2, right = parent 1
            HSGAIndividual child1 = new HSGAIndividual();
            child1.cardList = new List<Card>();
            HSGAIndividual child2 = new HSGAIndividual();
            child2.cardList = new List<Card>();

           /* for(int i = 0; i < 30; i++)
            {
                Card c = new Card("", "", "", "", "", "", "");
                child1.cardList.Add(c);
                child2.cardList.Add(c);
            }*/

            int point1 = rand.Next(3, parents[0].cardList.Count);

            if(point1 == 0)
            {
                //point1 = rand.Next()
            }

            // need to replace item in list            
            for(int i = 0; i <= point1; i++)
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

            List<HSGAIndividual> children = new List<HSGAIndividual>();
            children.Add(child1);
            children.Add(child2);

            return children;
        }
    }
}
