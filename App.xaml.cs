using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using CLWD.ViewModel;

namespace CLWD
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Book window = new Book();

            // Create the ViewModel to which 
            // the main window binds.
            //string path = "Data/customers.xml";

            var viewModel = new BookViewModel();

            // When the ViewModel asks to be closed, 
            // close the window.
            //EventHandler handler = null;
            //handler = delegate
            //{
            //    viewModel.RequestClose -= handler;
            //    window.Close();
            //};
            //viewModel.RequestClose += handler;

            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            window.DataContext = viewModel;
            string xmlDataDirectory = ConfigurationSettings.AppSettings.Get("googleKey");


            window.Show();




        }
    }



    

}
