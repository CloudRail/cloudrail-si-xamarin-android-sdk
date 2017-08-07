using System;

using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.OS;
using Android.Runtime;

using System.Linq;
using System.Collections.Generic;


using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Types;

using Java.IO;
using Java.Util;

namespace UnifiedCloudStorage
{
    public class CloudMetadataAdapter: BaseAdapter<CloudMetaData>
    {
		private IList<CloudMetaData> data;
		private ICloudStorage service;
        Activity context;

		public CloudMetadataAdapter(Activity context, int resource, IList<CloudMetaData> objects)
		{
            this.context = context;
			this.data = objects;
		}

		public CloudMetadataAdapter(Activity context, int resource, IList<CloudMetaData> objects, ICloudStorage service)
		{
			this.context = context;
			this.data = objects;
			this.service = service;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override CloudMetaData this[int position]
		{
			get { return data[position]; }
		}

		public override int Count
		{
            get { return data.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView; // re-use an existing view, if one is available
			if (view == null)
			{ // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.ActivityListItem, null);
			}
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = data[position].Name;


            ImageView img = (ImageView)view.FindViewById(Android.Resource.Id.Icon);

            if (img != null)
            {
				if (data[position].Folder)
				{
					img.SetImageResource(Resource.Drawable.ic_file_folder);
				}
                else
                {
                    img.SetImageResource(ResourcesID(data[position].Name));
                }
            }

            view.SetMinimumHeight(44);

			return view;
        }

		
        private int ResourcesID(string fileName) 
        {
            fileName = fileName.ToLower();
            if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg") || fileName.EndsWith(".bmp") || fileName.EndsWith(".gif"))
            {
                return Resource.Drawable.ic_file_pic;
            } 
            else if (fileName.EndsWith(".mov") || fileName.EndsWith(".mp4") || fileName.EndsWith(".avi") || fileName.EndsWith(".mkv") || fileName.EndsWith(".wmv"))
            {
                return Resource.Drawable.ic_file_video;
            }
			else if (fileName.EndsWith(".pdf"))
			{
                return Resource.Drawable.ic_file_pdf;
			}
            else
            {
                return Resource.Drawable.ic_file_doc;
            }

        }
    }
}
