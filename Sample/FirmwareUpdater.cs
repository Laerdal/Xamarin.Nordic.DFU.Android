using System;
using Android.App;
using Android.Util;
using Java.Interop;
using Java.Lang;
using NO.Nordicsemi.Android.Dfu;

namespace Sample
{
    public class FirmwareUpdater : IDisposable
    {
        private readonly DfuServiceInitiator _dfuServiceInitiator;
        private readonly DfuProgressListener _progressListener;

        public FirmwareUpdater(string deviceAddress)
        {
            //_context = Application.Context;
            _progressListener = new DfuProgressListener();
            //_progressListener.DfuFailed += DfuFailed;
            //_progressListener.DfuProgressChanged += DfuProgressChanged;
            //_progressListener.DfuStateChanged += DfuStateChanged;
            //_dfuServiceInitiator = new DfuServiceInitiator(deviceAddress);
            _dfuServiceInitiator.SetPacketsReceiptNotificationsEnabled(true);
            _dfuServiceInitiator.SetUnsafeExperimentalButtonlessServiceInSecureDfuEnabled(true);
            DfuServiceListenerHelper.RegisterProgressListener(Application.Context, _progressListener);
            _dfuServiceInitiator = new DfuServiceInitiator(deviceAddress);
        }

        public void Start()
        {
            _dfuServiceInitiator.Start(Application.Context, null);
        }

        public void Dispose()
        {
            DfuServiceListenerHelper.UnregisterProgressListener(Application.Context, _progressListener);
        }
    }
    /*
    class DfuService : DfuBaseService
    {
        protected override Class NotificationTarget => throw new NotImplementedException();
    }
    */

    class DfuProgressListener : DfuProgressListenerAdapter
    {
        public override void OnDfuAborted(string deviceAddress)
        {
            base.OnDfuAborted(deviceAddress);
        }
        public override void OnProgressChanged(string deviceAddress, int percent, float speed, float avgSpeed, int currentPart, int partsTotal)
        {
            Log.Info("Xamarin.Nordic.Dfu", $"Firmware update in progress: {percent} %");
            base.OnProgressChanged(deviceAddress, percent, speed, avgSpeed, currentPart, partsTotal);
        }
    }
}
