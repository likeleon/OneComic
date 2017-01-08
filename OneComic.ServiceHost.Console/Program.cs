using Core.Common.Core;
using OneComic.Business.Bootstrapper;
using OneComic.Business.Managers.Managers;
using OneComic.Common;
using System;
using System.Security.Principal;
using System.Threading;
using SM = System.ServiceModel;

namespace OneComic.ServiceHost
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var principalIdentity = new GenericIdentity("likeleon");
            var principal = new GenericPrincipal(principalIdentity, new[] { Security.OneComicAdminRole });
            Thread.CurrentPrincipal = principal;

            Global.Container = MefLoader.Init();

            Console.WriteLine("Starting up services...");
            Console.WriteLine("");

            var hostLibraryManager = new SM.ServiceHost(typeof(LibraryManager));
            var hostBookmarkManager = new SM.ServiceHost(typeof(BookmarkManager));
            var hostAccountManager = new SM.ServiceHost(typeof(AccountManager));

            StartService(hostLibraryManager, "LibraryManager");
            StartService(hostBookmarkManager, "BookmarkManager");
            StartService(hostAccountManager, "AccountManager");

            Console.WriteLine("");
            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();

            StopService(hostLibraryManager, "LibraryManager");
            StopService(hostBookmarkManager, "BookmarkManager");
            StopService(hostAccountManager, "AccountManager");
        }

        private static void StartService(SM.ServiceHost host, string serviceDescription)
        {
            host.Open();
            Console.WriteLine($"Service {serviceDescription} started.");

            foreach (var endpoint in host.Description.Endpoints)
            {
                Console.WriteLine("Listening on endpoint:");
                Console.WriteLine($"Address: {endpoint.Address.Uri.ToString()}");
                Console.WriteLine($"Binding: {endpoint.Binding.Name}");
                Console.WriteLine($"Contract: {endpoint.Contract.ConfigurationName}");
            }

            Console.WriteLine();
        }

        private static void StopService(SM.ServiceHost host, string serviceDescription)
        {
            host.Close();
            Console.WriteLine($"Service {serviceDescription} stopped.");
        }
    }
}
