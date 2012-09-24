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

namespace CLWD
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Sheet : Window
    {
        public Sheet()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((SpreadSheetViewModel)this.DataContext).Closing();
        }
    }
}
