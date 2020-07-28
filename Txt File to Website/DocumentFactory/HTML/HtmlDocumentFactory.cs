using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlDocumentFactory : IDocumentFactory {
        public IDocument CreateDocument(string fileName) {
            return new HtmlDocument(fileName);
        }

        public IElement CreateElement(string elementType, string props) {
            if (elementType == "Table") {
                return new HtmlTable(props);
            } else if (elementType == "Image") {
                return new HtmlImage(props);
            } else if (elementType == "Header") {
                return new HtmlHeader(props);
            } else  { // should be else if "List" but we are only implemetning the four types
                return new HtmlList(props);
            }
        }
    }
}
