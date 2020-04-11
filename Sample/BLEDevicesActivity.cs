
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Util;
using Android.Widget;
using Plugin.BluetoothLE;

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
            SetContentView(Resource.Layout.ble_device_list);

            _adapter = new ArrayAdapter<BLEDevice> (this, Resource.Layout.ble_device);

            ListAdapter = _adapter;
            ListView.TextFilterEnabled = true;

            ListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                var device = _adapter.GetItem(args.Position);
                CurrentParameters.CurrentDevice = device;
                StartActivity(typeof(FirmwareUpdateActivity));
            };

        }

        protected override async void OnResume()
        {
            base.OnResume();
            _adapter.Clear();
            _adapter.NotifyDataSetChanged();
            _seenDevices.Clear();

            var bleReady = await CheckPermissionAndAskIfNeeded();
            if (bleReady)
            {
                CrossBleAdapter.Current.Scan().Subscribe(FoundDevice);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            CrossBleAdapter.Current.StopScan();
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
            _adapter.Add(new BLEDevice(device, scanResult.Rssi));
            _adapter.NotifyDataSetChanged();
        }
    }
}
