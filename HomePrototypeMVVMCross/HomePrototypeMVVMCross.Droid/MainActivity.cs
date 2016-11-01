using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Locations;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using System.Threading.Tasks;
using System.Text;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Preferences;
using Android.Graphics;
using Android.Provider;

namespace HomePrototypeMVVMCross.Droid
{
    [Activity(Label = "", Icon = "@drawable/icon", Theme ="@style/MyTheme")]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {
        static readonly string TAG = "X:" + typeof(MainActivity).Name;
        public static int ImageId = 1000;
        public string getImage;
        private GoogleMap maps;
        Location currentLocation;
        LocationManager locationManager;

        string provider;
        ImageView profileImage;
        DrawerLayout drawerLayout;
        Android.Net.Uri uri;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);

            SetSupportActionBar(toolbar);
            Android.Support.V7.App.ActionBar supportActionBar = SupportActionBar;
            supportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);
            supportActionBar.SetDisplayHomeAsUpEnabled(true);
            NavigationView navView = FindViewById<NavigationView>(Resource.Id.nav_view);
            View imageView = navView.GetHeaderView(0);
            profileImage = imageView.FindViewById<ImageView>(Resource.Id.image);
            profileImage.Click += ProfileImage_Click;
            navView.NavigationItemSelected += (sender, e) =>
            {
                switch (e.MenuItem.ItemId)
                {
                    //Do something
                    case Resource.Id.request:
                        var intent = new Intent(this, typeof(RequestStatusActivity));
                        StartActivity(intent);
                        break;
                    case Resource.Id.history:
                        var history = new Intent(this, typeof(HistoryActivity));
                        StartActivity(history);
                        break;
                    case Resource.Id.favourites:
                        var favourites = new Intent(this, typeof(FavouritesActivity));
                        StartActivity(favourites);
                        break;
                    case Resource.Id.message:
                        var message = new Intent(this, typeof(Signal_RChat));
                        StartActivity(message);
                        break;
                    case Resource.Id.viewFloor:
                        var floor = new Intent(this, typeof(ViewFloorMap));
                        StartActivity(floor);
                        break;
					case Resource.Id.settings:
						var settings = new Intent(this, typeof(Settings));
						StartActivity(settings);
						break;
                    default:
                        break;
                }
            };
           InitializeLocationManager();
            SetUpMap();

           /* ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            var valueOfImage = pref.GetString("key_uri", "null");
            if (valueOfImage != "null")
            {
                uri = Android.Net.Uri.Parse(valueOfImage);
                Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, uri);
                profileImage.SetImageBitmap(Bitmap.CreateScaledBitmap(bitmap, 60, 60, false));
            }*/

        }

        private void ProfileImage_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Image"), ImageId);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {

            if ((requestCode == ImageId) && (resultCode == Result.Ok) && (data != null))
            {
                uri = data.Data;
                Bitmap bitmap = MediaStore.Images.Media.GetBitmap(this.ContentResolver, uri);
                profileImage.SetImageBitmap(Bitmap.CreateScaledBitmap(bitmap, 60, 60, false));
                
               /* ISharedPreferences pref = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                ISharedPreferencesEditor editor = pref.Edit();
                editor.PutString("key_uri", uri.ToString());
                editor.Apply();*/

            }
      
        }
        private void SetUpMap()
        {
            if (maps == null)
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.map).GetMapAsync(this);
        }
        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    drawerLayout.OpenDrawer((int)GravityFlags.Left);
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        void InitializeLocationManager()
        {
            locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                provider = acceptableLocationProviders.First();
            }
            else
            {
                provider = string.Empty;
            }
        }

        public  void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
              var textExplanation = "Unable to determine your location. Try again in couple minutes.";
                Toast.MakeText(this, textExplanation, ToastLength.Short).Show();
            }
            else
            {
                LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
                Console.WriteLine("My Location {0}", currentLocation.Latitude);
                maps.MoveCamera(CameraUpdateFactory.NewLatLng(latlng));
                maps.AnimateCamera(CameraUpdateFactory.ZoomTo(15));
            }
        }

        public void OnProviderDisabled(string provider)
        {
            
        }

        public void OnProviderEnabled(string provider)
        {
            
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
           
        }
        protected override void OnResume()
        {
            base.OnResume();
            locationManager.RequestLocationUpdates(provider, 0, 0, this);
        }
        protected override void OnPause()
        {
            base.OnPause();
            locationManager.RemoveUpdates(this);
        }
        public void OnMapReady(GoogleMap googleMap)
        {
                maps = googleMap;
                maps.MyLocationEnabled = true;
        }
    }
 }

