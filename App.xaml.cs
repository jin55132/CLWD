using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using CLWD.ViewModel;
using Microsoft.Win32;
using CLWD.Model;
using CLWD.View;

namespace CLWD
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {

        private GoogleOAuth2Login googleOAuth2Window = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create the ViewModel to which 
            // the main window binds.
            //string path = "Data/customers.xml";

            //var googleIDloginViewModel = new GoogleIDLoginViewModel();
            var googleOAuth2LoginViewModel = new GoogleOAuth2LoginViewModel();
            var bookViewModel = new BookViewModel(googleOAuth2LoginViewModel);

            Book bookWindow = new Book();
            googleOAuth2Window = new GoogleOAuth2Login();
           //googleOAuth2Window.Owner = bookWindow;
            //GoogleIDLogin loginWindow = new GoogleIDLogin();


            bookWindow.DataContext = bookViewModel;
            googleOAuth2Window.DataContext = googleOAuth2LoginViewModel;

            

            #region close command handler
            //EventHandler handler = null;
            //handler = delegate
            //{
            //    loginViewModel.RequestClose -= handler;
            //    loginWindow.Close();
            //};


            //loginViewModel.RequestClose += handler;
            //// loginViewModel.RequestClose += (s, eve) => loginWindow.Close();


            //EventHandler handler = null;
            //handler = delegate
            //{   //googleOAuth2LoginViewModel.RequestClose -= handler;
            //    googleOAuth2Window.Close();
            //};
            //googleOAuth2LoginViewModel.RequestClose += handler;

            //bookViewModel.RequestClose += (s, eve) => bookWindow.Hide();
            
            googleOAuth2LoginViewModel.RequestClose += (s, eve) => googleOAuth2Window.Close();
            googleOAuth2LoginViewModel.RequestOpen += (s, eve) => googleOAuth2Window.Show();
            
            #endregion


            bookWindow.Show();
            

            if (!googleOAuth2LoginViewModel.Authorized)
            {
                googleOAuth2Window.Show();

            }

            googleOAuth2LoginViewModel.Login();
            
        }




    }



    

}
