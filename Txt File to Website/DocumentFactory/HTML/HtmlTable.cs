using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlTable : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr = "<table>\n";
            string[] splitProps = props.Split(';');

            for (int i = 0; i < splitProps.Length; i++) {
                // split the properties even further
                string[] supSplitProps = splitProps[i].Split('$');
                formStr = formStr + "<tr>\n";
                for (int j = 0; j < supSplitProps.Length; j++) {
                    formStr = formStr + "<th>" + supSplitProps[j] + "</th>\n";
                }
                formStr = formStr + "</tr>\n";
            }
            formStr = formStr + "</table> <br />\n";

            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public HtmlTable(string props) {
            _props = toString(props);
        }
    }
}
