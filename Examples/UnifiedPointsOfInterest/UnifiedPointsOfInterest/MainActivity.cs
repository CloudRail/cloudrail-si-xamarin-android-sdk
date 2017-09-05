using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;

using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

namespace UnifiedPointsOfInterest
{
    [Activity(Label = "UnifiedPointsOfInterest", MainLauncher = true)]
    public class MainActivity : Activity
    {
        IPointsOfInterest service;

        double lat = 49.4871628;
        double lng = 8.4640606;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

			CloudRail.AppKey = "[Your CloudRail Key]";

			RadioButton radioFoursquare = FindViewById<RadioButton>(Resource.Id.radioFoursquare);
			RadioButton radioGoogleplaces = FindViewById<RadioButton>(Resource.Id.radioGoogleplaces);
			RadioButton radioYelp = FindViewById<RadioButton>(Resource.Id.radioYelp);

			radioFoursquare.Click += RadioButtonClick;
			radioGoogleplaces.Click += RadioButtonClick;
			radioYelp.Click += RadioButtonClick;

			RadioGroup rg = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
			updateLocations(rg.CheckedRadioButtonId);
        }


		private void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			updateLocations(rb.Id);
		}

        private void updateLocations(int buttonId)
        {
			switch (buttonId)
			{
				case Resource.Id.radioFoursquare:
					service = new Foursquare(
						this,
						"[Foursquare Client ID]",
						"[Foursquare Client Secret]"
					);
					break;
				case Resource.Id.radioGoogleplaces:
					service = new GooglePlaces(
						this,
						"[Google Places Client ID]"
					);
					break;
				case Resource.Id.radioYelp:
					service = new Yelp(
						this,
						"[Yelp Client ID]",
						"[Yelp Client Secret]"
					);
					break;
			}
            Java.Lang.Long range = new Java.Lang.Long(2000);
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				// run in background
				IList<POI> pois = service.GetNearbyPOIs(
					(Java.Lang.Double)lat,
					(Java.Lang.Double)lng,
					range,
					"Pizza",
					null
				);

				POI[] poiArray = new POI[pois.Count];
				pois.CopyTo(poiArray, 0);
				Array.Sort(poiArray, (x, y) => (dist(x).CompareTo(dist(y))));

				RunOnUiThread(() => {
					// manipulate UI controls
					ListView lv = FindViewById<ListView>(Resource.Id.listView1);
                    //lv.Adapter = new ArrayAdapter<POI>(this, Android.Resource.Layout.SimpleListItem1, pois);
                    lv.Adapter = new POIAdapter(this, poiArray, lat, lng);
				});
			})).Start();
        }

        private double dist(POI poi) {
            double earthRadius = 6371; // km

			double sLat1 = Math.Sin(radians(lat));
            double sLat2 = Math.Sin(radians(poi.Location.Latitude));
			double cLat1 = Math.Cos(radians(lat));
			double cLat2 = Math.Cos(radians(poi.Location.Latitude));
            double cLon = Math.Cos(radians(lng) - radians(poi.Location.Longitude));

			double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

			double d = Math.Acos(cosD);

			double dist = earthRadius * d;

			return dist;
        }

        private double radians(double degree) {
            return (degree * Math.PI / 180.0);
        }
    }
}

