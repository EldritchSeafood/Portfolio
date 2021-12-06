/*
 * Name: IRack.cs
 * Author: Dylan McNair
 * Purpose: Interface for the rack class
 */

namespace ScrabbleLibrary {
    public interface IRack {
        public int TotalPoints { get; }
        public int GetPoints(string candidate);
        public string PlayWord(string candidate);
        public string TopUp();
        public string ToString();
    }
}
