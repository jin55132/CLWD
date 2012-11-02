using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace CLWD.ViewModel
{
 
    public abstract class BaseViewModel : ObservableObject, IDisposable
    {
        #region Fields

        RelayCommand _closeCommand;
        RelayCommand _showCommand;
        RelayCommand _hideCommand;
        private bool _windowAlive;
        #endregion // Fields


        #region Constructor

        protected BaseViewModel()
        {
            _windowAlive = false;

        }

        #endregion // Constructor

        #region DisplayName

        /// <summary>
        /// Returns the user-friendly name of this object.
        /// Child classes can set this property to a new value,
        /// or override it to determine the value on-demand.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        #endregion // DisplayName

        #region CloseCommand

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand( () => this.OnRequestClose());

                return _closeCommand;
            }
        }

        public ICommand ShowCommand
        {
            get
            {
                if (_showCommand == null)
                    _showCommand = new RelayCommand(() => this.OnRequestShow());

                return _showCommand;
            }
        }

        public ICommand HideCommand
        {
            get
            {
                if (_hideCommand == null)
                    _hideCommand = new RelayCommand(() => this.OnRequestHide());

                return _showCommand;
            }
        }

        public bool WindowAlive
        {
            get
            {
                return _windowAlive;
            }
            set
            {
                _windowAlive = value;
                RaisePropertyChanged("WindowAlive");
            }
        }
        #endregion // ShowCommand


        #region Request [event]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        public void OnRequestClose()
        {
            WindowAlive = false;
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler RequestShow;

        public void OnRequestShow()
        {
            WindowAlive = true;
            EventHandler handler = this.RequestShow;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        public event EventHandler RequestHide;

        public void OnRequestHide()
        {
            EventHandler handler = this.RequestHide;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion // RequestClose [event]
        #region IDisposable Members

        /// <summary>
        /// Invoked when this object is being removed from the application
        /// and will be subject to garbage collection.
        /// </summary>
        public void Dispose()
        {
            this.OnDispose();
        }

        /// <summary>
        /// Child classes can override this method to perform 
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected virtual void OnDispose()
        {
        }

#if DEBUG
        /// <summary>
        /// Useful for ensuring that ViewModel objects are properly garbage collected.
        /// </summary>
        ~BaseViewModel()
        {
            string msg = string.Format("{0} ({1}) ({2}) Finalized", this.GetType().Name, this.DisplayName, this.GetHashCode());
            System.Diagnostics.Debug.WriteLine(msg);
        }
#endif

        #endregion // IDisposable Members



    }

}
