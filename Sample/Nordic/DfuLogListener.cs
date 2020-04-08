using System;
using Android.Util;

namespace Sample.Nordic
{
    public class DfuLogListener : Java.Lang.Object, NO.Nordicsemi.Android.Dfu.IDfuLogListener
    {
        public DfuLogListener()
        {
        }

        public void OnLogEvent(string deviceAddress, int level, string message)
        {
            Log.Info("DFU", $"{deviceAddress}, lvl {level}: {message}");
        }
    }
}
