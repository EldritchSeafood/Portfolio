using System;
using DocumentBuilderLibrary;
using DocumentBuilderLibrary.JSON;
using DocumentBuilderLibrary.XML;

namespace Project2 {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Document Builder Console Client\n");
            DisplayHelp();

            bool exit = false;
            string input;
            bool modeSet = false;
            Director director = new Director(new JSONBuilder());

            do {
                Console.Write("> ");
                input = Console.ReadLine();
                string[] splitInput = input.Split(':');

                if (String.Equals(splitInput[0], "help",
                   StringComparison.OrdinalIgnoreCase) && splitInput.Length == 1) {

                    DisplayHelp();

                } else if (String.Equals(splitInput[0], "mode",
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 2) {

                    if (String.Equals(splitInput[1], "xml", StringComparison.OrdinalIgnoreCase)) {
                        director = new Director(new XMLBuilder());
                        modeSet = true;
                    } else if (String.Equals(splitInput[1], "json", StringComparison.OrdinalIgnoreCase)) {
                        director = new Director(new JSONBuilder());
                        modeSet = true;
                    } else {
                        Console.WriteLine("Invalid input. For Usage, type 'Help'\n");
                    }

                } else if (String.Equals(splitInput[0], "branch", 
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 2 && modeSet == true) {

                    director.SetName(splitInput[1]);
                    director.BuildBranch();

                } else if (String.Equals(splitInput[0], "leaf", 
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 3 && modeSet == true) {

                    director.SetName(splitInput[1]);
                    director.SetContent(splitInput[2]);
                    director.BuildLeaf();

                } else if (String.Equals(splitInput[0], "close", 
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 1 && modeSet == true) {

                    director.CloseBranch();

                } else if (String.Equals(splitInput[0], "print", 
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 1 && modeSet == true) {

                    director.PrintDocument();

                } else if (String.Equals(splitInput[0], "exit", 
                    StringComparison.OrdinalIgnoreCase) && splitInput.Length == 1) {

                    exit = true;

                } else {
                    Console.WriteLine("\nInvalid input. For Usage, type 'Help'\n");
                }

            } while (!exit);
        }

        static void DisplayHelp() {
            Console.WriteLine("Usage:");
            Console.WriteLine("\thelp\t\t\t-Prints Usage (this page)");
            Console.WriteLine("\tmode:<JSON|XML>\t\t-Sets mode to JSON or XML. Must be set before creating or closing.");
            Console.WriteLine("\tbranch:<name>\t\t-Creates a new branch, assigning it the passed name.");
            Console.WriteLine("\tleaf:<name>:<content>\t-Creates a new leaf, assigning it the passed name and content.");
            Console.WriteLine("\tclose\t\t\t-Closes the current branch, as long as it is not the root.");
            Console.WriteLine("\tprint\t\t\t-Prints the document in its current state to the console.");
            Console.WriteLine("\texit\t\t\t-Exits the application.\n");
        }
    }
}
