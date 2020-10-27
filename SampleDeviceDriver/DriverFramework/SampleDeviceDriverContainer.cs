using VideoOS.Platform.DriverFramework;

namespace SampleDeviceDriver
{
    /// <summary>
    /// Container holding all the different managers.
    /// TODO: If your hardware does not support some of the functionality, you can remove the class and the instantiation below.
    /// </summary>
    public class SampleDeviceDriverContainer : Container
    {
        public new SampleDeviceDriverConnectionManager ConnectionManager => base.ConnectionManager as SampleDeviceDriverConnectionManager;
        public new SampleDeviceDriverStreamManager StreamManager => base.StreamManager as SampleDeviceDriverStreamManager;

        public SampleDeviceDriverContainer(DriverDefinition definition)
            : base(definition)
        {
            base.StreamManager = new SampleDeviceDriverStreamManager(this);
            base.PtzManager = new SampleDeviceDriverPtzManager(this);
            base.OutputManager = new SampleDeviceDriverOutputManager(this);
            base.SpeakerManager = new SampleDeviceDriverSpeakerManager(this);
            base.PlaybackManager = new SampleDeviceDriverPlaybackManager(this);
            base.ConnectionManager = new SampleDeviceDriverConnectionManager(this);
            base.ConfigurationManager = new SampleDeviceDriverConfigurationManager(this);
        }
    }
}
