﻿using System;

namespace IVPN.Models
{
    public class ConnectionInfo : ModelBase
    {
        private string __DurationString;
        private bool __IsDurationStopped;
        public ConnectionInfo(ServerLocation server, DateTime connectTime, string clientIPAddress, string serverIPAddress, string vpnProtocolInfo)
        {
            Server = server;
            ConnectTime = connectTime;
            ClientIPAddress = clientIPAddress;
            ServerIPAddress = serverIPAddress;
            VpnProtocolInfo = vpnProtocolInfo;
        }

        public void UpdateDuration()
        {
            if (__IsDurationStopped)
            {
                DurationString = "";
                return;
            }

            TimeSpan ts = DateTime.UtcNow.Subtract(ConnectTime);
            if (ts.Days == 0)
                DurationString = $"{ts.Hours + (ts.Days * 24):D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            else if (ts.Days == 1)
                DurationString = $"{ts.Days} day {ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            else
                DurationString = $"{ts.Days} days {ts.Hours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }

        /// <summary>
        /// Useful when pausing connection
        /// </summary>
        public void DurationStop()
        {
            __IsDurationStopped = true;
            UpdateDuration();
        }

        /// <summary>
        /// Useful when resuming connection
        /// </summary>
        public void DurationStart()
        {
            __IsDurationStopped = false;

            DoPropertyWillChange(nameof(ConnectTime));
            ConnectTime = DateTime.UtcNow;
            DoPropertyChanged(nameof(ConnectTime));

            UpdateDuration();
        }

        public string DurationString
        {
            get => __DurationString;

            private set
            {
                DoPropertyWillChange();
                __DurationString = value;
                DoPropertyChanged();
            }
        }

        public DateTime ConnectTime { get; private set; }

        public string ClientIPAddress { get; }

        public string ServerIPAddress { get; }

        public ServerLocation Server { get; }

        public string VpnProtocolInfo { get; }
    }
}
