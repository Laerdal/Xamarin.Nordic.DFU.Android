using System;
using Android.App;
using Android.Util;
using Java.Lang;
using NO.Nordicsemi.Android.Dfu;
using Sample.Nordic;

namespace Sample
{
    public class FirmwareUpdater : IDisposable
    {
        private readonly DfuServiceInitiator _dfuServiceInitiator;
        private readonly DfuProgressListener _progressListener;

        public FirmwareUpdater(string deviceAddress, Android.Net.Uri firmwareZipFile)
        {
            _progressListener = new DfuProgressListener();
            _dfuServiceInitiator.SetPacketsReceiptNotificationsEnabled(true);
            _dfuServiceInitiator.SetUnsafeExperimentalButtonlessServiceInSecureDfuEnabled(true);
            DfuServiceListenerHelper.RegisterProgressListener(Application.Context, _progressListener);
            _dfuServiceInitiator = new DfuServiceInitiator(deviceAddress);
            _dfuServiceInitiator.SetZip(firmwareZipFile);
        }

        public void Start()
        {
            _dfuServiceInitiator.Start(Application.Context, Class.FromType(typeof(DfuService)));
        }

        public void Dispose()
        {
            DfuServiceListenerHelper.UnregisterProgressListener(Application.Context, _progressListener);
        }
    }

    class DfuProgressListener : DfuProgressListenerAdapter
    {
        public override void OnDfuAborted(string deviceAddress)
        {
            base.OnDfuAborted(deviceAddress);
        }
        public override void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal)
        {
            Log.Info(GetType().Name, $"Firmware update in progress: {percent} %");
            base.OnProgressChanged(deviceAddress, percent, speed, avgSpeed, currentPart, partsTotal);
        }
    }
}
