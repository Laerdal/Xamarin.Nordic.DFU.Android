using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace Sample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static MainActivity Instance { get; private set; }
        public TaskCompletionSource<bool> PermissionPromise { get; set; }
        public TaskCompletionSource<bool> BluetoothEnablePromise { get; set; }
        
        private static int FILE_SELECT_CODE = 301;

        public static Android.Net.Uri CurrentFirmware;

        private TextView _fwTextView;
        private Button _chooseBleDeviceBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Instance = this;
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var selectFWButton = FindViewById<Button>(Resource.Id.chooseFirmwareButton);
            selectFWButton.Click += SelectFWButtonOnClick;
            _fwTextView = FindViewById<TextView>(Resource.Id.textViewFirmware);
            _chooseBleDeviceBtn = FindViewById<Button>(Resource.Id.chooseDeviceButton);
            _chooseBleDeviceBtn.Click += GotoDeviceList;
        }

        private void GotoDeviceList(object sender, EventArgs e)
        {
            StartActivity(typeof(BLEDevicesActivity));
        }

        private void SelectFWButtonOnClick(object sender, EventArgs e)
        {
            Intent intent = new Intent(Intent.ActionGetContent); 
            intent.SetType("*/*"); 
            intent.AddCategory(Intent.CategoryOpenable);

            try {
                StartActivityForResult(
                    Intent.CreateChooser(intent, "Select a file containing the firmware image"),
                    FILE_SELECT_CODE);
            } catch (ActivityNotFoundException) {
                // Potentially direct the user to the Market with a Dialog
                Toast.MakeText(this, "Please install a File Manager.", 
                    ToastLength.Short).Show();
            }
        }


        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PermissionHelper.ENABLE_BLUETOOTH_REQUEST)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    BluetoothEnablePromise.SetResult(true);
                }
                else
                {
                    BluetoothEnablePromise.SetResult(false);
                }
            }
            else if (requestCode == FILE_SELECT_CODE)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    CurrentFirmware = data.Data;
                    _fwTextView.Text = CurrentFirmware.LastPathSegment;
                    _chooseBleDeviceBtn.Enabled = true;
                }
            }
        }        
    }
}

