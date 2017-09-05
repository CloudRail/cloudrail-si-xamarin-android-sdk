using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Com.Cloudrail.SI.Types;
using Java.Lang;

namespace UnifiedPointsOfInterest
{
    public class POIAdapter : BaseAdapter<POI>
    {
        POI[] pois;
        Activity context;
        double lat;
        double lng;
        public POIAdapter(Activity context, POI[] pois, double lat, double lng)
        {
            this.context = context;
            this.pois = pois;
            this.lat = lat;
            this.lng = lng;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

        public override POI this[int position] {
            get { return pois[position]; }
        }

        public override int Count
		{
			get { return pois.Length; }
		}

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null) {
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            }
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = pois[position].Name + "(" + dist(pois[position]) + " meter)";
            return view;
        }

		private double dist(POI poi)
		{
			double earthRadius = 6371; // km

			double sLat1 = System.Math.Sin(radians(lat));
			double sLat2 = System.Math.Sin(radians(poi.Location.Latitude));
			double cLat1 = System.Math.Cos(radians(lat));
			double cLat2 = System.Math.Cos(radians(poi.Location.Latitude));
			double cLon = System.Math.Cos(radians(lng) - radians(poi.Location.Longitude));

			double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

			double d = System.Math.Acos(cosD);

			double dist = earthRadius * d;

            return (int)(dist*1000);
		}

		private double radians(double degree)
		{
			return (degree * System.Math.PI / 180.0);
		}
    }
}
