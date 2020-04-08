using System;
using Android.App;
using Java.Lang;
using NO.Nordicsemi.Android.Dfu;

namespace Sample.Nordic
{
    [Service]
    public class DfuService : DfuBaseService
    {

        public DfuService()
        {
        }

        protected override bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#endif
                return base.IsDebug;
            }
        }

        protected override Class NotificationTarget => Class.FromType(typeof(NotificationActivity));
    }
}
