using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace CLWD.DP
{
    public static class AttachedProperties
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(AttachedProperties), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser browser = o as WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                browser.Source = uri != null ? new Uri(uri) : null;
            }
        }


        public static readonly DependencyProperty ShowInLoginProperty =
          DependencyProperty.RegisterAttached("ShowInLogin", typeof(bool), typeof(AttachedProperties), new UIPropertyMetadata(false, ShowInLoginPropertyChanged));

        public static bool GetShowInLogin(DependencyObject obj)
        {
            return (bool)obj.GetValue(ShowInLoginProperty);
        }

        public static void SetShowInLogin(DependencyObject obj, bool value)
        {
            obj.SetValue(ShowInLoginProperty, value);
        }

        public static void ShowInLoginPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {

            if (o is WebBrowser)
            {
                WebBrowser obj = o as WebBrowser;
                bool isShowUI = (bool)e.NewValue;
                if (isShowUI)
                {
                    obj.Visibility = Visibility.Collapsed;
                }
                else
                {
                    obj.Visibility = Visibility.Visible;
                }


            }
            //else if (o is Button)
            //{

            //    Button obj = o as Button;
            //    bool isShowUI = (bool)e.NewValue;

            //    if (isShowUI)
            //    {
            //        obj.Visibility = Visibility.Visible;
            //    }
            //    else
            //    {
            //        obj.Visibility = Visibility.Collapsed;
            //    }
            //}


        }

    }

    //public class Attached
    //{
    //    public static DependencyProperty TestProperty =
    //        DependencyProperty.RegisterAttached("TestProperty", typeof(bool), typeof(Attached),
    //        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Inherits));

    //    public static bool GetTest(DependencyObject obj)
    //    {
    //        return (bool)obj.GetValue(TestProperty);
    //    }

    //    public static void SetTest(DependencyObject obj, bool value)
    //    {
    //        obj.SetValue(TestProperty, value);
    //    }
    //}


}
