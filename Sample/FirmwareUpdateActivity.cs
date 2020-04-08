
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Sample
{
    [Activity(Label = "FirmwareUpdateActivity")]
    public class FirmwareUpdateActivity : Activity
    {
        private TextView _statusTextView;
        private FirmwareUpdateProgressListener _progressListener;
        private FirmwareUpdater _updater;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.firmware_update);

            var abortButton = FindViewById<Button>(Resource.Id.fwUpdateAbort);
            var pauseButton = FindViewById<Button>(Resource.Id.fwUpdatePause);
            var continueButton = FindViewById<Button>(Resource.Id.fwUpdateContinue);
            _statusTextView = FindViewById<TextView>(Resource.Id.fwStatusText);
            var fwTitleText = FindViewById<TextView>(Resource.Id.fwTitleText);
            var fwProgressBar = FindViewById<ProgressBar>(Resource.Id.fwProgressBar);

            fwTitleText.Text = $"Updating device {CurrentParameters.CurrentDevice.Name} with firmware {CurrentParameters.CurrentFirmwareUri.LastPathSegment}:";
            _progressListener = new FirmwareUpdateProgressListener(_statusTextView, fwProgressBar);
            _updater = new FirmwareUpdater(CurrentParameters.CurrentDevice.Address, CurrentParameters.CurrentFirmwareUri, _progressListener);
            _updater.Start();
        }
    }
}
