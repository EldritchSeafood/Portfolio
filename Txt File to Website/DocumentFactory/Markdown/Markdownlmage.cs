using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.Markdown {
    public class MarkdownImage : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr;
            string[] splitProps = props.Split(';');
            formStr = "![" + splitProps[1] + "]"
                        + "(" + splitProps[0]
                        + " \"" + splitProps[2] + "\") <br>\n";

            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public MarkdownImage(string props) {
            _props = toString(props);
        }
    }
}
