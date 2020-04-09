using System;
using Android.App;
using Android.OS;

namespace Sample.Nordic
{
    public class DfuNotificationActivity : Activity
    {
        public DfuNotificationActivity()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Finish();
        }
    }
}
