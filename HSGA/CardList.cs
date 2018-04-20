using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSGA
{
    /// <summary>
    /// Class that stores all the cards for each hero class.
    /// This is initialised first.
    /// </summary>
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
        //Function to choose a hero class for specific card types.
        public void Choose(string type, Card card)
        {
            switch (type)
            {
                case "DRUID":
                    DruidCardList.Add(card);
                    break;

                case "HUNTER":
                    HunterCardList.Add(card);
                    break;

                case "MAGE":
                    MageCardList.Add(card);
                    break;

                case "PALADIN":
                    PaladinCardList.Add(card);
                    break;

                case "PRIEST":
                    PriestCardList.Add(card);
                    break;

                case "ROGUE":
                    RogueCardList.Add(card);
                    break;

                case "SHAMAN":
                    ShamanCardList.Add(card);
                    break;

                case "WARRIOR":
                    WarriorCardList.Add(card);
                    break;

                case "WARLOCK":
                    WarlockCardList.Add(card);
                    break;

                default:
                    NeutralCardList.Add(card);
                    break;
            }
        }

        //Function to choose a hero class for deck generation.
        public List<Card> ChooseSpecificList(string type)
        {
            switch (type)
            {
                case "DRUID":
                    return DruidCardList;      

                case "HUNTER":
                    return HunterCardList;

                case "MAGE":
                    return MageCardList;

                case "PALADIN":
                    return PaladinCardList;

                case "PRIEST":
                    return PriestCardList;

                case "ROGUE":
                    return RogueCardList;

                case "SHAMAN":
                    return ShamanCardList;

                case "WARRIOR":
                    return WarriorCardList;

                case "WARLOCK":
                    return WarlockCardList;

                default:
                    return NeutralCardList;
            }
        }

        // Function to get a deck class type.
        public List<Card> GetDeckClassType(string type)
        {
            List<Card> returnList = new List<Card>();

            switch (type)
            {
                case "DRUID":
                    returnList = DruidCardList;
                    break;

                case "HUNTER":
                    returnList = HunterCardList;
                    break;

                case "MAGE":
                    returnList = MageCardList;
                    break;

                case "PALADIN":
                    returnList = PaladinCardList;
                    break;

                case "PRIEST":
                    returnList = PriestCardList;
                    break;

                case "ROGUE":
                    returnList = RogueCardList;
                    break;

                case "SHAMAN":
                    returnList = ShamanCardList;
                    break;

                case "WARRIOR":
                    returnList = WarriorCardList;
                    break;

                case "WARLOCK":
                    returnList = WarlockCardList;
                    break;

                default:
                    returnList = NeutralCardList;
                    break;
            }

            return returnList;
        }

        public string GetDeckClassTypeFromCard(string type)
        {
            string returnType = "";

            switch (type)
            {
                case "DRUID":
                    returnType = "DRUID";
                    break;

                case "HUNTER":
                    returnType = "HUNTER";
                    break;

                case "MAGE":
                    returnType = "MAGE";
                    break;

                case "PALADIN":
                    returnType = "PALADIN";
                    break;

                case "PRIEST":
                    returnType = "PRIEST";
                    break;

                case "ROGUE":
                    returnType = "ROGUE";
                    break;

                case "SHAMAN":
                    returnType = "SHAMAN";
                    break;

                case "WARRIOR":
                    returnType = "WARRIOR";
                    break;

                case "WARLOCK":
                    returnType = "WARLOCK";
                    break;

                default:
                    returnType = "NEUTRAL";
                    break;
            }

            return returnType;
        }

        public void DeleteAllLists()
        {
            NeutralCardList.Clear();
            DruidCardList.Clear();
            MageCardList.Clear();
            HunterCardList.Clear();
            PaladinCardList.Clear();
            PriestCardList.Clear();
            RogueCardList.Clear();
            ShamanCardList.Clear();
            WarlockCardList.Clear();
            WarriorCardList.Clear();
        }
    }
}
