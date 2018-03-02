using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Views;

using Java.Util.Concurrent.Atomic;

using Com.Cloudrail.SI;


using System;

namespace UnifiedCloudStorage
{
    [Activity(Label = "UnifiedCloudStorage", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : ListActivity
    {
        public string CLOUDRAIL_APP_KEY = "5a213870f19b3c0ca0f5806f";
        string[] items;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CloudRail.AppKey = CLOUDRAIL_APP_KEY;

			items = new string[] { "Dropbox", "Google Drive", "Box", "One Drive", "Egnyte" };
			ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
        }

		protected override void OnListItemClick(ListView l, View v, int position, long id)
		{
            var activity = new Intent(this, typeof(ListFolderActivity));
			activity.PutExtra("Service", position);
			StartActivity(activity);
		}

        protected override void OnNewIntent(Intent intent) {
            if(intent.Categories.Contains("android.intent.category.BROWSABLE")) {
                CloudRail.AuthenticationResponse = intent;
            }

			base.OnNewIntent(Intent);
		}

        protected void OnResume(Intent intent) {
			if (intent.Categories.Contains("android.intent.category.BROWSABLE"))
			{
				CloudRail.AuthenticationResponse = intent;
			}

        }

    }
}

