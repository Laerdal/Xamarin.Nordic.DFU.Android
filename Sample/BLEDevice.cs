using Plugin.BluetoothLE;

namespace Sample
{
    public class BLEDevice
    {
        public BLEDevice(IDevice device)
        {
            Name = device.Name;
            Address = device.Uuid.ToString();
        }

        public string Name { get; }
        public string Address { get; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            return Address;
        }
    }
}