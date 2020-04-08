using Android.Bluetooth;
using Plugin.BluetoothLE;

namespace Sample
{
    public class BLEDevice
    {
        public BLEDevice(IDevice device, int rssi)
        {
            Name = device.Name;
            // Since the BLE plugin is cross platform we need tyo use native device to find address
            
            var nativeDevice = (BluetoothDevice)device.NativeDevice;
            Address = nativeDevice.Address;
            Rssi = rssi;
        }

        public string Name { get; }
        public string Address { get; }
        public int Rssi { get; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            return $"{Address} ({Rssi})";
        }
    }
}