using SampleDeviceDriver.DeviceCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.DriverFramework.Data.Settings;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace SampleDeviceDriver
{
    /// <summary>
    /// Class for triggering outputs.
    /// TODO: Implement, or remove the class if not supported by the device.
    /// </summary>
    public class SampleDeviceDriverOutputManager : OutputManager
    {
        private readonly HashSet<TriggerTimerMap> _triggerTimers = new HashSet<TriggerTimerMap>();

        private new SampleDeviceDriverContainer Container => base.Container as SampleDeviceDriverContainer;

        public SampleDeviceDriverOutputManager(SampleDeviceDriverContainer container) : base(container)
        {
        }

        public override bool? IsActivated(string deviceId)
        {
            // TODO: If supported make request to device
            return null;
        }

        public override void TriggerOutput(string deviceId, int durationMs)
        {
            if (new Guid(deviceId) == Constants.Output1)
            {
                ActivateOutput(deviceId);

                Container.EventManager.NewEvent(deviceId, EventId.OutputActivated);

                TriggerTimerMap map = new TriggerTimerMap()
                {
                    DeviceId = deviceId
                };
                map.TriggerTimer = new Timer(TimerCallback, map, durationMs, Timeout.Infinite);
                _triggerTimers.Add(map);
                return;
            }
            throw new MIPDriverException("E3 - Device does not support Output commands");
        }

        public override void ActivateOutput(string deviceId)
        {
            Toolbox.Log.Trace("activating...");
            Toolbox.Log.LogDebug(nameof(ActivateOutput), $"deviceId: {deviceId}");
            if (new Guid(deviceId) == Constants.Output1)
            {
                string url = this.Container.ConnectionManager.Uri.AbsoluteUri; // ex http://localhost:5000
                //var settings = this.Container.ConfigurationManager.FetchHardwareDefinition().Devices.OfType<OutputDeviceDefinition>().First().Settings;
                //string switchId = settings[Constants.Switch1];
                //string state = settings[Constants.Switch1Activated];
                string switchId = this.Container.SettingsManager.GetSetting(new DeviceSetting(Constants.Switch1, deviceId, null)).Value;
                string state = this.Container.SettingsManager.GetSetting(new DeviceSetting(Constants.Switch1Activated, deviceId, null)).Value;
                // TODO: make request to device
                DeviceProxy proxy = new DeviceProxy();

                Console.WriteLine("sending Activate...");
                proxy.Send(url, switchId, state);
                Console.WriteLine("Activate sent.");

                Container.EventManager.NewEvent(deviceId, EventId.OutputActivated);
                return;
            }
            throw new MIPDriverException("E4 - Device does not support Output commands");
        }

        public override void DeactivateOutput(string deviceId)
        {
            if (new Guid(deviceId) == Constants.Output1)
            {
                // TODO: make request to device
                string url = this.Container.ConnectionManager.Uri.AbsoluteUri; // ex http://localhost:5000
                string switchId = this.Container.SettingsManager.GetSetting(new DeviceSetting(Constants.Switch1, deviceId, null)).Value;
       //         var settings = this.Container.ConfigurationManager.FetchHardwareDefinition().Devices.OfType<OutputDeviceDefinition>().First().Settings;
                string state = this.Container.SettingsManager.GetSetting(new DeviceSetting(Constants.Switch1Deactivated, deviceId, null)).Value;
       //         settings[Constants.Switch1Deactivated];
                DeviceProxy proxy = new DeviceProxy();

                Console.WriteLine("sending Dectivate...");
                proxy.Send(url, switchId, state);
                Console.WriteLine("Dectivate sent.");

                Container.EventManager.NewEvent(deviceId, EventId.OutputDeactivated);
                return;
            }
            throw new MIPDriverException("E5 - Device does not support Output commands");
        }

        private void TimerCallback(object state)
        {
            TriggerTimerMap map = state as TriggerTimerMap;
            if (map == null) throw new Exception("E2 - Map state unknown");

            DeactivateOutput(map.DeviceId);
            _triggerTimers.Remove(map);
            map.TriggerTimer.Dispose();
        }
    }

    internal class TriggerTimerMap
    {
        internal Timer TriggerTimer;
        internal string DeviceId;
    }
}
