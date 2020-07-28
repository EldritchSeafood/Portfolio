using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary {
    public interface IBuilder {
        void BuildBranch(string name);
        void BuildLeaf(string name, string content);
        void CloseBranch();
        IComposite GetDocument();
    }
}
