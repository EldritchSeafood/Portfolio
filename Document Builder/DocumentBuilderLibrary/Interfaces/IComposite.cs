using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary {
    public interface IComposite {
        void AddChild(IComposite child);
        string Print(int depth);
    }
}
