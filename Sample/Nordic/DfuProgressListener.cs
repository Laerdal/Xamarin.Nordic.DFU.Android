using System;
using Android.Util;
using NO.Nordicsemi.Android.Dfu;

namespace Sample.Nordic
{
    public class DfuProgressListener : DfuProgressListenerAdapter
    {
        public event EventHandler<string> StatusChanged;
        public event EventHandler<int> ProgressChanged;
        public event EventHandler<string> Failed;
        public event EventHandler Finished;

        public DfuProgressListener()
        {
        }

        public override void OnDeviceConnected(string deviceAddress)
        {
            base.OnDeviceConnected(deviceAddress);
            StatusUpdated("Connected");
        }

        private void StatusUpdated(string status)
        {
            StatusChanged?.Invoke(this, status);
            Log.Info("DFU", status);
        }

        public override void OnDeviceDisconnected(string deviceAddress)
        {
            base.OnDeviceDisconnected(deviceAddress);
            StatusUpdated("Disconnected");
        }

        public override void OnDfuProcessStarted(string deviceAddress)
        {
            base.OnDfuProcessStarted(deviceAddress);          
            StatusUpdated("Started");
        }

        public override void OnEnablingDfuMode(string deviceAddress)
        {
            base.OnEnablingDfuMode(deviceAddress);
            StatusUpdated("Enabling dfu mode");
        }

        public override void OnFirmwareValidating(string deviceAddress)
        {
            base.OnFirmwareValidating(deviceAddress);
            StatusUpdated("Validating firmware");
        }

        public override void OnDeviceDisconnecting(string deviceAddress)
        {
            base.OnDeviceDisconnecting(deviceAddress);
            StatusUpdated("Device disconnecting");
        }
        public override void OnDfuCompleted(string deviceAddress)
        {
            base.OnDfuCompleted(deviceAddress);
            Log.Info("DFU", "Dfu completed");
            Finished?.Invoke(this, null);
        }
        public override void OnDfuAborted(string deviceAddress)
        {
            base.OnDfuAborted(deviceAddress);
            Log.Info("DFU", "Dfu aborted");
            Failed?.Invoke(this, "DFU process was aborted");
        }

        public override void OnDeviceConnecting(string deviceAddress)
        {
            base.OnDeviceConnecting(deviceAddress);
            StatusUpdated("Device connecting");
        }
        public override void OnDfuProcessStarting(string deviceAddress)
        {
            base.OnDfuProcessStarting(deviceAddress);
            StatusUpdated("Dfu process starting");
        }

        public override void OnProgressChanged(string deviceAddress, int percent,
                            float speed,  float avgSpeed,
                            int currentPart,  int partsTotal)
        {
            base.OnProgressChanged(deviceAddress, percent, speed, avgSpeed, currentPart, partsTotal);
            Log.Info("DFUProgress", $"Progress: {percent} % ({currentPart}/{partsTotal})");
            ProgressChanged?.Invoke(this, percent);
        }

        public override void OnError(string deviceAddress,
                  int error,  int errorType,  string message)
        {
            base.OnError(deviceAddress, error, errorType, message);
            Log.Info("DFU", $"Dfu failcode {error}, type {errorType}, msg: {message}");
            Failed?.Invoke(this, message);
        }
    }
}
