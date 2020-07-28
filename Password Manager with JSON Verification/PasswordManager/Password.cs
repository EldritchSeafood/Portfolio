using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordObj {
    class Password {
        public string Value { get; set; }
        public int StrengthNum { get; set; }
        public string StrengthText { get; set; }
        public string LastReset { get; set; }
    }
}
