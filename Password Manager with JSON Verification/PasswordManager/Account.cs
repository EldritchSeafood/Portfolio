using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using PasswordObj;

namespace Accounts {
    class Account {
        public string Description { get; set; }
        public string UserId { get; set; }
        public Password Password { get; set; }
        public string LoginUrl { get; set; }
        public string AccountNum { get; set; }
    }

    class AccountList {
        public List<Account> Accounts = new List<Account>();
        public void AddAccount(Account account) {
            Accounts.Add(account);
        }
    }
}
