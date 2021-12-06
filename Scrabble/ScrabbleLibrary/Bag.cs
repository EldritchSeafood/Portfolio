/*
 * Name: Bag.cs
 * Author: Dylan McNair
 * Purpose: Implements IBag, maintains a series of tokens with alphabetical letters that can be handed out
 */
using System.Collections.Generic;
using Microsoft.Office.Interop.Word;

namespace ScrabbleLibrary {
    public class Bag : IBag {
        public int tilesAmount;
        public Dictionary<char, int> tilesInBag;
        public Dictionary<string, int> tilePointValues; // how many points each letter is worth
        public Application applic;

        public Bag() {
            applic = new Application();
            tilesInBag = new Dictionary<char, int>() {
                { 'a', 9 },
                { 'b', 2 },
                { 'c', 2 },
                { 'd', 4 },
                { 'e', 12 },
                { 'f', 2 },
                { 'g', 3 },
                { 'h', 2 },
                { 'i', 9 },
                { 'j', 1 },
                { 'k', 1 },
                { 'l', 4 },
                { 'm', 2 },
                { 'n', 6 },
                { 'o', 8 },
                { 'p', 2 },
                { 'q', 1 },
                { 'r', 6 },
                { 's', 4 },
                { 't', 6 },
                { 'u', 4 },
                { 'v', 2 },
                { 'w', 2 },
                { 'x', 1 },
                { 'y', 2 },
                { 'z', 1 }
            };
            foreach (KeyValuePair<char, int> letter in tilesInBag) {
                tilesAmount += letter.Value;
            }
            tilePointValues = new Dictionary<string, int>() {
                { "a", 1 },
                { "b", 3 },
                { "c", 3 },
                { "d", 2 },
                { "e", 1 },
                { "f", 4 },
                { "g", 2 },
                { "h", 4 },
                { "i", 1 },
                { "j", 8 },
                { "k", 5 },
                { "l", 1 },
                { "m", 3 },
                { "n", 1 },
                { "o", 1 },
                { "p", 3 },
                { "q", 10 },
                { "r", 1 },
                { "s", 1 },
                { "t", 1 },
                { "u", 1 },
                { "v", 4 },
                { "w", 4 },
                { "x", 8 },
                { "y", 4 },
                { "z", 10 }
            };
        } //end Bag()

        // print information about the author and the client
        public string About { get { return "Test Client for: Scrabble (TM) Library, © 2021 D. McNair"; } }

        // read-only property that returns the amount of tiles in the bag
        public int TilesRemaining { get { return tilesAmount; } }

        // create a rack and top it up
        public IRack NewRack() {
            Rack rack = new Rack(this);
            rack.TopUp();
            return rack;
        } //end NewRack()

        // override ToString() to print out the tiles remaining in the bag and their amounts
        public override string ToString() {
            string output = "";
            int breakpoint = 0;
            foreach (KeyValuePair<char, int> letter in tilesInBag) {
                output += letter.Key + "(" + letter.Value.ToString() + ") ";
                breakpoint++;
                if (breakpoint == 13) { output += "\n"; }
            }
            return output;
        } //end ToString()
    }
}
