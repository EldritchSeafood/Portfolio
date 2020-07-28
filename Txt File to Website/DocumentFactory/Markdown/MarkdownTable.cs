using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.Markdown {
    public class MarkdownTable : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr = "| ";
            string[] splitProps = props.Split(';');

            for (int i = 0; i < splitProps.Length; i++) {
                // split the properties even further
                string[] supSplitProps = splitProps[i].Split('$');
                for (int j = 0; j < supSplitProps.Length; j++) {
                    formStr = formStr + supSplitProps[j] + " | ";
                }
                // don't print this on the final one
                if (i != splitProps.Length - 1)
                    formStr = formStr + "\n | ";
                // for every colum there needs the seperation but only once
                if (i == 0) { 
                    for (int k = 0; k < supSplitProps.Length; k++) {
                    formStr = formStr + "--- | ";
                }
                formStr = formStr + "\n";
                }
            }
            formStr = formStr + "<br>\n";

            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public MarkdownTable(string props) {
            _props = toString(props);
        }
    }
}
