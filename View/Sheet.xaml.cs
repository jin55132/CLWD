using System;
using System.Windows;
using System.Windows.Controls;
using CLWD.ViewModel;
using System.Windows.Controls.Primitives;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;

namespace CLWD
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Sheet : Window
    {

        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private WindowState m_storedWindowState = WindowState.Normal;
        static bool bFirstMinimize = false;
        public Sheet()
        {
            InitializeComponent();

            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "The Google Words stays in the tray bar. Click here either to open or close.";
            m_notifyIcon.BalloonTipTitle = "The Google Words";
            m_notifyIcon.Text = "The Google Words";
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/Google.ico")).Stream;
            m_notifyIcon.Icon = new System.Drawing.Icon(iconStream);
          

            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);


            m_notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] 
            { 
  
                new System.Windows.Forms.MenuItem("Close", (s, e) => this.Close()) 
            });

        }

        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ((SpreadSheetViewModel)this.DataContext).Closing();
            m_notifyIcon.Dispose();
            m_notifyIcon = null;

            
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            if (selector is DataGrid)
            {
                if (selector.SelectedItem != null)
                    (selector as DataGrid).ScrollIntoView(selector.SelectedItem);
            }

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();

                if (m_notifyIcon != null && bFirstMinimize == false)
                    m_notifyIcon.ShowBalloonTip(500);
                bFirstMinimize = true;
                
            }
            else
                m_storedWindowState = WindowState;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ShowTrayIcon(!IsVisible);
        }


        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }
    }
}
