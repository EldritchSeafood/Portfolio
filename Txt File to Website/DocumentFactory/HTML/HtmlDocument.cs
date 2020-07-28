using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlDocument : IDocument {

        private readonly string _fileName;
        private string _fileContents;

        public void AddElement(IElement element) {
            _fileContents = _fileContents + element.getProps();
        }

        public void RunDocument() {
            _fileContents = _fileContents + "</html>";
            File.WriteAllText($"{_fileName}", _fileContents);
            Console.WriteLine($"Creating {_fileName} at ...\\docs\\{_fileName}");
            System.Diagnostics.Process.Start(_fileName);
        }

        public HtmlDocument(string fileName) {
            _fileName = fileName;
            _fileContents = "<html> <br>\n";
        }
    }
}
