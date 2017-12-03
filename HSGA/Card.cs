﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSGA
{
    //Card Layout//
    // Card ID - string 
    // Card Type - string
    // Card Class Type - string
    // Card Rarity - string - used to check for legendary cards
    // which have can only be one instance per card per deck.
    // file name - need this as there are some card files that have
    // no card ID, however the ID is the name of the file instead


    public class Card
    {
        public string _CardID { get; set; }
        public string _CardType { get; set; }
        public string _CardClassType { get; set; }
        public string _CardRarity { get; set; }
        public string _CardFileName { get; set; }

        public Card(string id, string type, string cType, string rarity, string fileName)
        {
            _CardID = id;
            _CardType = type;
            _CardClassType = cType;
            _CardRarity = rarity;
            _CardFileName = fileName;
        }
    }
}
