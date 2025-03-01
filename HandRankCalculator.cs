using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace NEA_Computer_Science_Poker__️__️__️__️
{
    public class HandRankCalculator
    {
        //00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12
        // 2,  3,  4,  5,  6,  7,  8,  9, 10,  J,  Q,  K,  A
        //return an int[], with values [rankvalue, highestcard]?

        public List<string> VALID = new List<string>(); //list to contain the 5 cards that make up the hand rank

        public List<string> GetVALID()
        {
            return VALID;
        }



        public int CalculateHandRank(List<string> Cards)
        {
            VALID.Clear();

            List<string> ValidCards = new List<string>();
            int val = 1;

            //each subroutines returns a boolean, so as soon as one is true, the whole if statement ends
            //therefore the subroutines are ordered in descending rarity, as if you have both a three of a kind and a pair, the three of a kind is better so you would output a three of a kind

            if (CalculateRoyalFlush(Cards, ValidCards)) //royal flush
            {

                val = 10;
            }
            else
            {
                if (CalculateStraightFlush(Cards, ValidCards)) //straight flush
                {
                    val = 9;
                }
                else
                {
                    if (CalculateFourOfAKind(Cards, ValidCards)) //four of a kind
                    {
                        val = 8;
                    }
                    else
                    {
                        if (CalculateFullHouse(Cards, ValidCards)) // full house
                        {
                            val = 7;
                        }
                        else
                        {
                            if (CalculateFlush(Cards, ValidCards)) //flush
                            {
                                val = 6;
                            }
                            else
                            {
                                if (CalculateStraight(Cards, ValidCards)) // straight
                                {
                                    val = 5;
                                }
                                else
                                {
                                    if (CalculateThreeOfAKind(Cards, ValidCards)) //three of a kind
                                    {
                                        val = 4;
                                    }
                                    else
                                    {
                                        if (CalculateTwoPair(Cards, ValidCards)) //two pair
                                        {
                                            val = 3;
                                        }
                                        else
                                        {
                                            if (CalculatePair(Cards, ValidCards)) //pair
                                            {
                                                val = 2;
                                            }
                                            else
                                            {
                                                if (CalculateHighCard(Cards, ValidCards)) //high card
                                                {
                                                    val = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return val;

            //high card - 1
        }

        private int GetCardVal(string card)
        {
            return Convert.ToInt32(Convert.ToString(card[0]) + Convert.ToString(card[1]));
        }

        private int GetCardSuit(string card)
        {
            return Convert.ToInt32(Convert.ToString(card[2]));
        }


        private bool CalculateRoyalFlush(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 5 CARDS OF THE SAME SUIT WITH A VALUE OF 10, J, Q, K OR A
            //RETURNS IN THE FORMAT (A, K, Q, J, 10)

            ValidCards.Clear();
            bool valid = false;
            //add all cards over 10 to a list
            for (int i = 0; i < Cards.Count; i++)
            {
                if (GetCardVal(Cards[i]) >= 8)
                {
                    //Console.WriteLine(Cards[i]);
                    ValidCards.Add(Cards[i]);
                }
            }

            //find most common suit
            List<int> suits = new List<int>();
            suits.Add(0);
            suits.Add(0);
            suits.Add(0);
            suits.Add(0);

            for (int i = 0; i < ValidCards.Count; i++)
            {
                int num = Convert.ToInt32(Convert.ToString(ValidCards[i][2]));
                suits[num] += 1;
            }

            //find if there is a common suit (has to have 5 at least)

            for (int i = 0; i < suits.Count; i++)
            {
                if (suits[i] >= 5)
                {
                    ValidCards.Sort();
                    ValidCards.Reverse();
                    for (int j = 0; j < ValidCards.Count; j++)
                    {
                        if (Convert.ToInt32(Convert.ToString(ValidCards[j][2])) == i)
                        {
                            VALID.Add(ValidCards[j]); //add any card with the common suit to the list that will be used to display the cards
                        }
                    }
                    //if there is a common suit with 5 common members, and if all cards are above 10, then it has to be a royal flush
                    valid = true;
                }
                else
                {
                    //valid = false; //otherwise it is not a royal flush
                }
            }
            return valid;
        }

        private bool CalculateStraightFlush(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATED IF THERE ARE 5 CARDS IN A ROW WITH THE SAME SUIT
            //RETURNS 5 CARDS IN THE FORMAT (highest, second, third, fourth, fifth)

            ValidCards.Clear();
            bool valid = false;
            bool vvalid = false;
            bool straight = false;
            int val;
            int suitnumber = 0;
            List<string> CardsOfTheSameSuit = new List<string>();
            List<int> suits = new List<int>();
            suits.Add(0);
            suits.Add(0);
            suits.Add(0);
            suits.Add(0);


            //total up number of each suit
            for (int i = 0; i < Cards.Count; i++)
            {
                suits[Convert.ToInt32(Convert.ToString(Cards[i][2]))] += 1;
            }

            //if there is 5 of one suit, keep going, otherwise end the search here
            for (int i = 0; i < suits.Count; i++)
            {
                if (suits[i] >= 5)
                {
                    suitnumber = i;
                    vvalid = true;
                    break;
                }
                else
                {
                    vvalid = false;
                }
            }

            if (!vvalid)
            {
                return vvalid;
            }


            //add all cards of the same suit (the most common suit) to the same list to be used later
            for (int i = 0; i < Cards.Count; i++)
            {
                if (Convert.ToInt32(Convert.ToString(Cards[i][2])) == suitnumber)
                {
                    CardsOfTheSameSuit.Add(Cards[i]);
                }

            }



            for (int i = 0; i < CardsOfTheSameSuit.Count; i++) //used to loop through whole deck
            {
                ValidCards.Clear();
                ValidCards.Add(CardsOfTheSameSuit[i]);
                bool cont = true;
                int morethan = 1;
                int InaRow = 1;
                val = GetCardVal(CardsOfTheSameSuit[i]);
                while (cont) //continues if the search find another card with a value 1 greater than itself
                {
                    cont = false;
                    for (int j = 0; j < CardsOfTheSameSuit.Count; j++) //loops through the whole deck from the reference point of one card
                    {
                        if (j == i) //skip if the card we're looking at is the same as the reference card
                        {

                        }
                        else
                        {
                            int val2 = GetCardVal(CardsOfTheSameSuit[j]);
                            if (val2 == (val + morethan)) //if the value of the card we're looking at is the same as the value of the reference card, add it to te valid cards list
                            {
                                cont = true;
                                InaRow += 1;
                                morethan += 1;
                                ValidCards.Add(CardsOfTheSameSuit[j]);
                                break;
                            }
                        }

                    }
                }
                if (InaRow >= 5) //if there is 5 in a row, break the for loop
                {
                    straight = true;
                    break;
                }

            }


            if (straight) //because there is a straight, and the only cards that this could be made out of are in the same suit, this must be a straight flush
            {
                for (int i = 1; i < 6; i++)
                {
                    VALID.Add(ValidCards[ValidCards.Count - i]); //add the highest cards in the straight to obtain the highest straight
                }
                valid = true;



            }
            return valid;

        }

        private bool CalculateFourOfAKind(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 4 CARDS WITH THE SAME VALUE, THEN THE 5TH CARD IS THE HIGHEST CARD EXCLUDING THE VALUE OF THE 4 OF A KIND
            //RETURNS 5 CARDS IN THE FORMAT (foak, foak, foak, foak, high card)
            //foak = four of a kind

            ValidCards.Clear();
            bool valid = false;
            int OfAKind;

            for (int i = 0; i < Cards.Count; i++) //loop through all cards
            {
                ValidCards.Clear();
                OfAKind = 1;
                int val = GetCardVal(Cards[i]);
                ValidCards.Add(Cards[i]);

                for (int j = 0; j < Cards.Count; j++) //for each card, check every card in the deck to see if there are any that matches it
                {
                    if (j != i && val == GetCardVal(Cards[j])) //if there is another card with the same value, add it to the list
                    {
                        ValidCards.Add(Cards[j]);
                        OfAKind += 1;
                    }
                }
                if (OfAKind == 4) //if there is 4, it has to be the only thing on the board
                {
                    valid = true;
                    VALID = ValidCards;
                    int highest = -1;
                    int highestval = 0;
                    for (int k = 0; k < Cards.Count; k++) //there has to be a fifth card- so get the highest card on the table
                    {
                        if (GetCardVal(Cards[k]) > highest && GetCardVal(Cards[k]) != val)
                        {
                            highest = GetCardVal(Cards[k]);
                            highestval = k;
                        }
                    }
                    VALID.Add(Cards[highestval]); //add highest card other than the 4 of a kind to the VALID list
                    return valid;
                }
            }
            return valid;
        }


        private bool CalculateFullHouse(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE IS A 3 OF A KIND AND A PAIR, OR TWO THREE OF A KINDS (in which case the higher three of a kind is found)
            //RETURNS 5 CARD VALUES IN THE FORMAT (toak, toak, toak, pair, pair)
            //toak = three of a kind

            ValidCards.Clear();
            bool valid = false;
            int OfAKind = 0;
            List<string> ThreeOfAKindCards = new List<string>();
            List<string> TwoOfAKindCards = new List<string>();
            List<int> CardValues = new List<int>();
            int Triples = 0, Pairs = 0;

            //find any pairs or triples
            for (int i = 0; i < Cards.Count; i++)
            {
                ValidCards.Clear();
                OfAKind = 1;
                int val = GetCardVal(Cards[i]);
                ValidCards.Add(Cards[i]);
                for (int j = 0; j < Cards.Count; j++)
                {
                    if (i != j && val == GetCardVal(Cards[j]) && !CardValues.Contains(GetCardVal(Cards[j]))) //if card value matches, add it
                    {
                        ValidCards.Add(Cards[j]);
                        OfAKind += 1;
                    }
                }
                if (OfAKind == 3) //if it's a triple, add them to a new list
                {
                    CardValues.Add(GetCardVal(ValidCards[0]));
                    Triples += 1; //increase number of triples
                    foreach (string card in ValidCards)
                    {
                        ThreeOfAKindCards.Add(card);
                    }
                }

                if (OfAKind == 2) //if it's a double, add them to a new list
                {
                    CardValues.Add(GetCardVal(ValidCards[0]));
                    Pairs += 1; //increase the number of doubles
                    foreach (string card in ValidCards)
                    {
                        TwoOfAKindCards.Add(card);
                    }
                }
            }
            //Console.WriteLine("TRIPLES: {0}", Triples);
            //Console.WriteLine("DOUBLES: {0}", Pairs);

            if (Triples == 2) //if there are 2 triples, make the highest possible full house
            {
                valid = true;
                //find highest triple
                if (GetCardVal(ThreeOfAKindCards[0]) > GetCardVal(ThreeOfAKindCards[3]))
                {
                    VALID.Add(ThreeOfAKindCards[0]);
                    VALID.Add(ThreeOfAKindCards[1]);
                    VALID.Add(ThreeOfAKindCards[2]);
                    VALID.Add(ThreeOfAKindCards[3]);
                    VALID.Add(ThreeOfAKindCards[4]);
                }
                else
                {
                    VALID.Add(ThreeOfAKindCards[3]);
                    VALID.Add(ThreeOfAKindCards[4]);
                    VALID.Add(ThreeOfAKindCards[5]);
                    VALID.Add(ThreeOfAKindCards[0]);
                    VALID.Add(ThreeOfAKindCards[1]);
                }
            }
            else if (Triples == 1 && Pairs >= 1) //if there is 1 triple and at least 1 pair, make the higest possible full house
            {
                valid = true;
                VALID.Add(ThreeOfAKindCards[0]);
                VALID.Add(ThreeOfAKindCards[1]);
                VALID.Add(ThreeOfAKindCards[2]);

                if (Pairs > 1)
                {
                    if (GetCardVal(TwoOfAKindCards[0]) > GetCardVal(TwoOfAKindCards[2]))
                    {
                        VALID.Add(TwoOfAKindCards[0]);
                        VALID.Add(TwoOfAKindCards[1]);
                    }
                    else
                    {
                        VALID.Add(TwoOfAKindCards[2]);
                        VALID.Add(TwoOfAKindCards[3]);
                    }
                }
                else
                {
                    VALID.Add(TwoOfAKindCards[0]);
                    VALID.Add(TwoOfAKindCards[1]);
                }
            }
            return valid;
        }

        private bool CalculateFlush(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 5 CARDS WITH THE SAME SUIT, IF THERE ARE MORE, IT FINDS THE HIGHEST 5 CARDS
            //RETURNS 5 CARDS IN THE FORMAT (highest, second, third, fourth, fifth)

            ValidCards.Clear();
            bool valid = false;
            int[] Suits = { 0, 0, 0, 0 };
            int SuitNum = 0;

            for (int i = 0; i < Cards.Count; i++) //add number of each suit to a list
            {
                Suits[GetCardSuit(Cards[i])] += 1;
            }

            for (int i = 0; i < Suits.Length; i++) //search through suit list
            {
                if (Suits[i] >= 5) //if there are 5 of a suit, this must be the only 5 of the same suit, so has to be a flush
                {
                    SuitNum = i;
                    valid = true;
                    break;
                }
            }

            if (valid)
            {

                for (int i = 0; i < Cards.Count; i++)
                {
                    if (GetCardSuit(Cards[i]) == SuitNum)
                    {
                        ValidCards.Add(Cards[i]);
                    }
                }

                ValidCards.Sort(); //sort and reverse the cards to obtain a list of cards in descending order for later comparison
                ValidCards.Reverse();
                VALID = ValidCards; //set output list to valid cards
            }
            return valid;
        }

        private bool CalculateStraight(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 5 CARDS IN A ROW, IF THERE ARE MORE IN A ROW, FIND THE HIGHEST 5 IN A ROW
            //RETURNS IN THE FORMAT (highest, second, third, fourth, fifth)

            ValidCards.Clear();
            bool valid = false;
            int val;

            for (int i = 0; i < Cards.Count; i++) //used to loop through whole deck
            {
                ValidCards.Clear();
                ValidCards.Add(Cards[i]);
                bool cont = true;
                int morethan = 1;
                int InaRow = 1;
                val = GetCardVal(Cards[i]);
                while (cont) //continues if the search find another card with a value 1 greater than itself
                {
                    cont = false;
                    for (int j = 0; j < Cards.Count; j++) //loops through the whole deck from the reference point of one card
                    {
                        if (j == i) //skip if the card we're looking at is the same as the reference card
                        {

                        }
                        else
                        {
                            int val2 = GetCardVal(Cards[j]);
                            if (val2 == (val + morethan)) //if the value of the card we're looking at is the same as the value of the reference card, add it to te valid cards list
                            {
                                cont = true;
                                InaRow += 1;
                                morethan += 1;
                                ValidCards.Add(Cards[j]);
                                break;
                            }
                        }

                    }
                }
                if (InaRow >= 5) //if there is 5 in a row, break the for loop
                {
                    valid = true;
                    break;
                }

            }


            if (valid) //sort the cards for future comparison
            {
                ValidCards.Sort();
                ValidCards.Reverse();
                for (int i = 0; i < 5; i++)
                {
                    VALID.Add(ValidCards[i]);
                }

            }
            return valid;
        }

        private bool CalculateThreeOfAKind(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 3 CARDS WITH THE SAME VALUE, THEN ADDS THE NEXT HIGHEST CARDS THAT AREN'T INCLUDED IN THE THREE OF A KIND
            //RETURNS 5 CARDS IN THE FORMAT (toak, toak, toak, highest, second)
            //toak = three of a kind

            ValidCards.Clear();
            List<string> ExtraCards = new List<string>(); //will be used to store the 2 other cards that make up 5 cards
            bool valid = false;
            int OfAKind = 0;
            int val = 0;

            for (int i = 0; i < Cards.Count; i++)
            {
                ValidCards.Clear();
                OfAKind = 1;
                val = GetCardVal(Cards[i]);
                ValidCards.Add(Cards[i]);
                for (int j = 0; j < Cards.Count; j++)
                {
                    if (i != j && val == GetCardVal(Cards[j])) //if same rank as original card, add it to list and increase OfAKind by 1
                    {
                        OfAKind += 1;
                        ValidCards.Add(Cards[j]);
                    }

                }

                if (OfAKind == 3) //if the code has reached this stage, it is already confirmed there is no 4 of a kind, so we can safely only check for triples
                {
                    valid = true;
                    VALID = ValidCards;
                    break;
                }
            }
            if (valid)
            {
                for (int k = 0; k < Cards.Count; k++)
                {
                    if (val != GetCardVal(Cards[k]))
                    {
                        ExtraCards.Add(Cards[k]); //add all extra cards to list
                    }
                }
                ExtraCards.Sort(); //sort the extra cards to then add the top 2, meaning the second highest cards are added
                ExtraCards.Reverse();
                VALID.Add(ExtraCards[0]);
                VALID.Add(ExtraCards[1]);
            }
            return valid;
        }

        private bool CalculateTwoPair(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE TWO LOTS OF TWO CARDS WITH THE SAME VALUE, IF THERE ARE MORE THAN 2 PAIRS, FIND THE HIGHEST 2. ADDS THE NEXT HIGHEST CARD NOT INCLUDED IN THE TWO PAIR
            //RETURNS 5 CARDS IN THE FORMAT (pair1, pair1, pair2, pair2, highest)

            bool valid = false;
            ValidCards.Clear();
            List<string> ExtraCards = new List<string>(); //will be used to store the 3 other cards that make up 5 cards
            int Pairs = 0;
            int val = 0;

            for (int i = 0; i < Cards.Count; i++)
            {
                val = GetCardVal(Cards[i]);
                for (int j = 0; j < Cards.Count; j++)
                {
                    if (i != j && val == GetCardVal(Cards[j]) && !ValidCards.Contains(Cards[j])) //if same rank as original card, add it to list and increase OfAKind by 1
                    {
                        Pairs += 1;
                        ValidCards.Add(Cards[i]);
                        ValidCards.Add(Cards[j]);
                    }
                }
            }

            if (Pairs > 2)
            {
                valid = true;
                ValidCards.Sort();
                ValidCards.Reverse();
                VALID.Add(ValidCards[0]);
                VALID.Add(ValidCards[1]);
                VALID.Add(ValidCards[2]);
                VALID.Add(ValidCards[3]);
                Cards.Sort();
                Cards.Reverse();
                foreach (string card in Cards)
                {
                    if (!VALID.Contains(card))
                    {
                        VALID.Add(card);
                        break;
                    }
                }

            }
            else if (Pairs == 2)
            {
                valid = true;
                foreach (string card in ValidCards)
                {
                    VALID.Add(card);
                }
                VALID.Sort();
                VALID.Reverse();
                Cards.Sort();
                Cards.Reverse();
                foreach (string card in Cards)
                {
                    if (!VALID.Contains(card))
                    {
                        VALID.Add(card);
                        break;
                    }
                }
            }
            return valid;
        }

        private bool CalculatePair(List<string> Cards, List<string> ValidCards)
        {
            //CALCULATES IF THERE ARE 2 CARDS OF THE SAME VALUE, THEN ADDS THE 3 NEXT HIGHEST CARDS
            //RETURNS 5 CARDS IN THE FORMAT (pair, pair, highest, second, third)

            bool valid = false;
            ValidCards.Clear();
            List<string> ExtraCards = new List<string>(); //will be used to store the 3 other cards that make up 5 cards
            int Pairs = 0;
            int val = 0;

            for (int i = 0; i < Cards.Count; i++)
            {
                val = GetCardVal(Cards[i]);
                for (int j = 0; j < Cards.Count; j++)
                {
                    if (i != j && val == GetCardVal(Cards[j]) && !ValidCards.Contains(Cards[j])) //if same rank as original card, add it to list and increase Pairs by 1
                    {
                        Pairs += 1;
                        ValidCards.Add(Cards[i]);
                        ValidCards.Add(Cards[j]);
                    }

                }
            }

            if (Pairs == 1)
            {
                valid = true;
                foreach (string card in ValidCards) //add all cards to different list
                {
                    VALID.Add(card);
                }

                Cards.Sort();
                Cards.Reverse(); //sort validcards 
                int Added = 0;
                foreach (string card in Cards)
                {
                    if (!VALID.Contains(card))
                    {
                        VALID.Add(card);
                        Added += 1;
                        if (Added == 3) //add 3 more of the highest cards to the list of cards
                        {
                            break;
                        }
                    }
                }
            }
            return valid;
        }

        private bool CalculateHighCard(List<string> Cards, List<string> ValidCards)
        {
            //SORTS THE CARDS AND RETURNS THE 5 HIGHEST CARDS

            bool valid = true;
            ValidCards.Clear();

            foreach (string card in Cards)
            {
                ValidCards.Add(card);
            }

            ValidCards.Sort();
            ValidCards.Reverse();
            int Added = 0;
            foreach (string card in ValidCards)
            {
                VALID.Add(card);
                Added += 1;
                if (Added == 5)
                {
                    break;
                }
            }



            return valid;
        }

    }
}
