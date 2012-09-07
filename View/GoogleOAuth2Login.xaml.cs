using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CLWD.ViewModel;

namespace CLWD.View
{
    /// <summary>
    /// GoogleOAuth2Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GoogleOAuth2Login : Window
    {
        public GoogleOAuth2Login()
        {
            InitializeComponent();
        }

        private void webBrowser1_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var doc = (mshtml.IHTMLDocument2)webBrowser1.Document;

            if (doc != null)
            {
                string title = string.Copy(doc.title);

                if (title.StartsWith("Success code="))
                {
                  

                    ((GoogleOAuth2LoginViewModel)this.DataContext).AccessCode = title.Remove(0, 13);
                    ((GoogleOAuth2LoginViewModel)this.DataContext).OnRequestClose();
                    //((GoogleOAuth2LoginViewModel)this.DataContext).Authorized = true;
                    //((GoogleOAuth2LoginViewModel)this.DataContext).Authorized = true;
                   
                }
            }

 
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ((GoogleOAuth2LoginViewModel)this.DataContext).WindowAlive = false ;
        }

     
    }
}
