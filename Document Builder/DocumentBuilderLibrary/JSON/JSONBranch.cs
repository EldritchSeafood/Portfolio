using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary.JSON {
    public class JSONBranch : IComposite {

        private string _name;
        private JSONBranch _parent;
        private List<IComposite> _children = new List<IComposite>();
        private bool _isRoot;

        public void AddChild(IComposite child) {
            _children.Add(child);
        }

        public string Print(int depth) {
            string output = "";
            int tabs = 0;

            if (!_isRoot) { 
                // add tabs for the depth
                while (tabs != depth) {
                    output += "    ";
                    tabs++;
                }
                output += "'" + _name + "':\n";
            }
            tabs = 0;
            // add tabs for the depth
            while (tabs != depth) {
                output += "    ";
                tabs++;
            }
            output += "{\n";
            // repeat print for each child
            for (int i = 0; i < _children.Count; i++) {
                output += _children[i].Print(depth + 1);
            }

            tabs = 0;
            // add tabs for the depth
            while (tabs != depth) {
                output += "    ";
                tabs++;
            }
            output += "}\n";

            return output;
        }

        public JSONBranch(string name, bool isRoot, JSONBranch parent) {
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

        public JSONBranch GetParent() {
            return _parent;
        }

        public void SetParent(JSONBranch parent) {
            _parent = parent;
        }
    }
}
