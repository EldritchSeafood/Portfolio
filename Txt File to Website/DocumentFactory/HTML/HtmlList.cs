using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlList : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr;
            string[] splitProps = props.Split(';');

            if (splitProps[0] == "Ordered") {
                formStr = "<ol>\n";
                for (int i = 1; i < splitProps.Length; i++) {
                    formStr = formStr + "<li>" + splitProps[i] + "</li>\n";
                }
                formStr = formStr + "</ol> <br />\n";
            } else { 
                formStr = "<ul>\n";
                for (int i = 1; i < splitProps.Length; i++) {
                    formStr = formStr + "<li>" + splitProps[i] + "</li>\n";
                }
                formStr = formStr + "</ul> <br />\n";
            }

            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public HtmlList(string props) {
            _props = toString(props);
        }
    }
}
