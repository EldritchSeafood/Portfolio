using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary.XML {
    public class XMLLeaf : IComposite {

        private string _name;
        private string _content;
        private XMLBranch _parent;

        public void AddChild(IComposite child) {
            Console.WriteLine("Leaves cannot have children.\n");
        }

        public string Print(int depth) {
            string output = "";
            int i = 0;
            while (i != depth) {
                output += "    ";
                i++;
            }
            return output += "<" + _name + ">" + _content + "</" + _name + ">\n";
        }

        public XMLLeaf(string name, string content, XMLBranch parent) {
            _name = name;
            _content = content;
            _parent = parent;
        }
    }
}
