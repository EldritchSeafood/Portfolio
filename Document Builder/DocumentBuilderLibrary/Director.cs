using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentBuilderLibrary {
    public class Director : IDirector {

        protected IBuilder _type;
        private string _name;
        private string _content;

        public void BuildBranch() {
            _type.BuildBranch(_name);
        }

        public void BuildLeaf() {
            _type.BuildLeaf(_name, _content);
        }

        public void CloseBranch() {
            _type.CloseBranch();
        }

        public void PrintDocument() {
            _type.GetDocument();
        }

        public Director(IBuilder type) {
            _type = type;
        }

        public void SetName(string name) {
            _name = name;
        }

        public void SetContent(string content) {
            _content = content;
        }

    }
}
