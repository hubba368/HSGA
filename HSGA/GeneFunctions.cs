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

        public void CalculateAvgWinRate(Dictionary<string, float> values)
        {
            //this is called for each game an individual plays
            float winRate = values["Winrate"];
            winRateSum += winRate;
            standardDeviationValues.Add(winRate);
        }

        public HSGAIndividual SelectIndividual(List<HSGAIndividual> pop)
        {
            float winRateSum = 0f;
            float legalitySum = 0f;
            float deviationSum = 0f;

            for(int i = 0; i < pop.Count; i++)
            {
                winRateSum += pop[i].winRateFitness;
                legalitySum += pop[i].legalFitness;
                deviationSum += pop[i].standardDeviationFitness;
            }
            //binary tournament selection
            // select 2 members from current population at random
            int index = rand.Next(0, pop.Count);
            HSGAIndividual parent1 = pop[index];
            index = rand.Next(0, pop.Count);
            HSGAIndividual parent2 = pop[index];

            /* if(parent1 == parent2)
             {
                 index = rand.Next(0, pop.Count);
                 parent1 = pop[index];
             }*/

            // todo get fitness values and compare distance from optimum values per individual
            float p1WRProbability = optimumWinRate - parent1.winRateFitness;
            float p2WRProbability = optimumWinRate - parent2.winRateFitness;
            float p1SDProbability = optimumStandardDeviation - parent1.standardDeviationFitness;
            float p2SDProbability = optimumStandardDeviation - parent2.standardDeviationFitness;
            HSGAIndividual selectedParent = new HSGAIndividual();

            // check legality fitness first
            // if one is not legal then select the other regardless
            if(parent1.legalFitness > 0f || parent2.legalFitness > 0f)
            {
                return null;
            }

            if (parent1.legalFitness > 0f)
            {
                selectedParent = parent2;
            }
            if (parent2.legalFitness > 0f)
            {
                selectedParent = parent1;
            }
            // check other fitness
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
            // if probabilities are the same, return either parent
            if (p1WRProbability == p2WRProbability)
            {
                if (p1SDProbability == p2SDProbability)
                {
                    selectedParent = parent1;
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

            int point1 = rand.Next(0, parents[0].cardList.Count);
            //int point2 = rand.Next(0, parents[1].cardList.Count);

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
