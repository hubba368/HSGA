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
    // Do decks work within the simulator? - YES
    // Implement deck legality checking - card counts/rarity and class types - done
    // Implement the rest of the class specific cards into their respective lists - done
    // Construction of randomly chosen class decks - done 
    // Construction of specifically chosen class decks - done
    // Retrieve end of sim statistics from metastone - DONE    
    //  - need to implement Individual class - DONE - requires fitness value variables
    // Automate the generation process - including the metastone testing - DONE

    ///////////TODO/////////// - In order of most needed to be done
    // implement gen algo requirements: 
    //  - Fitness, crossover, selection, mutation function
    // Implement the genetic algorithm!!!!
    // Represent deck as list for mutation function etc
    // stat collection - per generation and per individual
    // save the deck files in Metastone directory - change to auto find deck directory - can be worked around by having the metastone program in same folder as this solution.

    /////QOL IMPROVEMENTS/////
    // Allow resetting of the current displayed deck
    // could do with editing the validation function to swap cards of specific class type
    // could make initial deck creation have mix of neutral and class cards
    // probably would do that when mutating gene decks though.



    /// <summary>
    /// Class for handling JSON serialization/deserialization.
    /// This is used this to deserialize all cards to be used.
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

        public CardList allCardsList;

        public string finalGeneratedDeck;
        public string finalGeneratedDeckCardList;
        public string finalGeneratedDeckName;
        public string finalGeneratedDeckClass;
        public string filePath;
        public int cardCount = 0;
        private List<Card> finalGeneratedCardList;

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
            finalGeneratedCardList = new List<Card>();
        }
        //{\n \"cards\": [\n  ],\n \"name\": \n \"heroClass\" \n \"arbitrary\": false\n}


        /// <summary>
        /// Generates a random valid deck with a random hero class.
        /// Not very useful for the overall project but added as an extra function.
        /// </summary>
        public void GenerateRandomDeck()
        {
            List<Card> cardList = new List<Card>();
            int index = 0;
            Card currentCard = new Card("","","","","","", "");
            string currentCardName = "";
            string currentCardRarity = "";
            string currentCardClassType = "";

            // choose a class at random
            Array ar = Enum.GetValues(typeof(HERO_CLASSES));
            string currentType = ar.GetValue(rand.Next(ar.Length)).ToString();
            // Get the corresponding list of cards to pick from
            List<Card> randomList = allCardsList.ChooseSpecificList(currentType);

            for (int i = 0; i < DECK_LENGTH + 1; i++)
            {
                // if at max deck count
                if (i == 30)
                {
                    // if last card omit the comma at the end and finish
                    index = rand.Next(randomList.Count);
                    currentCard = randomList[index];

                    if (currentCard._CardID == null)
                    {
                        // check if last card has valid id
                        currentCard._CardID = randomList[index]._CardFileName;
                    }

                    currentCardName = currentCard._CardID;
                    currentCardRarity = currentCard._CardRarity;
                    currentCardClassType = currentCard._CardType;

                    // Add last card to the list and validate it for legality
                    cardList.Add(currentCard);
                    ValidateAndFixDeck(cardList, currentType);

                    // assemble the deck section of JSON string
                    for (int j = 0; j < cardList.Count; j++)
                    {
                        finalGeneratedDeckCardList += "\t\"" + cardList[j]._CardID + "\"\n";
                    }


                    //generate deck name and hero class
                    finalGeneratedDeckName = "geneDeck";

                    finalGeneratedDeckClass = currentType;
                    // assemble the json string
                    finalGeneratedDeck = "{\n  \"cards\": [\n" + finalGeneratedDeckCardList + "  ],\n    \"name\": \"" + finalGeneratedDeckName + "\",\n    \"heroClass\": \"" + finalGeneratedDeckClass + "\",\n    \"arbitrary\": false\n}";
                    GenerateDeckAsJson(finalGeneratedDeck, filePath);

                    return;
                }

                //grab a card at psuedo random 
                index = rand.Next(randomList.Count);
                currentCard = randomList[index];
                currentCardName = currentCard._CardID;
                currentCardRarity = currentCard._CardRarity;
                currentCardClassType = currentCard._CardType;

                if (currentCard._CardID == null)
                {
                    // some of the files have no ids, so use the file name instead
                    currentCard._CardID = randomList[index]._CardFileName;
                }

                cardList.Add(currentCard);

                //finalGeneratedDeckCardList += "\t\"" + currentCard._CardID + "\", \n";

                cardCount++;
            }
        }

        /// <summary>
        /// Generates a valid standard type deck of specific hero class.
        /// </summary>
        /// <param name="heroClass"></param>
        /// <returns></returns>
        public Tuple<string,List<Card>> GenerateSpecificDeck(string heroClass)
        {
            string finalDeckString = "";
            List<Card> finalDeckCardList = new List<Card>();
            finalDeckCardList.Clear();

            List<Card> cardList = new List<Card>();
            int index = 0;
            Card currentCard = new Card("", "", "", "", "", "", "");
            string currentCardName = "";
            string currentCardRarity = "";
            string currentCardClassType = "";

            string currentType = heroClass.ToUpper();
            // Get the corresponding list of cards to pick from
            List<Card> randomList = allCardsList.ChooseSpecificList(currentType);
            List<Card> neutralList = allCardsList.NeutralCardList;

            for (int i = 0; i < DECK_LENGTH + 1; i++)
            {
                // if at max deck count
                if (i == 30)
                {
                    // if last card omit the comma at the end and finish
                    if(rand.NextDouble() >= 0.5)
                    {
                        index = rand.Next(neutralList.Count);
                        currentCard = neutralList[index];
                        if (currentCard._CardID == null)
                        {
                            currentCard._CardID = neutralList[index]._CardFileName;
                        }
                    }
                    else
                    {
                        index = rand.Next(randomList.Count);
                        currentCard = randomList[index];

                        if (currentCard._CardID == null)
                        {
                            // check if last card has valid id
                            currentCard._CardID = randomList[index]._CardFileName;
                        }
                    }

                    currentCardName = currentCard._CardID;
                    currentCardRarity = currentCard._CardRarity;
                    currentCardClassType = currentCard._CardType;

                    // Add last card to the list and validate it for legality
                    cardList.Add(currentCard);
                    cardList = ValidateAndFixDeck(cardList, currentType);

                    string finalCardList = "";
                    // assemble the deck section of JSON string
                    for (int j = 0; j < cardList.Count; j++)
                    {
                        finalCardList += "\t\"" + cardList[j]._CardID + "\",\n";
                    }
                   // finalGeneratedDeckCardList += "\t\"" + cardList[29]._CardID + "\"\n";

                    //generate deck name and hero class
                    finalGeneratedDeckName = "aGeneDeck";

                    finalGeneratedDeckClass = currentType;
                    // assemble the json string
                    string finalDeck = "";
                    finalDeck = "{\n  \"cards\": [\n" + finalCardList + "  ],\n    \"name\": \"" 
                        + finalGeneratedDeckName + "\",\n    \"heroClass\": \"" 
                        + finalGeneratedDeckClass + "\",\n    \"arbitrary\": false\n}";
                    GenerateDeckAsJson(finalDeck, filePath);

                    finalGeneratedDeck = finalDeck;
                    finalDeckCardList = cardList;
                    finalDeckString = finalGeneratedDeck;
                }
                if(i != 30)
                {
                    //grab a card at psuedo random 
                    // choose either class or neutral cards at random
                    if (rand.NextDouble() >= 0.5)
                    {
                        index = rand.Next(neutralList.Count);
                        currentCard = neutralList[index];
                        if (currentCard._CardID == null)
                        {
                            currentCard._CardID = neutralList[index]._CardFileName;
                        }
                    }
                    else
                    {
                        index = rand.Next(randomList.Count);
                        currentCard = randomList[index];
                        if (currentCard._CardID == null)
                        {
                            // some of the files have no ids, so use the file name instead
                            currentCard._CardID = randomList[index]._CardFileName;
                        }
                    }

                    currentCardName = currentCard._CardID;
                    currentCardRarity = currentCard._CardRarity;
                    currentCardClassType = currentCard._CardType;

                    cardList.Add(currentCard);
                    cardCount++;
                }
            }
            Tuple<string, List<Card>> t = new Tuple<string, List<Card>>(finalDeckString, finalDeckCardList);
            return t;

        }
       

        /// <summary>
        /// Validates the input card list by checking if the deck has max 2 per card,
        /// or 1 per card of legendary rarity.
        /// This function returns a 'fixed' card list.
        /// We only need to call this when generating the initial population.
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        private List<Card> ValidateAndFixDeck(List<Card> cardList, string classType)
        {
            // We need to validate the deck, to make sure that there are no more than
            // 2 of each card or 1 for each legendary card
            int prevCardCount = 0;
            int newNeutralCardIndex = 0;
            int newClassCardIndex = 0;

           /* string firstChar = cardList[0]._CardID.Split('_')[0];
            int d = cardList[0]._CardID.Split('_').Count();
            string secondChar = cardList[0]._CardID.Split('_').Count() >= 3 ? cardList[0]._CardID.Split('_')[2] : cardList[0]._CardID.Split('_')[1];*/

            // sort card list in alphabetical order
            List<Card> test = cardList.OrderBy(Card => Card._CardID.Split('_')[1].ToCharArray()[0])
                .ThenBy(Card => Card._CardID.Split('_').Count() >= 3 ? Card._CardID.Split('_')[2].ToCharArray()[0] : Card._CardID.Split('_')[1].ToCharArray()[0]).ToList();
            cardList = test;

            List <string> duplicatesList = new List<string>();
            List<string> cards = new List<string>();

            for (int i = 0; i < cardList.Count; i++)
            {
                cards.Add(cardList[i]._CardID);
            }

            if (cardList.Count > 30)
            {
                if (cardList.ElementAt(30) != null)
                {
                    cardList.Remove(cardList.ElementAt(30));
                }
            }

            for (int j = 0; j < cardList.Count - 1; j++)
            {
                // get current card
                // and current card rarity
                string cardToCompare = cardList[j]._CardID;
                string cardToCompareRarity = cardList[j]._CardRarity;

                for (int l = j + 1; l < cardList.Count; l++)
                {
                    // get the next card and rarity in the list
                    string nextCard = cardList[l]._CardID;
                    string nextCardRarity = cardList[l]._CardRarity;
                    // compare the 2 card values
                    bool check = nextCard.Equals(cardToCompare);

                    List<Card> ClassCardList = allCardsList.GetDeckClassType(classType);
                    List<Card> NeutralCardList = allCardsList.NeutralCardList;
                    newNeutralCardIndex = rand.Next(NeutralCardList.Count);
                    newClassCardIndex = rand.Next(ClassCardList.Count);

                    // if the names are the same, increment the card counter
                    // This is used to check for any cards that aren't
                    // legendary.
                    if (check == true)
                    {
                        prevCardCount+= 2;
                        duplicatesList.Add(cardList[l]._CardID);
                    }

                    // remove all cards from both lists that are included in the current deck at least
                    // 2+ times.
                    for (int i = 0; i < duplicatesList.Count; i++)
                    {
                        ClassCardList.RemoveAll(Card => Card._CardID == duplicatesList[i]);
                        NeutralCardList.RemoveAll(Card => Card._CardID == duplicatesList[i]);

                        allCardsList.GetDeckClassType(classType).RemoveAll(Card => Card._CardID == duplicatesList[i]);
                        allCardsList.NeutralCardList.RemoveAll(Card => Card._CardID == duplicatesList[i]);
                    }

                    if (prevCardCount > 2)
                    {
                        newNeutralCardIndex = rand.Next(NeutralCardList.Count);
                        newClassCardIndex = rand.Next(ClassCardList.Count);

                        Card newNeutralCard;
                        Card newClassCard;
                        bool cardTypeChosen = false;

                        newNeutralCard = allCardsList.NeutralCardList[newNeutralCardIndex];
                        newClassCard = ClassCardList[newClassCardIndex];
                        cardTypeChosen = rand.Next(100) < 50 ? true : false;

                        // choose randomly whether to swap with a neutral or class card.
                        if (cardTypeChosen == true)
                        {
                            if (cardList[l]._CardID == newNeutralCard._CardID || cardList[l]._CardFileName == newNeutralCard._CardFileName)
                            {
                                newNeutralCard = allCardsList.NeutralCardList[rand.Next(allCardsList.NeutralCardList.Count)];

                            }
                        }
                        else if (cardTypeChosen == false)
                        {
                            for(int i = 0; i < duplicatesList.Count; i++)
                            {
                                if (cardList[l]._CardID == duplicatesList[i] || cardList[l]._CardFileName == duplicatesList[i])
                                {
                                    newClassCard = ClassCardList[rand.Next(ClassCardList.Count)];

                                }
                            }

                        }

                        cardList.Remove(cardList[l]);
                        if (cardTypeChosen == true)
                        {
                            cardList.Insert(l, newNeutralCard);
                        }
                        else
                        {
                            cardList.Insert(l, newClassCard);
                        }

                        prevCardCount = 0;
                    }

                    // if the card is legendary, remove the next instance of it
                    if (check == true && nextCardRarity == "LEGENDARY")
                    {
                        Card newNeutralCard;
                        Card newClassCard;
                        bool cardTypeChosen = false;

                        newNeutralCardIndex = rand.Next(NeutralCardList.Count);
                        newClassCardIndex = rand.Next(ClassCardList.Count);

                        newNeutralCard = allCardsList.NeutralCardList[newNeutralCardIndex];
                        newClassCard = ClassCardList[newClassCardIndex];
                        cardTypeChosen = rand.Next(100) < 50 ? true : false;

                        // choose randomly whether to swap with a neutral or class card.
                        if (cardTypeChosen == true)
                        {
                            if (cardList[l]._CardID == newNeutralCard._CardID || cardList[l]._CardFileName == newNeutralCard._CardFileName)
                            {
                                newNeutralCard = allCardsList.NeutralCardList[rand.Next(allCardsList.NeutralCardList.Count)];

                            }
                        }
                        else if (cardTypeChosen == false)
                        {
                            if (cardList[l]._CardID == newClassCard._CardID || cardList[l]._CardFileName == newClassCard._CardFileName)
                            {
                                newClassCard = ClassCardList[rand.Next(ClassCardList.Count)];

                            }
                        }

                        cardList.Remove(cardList[l]);
                        if (cardTypeChosen == true)
                        {
                            cardList.Insert(l, newNeutralCard);
                        }
                        else
                        {
                            cardList.Insert(l, newClassCard);
                        }

                    }

                }


            }
            cards.Clear();
            for (int i = 0; i < cardList.Count; i++)
            {
                cards.Add(cardList[i]._CardID);
            }
            return cardList;
        }

        /// <summary>
        /// Validates the input card list by checking if the deck has max 2 per card,
        /// or 1 per card of legendary rarity.
        /// </summary>
        /// <param name="cardList"></param>
        /// <returns></returns>
        public bool ValidateDeck(List<Card> cardList)
        {
            // We need to validate the deck, to make sure that there are no more than
            // 2 of each card or 1 for each legendary card
            int prevCardCount = 0;
            bool isDeckLegal = true;

            // deck is illegal if the max count is over 30.
            if (cardList.Count != 30)
            {
                isDeckLegal = false;
                return isDeckLegal;
            }

            for (int j = 0; j < cardList.Count - 1; j++)
            {
                // get current card
                // and current card rarity
                string cardToCompare = cardList[j]._CardID;
                string cardToCompareRarity = cardList[j]._CardRarity;
                prevCardCount = 0;

                for (int l = j + 1; l < cardList.Count; l++)
                {
                    // get the next card and rarity in the list
                    string nextCard = cardList[l]._CardID;
                    string nextCardRarity = cardList[l]._CardRarity;
                    // compare the 2 card values returns 0 if the same
                    bool check = nextCard.Equals(cardToCompare);

                    // if the names are the same, increment the card counter
                    // This is used to check for any cards that aren't
                    // legendary.
                    // illegal deck if duplicate legendary card.
                    if (check == true && nextCardRarity == "LEGENDARY")
                    {
                        isDeckLegal = false;
                        return isDeckLegal;
                    }

                    if (check == true)
                    {
                        prevCardCount++;
                    }
                    if (prevCardCount > 2)
                    {
                        isDeckLegal = false;
                        return isDeckLegal;
                    }
                }
            }
            isDeckLegal = true;
            return isDeckLegal;

        }


        /// <summary>
        /// Generates the final deck into a valid JSON string that MetaStone can parse.
        /// </summary>
        /// <param name="deckString"></param>
        /// <param name="filePath"></param>
        private void GenerateDeckAsJson(string deckString, string filePath)
        {
            string secondPath = "C:\\Users\\Elliott\\Documents\\metastone\\decks";
            if (File.Exists(filePath + "\\aGeneDeck.json"))
            {
                
                File.Delete(filePath + "\\aGeneDeck.json");
            }
                       
            using(StreamWriter file = File.CreateText(filePath + "\\aGeneDeck.json"))
            using(JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.WriteRaw(deckString);
            }

            // we also need to write the same file to a second directory
            if(File.Exists(secondPath + "\\aGeneDeck.json"))
            {
                File.Delete(secondPath + "\\aGeneDeck.json");
            }
            using (StreamWriter file = File.CreateText(secondPath + "\\aGeneDeck.json"))
            using (JsonTextWriter writer = new JsonTextWriter(file))
            {
                writer.WriteRaw(deckString);
            }

        }


        /// <summary>
        /// Function to retreive all cards from the Assets folder of the project.
        /// This is the default option and is called when the program is first started.
        /// </summary>
        /// <param name="fileCount"></param>
        /// <param name="initialPath"></param>
        public void GetAllCards(int fileCount, string initialPath)
        {
            // go through all Card Folders
            
            for(int j = 0; j < fileCount; j++)
            {
                string[] cardFilePath = Directory.GetDirectories(initialPath, "*", SearchOption.TopDirectoryOnly);
                string currentFolder = cardFilePath[j];

                // get all card files in the current directory
                string[] filesInFolder = Directory.GetFiles(currentFolder);

                int cardCount = filesInFolder.Length;

                for (int i = 0; i < cardCount; i++)
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
                    string cardCost = (string)currentCard.SelectToken("baseManaCost");
                    string cardSet = (string)currentCard.SelectToken("set");

                    if (id == null)
                    {
                        id = fileName;
                    }

                    // assemble and add new card to its respective list
                    Card newCard = new Card(id, cardType, classType, rarity, fileName, cardCost, cardSet);

                    if (classType == "DRUID")
                        allCardsList.DruidCardList.Add(newCard);
                    if (classType == "MAGE")
                        allCardsList.MageCardList.Add(newCard);
                    if (classType == "HUNTER")
                        allCardsList.HunterCardList.Add(newCard);
                    if (classType == "PALADIN")
                        allCardsList.PaladinCardList.Add(newCard);
                    if (classType == "PRIEST")
                        allCardsList.PriestCardList.Add(newCard);
                    if (classType == "ROGUE")
                        allCardsList.RogueCardList.Add(newCard);
                    if (classType == "SHAMAN")
                        allCardsList.ShamanCardList.Add(newCard);
                    if (classType == "WARRIOR")
                        allCardsList.WarriorCardList.Add(newCard);
                    if (classType == "WARLOCK")
                        allCardsList.WarlockCardList.Add(newCard);
                    if (classType == "ANY")
                        allCardsList.NeutralCardList.Add(newCard);
                }
            }

            
        }


        /// <summary>
        /// Retrieves all cards of a specific class.
        /// Not inherently useful for the overall project, but kept as an extra
        /// function.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="cardClass"></param>
        public void GetAllSelectedCards(string filePath, string cardClass)
        {
            // get all card files in the current directory
            string[] filesInFolder = Directory.GetFiles(filePath);

            int cardCount = filesInFolder.Length;

            for (int i = 0; i < cardCount; i++)
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
                string cardCost = (string)currentCard.SelectToken("baseManaCost");
                string cardSet = (string)currentCard.SelectToken("set");

                if (id == null)
                {
                    id = fileName;
                }

                // assemble and add new card to its respective list
                Card newCard = new Card(id, cardType, classType, rarity, fileName, cardCost, cardSet);

                // check if the card class type matches to the input
                if(newCard._CardClassType.ToUpper() == cardClass.ToUpper())
                {
                    allCardsList.Choose(cardClass.ToUpper(), newCard);
                }
            }
        
    }


        public string GenerateDeckString(HSGAIndividual ind, string classType)
        {
            string cardList = "";
            string finalDeck = "";

            for (int j = 0; j < ind.cardList.Count; j++)
            {
               cardList += "\t\"" + ind.cardList[j]._CardID + "\",\n";
            }


            finalDeck = "{\n  \"cards\": [\n" + cardList + "  ],\n    \"name\": \""
                + finalGeneratedDeckName + "\",\n    \"heroClass\": \""
                + classType.ToUpper() + "\",\n    \"arbitrary\": false\n}";
            GenerateDeckAsJson(finalDeck, filePath);
            return finalDeck;
        }

        //DEBUG FUNCTIONS//

        /// <summary>
        /// Test function to test ValidateDeck.
        /// </summary>
        /// <returns></returns>
        public string TestValidation()
        {
            List<Card> testDeck = new List<Card>();
            Card minionCard1 = new Card("", "", "", "", "", "", "");
            Card minionCard2 = new Card("", "", "", "", "", "", "");
            Card minionCard3 = new Card("", "", "", "", "", "", "");
            Card legendCard1 = new Card("", "", "", "", "", "", "");
            Card legendCard2 = new Card("", "", "", "", "", "", "");
            Card weaponCard1 = new Card("", "", "", "", "", "", "");
            Card weaponCard2 = new Card("", "", "", "", "", "", "");
            Card weaponCard3 = new Card("", "", "", "", "", "", "");

            // test duplicate non legendary, non weapon cards
            minionCard1 = allCardsList.NeutralCardList[0];
            minionCard2 = allCardsList.NeutralCardList[0];
            minionCard3 = allCardsList.NeutralCardList[0];

            // test duplicate legendary cards
            legendCard1 = allCardsList.NeutralCardList[5];
            legendCard2 = allCardsList.NeutralCardList[5];

            // test duplicate weapon cards
            weaponCard1 = allCardsList.HunterCardList[36];
            weaponCard2 = allCardsList.HunterCardList[36];
            weaponCard3 = allCardsList.HunterCardList[36];


            testDeck.Add(minionCard1);
            testDeck.Add(minionCard2);
            testDeck.Add(minionCard3);
            testDeck.Add(legendCard1);
            testDeck.Add(legendCard2);
            testDeck.Add(weaponCard1);
            testDeck.Add(weaponCard2);
            testDeck.Add(weaponCard3);

            ValidateAndFixDeck(testDeck, weaponCard1._CardClassType);

            string result = "";

            for(int i = 0; i < testDeck.Count; i++)
            {
                result += testDeck[i]._CardID + ", \n";
            }

            return result;
        }
    }
}
