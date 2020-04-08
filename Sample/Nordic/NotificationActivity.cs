using System;
using Android.App;
using Android.OS;

namespace Sample.Nordic
{
    public class NotificationActivity : Activity
    {
        public NotificationActivity()
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Finish();
        }
    }
}
