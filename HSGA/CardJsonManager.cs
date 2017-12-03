using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HSGA
{
    /////////COMPLETED/////////
    // Deserialise json card files into Card class format - done
    // Generate Valid JSON for the Deck files - done
    // Create Test Method that generates a random deck with random hero class - done

    ///////////TODO////////// - In order of most needed to be done
    // Implement the rest of the class specific cards into their respective lists
    // Test the generated decks in Metastone - save the files in Metastone directory
    // Implement deck legality checking - card counts/rarity and class types
    // Retrieve end of sim statistics from metastone
    // Implement the genetic algorithm!!!!
    // Automate the generation process - including the metastone testing




    // Manager for handling deserialization of cards
    // and serialization of a generated deck for use 
    // in MetaStone.

    /// <summary>
    /// Class for handling JSON serialization/deserialization.
    /// Should use this to deserialize all cards to be used.
    /// </summary>
    public class CardJsonManager
    {
        enum HERO_CLASSES
        {
            DRUID = 0,
            HUNTER = 1,
            MAGE = 2,
            PALADIN = 3,
            PRIEST = 4,
            ROGUE = 5,
            SHAMAN = 6,
            WARLOCK = 7,
            WARRIOR = 8
        }


        CardList allCardsList;

        public string finalGeneratedDeck;
        public string finalGeneratedDeckCardList;
        public string finalGeneratedDeckName;
        public string finalGeneratedDeckClass;
        public string filePath;
        public int cardCount = 0;


        Random rand;
        const int DECK_LENGTH = 30;

        public CardJsonManager()
        {
            rand = new Random();
            allCardsList = new CardList();
            finalGeneratedDeckName = "";
            finalGeneratedDeckCardList = "";
            finalGeneratedDeckClass = "";
            finalGeneratedDeck = ""; 
        }
        //{\n \"cards\": [\n  ],\n \"name\": \n \"heroClass\" \n \"arbitrary\": false\n}

        public void TestGenerateNewDeck()
        {
            int index = 0;
            string currentCard = "";
            int maxNumOfCard = 2;
            int maxNumOfLegendary = 1;
            
            for(int i = 0; i < DECK_LENGTH + 1; i++)
            {
                if(i == 30)
                {
                    if (currentCard == null)
                    {
                        // check if last card has valid id
                        currentCard = allCardsList.NeutralCardList[index]._CardFileName;
                    }

                    // if last card omit the comma at the end and finish
                    index = rand.Next(allCardsList.NeutralCardList.Count);
                    currentCard = allCardsList.NeutralCardList[index]._CardID;
                    finalGeneratedDeckCardList += "\t\"" + currentCard + "\"\n";

                    //generate deck name and hero class
                    finalGeneratedDeckName = "geneDeck";
                    Array ar = Enum.GetValues(typeof(HERO_CLASSES));
                    finalGeneratedDeckClass = ar.GetValue(rand.Next(ar.Length)).ToString();
                    finalGeneratedDeck = "{\n  \"cards\": [\n" + finalGeneratedDeckCardList + "  ],\n    \"name\": \"" + finalGeneratedDeckName + "\",\n    \"heroClass\": \"" + finalGeneratedDeckClass + "\",\n    \"arbitrary\": false\n}";
                    GenerateDeckAsJson(finalGeneratedDeck, filePath);

                    return;
                }

                //grab a card at psuedo random 
                index = rand.Next(allCardsList.NeutralCardList.Count);
                currentCard = allCardsList.NeutralCardList[index]._CardID;

                if(currentCard == null)
                {
                    // some of the files have no ids, so use the file name instead
                    currentCard = allCardsList.NeutralCardList[index]._CardFileName;
                }

                finalGeneratedDeckCardList += "\t\"" + currentCard + "\", \n";

                cardCount++;
            }
        }


        private void GenerateDeckAsJson(string deckString, string filePath)
        {
            using(StreamWriter file = File.CreateText(filePath + "\\geneDeck.json"))
            using(JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.WriteRaw(deckString);
            }
        }


        public void GetAllNeutrals(string filePath)
        {
            //TODO: should only need to use this once

            // get all card files in the current directory
            string[] filesInFolder = Directory.GetFiles(filePath);

            int cardCount = filesInFolder.Length;

            for(int i = 0; i < cardCount; i++)
            {
                string data = "";

                var fs = new FileStream(filesInFolder[i], FileMode.Open, FileAccess.Read,
                        FileShare.ReadWrite | FileShare.Delete);

                // As we only need a few specific properties from each JSON file, we will
                // use streamreader and Newtonsofts JObject class to create our objects.
                using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    data = reader.ReadToEnd();
                }

                // Deserialise the JSON string
                JObject currentCard = JObject.Parse(data);

                // Get the corresponding values 
                // we only need id to create the final deck, 
                // the rarity to for deck legality checking,
                // and hero class for other deck checking.
                string id = (string)currentCard.SelectToken("id");
                string cardType = (string)currentCard.SelectToken("type");
                string classType = (string)currentCard.SelectToken("heroClass");
                string rarity = (string)currentCard.SelectToken("rarity");
                string fileName = Path.GetFileNameWithoutExtension(filesInFolder[i]);

                // assemble and add new card to main list
                Card newCard = new Card(id,cardType,classType,rarity, fileName);

                allCardsList.NeutralCardList.Add(newCard);

            }
        }


    }
}
