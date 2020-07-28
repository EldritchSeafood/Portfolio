using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary {
    public interface IDirector {
        void BuildBranch();
        void BuildLeaf();
        void CloseBranch();
    }
}
