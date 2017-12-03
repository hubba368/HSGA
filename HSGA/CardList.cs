using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSGA
{
    public class CardList
    {
        public List<Card> NeutralCardList;
        public List<Card> DruidCardList;
        public List<Card> MageCardList;
        public List<Card> HunterCardList;
        public List<Card> PaladinCardList;
        public List<Card> PriestCardList;
        public List<Card> RogueCardList;
        public List<Card> ShamanCardList;
        public List<Card> WarlockCardList;
        public List<Card> WarriorCardList;

        public CardList()
        {
            NeutralCardList = new List<Card>();
            DruidCardList = new List<Card>();
            MageCardList = new List<Card>();
            HunterCardList = new List<Card>();
            PaladinCardList = new List<Card>();
            PriestCardList = new List<Card>();
            RogueCardList = new List<Card>();
            ShamanCardList = new List<Card>();
            WarlockCardList = new List<Card>();
            WarriorCardList = new List<Card>();
        }
    }
}
