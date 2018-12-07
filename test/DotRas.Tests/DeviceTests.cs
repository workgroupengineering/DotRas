﻿using NUnit.Framework;
using DotRas.Tests.Stubs;

namespace DotRas.Tests
{
    [TestFixture]
    public class DeviceTests
    {
        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsNull()
        {
            var target = new TestDevice(null);
            Assert.AreEqual(null, target.Name);
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsWhitespace()
        {
            var target = new TestDevice("            ");
            Assert.AreEqual("            ", target.Name);
        }

        [Test]
        public void ShouldNotThrowAnExceptionWhenTheDeviceNameIsEmpty()
        {
            var target = new TestDevice(string.Empty);
            Assert.AreEqual(string.Empty, target.Name);
        }
    }
}