using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFactory.Interfaces;

namespace DocumentFactory.HTML {
    public class HtmlImage : IElement {

        private readonly string _props;

        public string getProps() {
            return _props;
        }

        public string toString(string props) {
            string formStr;
            string[] splitProps = props.Split(';');
            formStr = "<img src=\"" + splitProps[0] + "\""
                        + " alt=\"" + splitProps[1] + "\""
                        + " title=\"" + splitProps[2] + "\" /> <br />\n";
            //Console.WriteLine($"{formStr}");
            return formStr;
        }

        public HtmlImage(string props) {
            _props = toString(props);
        }
    }
}
