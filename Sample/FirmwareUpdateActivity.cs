using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using Sample.Nordic;

namespace Sample
{
    [Activity(Label = "FirmwareUpdateActivity")]
    public class FirmwareUpdateActivity : Activity
    {
        private TextView _statusTextView;
        private TextView _errorTextView;
        private TextView _successTextView;
        private DfuProgressListener _progressListener;
        private FirmwareUpdater _updater;
        private ProgressBar _fwProgressBar;
        private ProgressBar _fwProgressSpinner;
        private Button _abortButton;
        private Button _pauseButton;
        private Button _continueButton;
        private bool _uploadHasStarted;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.firmware_update);

            _abortButton = FindViewById<Button>(Resource.Id.fwUpdateAbort);
            _pauseButton = FindViewById<Button>(Resource.Id.fwUpdatePause);
            _continueButton = FindViewById<Button>(Resource.Id.fwUpdateContinue);
            _statusTextView = FindViewById<TextView>(Resource.Id.fwStatusText);
            _errorTextView = FindViewById<TextView>(Resource.Id.fwErrorText);
            _successTextView = FindViewById<TextView>(Resource.Id.fwSuccessText);
            var fwTitleText = FindViewById<TextView>(Resource.Id.fwTitleText);
            _fwProgressBar = FindViewById<ProgressBar>(Resource.Id.fwProgressBar);
            _fwProgressSpinner = FindViewById<ProgressBar>(Resource.Id.fwSpinner);

            fwTitleText.Text = $"Updating device {CurrentParameters.CurrentDevice.Name} with firmware {CurrentParameters.CurrentFirmwareUri.LastPathSegment}:";
            _progressListener = new DfuProgressListener();
            _updater = new FirmwareUpdater(CurrentParameters.CurrentDevice.Address, CurrentParameters.CurrentFirmwareUri, _progressListener);
            _updater.Start();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _progressListener.ProgressChanged += _progressListener_ProgressChanged;
            _progressListener.StatusChanged += _progressListener_StatusChanged;
            _progressListener.Failed += _progressListener_Failed;
            _progressListener.Finished += _progressListener_Finished;
            _pauseButton.Click += _pauseButton_Click;
            _continueButton.Click += _continueButton_Click;
            _abortButton.Click += _abortButton_Click;
        }

        protected override void OnPause()
        {
            base.OnPause();
            _progressListener.ProgressChanged -= _progressListener_ProgressChanged;
            _progressListener.StatusChanged -= _progressListener_StatusChanged;
            _progressListener.Failed -= _progressListener_Failed;
            _progressListener.Finished -= _progressListener_Finished;
            _pauseButton.Click -= _pauseButton_Click;
            _continueButton.Click -= _continueButton_Click;
            _abortButton.Click -= _abortButton_Click;
        }

        private void _abortButton_Click(object sender, EventArgs e)
        {
            _pauseButton.Enabled = false;
            _continueButton.Enabled = false;
            _abortButton.Enabled = false;
            _updater.Abort();
        }

        private void _continueButton_Click(object sender, EventArgs e)
        {
            _pauseButton.Enabled = true;
            _continueButton.Enabled = false;
            _updater.Resume();
        }

        private void _pauseButton_Click(object sender, EventArgs e)
        {
            _pauseButton.Enabled = false;
            _continueButton.Enabled = true;
            _updater.Pause();            
        }

        // BLE callbacks typically come back on other threads than the UI thread,
        // so we need to make sure we are running on UI thread to update ui stuff:
        private void _progressListener_Finished(object sender, EventArgs e)
        {
            RunOnUiThread(() =>
            {
                _successTextView.Visibility = ViewStates.Visible;
                _fwProgressBar.Visibility = ViewStates.Gone;
                _abortButton.Enabled = false;
                _pauseButton.Enabled = false;
                _continueButton.Enabled = false;
            });
        }

        private void _progressListener_Failed(object sender, string errorMessage)
        {
            RunOnUiThread(() =>
            {
                _errorTextView.Text = errorMessage;
                _fwProgressBar.Visibility = ViewStates.Gone;
                _errorTextView.Visibility = ViewStates.Visible;
                _abortButton.Enabled = false;
                _pauseButton.Enabled = false;
                _continueButton.Enabled = false;
            });

        }

        private void _progressListener_StatusChanged(object sender, string status)
        {
            RunOnUiThread(() => _statusTextView.Text = status);
        }

        private void _progressListener_ProgressChanged(object sender, int progress)
        {
            RunOnUiThread(() =>
            {
                if (!_uploadHasStarted)
                {
                    _fwProgressBar.Visibility = ViewStates.Visible;
                    _fwProgressSpinner.Visibility = ViewStates.Gone;
                    _uploadHasStarted = true;
                    _pauseButton.Enabled = true;
                }
                _fwProgressBar.Progress = progress;
                _statusTextView.Text = $"Progress: {progress} %";
            });
        }
    }
}
