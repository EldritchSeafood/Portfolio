using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.Markdown {
    public class MarkdownHeader : IElement {

        private readonly string _props;
        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr;
            string[] splitProps = props.Split(';');
            // check the whether h1, h2, or h3
            if (splitProps[0] == "1") {
                formStr = "# " + splitProps[1] + "<br>\n";
            } else if (splitProps[0] == "2") {
                formStr = "## " + splitProps[1] + "<br>\n";
            } else {
                formStr = "### " + splitProps[1] + "<br>\n";
            }
            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public MarkdownHeader(string props) {
            _props = toString(props);
        }
    }
}
