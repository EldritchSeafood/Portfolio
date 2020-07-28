using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary.JSON {
    public class JSONBuilder : IBuilder {

        private JSONBranch root;
        private JSONBranch currentBranch;

        public void BuildBranch(string name) {
            JSONBranch branch = new JSONBranch(name, false, currentBranch);
            currentBranch.AddChild(branch);
            currentBranch = branch;
        }

        public void BuildLeaf(string name, string content) {
            JSONLeaf leaf = new JSONLeaf(name, content, currentBranch);
            currentBranch.AddChild(leaf);
        }

        public void CloseBranch() {
            currentBranch = currentBranch.GetParent();
        }

        public IComposite GetDocument() {
            Console.Write(root.Print(0));
            return root;
        }

        public JSONBuilder() {
            root = new JSONBranch("root", true, null);
            root.SetParent(root);
            currentBranch = root;
        }
    } //end class
}
