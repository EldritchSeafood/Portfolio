/*
 * Program:         PasswordManager.exe
 * Module:          PasswordManager.cs
 * Date:            30/5/2020
 * Author:          Dylan McNair
 * Description:     
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;  // JavaScriptSerializer class
using System.IO;                        // File class
using Accounts;                         // Account Class
using PasswordObj;                      // Password Class
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PasswordManager {
    class Program {

        static void Main(string[] args) {
            Console.Write("PASSWORD MANAGEMENT SYSTEM\n\n");

            string curFile = @"..\\..\\accounts.json";
            AccountList accList = null;

            // Check if an accounts.json file already exists in the C-Drive
            // if one does then deserealize it
            if (File.Exists(curFile)) {
                Console.WriteLine("Accessing your accounts...\n");
                try {
                    accList = ReadJsonFileToAcc();
                    // Uncomment below to print a list of all accounts and their information at the start of the program
                    //Console.WriteLine("\n\nAccount Entries\n");
                    //foreach (Account account in accList.Accounts) {
                    //    Console.WriteLine($"Description: {account.Description}");
                    //    Console.WriteLine($"UserId: {account.UserId}");
                    //    Console.WriteLine($"Password: {account.Password.Value}");
                    //    Console.WriteLine($"Password Strength: {account.Password.StrengthText} ({account.Password.StrengthNum}%)");
                    //    Console.WriteLine($"Password Reset: {account.Password.LastReset}");
                    //    Console.WriteLine($"Login URL: {account.LoginUrl}");
                    //    Console.WriteLine($"Account #: {account.AccountNum}\n");
                    //}
                }
                catch (IOException) {
                    Console.WriteLine("ERROR: Can not read the JSON data file.");
                }
                catch (ArgumentException) {
                    Console.WriteLine("ERROR: Can not convert the JSON data to a AccountList object.");
                }
            } else { // if the file doesn't exist then create an empty collection
                Console.Write("Creating new accounts list\n");
                accList = new AccountList();
            }

            string choice;
            int intChoice = 0;
            bool done = false;
            bool valid;

            do { 
                do { // validate the users command input
                    if (accList.Accounts.Count == 0) {
                        Console.WriteLine("Press A to add a new entry.");
                        Console.WriteLine("Press X to exit.");
                        Console.Write("\nEnter a command: ");
                        choice = Console.ReadLine();
                        valid = (choice == "a" || choice == "A" || choice == "x" || choice == "X");
                        // if the choice is not a valid one then ask again
                        if (!valid)
                            Console.WriteLine("Invalid choice, try again.\n");
                    } else {
                        int count = 0;
                        Console.WriteLine("");
                        foreach (Account account in accList.Accounts) {
                            count++;
                            Console.WriteLine($"{count}. {account.Description}");
                        }
                        Console.WriteLine("\nPress # from above list to select an entry");
                        Console.WriteLine("Press A to add a new entry.");
                        Console.WriteLine("Press X to exit.");
                        Console.Write("\nEnter a command: ");
                        choice = Console.ReadLine();
                        valid = (choice == "a" || choice == "A" || choice == "x" || choice == "X");

                        // if the choice is not a valid one then check if its an int
                        // if it isn't then ask again
                        if (!valid) {
                            int i = 0;
                            try {
                                i = System.Convert.ToInt32(choice);
                                if (i > 0 && i <= accList.Accounts.Count) {
                                    intChoice = i;
                                    valid = true;
                                } else {
                                    Console.WriteLine("Invalid choice, try again.\n");
                                }
                            } catch (FormatException) {
                                // thrown when string does not convert to a valid integer
                                Console.WriteLine("Invalid choice, try again.\n");
                            } catch (OverflowException) {
                                // thrown if the string is valid but not for a 32bit integer
                                Console.WriteLine("Invalid choice, try again.\n");
                            }
                        }
                    }
                } while (!valid);

                if (choice == "x" || choice == "X") {
                    // END PROGRAM
                    done = true;
                } else if (choice == "a" || choice == "A") {
                    Account account = new Account();
                    Password password = new Password();
                    account.Password = password;

                    do {
                        Console.WriteLine("Please key-in values for the following fields...\n");
                        Console.Write("Description: ");
                        account.Description = Console.ReadLine();
                        Console.Write("UserId: ");
                        account.UserId = Console.ReadLine();
                        Console.Write("Password: ");
                        password.Value = Console.ReadLine();
                        // After entering the password assign the strength text and percent variables by calling the passwordtester
                        try {
                            PasswordTester pw = new PasswordTester(password.Value);
                            password.StrengthText = pw.StrengthLabel;
                            password.StrengthNum = pw.StrengthPercent;
                            password.LastReset = "";
                        } catch (ArgumentException) {
                            Console.WriteLine("ERROR: Invalid password format");
                        }
                        Console.Write("Login URL: ");
                        account.LoginUrl = Console.ReadLine();
                        if (account.UserId == null)
                            account.LoginUrl = "";
                        Console.Write("Account #: ");
                        account.AccountNum = Console.ReadLine();

                        // check the validaty of the data entered against the schema
                        using (StreamReader file = File.OpenText(@"..\..\AccountSchema.json"))
                        using (JsonTextReader reader = new JsonTextReader(file)) {
                            JSchema schema = JSchema.Load(reader);
                            JavaScriptSerializer ser = new JavaScriptSerializer();
                            string json = ser.Serialize(account);
                            JObject obj = JObject.Parse(json);
                            valid = obj.IsValid(schema);
                        }
                        // save if valid, repeat loop of not
                        if (valid == true) {
                            accList.AddAccount(account);
                            WriteAccToJson(accList);
                        } else {
                            Console.WriteLine("\nERROR: Invalid account information entered. Please try again.\n");
                        }
                    } while (!valid);
                } else { // the user has chosen an account to display
                    Console.WriteLine($"\n{intChoice}. {accList.Accounts[intChoice - 1].Description}");
                    Console.WriteLine($"UserId: {accList.Accounts[intChoice - 1].UserId}");
                    Console.WriteLine($"Password: {accList.Accounts[intChoice - 1].Password.Value}");
                    Console.WriteLine($"Password Strength: {accList.Accounts[intChoice - 1].Password.StrengthText} ({accList.Accounts[intChoice - 1].Password.StrengthNum}%)");
                    Console.WriteLine($"Password Reset: {accList.Accounts[intChoice - 1].Password.LastReset}");
                    Console.WriteLine($"Login URL: {accList.Accounts[intChoice - 1].LoginUrl}");
                    Console.WriteLine($"Account #: {accList.Accounts[intChoice - 1].AccountNum}\n\n");

                    do {
                        Console.WriteLine("Press P to change this password.");
                        Console.WriteLine("Press D to delete this entry.");
                        Console.WriteLine("Press M to return to the main menu.");
                        Console.Write("\nEnter a command: ");
                        choice = Console.ReadLine();
                        
                        valid = (choice == "p" || choice == "P" || choice == "d" || choice == "D" || choice == "m" || choice == "M");
                        // if the choice is not a valid one then ask again
                        if (!valid)
                            Console.WriteLine("Invalid choice, try again.\n");
                        if (choice == "p" || choice == "P") {
                            Console.Write("Password: ");
                            accList.Accounts[intChoice - 1].Password.Value = Console.ReadLine();
                            // After entering the password assign the strength text and percent variables by calling the passwordtester
                            try {
                                PasswordTester pw = new PasswordTester(accList.Accounts[intChoice - 1].Password.Value);
                                accList.Accounts[intChoice - 1].Password.StrengthText = pw.StrengthLabel;
                                accList.Accounts[intChoice - 1].Password.StrengthNum = pw.StrengthPercent;
                                accList.Accounts[intChoice - 1].Password.LastReset = DateTime.Now.ToString("yyyy/MM/dd");
                                WriteAccToJson(accList);
                            } catch (ArgumentException) {
                                Console.WriteLine("ERROR: Invalid password format");
                            }
                        } else if (choice == "d" || choice == "D") {
                            do { 
                                Console.Write("Delete? (Y/N): ");
                                choice = Console.ReadLine();
                                Console.Write("\n");
                                valid = (choice == "y" || choice == "Y" || choice == "n" || choice == "N");
                                if (!valid)
                                    Console.WriteLine("Invalid choice, try again.\n");
                            } while(!valid);
                            if (choice == "y" || choice == "Y") {
                                accList.Accounts.RemoveAt(intChoice - 1);
                                WriteAccToJson(accList);
                            }
                        } 
                    } while(!valid);
                } // end else
            } while(!done);


        } // end program

        // A helper method to accept an object of type AccountList, convert the object's  
        // data to a string containing JSON code and then write the string to a file 
        // This version of the method uses the JavaScriptSerializer class
        private static void WriteAccToJson(AccountList acc) {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string json = ser.Serialize(acc);
            File.WriteAllText("..\\..\\accounts.json", json);
        }

        // A helper method to read JSON data from a file as a string and then convert 
        // it into a AccountList object and return it from the method
        // This version of the method uses the JavaScriptSerializer class
        private static AccountList ReadJsonFileToAcc() {
            string json = File.ReadAllText("..\\..\\accounts.json");
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<AccountList>(json);
        }

    } // end class
}
