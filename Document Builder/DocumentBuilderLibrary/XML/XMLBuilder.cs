using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary.XML {
    public class XMLBuilder : IBuilder {

        private XMLBranch root;
        private XMLBranch currentBranch;

        public void BuildBranch(string name) {
            XMLBranch branch = new XMLBranch(name, false, currentBranch);
            currentBranch.AddChild(branch);
            currentBranch = branch;
        }

        public void BuildLeaf(string name, string content) {
            XMLLeaf leaf = new XMLLeaf(name, content, currentBranch);
            currentBranch.AddChild(leaf);
        }

        public void CloseBranch() {
            currentBranch = currentBranch.GetParent();
        }

        public IComposite GetDocument() {
            Console.Write(root.Print(0));
            return root;
        }

        public XMLBuilder() {
            root = new XMLBranch("root", true, null);
            root.SetParent(root);
            currentBranch = root;
        }
    } //end class
}
