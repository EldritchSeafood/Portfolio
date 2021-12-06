/*
 * Name: IBag.cs
 * Author: Dylan McNair
 * Purpose: Interface for the bag class
 */

namespace ScrabbleLibrary {
    interface IBag {
        public string About { get; }
        public int TilesRemaining { get; }
        public IRack NewRack();
        public string ToString();
    }
}
