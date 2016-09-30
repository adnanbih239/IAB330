using Android.App;
using Android.Widget;
using Android.OS;

namespace login
{
    [Activity(Label = "login", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button1 = FindViewById<Button>(Resource.Id.loginbutton);
            Button button2 = FindViewById<Button>(Resource.Id.clearbutton);



            button1.Click += delegate { button1.Text = string.Format("login", count++); };
            button2.Click += delegate { button2.Text = string.Format("clear"); };
        }
    }
}

