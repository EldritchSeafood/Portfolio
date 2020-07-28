using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary.JSON {
    public class JSONLeaf : IComposite {

        private string _name;
        private string _content;
        private JSONBranch _parent;

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
            return output += "'" + _name + "': '" + _content + "'\n";
        }

        public JSONLeaf(string name, string content, JSONBranch parent) {
            _name = name;
            _content = content;
            _parent = parent;
        }
    }
}
