using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace Sample
{
    public class PermissionHelper
    {

        public static int ENABLE_BLUETOOTH_REQUEST = 201;
        public static int LOCATION_REQUEST_CODE = 101;

        public static async Task<bool> CheckBluetoothEnabled(Activity activity)
        {
            var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            if(bluetoothAdapter == null)
            {
                return false;
            }

            if(!bluetoothAdapter.IsEnabled)
            {
                activity.RunOnUiThread(() =>
                {
                    var builder = new AlertDialog.Builder(activity);

                    builder.SetMessage("Please enable Bluetooth to use this app")
                        .SetTitle("Enable Bluetooth")
                        .SetPositiveButton("OK", (sender, args) =>
                        {
                            var enableIntent = new Intent(BluetoothAdapter.ActionRequestEnable);
                            activity.StartActivityForResult(enableIntent, ENABLE_BLUETOOTH_REQUEST);
                        })
                        .SetNegativeButton("Cancel", (sender, args) => { });

                    var dialog = builder.Create();
                    dialog.Show();
                });
            } else
            {
                return true;
            }

            MainActivity.Instance.BluetoothEnablePromise = new TaskCompletionSource<bool>();

            return await MainActivity.Instance.BluetoothEnablePromise.Task;
        }


        public static async Task<bool> CheckLocationPermissionAsync()
        {
            if (ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                return true;
            }
            else
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.AccessFineLocation }, LOCATION_REQUEST_CODE);
            }

            MainActivity.Instance.PermissionPromise = new TaskCompletionSource<bool>();

            return await MainActivity.Instance.PermissionPromise.Task;
        }

   }
}