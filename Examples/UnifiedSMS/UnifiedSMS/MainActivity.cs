using Android.App;
using Android.Widget;
using Android.OS;
using System;

using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Services;

namespace UnifiedSMS
{
    [Activity(Label = "UnifiedSMS", MainLauncher = true)]
    public class MainActivity : Activity
    {
        ISMS service;
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
			Toast.MakeText(this, "Sending SMS...", ToastLength.Short).Show();

			TextView fromView = FindViewById<TextView>(Resource.Id.editTextFrom);
			TextView toView = FindViewById<TextView>(Resource.Id.editTextTo);

			String from = fromView.Text;
			String to = toView.Text;

			RadioGroup radios = FindViewById<RadioGroup>(Resource.Id.radioGroup1);
			switch (radios.CheckedRadioButtonId)
			{
                case Resource.Id.radioButtonNexmo:
                    service = new Nexmo(
                        this,
                        "[Nexmo Client ID]",
                        "[Nexmo Client Secret]"
                    );
					break;
                case Resource.Id.radioButtonTwilio:
					service = new Twilio(
						this,
						"[Twilio Client ID]",
						"[Twilio Client Secret]"
					);
					break;
				case Resource.Id.radioButtonTwizo:
                    service = new Twizo(
						this,
						"[Twizo Key]"
					);
					break;
			}


			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			{
				// run in background

				service.SendSMS(
                    from,
                    to,
                    "CloudRail is awesome!"
                );
				RunOnUiThread(() => {
					Toast.MakeText(this, "SMS was send", ToastLength.Short).Show();
				});
			})).Start();
		}
    }
}

