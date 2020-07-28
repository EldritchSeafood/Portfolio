using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DocumentBuilderLibrary.XML {
    public class XMLBranch : IComposite {

        private string _name;
        private XMLBranch _parent;
        private List<IComposite> _children = new List<IComposite>();
        private bool _isRoot;

        public void AddChild(IComposite child) {
            _children.Add(child);
        }

        public string Print(int depth) {
            string output = "";
            int tabs = 0;

            // add tabs for the depth
            while (tabs != depth) {
                output += "    ";
                tabs++;
            }

            output += "<" + _name + ">\n";

            for (int i = 0; i < _children.Count; i++) {
                output += _children[i].Print(depth + 1);
            }

            tabs = 0;
            while (tabs != depth) {
                output += "    ";
                tabs++;
            }
            output += "</" + _name + ">\n";

            return output;
        }

        public XMLBranch(string name, bool isRoot, XMLBranch parent) {
            _name = name;
            _isRoot = isRoot;
            _parent = parent;
        }

        public List<IComposite> GetChildren() {
            return _children;
        }

        public bool GetIsRoot() {
            return _isRoot;
        }

        public XMLBranch GetParent() {
            return _parent;
        }

        public void SetParent(XMLBranch parent) {
            _parent = parent;
        }
    }
}
