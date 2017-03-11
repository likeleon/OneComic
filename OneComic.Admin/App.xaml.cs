using System;
using System.Windows;
using System.Windows.Threading;

namespace OneComic.Admin
{
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnAppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        }

        private void OnAppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
            e.Handled = true;
        }

        private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
