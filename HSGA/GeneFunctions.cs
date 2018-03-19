using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSGA
{
    public class GeneFunctions
    {
        // fitness 
        // selection
        // mutation 
        // crossover
        float winRateFitness = 0f;
        float legalityFitness = 0f;

        public void CalculateFitness(Dictionary<string, float> values, bool legality)
        {
            // calc deck legality fitness value
            // if not legal stop here as no need to eval further.
            if (legality != true)
            {
                legalityFitness = 1f;
                return;
            }
            else if (legality)
            {
                legalityFitness = -1f;
            }
            // calc winrate fitness value
            float winRate = values["Win Rate"];


        }
    }
}
