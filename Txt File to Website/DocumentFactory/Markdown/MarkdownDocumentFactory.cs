using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.Markdown {
    public class MarkdownDocumentFactory : IDocumentFactory {
        public IDocument CreateDocument(string fileName) {
            return new MarkdownDocument(fileName);
        }

        public IElement CreateElement(string elementType, string props) {
            if (elementType == "Table") {
                return new MarkdownTable(props);
            } else if (elementType == "Image") {
                return new MarkdownImage(props);
            } else if (elementType == "Header") {
                return new MarkdownHeader(props);
            } else  { // should be else if "List" but we are only implemetning the four types
                return new MarkdownList(props);
            }
        }
    }
}
