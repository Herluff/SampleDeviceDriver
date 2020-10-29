using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace SampleDeviceDriver
{
    /// <summary>
    /// Class handling connection to one hardware
    /// TODO: Add methods for making the needed requests to your hardware type, and use these from the other classes
    /// </summary>
    public class SampleDeviceDriverConnectionManager : ConnectionManager
    {
        private Uri _uri;
        private string _userName;
        private SecureString _password;
        private bool _connected = false;
        public string FakeMacAddress { get; private set; }
        public Uri Uri { get { return _uri; } }

        public SampleDeviceDriverConnectionManager(SampleDeviceDriverContainer container) : base(container)
        {
        }

        /// <summary>
        /// Implementation of the DFW platform method.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="hardwareSettings"></param>
        public override void Connect(Uri uri, string userName, SecureString password, ICollection<HardwareSetting> hardwareSettings)
        {
            if (_connected)
            {
                return;
            }
            _uri = uri;
            _userName = userName;
            _password = password;
            FakeMacAddress = MakeFakeMacAddressFromUri(uri);

            // TODO: Establish connection

            // TODO DEMO "connecting w/sucess..!
            _connected = true;

            // polling for events from the device. Might not be needed if the event mechanism of your hardware is not poll based
            //_inputPoller = new InputPoller(Container.EventManager, this);
            //_inputPoller.Start();
        }

        /// <summary>
        /// Implementation of the DFW platform method.
        /// </summary>
        public override void Close()
        {
            // TODO: Disconnect/close connection
            _connected = false;
        }

        /// <summary>
        /// Implementation of the DFW platform property.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                return _connected;
            }
        }
        private static string MakeFakeMacAddressFromUri(Uri uri)
        {
            Toolbox.Log.Trace("MakeFakeMacAddressFromUri");

            const string defaultValue = "00";
            string hostName = uri.IsLoopback ? "127.0.0.1" : uri.DnsSafeHost;

            Toolbox.Log.LogDebug(nameof(MakeFakeMacAddressFromUri), "host name " + hostName);

            var ipSegments = hostName.Split('.');
            var macAddress = new string[6];
            for (int i = 0; i < 4; i++)
            {
                if (i > ipSegments.Length - 1)
                    macAddress[i] = defaultValue;

                if (!int.TryParse(ipSegments[i], out int value))
                    macAddress[i] = defaultValue; // Fallback value

                macAddress[i] = (value % (byte.MaxValue + 1)).ToString("X2");
            }

            var portHexString = (uri.Port % (ushort.MaxValue + 1)).ToString("X2");

            macAddress[4] = portHexString.Substring(0, 2);

            if (portHexString.Length > 2)
                macAddress[5] = portHexString.Substring(2, 2);
            else
                macAddress[5] = defaultValue;

            return string.Join("-", macAddress);
        }
    }
}
