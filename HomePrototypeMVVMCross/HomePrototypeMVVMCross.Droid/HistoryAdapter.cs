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
using Java.Lang;
using HomePrototypeMVVMCross.Models;

namespace HomePrototypeMVVMCross.Droid
{
    public class HistoryAdapter : BaseAdapter<Data>, IFilterable
    {
        List<Data> data;
        List<Data> items;
        Activity context;
        public HistoryAdapter(Activity context, IEnumerable<Data> Data) : base()
        {
            this.context = context;
            items = Data.OrderBy(s => s.Name).ToList();

            Filter = new DaTaFilter(this);
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override Data this[int position]
        {
            get { return items[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var list = items[position];
            View view = convertView; 
            if (view == null) 
                view = (convertView ??
           context.LayoutInflater.Inflate(
         Resource.Layout.HistoryAdapter,
         parent,
         false)) as LinearLayout;

            var name = view.FindViewById<TextView>(Resource.Id.textFirst) as TextView;
            var image = view.FindViewById<ImageView>(Resource.Id.image) as ImageView;
            name.Text = list.Name;
            image.SetImageResource(list.Image);
            return view;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public Filter Filter
        {
            get; set;
        }
        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        }

        private class DaTaFilter : Filter
        {
            private readonly HistoryAdapter adapter;
            public DaTaFilter(HistoryAdapter adapter)
            {
                this.adapter = adapter;
            }

            protected override FilterResults PerformFiltering(ICharSequence constraint)
            {
                var returnObj = new FilterResults();
                var results = new List<Data>();
                if (adapter.data == null)
                    adapter.data = adapter.items;

                if (constraint == null) return returnObj;

                if (adapter.data != null && adapter.data.Any())
                {

                    results.AddRange(
                        adapter.data.Where(
                            history => history.Name.ToLower().Contains(constraint.ToString())));
                }


                returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
                returnObj.Count = results.Count;

                constraint.Dispose();

                return returnObj;
            }

            protected override void PublishResults(ICharSequence constraint, FilterResults results)
            {
                using (var values = results.Values)
                    adapter.items = values.ToArray<Java.Lang.Object>()
                        .Select(r => r.ToNetObject<Data>()).ToList();

                adapter.NotifyDataSetChanged();


                constraint.Dispose();
                results.Dispose();
            }
        }
    }
}