using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.Markdown {
    public class MarkdownList : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr = "";
            string[] splitProps = props.Split(';');

            if (splitProps[0] == "Ordered") {
                for (int i = 1; i < splitProps.Length; i++) {
                    formStr = formStr + i + ". " + splitProps[i] + "\n";
                }
                formStr = formStr + "<br>\n[//]: # (EndList)\n";
            } else { 
                for (int i = 1; i < splitProps.Length; i++) {
                    formStr = formStr + "- " + splitProps[i] + "\n";
                }
                formStr = formStr + "<br>\n[//]: # (EndList)\n";
            }

            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public MarkdownList(string props) {
            _props = toString(props);
        }
    }
}
