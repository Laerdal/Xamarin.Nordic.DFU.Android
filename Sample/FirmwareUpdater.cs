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
        private readonly DfuProgressListenerAdapter _progressListener;
        private DfuServiceController _controller;
        private DfuLogListener _dfuLogListener;

        public FirmwareUpdater(string deviceAddress, Android.Net.Uri firmwareZipFile, DfuProgressListenerAdapter dfuProgressListenerAdapter)
        {
            _dfuLogListener = new DfuLogListener();
            DfuServiceListenerHelper.RegisterLogListener(Application.Context, _dfuLogListener);
            _progressListener = dfuProgressListenerAdapter;
            _dfuServiceInitiator = new DfuServiceInitiator(deviceAddress);
            _dfuServiceInitiator.SetPacketsReceiptNotificationsEnabled(true);
            _dfuServiceInitiator.SetDisableNotification(true);
            _dfuServiceInitiator.SetUnsafeExperimentalButtonlessServiceInSecureDfuEnabled(true);
            DfuServiceListenerHelper.RegisterProgressListener(Application.Context, _progressListener);
            _dfuServiceInitiator.SetZip(firmwareZipFile);
        }

        public void Start()
        {
            _controller = _dfuServiceInitiator.Start(Application.Context, Class.FromType(typeof(DfuService)));
        }

        public void Abort()
        {
            _controller.Abort();
        }

        public void Pause()
        {
            _controller.Pause();
        }

        public void Resume()
        {
            _controller.Resume();
        }

        public void Dispose()
        {
            DfuServiceListenerHelper.UnregisterProgressListener(Application.Context, _progressListener);
        }
    }
}
