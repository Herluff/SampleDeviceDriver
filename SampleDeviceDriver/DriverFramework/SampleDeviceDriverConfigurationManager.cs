using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.DriverFramework.Definitions;
using VideoOS.Platform.DriverFramework.Exceptions;
using VideoOS.Platform.DriverFramework.Managers;
using VideoOS.Platform.DriverFramework.Utilities;

namespace SampleDeviceDriver
{
    /// <summary>
    /// This class returns information about the hardware including capabilities and settings supported.
    /// TODO: Update it to match what is supported by your hardware.
    /// </summary>
    public class SampleDeviceDriverConfigurationManager : ConfigurationManager
    {
        private const string _firmware = "SampleDeviceDriver Firmware";
        private const string _firmwareVersion = "1.0";
        private const string _hardwareName = "SampleDeviceDriver Hardware";
        private const string _serialNumber = "12345";
//        private readonly string macAddress = "AA:BB:CC:DD:EE:86"; // + new Random(DateTime.Now.Millisecond).Next(10, 99).ToString(); // TODO: fake mac

        private new SampleDeviceDriverContainer Container => base.Container as SampleDeviceDriverContainer;

        public SampleDeviceDriverConfigurationManager(SampleDeviceDriverContainer container) : base(container)
        {
        }

        protected override ProductInformation FetchProductInformation()
        {
            if (!Container.ConnectionManager.IsConnected)
            {
                throw new ConnectionLostException("E1 - Connection not established");
            }

            var driverInfo = Container.Definition.DriverInfo;
            var product = driverInfo.SupportedProducts.FirstOrDefault();
            // Fake MAC from VirtualDevice project (unique pr. host:port)
            var macAddress = Container.ConnectionManager.FakeMacAddress;
            Toolbox.Log.LogError(nameof(ProductInformation), $"Just INFO! Mac: {macAddress}");
            return new ProductInformation
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductVersion = driverInfo.Version,
                MacAddress = macAddress,
                FirmwareVersion = _firmwareVersion,
                Firmware = _firmware,
                HardwareName = _hardwareName,
                SerialNumber = _serialNumber
            };
        }

        protected override IDictionary<string, string> BuildHardwareSettings()
        {
            return new Dictionary<string, string>()
            {
                // TODO: Add settings supported by the hardware
            };
        }

        protected override ICollection<ISetupField> BuildFields()
        {
            var fields = new List<ISetupField>();

            // TODO: Add definition of setup fields supported by hardware and devices

            fields.Add(new StringSetupField()
            {
                Key = Constants.Switch1,
                DisplayName = "Switch id (part of the uri)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.Switch1RefId,
                DefaultValue = "DoorA",
            });
            fields.Add(new StringSetupField()
            {
                Key = Constants.Switch1Activated,
                DisplayName = "Activation value (fx. on)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.Switch1ActivatedRefId,
                DefaultValue = "on",
            }); fields.Add(new StringSetupField()
            {
                Key = Constants.Switch1Deactivated,
                DisplayName = "Dectivation value (fx. off)",
                DisplayNameReferenceId = Guid.Empty,
                IsReadOnly = false,
                ReferenceId = Constants.Switch1DeactivatedRefId,
                DefaultValue = "off",
            });
            return fields;
        }

        protected override ICollection<EventDefinition> BuildHardwareEvents()
        {
            var hardwareEvents = new List<EventDefinition>();

            // TODO: Add supported hardware level events

            return hardwareEvents;
        }

        protected override ICollection<DeviceDefinitionBase> BuildDevices()
        {
            var devices = new List<DeviceDefinitionBase>();

            // TODO: If supported by the hardware, add more camera devices (same for below device types). Also remove the devices not supported.

            devices.Add(new OutputDeviceDefinition()
            {
                DisplayName = "Switching device output",
                DeviceId = Constants.Output1.ToString(),
                SupportSetState = true,
                SupportTrigger = true,
                Settings = new Dictionary<string, string>()
                {
                    { Constants.Switch1, "Door2" },
                    { Constants.Switch1Activated, "open" },
                    { Constants.Switch1Deactivated, "close" }
                }
            });

            return devices;
        }

        private static ICollection<EventDefinition> BuildDeviceEvents()
        {
            var deviceEvents = new List<EventDefinition>();

            // TODO: Add events supported by device.
            return deviceEvents;
        }
    }
}
