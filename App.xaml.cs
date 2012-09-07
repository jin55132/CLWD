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
       // private GoogleOAuth2LoginViewModel googleOAuth2LoginViewModel = null;
        private Book bookWindow = null;
        private BookViewModel bookViewModel = null;
        
        

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ViewModels
            bookViewModel = new BookViewModel();

            // Windows
            bookWindow = new Book();
            bookWindow.DataContext = bookViewModel;
          
            
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
            

            
            #endregion

            // is it ok to be here..??
            bookViewModel.GoogleOAuth2LoginViewModel.RequestClose += (s, eve) => googleOAuth2Window.Close();
            bookViewModel.GoogleOAuth2LoginViewModel.RequestHide += (s, eve) => googleOAuth2Window.Hide();
            bookViewModel.GoogleOAuth2LoginViewModel.RequestShow += (s, eve) => googleOAuth2Window.Show();
            // googleOAuth2LoginViewModel.RequestOpen += (s, eve) => googleOAuth2Window.Show();

            bookWindow.Show();
            bookViewModel.GoogleOAuth2LoginViewModel.initialize();

            

            //googleOAuth2LoginViewModel.Login();
            
        }


        public void CreateLoginWindow(GoogleOAuth2LoginViewModel vm)
        {
            googleOAuth2Window = new GoogleOAuth2Login();
            googleOAuth2Window.DataContext = vm;
         
        }

    }



    

}
