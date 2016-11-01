using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using System.Threading.Tasks;

namespace HomePrototypeMVVMCross.Droid
{
    [Activity(Label = "View Floor Map", Theme ="@style/MyTheme")]
    public class ViewFloorMap : AppCompatActivity, IOnMapReadyCallback, ILocationListener
    {
        private GoogleMap maps;
        Location currentLocation;
        LocationManager locationManager;
        string provider;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewFloorMap);
            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolBar);
            SetSupportActionBar(toolbar);
            Android.Support.V7.App.ActionBar supportActionBar = SupportActionBar;
            supportActionBar.SetDisplayHomeAsUpEnabled(true);
            SetUpMap();

            InitializeLocationManager();
            SetUpMap();
        }
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
        private void SetUpMap()
        {
            if (maps == null)
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.floorMap).GetMapAsync(this);
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

        public void OnLocationChanged(Location location)
        {
            currentLocation = location;
            if (currentLocation == null)
            {
              var nullExplanation = "Can't find your location. Try again in couple minutes.";
                Toast.MakeText(this, nullExplanation, ToastLength.Short).Show();
            }
            else
            {
                LatLng latlng = new LatLng(currentLocation.Latitude, currentLocation.Longitude);
                maps.MoveCamera(CameraUpdateFactory.NewLatLng(latlng));
                maps.AnimateCamera(CameraUpdateFactory.ZoomTo(16));        
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