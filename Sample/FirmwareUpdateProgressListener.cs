using System;
using Android.Widget;
using NO.Nordicsemi.Android.Dfu;

namespace Sample
{
    public class FirmwareUpdateProgressListener : DfuProgressListenerAdapter
    {
        private TextView _statusText;
        private ProgressBar _progressBar;

        public FirmwareUpdateProgressListener(TextView statusText, ProgressBar progressBar)
        {
            _statusText = statusText;
            _progressBar = progressBar;
        }

        public override void OnDeviceConnected(string p0)
        {
            base.OnDeviceConnected(p0);
            _statusText.Text = "Connected";
        }

        public override void OnDeviceDisconnected(string p0)
        {
            base.OnDeviceDisconnected(p0);
            _statusText.Text = "Disconnected";
        }

        public override void OnDfuProcessStarted(string p0)
        {
            base.OnDfuProcessStarted(p0);
            _statusText.Text = "Started";
        }

        public override void OnProgressChanged(string p0, int p1, float p2, float p3, int p4, int p5)
        {
            base.OnProgressChanged(p0, p1, p2, p3, p4, p5);
            _progressBar.Progress = (int)(p2 * 100);
        }
    }
}
