using Android.App;
using Android.Widget;
using Android.OS;

namespace profile_page
{
    [Activity(Label = "profile_page", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
           // Button imageButton1 = FindViewById<Button>(Resource.Id.imageButton1);
            Button update = FindViewById<Button>(Resource.Id.update);
            Button save = FindViewById<Button>(Resource.Id.save);



          //  imageButton1.Click += delegate { imageButton1.Text = string.Format("update image please"); };
            update.Click += delegate { update.Text = string.Format("update profile please"); };
            save.Click += delegate { save.Text = string.Format("save profile please"); };
        }
    }
}

