using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace Director {
    class Program {

        static void Main(string[] args) {
            string[] commands;
            IDocumentFactory factory = null;
            IDocument doc = null;
            var list = File.ReadAllText("CreateDocumentScript.txt");
            commands = list.Split('#');

            foreach (var command in commands) {
                var strippedCommand = Regex.Replace(command, @"\t|\n|\r", "");
                var commandList = strippedCommand.Split(':');
                switch (commandList[0]) {
                    case "Document":
                        string[] docType = commandList[1].Split(';');
                        // Your document creation code goes here
                        if (docType[0] == "Html") {
                            factory = new DocumentFactory.HTML.HtmlDocumentFactory();
                            doc = factory.CreateDocument(docType[1]);
                        } else {
                            factory = new DocumentFactory.Markdown.MarkdownDocumentFactory();
                            doc = factory.CreateDocument(docType[1]);
                        }
                        break;
                    case "Element":
                        // Your element creation code goes here
                        doc.AddElement(factory.CreateElement(commandList[1], commandList[2]));
                        break;
                    case "Run":
                        // Your document running code goes here
                        doc.RunDocument();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
