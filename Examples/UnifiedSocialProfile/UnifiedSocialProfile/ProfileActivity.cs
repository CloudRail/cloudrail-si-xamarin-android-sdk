
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
					service = new Facebook(this, "[Facebook Client Identifier]", "[Facebook Client Secret]");
					break;
				case 1:
					service = new GitHub(this, "[GitHub Client Identifier]", "[GitHub Client Secret]");
					break;
				case 2:
					service = new GooglePlus(this, "[Google+ Client Identifier]", "", "com.cloudrail.unifiedsocialprofile:/oauth2redirect", "someState");
                    ((GooglePlus)service).UseAdvancedAuthentication();
					break;
				case 3:
					service = new Heroku(this, "[Heroku Client Identifier]", "[Heroku Client Secret]");
					break;
				case 4:
					service = new Instagram(this, "[Instagram Client Identifier]", "[Instagram Client Secret]");
					break;
				case 5:
					service = new LinkedIn(this, "[LinkedIn Client Identifier]", "[LinkedIn Client Secret]");
					break;
				case 6:
					service = new ProductHunt(this, "[ProductHunt Client Identifier]", "[ProductHunt Client Secret]");
					break;
				case 7:
					service = new Slack(this, "[Slack Client Identifier]", "[Slack Client Secret]");
					break;
				case 8:
					service = new Twitter(this, "[Twitter Client Identifier]", "[Twitter Client Secret]");
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
