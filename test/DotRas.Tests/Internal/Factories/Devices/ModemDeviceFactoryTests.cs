﻿using NUnit.Framework;
using DotRas.Devices;
using DotRas.Internal.Factories.Devices;

namespace DotRas.Tests.Internal.Factories.Devices
{
    [TestFixture]
    public class ModemDeviceFactoryTests
    {
        [Test]
        public void ReturnADeviceInstance()
        {
            var target = new ModemDeviceFactory();
            var result = target.Create("Test");

            Assert.AreEqual("Test", result.Name);
            Assert.IsAssignableFrom<Modem>(result);
        }
    }
}