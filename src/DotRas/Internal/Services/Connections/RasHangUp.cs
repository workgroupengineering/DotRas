﻿using System;
using System.Threading;
using DotRas.Internal.Abstractions.Policies;
using DotRas.Internal.Abstractions.Services;
using DotRas.Win32;
using DotRas.Win32.SafeHandles;
using static DotRas.Win32.RasError;
using static DotRas.Win32.WinError;

namespace DotRas.Internal.Services.Connections
{
    internal class RasHangUp : IRasHangUp
    {
        private readonly IRasApi32 api;
        private readonly IExceptionPolicy exceptionPolicy;

        public RasHangUp(IRasApi32 api, IExceptionPolicy exceptionPolicy)
        {
            this.api = api ?? throw new ArgumentNullException(nameof(api));
            this.exceptionPolicy = exceptionPolicy ?? throw new ArgumentNullException(nameof(exceptionPolicy));
        }

        public void HangUp(RasHandle handle, CancellationToken cancellationToken)
        {
            if (handle == null)
            {
                throw new ArgumentNullException(nameof(handle));
            }
            else if (handle.IsClosed || handle.IsInvalid)
            {
                throw new ArgumentException("The handle is invalid.", nameof(handle));
            }

            CloseAllConnectionsToTheHandle(handle, cancellationToken);
            EnsurePortHasBeenReleased();

            handle.SetHandleAsInvalid();
        }

        private void CloseAllConnectionsToTheHandle(RasHandle handle, CancellationToken cancellationToken)
        {
            int ret;

            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                ret = api.RasHangUp(handle);
                if (ShouldThrowExceptionFromReturnCode(ret))
                {
                    throw exceptionPolicy.Create(ret);
                }
            } while (ret == SUCCESS);
        }

        private bool ShouldThrowExceptionFromReturnCode(int ret)
        {
            return ret != SUCCESS && ret != ERROR_NO_CONNECTION;
        }

        private void EnsurePortHasBeenReleased()
        {
            // ATTENTION! This required pause comes from the Windows SDK. Failure to perform this pause may cause the state machine to leave 
            // the port open which will require the machine to be rebooted to release the port.
            Thread.Sleep(1000);
        }
    }
}