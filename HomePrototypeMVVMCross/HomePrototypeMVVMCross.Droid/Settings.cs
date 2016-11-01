using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Provider;
using Android.Preferences;

namespace HomePrototypeMVVMCross.Droid
{
	[Activity(Label = "Settings", Theme="@style/MyTheme")]
	public class Settings : AppCompatActivity
	{
        public static int ImageId = 1000;
        public static string ImageValue = "HomePrototypeMVVMCross.Droid.HomePrototypeMVVMCross.Droid";
        ImageView imageView;
        Button buttonImageChange;
        Button buttonLogin;
        Android.Net.Uri uri;
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Settings);
			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);
			SetSupportActionBar(toolbar);
            //imageView = FindViewById<ImageView>(Resource.Id.imageView);
          //  buttonImageChange = FindViewById<Button>(Resource.Id.imageChange);
            buttonLogin = FindViewById<Button>(Resource.Id.btnLogin);

           // buttonImageChange.Click += ButtonImageChange_Click;
            buttonLogin.Click += ButtonLogin_Click;

        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            var showText = "Password changed.";
            Toast.MakeText(this, showText, ToastLength.Short).Show();
        }

      /*  private void ButtonImageChange_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Image"), ImageId);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == ImageId) && (resultCode == Result.Ok) && (data != null))
            {
                    uri = data.Data;
                    imageView.SetImageURI(uri);
                    imageView.Rotation = 90;
                    ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                    ISharedPreferencesEditor editor = pref.Edit();
                    editor.PutString("key_uri", uri.ToString());
                    editor.Apply();
                
            }
        }*/


        public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					OnBackPressed();
					break;
				default:
					break;
			}
			return base.OnOptionsItemSelected(item);
		}
        public override void OnBackPressed()
        {
            
            base.OnBackPressed();

        }
    }
}
