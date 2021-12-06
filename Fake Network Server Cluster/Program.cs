using System;
using System.Collections.Generic;

namespace Assi3 {
    class Program {
        static void Main(string[] args) {
            List<Server> servers = new List<Server>();
            Queue<Request> pendingRequests = new Queue<Request>();
            Query query = new ServerQuery();
            Route addRoute = new AddRoute("/add");
            Route mul4Route = new Multiply4Route("/mul/4", addRoute);
            Route mulRoute = new MultiplyRoute("/mul", mul4Route);
            Route baseRoute = new Route(null, mulRoute);

            Console.WriteLine("Please enter a command.");
            string command = "";

            while(command != "quit") {
                string[] commandArgs = command.Split(":");
                Console.WriteLine();

                switch (commandArgs[0]) {
                    case "help":
                        Console.WriteLine("help\t\t\tDisplay this menu");
                        Console.WriteLine("createserver\t\tCreate a new server.");
                        Console.WriteLine("deleteserver:[id]\tDelete server #ID.");
                        Console.WriteLine("listservers\t\tList all servers.");
                        Console.WriteLine("new:[path]:[payload]\tCreate a new pending request.");
                        Console.WriteLine("dispatch\t\tSend a pending request to a server.");
                        Console.WriteLine("server:[id]\t\tHave server #ID execute its pending request and print the result.");
                        Console.WriteLine("quit\t\t\tQuit the application");
                        break;
                    case "createserver":
                        servers.Add(new Server());
                        Console.WriteLine("Created server " + (servers.Count - 1) + ".");
                        break;
                    case "deleteserver":
                        int i = int.Parse(commandArgs[1]);
                        if (i < servers.Count && i >= 0 && servers.Count != 0) {
                            servers.RemoveAt(i);
                            Console.WriteLine("Deleted server " + i + ".");
                        } else {
                            Console.WriteLine("Invelid ID specified");
                        }
                        break;
                    case "listservers":
                        if (servers.Count != 0) { 
                            for (int ii = 0; ii < servers.Count; ii++) {
                                Console.WriteLine(ii + "\tServer");
                            }
                        } else {
                            Console.WriteLine("No servers available.");
                        }
                        break;
                    case "new":
                        int payload;
                        if (int.TryParse(commandArgs[2], out payload)) {
                            string path = commandArgs[1];
                            pendingRequests.Enqueue(new Request(path, payload));
                            Console.WriteLine("Created request with data " + payload.ToString() + " going to " + path + ".");
                        } else {
                            Console.WriteLine("Invalid payload specified.");
                        }
                        break;
                    case "dispatch":
                        if (pendingRequests.Count != 0) {
                            bool notAvail = true;
                            for (int iii = 0; iii < servers.Count; iii++) { 
                                if (servers[iii].AcceptQuery(query)) {
                                    servers[iii].SetRequest(pendingRequests.Dequeue());
                                    Console.WriteLine("Sent request to Server " + iii.ToString() + ".");
                                    notAvail = false;
                                    break;
                                }
                            }
                            // if none of the servers were available
                            if (notAvail) {
                                Console.WriteLine("No servers available (521).");
                            }
                        } else {
                            Console.WriteLine("No pending request.");
                        }
                        break;
                    case "server":
                        int serverIndex;
                        if (int.TryParse(commandArgs[1], out serverIndex)) {
                            if (serverIndex < servers.Count && serverIndex >= 0 && servers.Count != 0) {
                                Console.WriteLine(servers[serverIndex].GetRequestInfo());
                                Console.WriteLine("Result: " + servers[serverIndex].Execute(baseRoute).ToString());
                                servers[serverIndex].SetRequest(null);
                            } else {
                                Console.WriteLine("Invalid ID specified.");
                            }
                        } else {
                            Console.WriteLine("Invalid ID specified.");
                        }
                        break;
                    default:
                        if(command != "") {
                            Console.WriteLine("Invalid command.");
                        }
                        break;
                }

                Console.WriteLine();
                command = Console.ReadLine();
            }
        }
    }
}
