using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

using Com.Cloudrail.SI;

namespace UnifiedSocialProfile
{
    [Activity(Label = "UnifiedSocialProfile", MainLauncher = true)]
    public class MainActivity : ListActivity
    {
        public string CLOUDRAIL_APP_KEY = "[Your CloudRail Key]";
        string[] items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CloudRail.AppKey = CLOUDRAIL_APP_KEY;

            // Set our view from the "main" layout resource
            // SetContentView(Resource.Layout.Main);

			items = new string[] { "Facebook", "GitHub", "GooglePlus", "Heroku", "Instagram", "LinkedIn", "Product Hunt", "Slack", "Twitter", "Windows Live", "Yahoo!" };
			ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
		}

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
            var activity = new Intent(this, typeof(ProfileActivity));
			activity.PutExtra("Service", position);
			StartActivity(activity);
		}
    }
}

