using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;

using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Services;
using Android.Content;

namespace UnifiedEmail
{
	[Activity(Label = "UnifiedEmail", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
	[IntentFilter(new[] { Intent.ActionView },
				  Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
                  DataScheme = "com.cloudrail.unifiedemail")]
	public class MainActivity : Activity
	{
		IEmail service;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			CloudRail.AppKey = "[Your CloudRail Key]";

			Button sendButton = FindViewById<Button>(Resource.Id.button1);
			sendButton.Click += sendButtonClicked;
		}

		private void sendButtonClicked(object sender, EventArgs ea)
		{
			RunOnUiThread(() => {
                Toast.MakeText(this, "Sending Mail...", ToastLength.Short).Show();
			});

            TextView fromAddrView = FindViewById<TextView>(Resource.Id.editTextFromAddress);
            TextView fromNameView = FindViewById<TextView>(Resource.Id.editTextFromName);
            TextView toAddrView = FindViewById<TextView>(Resource.Id.editTextToAddress);

			String fromAddress = fromAddrView.Text;
			String fromName = fromNameView.Text;
			List<String> toAddresses = new List<string>();
			toAddresses.Add(toAddrView.Text);

			RadioGroup radios = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
			switch (radios.CheckedRadioButtonId)
			{
				case Resource.Id.radioButtonGmail:
					service = new GMail(
						this,
						"[Gmail Client ID]",
						"",
                        "com.cloudrail.UnifiedEmail:/oauth2redirect",
                        "someState"
					);
                    ((GMail)service).UseAdvancedAuthentication();
					break;
				case Resource.Id.radioButtonMailjet:
					service = new MailJet(
						this,
						"[MailJet Client ID]",
						"[MailJet Client Secret]"
					);
					break;
				case Resource.Id.radioButtonSendgrid:
					service = new SendGrid(
						this,
						"[SendGrid App Key]"
					);
					break;
			}

                  
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				// run in background
				service.SendEmail(
					fromAddress,
					fromName,
					toAddresses,
					"CloudRail is awesome!",
					"Go ahead and try it for yourself!",
					null,
					null,
					null,
					null
				);
				RunOnUiThread(() => {
                    Toast.MakeText(this, "Mail was send", ToastLength.Short).Show();
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
	}
}

