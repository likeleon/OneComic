using Core.Common.Core;
using Microsoft.WindowsAzure.ServiceRuntime;
using OneComic.Business.Bootstrapper;
using OneComic.Business.Contracts;
using OneComic.Business.Managers.Managers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using SM = System.ServiceModel;

namespace OneComic.ServiceHost.Azure.WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private const string EndpointName = "WCFEndpoint";
        private const string WorkerName = "OneComic.ServiceHost.Azure.WorkerRole";

        private readonly List<SM.ServiceHost> _hosts = new List<SM.ServiceHost>();

        public override void Run()
        {
            Trace.TraceInformation($"{WorkerName} is running");

            Global.Container = MefLoader.Init();
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            Trace.TraceInformation($"{WorkerName} is starting");

            _hosts.Add(CreateServiceHost<LibraryManager, ILibraryService>("LibraryService"));
            _hosts.Add(CreateServiceHost<BookmarkManager, IBookmarkService>("BookmarkService"));
            _hosts.Add(CreateServiceHost<AccountManager, IAccountService>("AccountService"));

            _hosts.ForEach(host => host.Open());

            Trace.TraceInformation($"{WorkerName} has been started");

            return true;
        }

        private static SM.ServiceHost CreateServiceHost<T, IContract>(string serviceName)
            where T : IContract
        {
            var host = new SM.ServiceHost(typeof(T));

            var binding = new NetTcpBinding(SecurityMode.None);

            var roleEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[EndpointName];
            var address = $"{binding.Scheme}://{roleEndpoint.IPEndpoint}/{serviceName}";

            host.AddServiceEndpoint(typeof(IContract), binding, address);
            
            return host;
        }

        public override void OnStop()
        {
            Trace.TraceInformation($"{WorkerName} is stopping");

            _hosts.ForEach(host => host.Close());

            base.OnStop();

            Trace.TraceInformation($"{WorkerName} has stopped");
        }
    }
}
