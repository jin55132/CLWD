using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using CLWD.ViewModel;
using Microsoft.Win32;
using CLWD.Model;

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

            

            // Create the ViewModel to which 
            // the main window binds.
            //string path = "Data/customers.xml";

            var loginViewModel = new LoginViewModel();
            var bookViewModel = new BookViewModel(loginViewModel);
            
            Book bookWindow = new Book();
            bookWindow.DataContext = bookViewModel;
            bookWindow.Show();


            if (!loginViewModel.Authorized)
            {
                Login loginWindow = new Login();
                loginWindow.DataContext = loginViewModel;
                loginWindow.Show();
            }
            
            

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
            // 
            // 






        }


    }



    

}
