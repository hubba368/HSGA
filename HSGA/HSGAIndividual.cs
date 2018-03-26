using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSGA
{
    /// <summary>
    /// Class that represents an Individual/Allele for 
    /// the genetic algorithm.
    /// This class is essentially just a single pre generated deck, with an assigned 
    /// fitness value.
    /// </summary>
    public class HSGAIndividual
    {
        public string deck;
        public List<Card> cardList = new List<Card>();

        //fitness values
        public float winRateFitness;
        public float legalFitness;
        public float standardDeviationFitness;
    }
}
