﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentFactory.Interfaces {
    public interface IDocument {
        void AddElement(IElement element);
        void RunDocument();
    }
}
