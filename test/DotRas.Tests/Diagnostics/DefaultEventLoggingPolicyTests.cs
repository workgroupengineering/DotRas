﻿using System;
using Moq;
using NUnit.Framework;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;

namespace DotRas.Tests.Diagnostics
{
    [TestFixture]
    public class DefaultEventLoggingPolicyTests
    {
        [Test]
        public void ThrowsAnExceptionWhenTheLogIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var unused = new DefaultEventLoggingPolicy(null);
            });
        }

        [Test]
        public void ThrowsAnExceptionWhenTheEventDataIsNull()
        {
            var target = new DefaultEventLoggingPolicy(new Mock<ILog>().Object);
            Assert.Throws<ArgumentNullException>(() => target.LogEvent(EventLevel.Error, null));
        }

        [Test]
        public void LogsTheEventInformation()
        {
            var log = new Mock<ILog>();

            var target = new DefaultEventLoggingPolicy(log.Object);
            target.LogEvent(EventLevel.Error, new PInvokeCallCompletedTraceEvent());

            log.Verify(o => o.Event(EventLevel.Error, It.IsAny<TraceEvent>()), Times.Once);
        }

        [Test]
        public void SwallowExceptionsWhenLoggingEvents()
        {
            var log = new Mock<ILog>();
            log.Setup(o => o.Event(EventLevel.Error, It.IsAny<TraceEvent>())).Throws<Exception>().Verifiable();

            var target = new DefaultEventLoggingPolicy(log.Object);
            target.LogEvent(EventLevel.Error, new PInvokeCallCompletedTraceEvent());

            log.Verify();
        }
    }
}