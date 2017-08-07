using System;

using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.OS;
using Android.Runtime;

using System.Collections.Generic;

using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Types;

namespace UnifiedCloudStorage
{
    public class AlertListViewAdapter: BaseAdapter<string>
    {
		Activity context = null;
		List<String> lstDataItem = null;


		public AlertListViewAdapter(Activity context, List<String> lstDataItem)
		{
			this.context = context;
			this.lstDataItem = lstDataItem;
		}


		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get { return lstDataItem.Count; }
		}

		public override string this[int position]
		{
			get { return lstDataItem[position]; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			if (convertView == null)
				convertView = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

			(convertView.FindViewById<TextView>(Android.Resource.Id.Text1))
				.SetText(this[position], TextView.BufferType.Normal);

			return convertView;
		}

    }
}
