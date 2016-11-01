using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using HomePrototypeMVVMCross.Models;

namespace HomePrototypeMVVMCross.Droid
{
    [Activity(Label = "Favourites", Theme ="@style/Theme.AppCompat.Light")]
    public class FavouritesActivity : ActionBarActivity
    {
        
        private Android.Support.V7.Widget.SearchView _searchView;
        ListView listView;
        ListView listViewAll;
        List<Data> data;
        List<Data> dataAll;
        RequestStatusAdapter adapter;
        RequestStatusAdapter adapterAll;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Favourites);

            dataAll = new List<Data>() { new Data() { Name = "User One (Administration)", Image = Resource.Drawable.ic_action_man, Check = false },
                new Data() { Name = "User Two (Sales)", Image = Resource.Drawable.ic_action_man, Check = false },
                new Data() { Name = "User Three (Human Resourses)", Image = Resource.Drawable.ic_action_man, Check = false } };

            data = new List<Data>() { new Data() { Name = "User One (Administration)", Image = Resource.Drawable.ic_action_man, Check = false },
                new Data() { Name = "User Two (Sales)", Image = Resource.Drawable.ic_action_man, Check = false },
                 };

            listViewAll = FindViewById<ListView>(Resource.Id.listViewAll);
            listView = FindViewById<ListView>(Resource.Id.listView);
            adapter = new RequestStatusAdapter(this, data);
            listView.Adapter = adapter;

            adapterAll = new RequestStatusAdapter(this, dataAll);
            listViewAll.Adapter = adapterAll;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);

            var item = menu.FindItem(Resource.Id.action_search);

            var searchItem = MenuItemCompat.GetActionView(item);
            _searchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();
            _searchView.QueryTextChange += (s, e) => adapter.Filter.InvokeFilter(e.NewText);
            _searchView.QueryTextSubmit += (s, e) =>
            {
                Toast.MakeText(this, "Searched for: " + e.Query, ToastLength.Short).Show();
                e.Handled = true;
            };
            MenuItemCompat.SetOnActionExpandListener(item, new SearchViewExpandListener(adapter));
            return true;
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
            return true;
        }

        public bool OnMenuItemActionCollapse(IMenuItem item)
        {
            OnBackPressed();
            return true;
        }

        public bool OnMenuItemActionExpand(IMenuItem item)
        {
            return true;
        }
    }
}