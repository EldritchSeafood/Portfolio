/*
 * Name: Program.cs
 * Author: Dylan McNair
 * Purpose: Contains the main method, allows the user to set up to 8 users for a simplified game of scrabble
 */
using System;

namespace Info5060Project1 {
    class ScrabbleClient {
        static void Main(string[] args) {
            try {
                ScrabbleLibrary.Bag bag = new ScrabbleLibrary.Bag();

                Console.WriteLine(bag.About);

                Console.WriteLine("\nBag initialized with the following " + bag.TilesRemaining + " tiles...");
                Console.WriteLine(bag.ToString());

                // get user input on the number of players
                bool acceptedInput = false;
                int playerCount;
                string input;
                do {
                    Console.Write("\nEnter the number of players (1-8): ");
                    input = Console.ReadLine();
                    if (Int32.TryParse(input, out playerCount)) {
                        if (playerCount > 8 || playerCount <= 0) {
                            Console.WriteLine("invalid number of players, please try again");
                        } else {
                            acceptedInput = true;
                        }
                    } else {
                        Console.WriteLine("invalid input, please try again");
                    }
                } while (!acceptedInput);

                // create a rack and give a turn to each player
                for (int i = 1; i <= playerCount; i++) {
                    Console.WriteLine("-----------------------------------------------------------------------------\n" +
                        "                                   Player " + i.ToString() +
                        "\n-----------------------------------------------------------------------------");
                    ScrabbleLibrary.IRack rack = bag.NewRack();
                    Console.WriteLine("Your rack contains [" + rack.ToString() + "].");

                    bool testingWord = false;

                    acceptedInput = false;
                    int points = 0;
                    do {
                        points = 0;
                        Console.Write("Test a word for its points value? (y/n): ");
                        input = Console.ReadLine();
                        
                        if (input.ToLower() == "y") {
                            testingWord = true;
                        } else if (input.ToLower() =="n") {
                            testingWord = false;
                            acceptedInput = true;
                        } else {
                            Console.WriteLine("invalid input, please try again");
                        }

                        if (testingWord) {
                            Console.Write("Enter a word using the letters [" + rack.ToString() + "]: ");
                            input = Console.ReadLine();
                            points = rack.GetPoints(input);
                            Console.WriteLine("The word [" + input + "] is worth " + points.ToString() + " points.");
                            if (points != 0) {
                                testingWord = false;
                                do {
                                    Console.Write("Do you want to play the word [" + input + "]? (y/n): ");
                                    string input2 = Console.ReadLine();
                                    if (input2.ToLower() == "y") {
                                        testingWord = true;
                                        acceptedInput = true;
                                    } else if (input2.ToLower() =="n") {
                                        testingWord = true;
                                        acceptedInput = false;
                                    } else {
                                        Console.WriteLine("invalid input, please try again");
                                    }
                                } while (!testingWord);
                            } //end if
                        } //end if
                    } while (!acceptedInput);

                    if (points == 0) {
                        Console.Write("Enter a word using the letters [" + rack.ToString() + "]: ");
                        input = Console.ReadLine();
                        points = rack.GetPoints(input);
                        rack.PlayWord(input);
                        Console.WriteLine("\t------------------------\n" +
                            "\tWord Played: " + input +
                            "\n\tTotal Points: " + rack.TotalPoints.ToString() +
                            "\n\t------------------------");
                    } else {
                        rack.PlayWord(input);
                        Console.WriteLine("\t------------------------\n" +
                            "\tWord Played: " + input + 
                            "\n\tTotal Points: " + rack.TotalPoints.ToString() +
                            "\n\t------------------------");
                    }

                    Console.WriteLine("Your rack contains [" + rack.ToString() + "].");
                    Console.WriteLine("\nBag now contains the following " + bag.TilesRemaining + " tiles...");
                    Console.WriteLine(bag.ToString());
                } //end for()

                Console.WriteLine("\nAll Done!");
                bag.applic.Quit();

            } catch (InvalidOperationException ex) {
                throw new InvalidOperationException(
                    "Method was called in an inappropriate state", ex);
            } catch (ArgumentException ex) {
                Console.WriteLine("{0}: {1}", ex.GetType().Name, ex.Message);
            }
        }
    }
}
