using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLWD.Model
{
    class Voca : ObservableObject
    {
        #region Members
        string _word;
        string _definition;
        DateTime _date;
        int _key;
        bool _checked;
        #endregion


        #region Properties
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                //RaisePropertyChanged("Word");
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }
        public string Definition
        {
            get { return _definition; }
            set
            {
                _definition = value;
                //RaisePropertyChanged("Definition");
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                //RaisePropertyChanged("Date");
            }
        }

        public int Key
        {
            get { return _key; }
            set
            {
                _key = value;
                //RaisePropertyChanged("Date");
            }
        }
        #endregion
    }
}
