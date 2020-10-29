using System;
using VideoOS.Platform.DriverFramework.Definitions;

namespace SampleDeviceDriver
{
    public static class Constants
    {
        public static readonly ProductDefinition Product1 = new ProductDefinition
        {
            Id = new Guid("6ea27d79-b57d-498a-977f-d99bb3d8d639"),
            Name = "My SampleDeviceDriver"
        };

        public static readonly Guid DriverId = new Guid("dd2bc6ee-644a-465a-9586-e7bad47333e8");

//        public static readonly Guid Video1 = new Guid("6f4f9cd3-05f3-4ae8-bb1e-b4977f2a449e");
//        public static readonly Guid Input1 = new Guid("1671678b-e8d2-4fe7-a2da-5059967fa2c9");
//        public static readonly Guid Metadata1 = new Guid("f877f660-4f40-4ffc-955c-5e15042e3784");
        public static readonly Guid Output1 = new Guid("79d8d6a8-0cbc-4d82-853b-ba696d23d94a");
        public static readonly string Switch1 = nameof(Switch1);
        public static readonly string Switch1Activated = nameof(Switch1Activated);
        public static readonly string Switch1Deactivated = nameof(Switch1Deactivated);
        public static readonly Guid Switch1RefId = new Guid("2C8B94C1-B64B-44AE-A4A3-544E8D5AAD6E");
        public static readonly Guid Switch1ActivatedRefId = new Guid("8495B8A4-2189-4B9B-89EE-76A9813EC142");
        public static readonly Guid Switch1DeactivatedRefId = new Guid("CEBBC56C-1955-44FB-A996-592A56E82EDE");
    }
}
