using Android.App;
using Android.Widget;
using Android.OS;

namespace search
{
    [Activity(Label = "search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.requestbutton);
            // Button button1 = FindViewById<Button>(Resource.Id.requestbutton);



            button.Click += delegate { button.Text = string.Format("request sent"); };
            // button1.Click += delegate { button1.Text = string.Format("request sent"); };
        }
    }
}

