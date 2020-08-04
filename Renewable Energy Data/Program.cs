using System;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace Renewable_Energy_Project {
    class Program {
        // Static member constants and variables
        private static string _XML_FILE = "renewable-energy.xml"; // File is in the bin\Debug\netcoreapp3.1 folder
        private static string[] _parameters = null;
        private static XmlDocument _doc = null;

        static void Main(string[] args) {
            Console.WriteLine("Renewable Energy Production in 2016");
            Console.WriteLine("===================================");

            bool valid, done = false;
            string input = "", output = "";
            string[] renewNames = { "Hydro", "Wind", "Biomass", "Solar", "Geothermal" };

            try {
                _doc = new XmlDocument();
                _doc.Load(_XML_FILE);
                getParameters();

                // start looping for user input until they choose to stop
                do {
                    do {
                        valid = true;
                        Console.Write("\nEnter 'C' to select a country, 'R' to select a specific renewable, " +
                            "or 'P' to select \na % range of renewables production: ");
                        input = Console.ReadLine();
                        Console.Write("\n");

                        if (String.Equals(input, "c", StringComparison.OrdinalIgnoreCase)) {
                            int selection;
                            Console.WriteLine("Select a country by number as shown below...");
                            displayAll();
                            valid = false;
                            do {
                                Console.Write("\n\nEnter a Country #: ");
                                input = Console.ReadLine();
                                if (int.TryParse(input, out selection)) {
                                    valid = true;
                                    displayByCountry(selection);
                                } else {
                                    Console.WriteLine("\nInput not valid, please try again.\n");
                                }
                            } while (!valid);

                        } else if (String.Equals(input, "r", StringComparison.OrdinalIgnoreCase)) {
                            displayByRenewable();

                        } else if (String.Equals(input, "p", StringComparison.OrdinalIgnoreCase)) {
                            Double min, max;

                            // get the minimum percentage of renewables
                            valid = false;
                            do {
                                Console.Write("Enter the minimum % of renewables produced of press enter for no minimum: ");
                                input = Console.ReadLine();
                                if (Double.TryParse(input, out min)) {
                                    if (min >= 0) {
                                        valid = true;
                                    } else {
                                        Console.WriteLine("\nMinimum need to be greater than or equal to 0.\n");
                                    }
                                } else if (input == "") {
                                    valid = true;
                                    min = 0.0;
                                } else {
                                    Console.WriteLine("\nInput not valid, please try again.\n");
                                }
                            } while (!valid);

                            // now get the maximum
                            valid = false;
                            do {
                                Console.Write("Enter the maximum % of renewables produced of press enter for no maximum: ");
                                input = Console.ReadLine();
                                if (Double.TryParse(input, out max)) {
                                    if (max <= 100 && max > min) {
                                        valid = true;
                                    } else {
                                        Console.WriteLine("\nMaximum needs to be less than or equal to 100 and greater than minimum.\n");
                                    }
                                } else if (input == "") {
                                    valid = true;
                                    max = 100.0;
                                } else {
                                    Console.WriteLine("\nInput not valid, please try again.\n");
                                }
                            } while (!valid);

                            // print title based on user input
                            if (min == 0.0 && max == 100.0) {
                                output = "\nCombined Renewables for All Countries";
                            } else if (min == 0.0 && max != 100.0) {
                                output = $"\nCountries Where Renewables Count for Up To {max.ToString("0.00")}% of Energy Production";
                            } else if (min != 0.0 && max == 100.0) {
                                output = $"\nCountries Where Renewables Count for At Least {min.ToString("0.00")}% of Energy Production";
                            } else {
                                output = $"\nCountries Where Renewables Count for {min.ToString("0.00")}% to {max.ToString("0.00")}% of Energy Production";
                            }
                            Console.WriteLine(output);
                            Console.WriteLine(new String('-', output.Length) + "\n");
                            displayByProduction(min, max);

                        } else {
                            valid = false;
                            Console.WriteLine("\nInput not valid, please try again.\n");
                        }
                    } while (!valid);

                    // ask the user if they want to continue
                    Console.Write("\n");
                    do {
                        valid = true;
                        Console.Write("Would you like to continue? (y or n): ");
                        input = Console.ReadLine();
                        if (String.Equals(input, "n", StringComparison.OrdinalIgnoreCase)) {
                            done = true;
                        } else if (!(String.Equals(input, "y", StringComparison.OrdinalIgnoreCase))) {
                            valid = false;
                            Console.WriteLine("\nInput not valid, please try again.\n");
                        }
                    } while (!valid);

                } while (!done);

            } catch (XmlException ex) {
                Console.WriteLine($"ERROR: {ex.Message}");
            } catch (XPathException ex) {
                Console.WriteLine($"ERROR: {ex.Message}");
            } catch (Exception ex) {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        } // end main()

        /*--------------------Helper Methods--------------------*/
        private static void getParameters() {
            // Obtain all country names
            XmlNodeList nodes = _doc.SelectNodes("//country/@name");
            _parameters = new string[nodes.Count];

            // For each child element returned ...
            for (int i = 0; i < nodes.Count; ++i)
            {
                // Store the element name in _parameters[i]
                _parameters[i] = nodes[i].Value;
            }

        } // end getParameters()

        private static void displayAll() {
            int count = 1, i = 1;
            string format = "{0, 5} {1, -40}";
            string output = "";
            XPathNavigator nav = _doc.CreateNavigator();
            XPathNodeIterator nodeIt = nav.Select("//country/@name");

            while (nodeIt.MoveNext()) {
                output = string.Format(format, i++ + ". ", nodeIt.Current.Value);
                //Console.Write($"\t{i++}. {nodeIt.Current.Value}");
                Console.Write(output);
                if (count++ == 3) {
                    Console.Write("\n");
                    count = 1;
                }
            }
        } // end displayAll()

        private static void displayByCountry(int selection) {
            int count = 0;
            string format = "{0, 17}", output, input;
            XmlNodeList nodes = _doc.SelectNodes($"//country[@name = '{_parameters[selection - 1]}']/renewable");

            output = $"Renewable Energy Production in {_parameters[selection - 1]}";
            Console.WriteLine(output);
            Console.WriteLine(new String('-', output.Length));

            output = String.Format(format, "Renewable Type") + String.Format(format, "Amount (Gwh)")
                + String.Format(format, "% of Totals") + String.Format(format, "% of Renewables");

            Console.WriteLine("\n" + output + "\n");

            // for each renewable energy source in the nodes list get the information and print it
            string[] attributes = { "type", "amount", "percent-of-all", "percent-of-renewables" };
            foreach (XmlElement node in nodes) {
                count++;
                output = "";

                for (int i = 0; i <= 3; i++) {
                    input = node.HasAttribute(attributes[i]) ? node.GetAttribute(attributes[i]) : "n/a";
                    // format the attribute to fit, capitalizing types and putting thousand seperators in amount
                    if (i == 0 && input != "n/a")
                        input = input.First().ToString().ToUpper() + input.Substring(1);
                    if (i == 1 && input != "n/a")
                        input = String.Format("{0:#,0.##}", double.Parse(input));
                    output += String.Format(format, input);
                }
                Console.WriteLine(output);
            }

            Console.WriteLine($"\n{count} match(es) found");

        } // end displayByCountry()

        private static void displayByRenewable() {
            string input = "", format = "{0, 17}";
            bool valid = true;
            int count = 0, selection;

            // use XML to search for every type of energy and then only keep distinct ones
            XmlNodeList nodes = _doc.SelectNodes($"//country/renewable/@type");
            string[] renewNames = new string[nodes.Count];
            string output = "";
            for (int i = 0; i < nodes.Count; ++i) {
                renewNames[i] = nodes[i].Value;
            }
            renewNames = renewNames.Distinct().ToArray();

            do {
                valid = true;
                Console.WriteLine("Select a renewable by number as shown below...");

                for (int i = 0; i < renewNames.Length; ++i) {
                    Console.WriteLine($"{ i + 1}. {renewNames[i]}");
                }


                Console.Write("\nEnter a renewable #: ");
                input = Console.ReadLine();
                Console.Write("\n");

                if (int.TryParse(input, out selection) && selection - 1 <= renewNames.Length && selection > 0) {
                    string title = String.Format("{0}{1} Energy Production", 
                        renewNames[selection - 1].First().ToString().ToUpper(), renewNames[selection - 1].Substring(1));
                    Console.WriteLine(title);
                    Console.WriteLine(new String('-', title.Length) + "\n");

                    // get the names for all countries that fit the criteria provided
                    XmlNodeList countryNodes = _doc.SelectNodes($"//country[renewable[@type = '{renewNames[selection - 1]}']]/@name");
                    string[] countryNames = new string[countryNodes.Count];

                    for (int i = 0; i < countryNodes.Count; ++i) {
                        countryNames[i] = countryNodes[i].Value;
                    }

                    XmlNodeList energyNodes = _doc.SelectNodes($"//renewable[@type = '{renewNames[selection - 1]}']");

                    output = String.Format("{0, 35}", "Country") + String.Format(format, "Amount (Gwh)")
                        + String.Format(format, "% of Totals") + String.Format(format, "% of Renewables");

                    Console.WriteLine("\n" + output + "\n");

                    // for each renewable energy source in the nodes list get the information and print it
                    string[] attributes = { "amount", "percent-of-all", "percent-of-renewables" };
                    foreach (XmlElement node in energyNodes) {
                        output = String.Format("{0, 35}", countryNames[count]);

                        for (int i = 0; i <= 2; i++) {
                            input = node.HasAttribute(attributes[i]) ? node.GetAttribute(attributes[i]) : "n/a";
                            if (i == 0 && input != "n/a")
                                input = String.Format("{0:#,0.##}", double.Parse(input));
                            output += String.Format(format, input);
                        }
                        Console.WriteLine(output);
                        count++;
                    }

                    Console.WriteLine($"\n{count} match(es) found");

                } else {
                    valid = false;
                    Console.WriteLine("\nInput not valid, please try again.\n");
                }
            } while (!valid);

        } // end displayByRenewable()

        private static void displayByProduction(double min, double max) {
            string format = "{0, 18}", input = "";
            int count = 0;

            // get the names for all countries that fit the criteria provided
            XmlNodeList countryNodes = _doc.SelectNodes($"//country[totals[@renewable-percent >= '{min}' and @renewable-percent <= '{max}']]/@name");
            string[] countryNames = new string[countryNodes.Count];

            for (int i = 0; i < countryNodes.Count; ++i) {
                countryNames[i] = countryNodes[i].Value;
            }

            string output = String.Format("{0, 35}", "Country") + String.Format(format, "All Energy (Gwh)")
                        + String.Format(format, "Renewable (Gwh)") + String.Format(format, "% Renewable");

            Console.WriteLine("\n" + output + "\n");

            XmlNodeList energyNodes = _doc.SelectNodes($"//country/totals[@renewable-percent >= '{min}' and @renewable-percent <= '{max}']");

            // for each renewable energy source in the nodes list get the information and print it
            string[] attributes = { "all-sources", "all-renewables", "renewable-percent" };
            foreach (XmlElement node in energyNodes) {
                output = String.Format("{0, 35}", countryNames[count]);

                for (int i = 0; i <= 2; i++) {
                    input = node.HasAttribute(attributes[i]) ? node.GetAttribute(attributes[i]) : "n/a";
                    if (input != "n/a")
                        input = String.Format("{0:#,0.##}", double.Parse(input));
                    output += String.Format(format, input);
                }
                Console.WriteLine(output);
                count++;
            }

            Console.WriteLine($"\n{count} match(es) found");

        }
    }
}
