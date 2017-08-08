
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

using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

using Com.Cloudrail.SI;
using Android.Graphics;
using System.Net;

namespace UnifiedSocialProfile
{
    [Activity(Label = "Profile", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
	[IntentFilter(new[] { Intent.ActionView },
				  Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
				  DataScheme = "com.cloudrail.unifiedsocialprofile")]
    public class ProfileActivity : Activity
    {
        private string serviceValue = "";
        private int servicePosition = 0;
        private IProfile service;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Profile);


            servicePosition = Intent.GetIntExtra("Service", 0);
            switch (servicePosition) {
                case 0:
					service = new Facebook(this, "439557219752767", "7265db555fbf26606870451605e1ae37");
					break;
				case 1:
					service = new GitHub(this, "ff2f6d88dd8a49366e30", "eeafe874a8b80599a09b5195022583fbda4ca2ad");
					break;
				case 2:
					service = new GooglePlus(this, "141870010879-td4fdoobrsm6ecki2kvveki6kdauu4su.apps.googleusercontent.com", "", "com.cloudrail.unifiedsocialprofile:/oauth2redirect", "someState");
                    ((GooglePlus)service).UseAdvancedAuthentication();
					break;
				case 3:
					service = new Heroku(this, "d81a8071-ab2b-4a8b-8385-a83d70ed6095", "ece9f5b1-c977-4d85-b6fa-c0403cdcbdff");
					break;
				case 4:
					service = new Instagram(this, "d714c3c872cb443e975d874922e66fcc", "ead73573c11c4a9e831460f0ff3bf164");
					break;
				case 5:
					service = new LinkedIn(this, "[LinkedIn Client Identifier]", "[LinkedIn Client Secret]");
					break;
				case 6:
					service = new ProductHunt(this, "9e79858debc14d0aab52bcad4430cbea06645e6df43986d3fe65fa9e4a46db47", "c04c57684eaf498eda439ed077270b09ea3a34218b283c9a4a6fb8a6bc24e88f");
					break;
				case 7:
					service = new Slack(this, "[Slack Client Identifier]", "[Slack Client Secret]");
					break;
				case 8:
					service = new Twitter(this, "FW6M3WmjhyiT2AYsohDZHGDTw", "1Ts5gOmPzXUgeckMDPV0dSEuY51L77cJCYBfftQFu6kI9kv2dp");
					break;
				case 9:
					service = new MicrosoftLive(this, "[Windows Live Client Identifier]", "[Windows Live Client Secret]");
					break;
				case 10:
					service = new Yahoo(this, "[Yahoo Client Identifier]", "[Yahoo Client Secret]");
					break;
            }
            serviceValue = servicePosition.ToString();
			//If Service exist in Shared Prefence load, it. 
			LoadService();

			ImageView imageView = (ImageView)FindViewById(Resource.Id.imageView1);
			TextView nameTextView = (TextView)FindViewById(Resource.Id.textViewName);
			TextView mailTextView = (TextView)FindViewById(Resource.Id.textViewEmail);
			TextView birthDateTextView = (TextView)FindViewById(Resource.Id.textViewBirthDate);
			TextView descriptionTextView = (TextView)FindViewById(Resource.Id.textViewDescription);
			TextView localeTextView = (TextView)FindViewById(Resource.Id.textViewLocale);
			TextView idTextView = (TextView)FindViewById(Resource.Id.textViewID);

			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
                String name = service.FullName;
                SaveService();
				String mail = service.Email;
                String birthDate = "";
				if (service.DateOfBirth != null)
				{
					birthDate = service.DateOfBirth.Day + "." + service.DateOfBirth.Month + "." + service.DateOfBirth.Year;
                }
				String description = service.Description;
				String locale = service.Locale;
				String id = service.Identifier;

                string url = service.PictureURL;

				Bitmap imageBitmap = null;
                if (url != null) {
					using (var webClient = new WebClient())
					{
						var imageBytes = webClient.DownloadData(url);
						if (imageBytes != null && imageBytes.Length > 0)
						{
							imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
						}
					}
                }

                RunOnUiThread(() =>
                {
                    nameTextView.Text = name;
                    mailTextView.Text = mail;
                    birthDateTextView.Text = birthDate;
                    descriptionTextView.Text = description;
                    localeTextView.Text = locale;
                    idTextView.Text = id;
					if (imageBitmap != null)
					{
						imageView.SetImageBitmap(imageBitmap);
                    }
                });
            })).Start();
        }
		protected override void OnNewIntent(Intent intent)
		{
			if (intent.Categories.Contains(Intent.CategoryBrowsable))
			{
				CloudRail.AuthenticationResponse = intent;
			}

			base.OnNewIntent(Intent);
		}

		//Load Service - LoadAsString() method used pass in the value from SaveAsString() which contains the tokens
		private void LoadService()
		{
			ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);
			string defaultValue = prefs.GetString(serviceValue, "");

			if (defaultValue != "")
			{
				service.LoadAsString(defaultValue);
			}
		}

		//Save Service - SaveAsString() method used
		private void SaveService()
		{
			ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);
			ISharedPreferencesEditor editor = prefs.Edit();
			editor.PutString(serviceValue, service.SaveAsString());
			editor.Apply();
		}
    }
}
