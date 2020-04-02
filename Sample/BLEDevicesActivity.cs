
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.BluetoothLE;
using Xamarin.Essentials;
using Adapter = Android.Widget.Adapter;

namespace Sample
{
    [Activity(Label = "Select Device")]
    public class BLEDevicesActivity : ListActivity
    {
        private HashSet<Guid> _seenDevices = new HashSet<Guid>();
        private ArrayAdapter<BLEDevice> _adapter;

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            _adapter = new ArrayAdapter<BLEDevice> (this, Resource.Layout.ble_device);

            ListAdapter = _adapter;
            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                Toast.MakeText(Application, ((TextView)args.View).Text, ToastLength.Short).Show();
            };
        }
        protected override async void OnResume()
        {
            base.OnResume();

            var bleReady = await CheckPermissionAndAskIfNeeded();
            if (bleReady)
            {
                CrossBleAdapter.Current.Scan().Subscribe(FoundDevice);
            }
        }

        private async Task<bool> CheckPermissionAndAskIfNeeded()
        {
            var status = await PermissionHelper.CheckBluetoothEnabled(this);
            if (status)
            {
                return await PermissionHelper.CheckLocationPermissionAsync();
            }
            return false;
        }

        private void FoundDevice(IScanResult scanResult)
        {
            var device = scanResult.Device;
            Log.Info(GetType().Name, $"Found device {device.Name}, Address: {device.Uuid}");
            if (_seenDevices.Contains(device.Uuid)) return;
            _seenDevices.Add(device.Uuid);
            _adapter.Add(new BLEDevice(device));
            _adapter.NotifyDataSetChanged();
        }
    }
}
