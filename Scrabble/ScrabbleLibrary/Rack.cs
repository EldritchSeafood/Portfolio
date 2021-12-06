/*
 * Name: Rack.cs
 * Author: Dylan McNair
 * Purpose: Implements IRack, should not be visible to client, contains methods that allow the user to play a simplified turn of scrabble
 */
using System;
using System.Collections.Generic;

namespace ScrabbleLibrary {
    class Rack : IRack {
        int points;
        List<char> lettersAvailable;
        Bag bag;
        
        public Rack(Bag b) {
            points = 0;
            lettersAvailable = new List<char>();
            bag = b;
        }

        // read-only property that returns the users total points
        public int TotalPoints { get { return points; } }

        // checks the validity of a word both by how it is spelt and whether the rack contains the letters necessary
        //      to spell it, then returns how many points that word would be worth according to its component letters
        public int GetPoints(string candidate) {
            if (bag.applic.CheckSpelling(candidate.ToLower())) { //if the candidate is a word then check if all the letters used are available in the rack
                int totalScore = 0;
                char[] candArr = candidate.ToCharArray(0, candidate.Length);
                List<char> rackCopy = new List<char>(lettersAvailable);


                for (int i = 0; i < candArr.Length; i++) {
                    bool validLetter = false;
                    for (int j = 0; j < rackCopy.Count; j++) {
                        if (rackCopy[j].ToString().Equals(candArr[i].ToString(), StringComparison.InvariantCultureIgnoreCase) && validLetter == false) {
                            rackCopy.RemoveAt(j);
                            validLetter = true;
                            totalScore += bag.tilePointValues[candArr[i].ToString().ToLower()];
                        }
                    }
                    if (validLetter == false) {
                        return 0;
                    }
                }
                return totalScore;
            } else { // if not a word then it is disqualified
                return 0;
            }
        } //end GetPoints()

        // checks the words validity and points value using GetPoints() then add the points to the rack's points and remove used letters from the lettersAvailable
        public string PlayWord(string candidate) {
            int totalScore = GetPoints(candidate);
            if (totalScore != 0) { 
                char[] candArr = candidate.ToCharArray(0, candidate.Length);

                for (int i = 0; i < candArr.Length; i++) {
                    for (int j = 0; j < lettersAvailable.Count; j++) {
                        if (lettersAvailable[j].ToString().Equals(candArr[i].ToString(), StringComparison.InvariantCultureIgnoreCase)) {
                            lettersAvailable.RemoveAt(j);
                        }
                    }
                }
            }//end if

            TopUp();
            points = totalScore;
            return "word";
        } //end PlayWord()

        // Fill the rack's lettersAvailabing up to 7 by removing tokens from the bag
        public string TopUp() {
            List<char> lettersInBag = new List<char>();
            int tilesNeeded = 7 - lettersAvailable.Count;
            string output = "";
            Random rnd = new Random();

            foreach (char letter in lettersAvailable) {
                output += letter;
            }

            if (tilesNeeded != 0 && bag.tilesAmount != 0) {
                foreach (KeyValuePair<char, int> letter in bag.tilesInBag) { // add every letter still in the bag 
                    lettersInBag.Add(letter.Key);
                }
                // randomly select enough tokens to fill the lettersAvailable
                do {
                    int randomNumb = rnd.Next(0, lettersInBag.Count);
                    char letter = lettersInBag[randomNumb];
                    lettersAvailable.Add(letter);
                    output += letter;
                    bag.tilesAmount--;
                    bag.tilesInBag[letter]--;
                    if (bag.tilesInBag[letter] == 0) {
                        bag.tilesInBag.Remove(letter);
                        lettersInBag.RemoveAt(randomNumb);
                    }
                    tilesNeeded--;
                } while (tilesNeeded != 0 && bag.tilesAmount != 0);
            }
            
            return output;
        } //end TopUp()

        // override ToString() to print out the rack's lettersAvailable
        public override string ToString() {
            string output = "";
            for (int i = 0; i < lettersAvailable.Count; i++) {
                output += lettersAvailable[i];
            }
            return output;
        } //end ToString()
    }
}
