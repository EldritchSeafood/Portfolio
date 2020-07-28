using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlHeader : IElement {

        private readonly string _props;
        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr;
            string[] splitProps = props.Split(';');
            // check the whether h1, h2, or h3
            if (splitProps[0] == "1") {
                formStr = "<h1>" + splitProps[1] + "</h1> <br />\n";
            } else if (splitProps[0] == "2") {
                formStr = "<h2>" + splitProps[1] + "</h2> <br />\n";
            } else {
                formStr = "<h3>" + splitProps[1] + "</h3> <br />\n";
            }
            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public HtmlHeader(string props) {
            _props = toString(props);
        }
    }
}
