using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLWD.Model
{
    class GoogleIDAccount : ObservableObject
    {
        #region Members
        string _id;
        string _password;

        #endregion

        public GoogleIDAccount()
        {
            ID = "";
            Password = "";
        }


        #region Properties
        public string ID
        {
            get { return _id; }
            set
            {
                _id = value;
                RaisePropertyChanged("ID");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }
        


        #endregion
    }

}
