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
using Android.Support.V7.App;
using Android.Support.V4.View;
using MvvmCross.Droid.Support.V7.AppCompat;
using HomePrototypeMVVMCross.Models;

namespace HomePrototypeMVVMCross.Droid
{
    [Activity(Label = "", Theme = "@style/Theme.AppCompat.Light")]
    public class RequestStatusActivity : AppCompatActivity, MenuItemCompat.IOnActionExpandListener
    {
        private Android.Support.V7.Widget.SearchView searchView;
        ListView listView;
        List<Data> data;
        Button buttonRequest;
        RequestStatusAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RequestStatus);

            data = new List<Data>() { new Data() { Name = "User One (Administration)", Image = Resource.Drawable.ic_action_man, Check = false },
                new Data() { Name = "User Two (Sales)", Image = Resource.Drawable.ic_action_man, Check = false },
                new Data() { Name = "User Three (Human Resourses)", Image = Resource.Drawable.ic_action_man, Check = false } };

            listView = FindViewById<ListView>(Resource.Id.listView);
            buttonRequest = FindViewById<Button>(Resource.Id.buttonRequest);
            adapter = new RequestStatusAdapter(this, data);
            listView.Adapter = adapter;
            buttonRequest.Click += ButtonRequest_Click;
        }

        private void ButtonRequest_Click(object sender, EventArgs e)
        {
            string text = "Request sent";
            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);

            var item = menu.FindItem(Resource.Id.action_search);

            var searchItem = MenuItemCompat.GetActionView(item);
            searchView = searchItem.JavaCast<Android.Support.V7.Widget.SearchView>();
            searchView.QueryTextChange += (sender, e) => adapter.Filter.InvokeFilter(e.NewText);
            searchView.QueryTextSubmit += (sender, e) =>
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