namespace WealthLab.Extensions.Agent
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Ipc;
    using System.Windows.Forms;

    public class Remoting
    {
        private AppType appType_0;
        private IpcChannel ipcChannel_0;
        private RemotingUpdateAction remotingUpdateAction_0;

        public Remoting(AppType type)
        {
            this.appType_0 = type;
        }

        public string Initialize()
        {
            string message = null;
            try
            {
                switch (this.appType_0)
                {
                    case AppType.Server:
                        this.ipcChannel_0 = new IpcChannel("localhost:9090");
                        ChannelServices.RegisterChannel(this.ipcChannel_0, false);
                        RemotingConfiguration.RegisterWellKnownServiceType(typeof(RemotingUpdateAction), "UpdateAction.rem", WellKnownObjectMode.Singleton);
                        return message;

                    case AppType.Client:
                    {
                        this.ipcChannel_0 = new IpcChannel();
                        ChannelServices.RegisterChannel(this.ipcChannel_0, false);
                        object obj2 = Activator.GetObject(typeof(RemotingUpdateAction), "ipc://localhost:9090/UpdateAction.rem");
                        this.remotingUpdateAction_0 = (RemotingUpdateAction) obj2;
                        return message;
                    }
                }
                return message;
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public string StartServer()
        {
            string message = null;
            try
            {
                string str2 = Path.Combine(Application.StartupPath, "WealthLab.Extensions.Agent.exe");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    startInfo.Verb = "runas";
                }
                startInfo.FileName = str2;
                startInfo.Arguments = "/rem";
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
            return message;
        }

        public void Unregister()
        {
            if (this.ipcChannel_0 != null)
            {
                ChannelServices.UnregisterChannel(this.ipcChannel_0);
            }
        }

        public RemotingUpdateAction UpdateAction
        {
            get
            {
                return this.remotingUpdateAction_0;
            }
        }
    }
}

