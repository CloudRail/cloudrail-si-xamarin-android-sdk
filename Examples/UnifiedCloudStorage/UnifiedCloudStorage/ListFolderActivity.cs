
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
using Android.Util;

using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

using Com.Cloudrail.SI;
using Android.Content.Res;

namespace UnifiedCloudStorage
{
    [Activity(Label = "List Folder", LaunchMode = Android.Content.PM.LaunchMode.SingleTask)]
    [IntentFilter(new[] { Intent.ActionView },
                  Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
                  DataScheme = "com.cloudrail.unifiedcloudstorage")]
    public class ListFolderActivity : ListActivity
    {
        private ICloudStorage service = null;

        IList<CloudMetaData> filesFolders = null;

        private List<String> options = new List<String>() { "Download", "Share Link", "Delete" };

        private AlertDialog alertDialog = null;

        private int dataPosition = 0;

        private string serviceValue = "";
        private int servicePosition = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            servicePosition = Intent.GetIntExtra("Service",0);

            if (servicePosition == 1) 
            {
                GoogleDrive googleDrive = new GoogleDrive(this, "[Client Id]", "", "com.cloudrail.unifiedcloudstorage:/oauth2redirect", "state");
                googleDrive.UseAdvancedAuthentication();
                service = googleDrive;
            } 
            else if (servicePosition == 2)
            {
                service = new Box(this,"[Client Id]","[Client Secret]");
            }
			else if (servicePosition == 3)
			{
                service = new OneDrive(this, "[Application Id]", "[Application Secret]");
			}
			else if (servicePosition == 4)
			{
                service = new Egnyte(this,"[Domain]","[Client Id]","[Client Secret]");
            }
            else
            {
                Dropbox dropbox = new Dropbox(this, "zsoojeklqxn3zrc", "b8jjaf2d62kbdnf", "https://auth.cloudrail.com/com.cloudrail.unifiedcloudstorage", "state");
                dropbox.UseAdvancedAuthentication();
                service = dropbox;
            }


            serviceValue = servicePosition.ToString();

            //If Service exist in Shared Prefence load, it. 
            LoadService();

            //Login Method Optional
            //LoginMethod();

            //Get Files / Folder at Path. "/" = root path

            GetChildrenAtPath("/");
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
			return base.OnCreateOptionsMenu(menu);
		}

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            this.dataPosition = position;

			alertDialog = (new AlertDialog.Builder(this)).Create();
			alertDialog.SetTitle("Options");
			var listView = new ListView(this);
			listView.Adapter = new AlertListViewAdapter(this, options);
			listView.ItemClick += listViewItemClick;
			alertDialog.SetView(listView);
			alertDialog.Show();
        }


		void listViewItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
            alertDialog.Hide();

            if (filesFolders.Count > 0)
            {
                CloudMetaData metaData = filesFolders[dataPosition];

                if (e.Position == 0)
                {
                    if (metaData.Folder) 
                    {
						Toast.MakeText(this, "Can only download files", ToastLength.Short).Show();
                    }
                    else
                    {
                        DownloadFile(metaData);
                    }

                }
                else if (e.Position == 1)
                {
                    ShareLink(metaData);
                }
                else if (e.Position == 2)
                {
                    DeleteFileFolder(metaData);
                }
            }
			
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			//Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
			//ToastLength.Short).Show();

			var imageIntent = new Intent();
			imageIntent.SetType("image/*");
			imageIntent.SetAction(Intent.ActionGetContent);
			StartActivityForResult(
				Intent.CreateChooser(imageIntent, "Select photo"), 0);

			return base.OnOptionsItemSelected(item);
		}

		protected override void OnNewIntent(Intent intent)
		{
			if (intent.Categories.Contains(Intent.CategoryBrowsable))
			{
				CloudRail.AuthenticationResponse = intent;
			}

			base.OnNewIntent(Intent);
		}


		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
            System.IO.Stream stream = ContentResolver.OpenInputStream(data.Data);
            UploadImage(stream);
		}


		//Cloud Rail Services

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
			if (filesFolders.Count() > 0)
			{
                ISharedPreferences prefs = Application.Context.GetSharedPreferences("PREF_NAME", FileCreationMode.Private);
				ISharedPreferencesEditor editor = prefs.Edit();
				editor.PutString(serviceValue, service.SaveAsString());
				editor.Apply();
			}
        }

        //Login (Get User Name / User Email)
        private void LoginMethod()
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                try
                {
                    service.Login();
                    Console.WriteLine(service.UserName);
                    Console.WriteLine(service.UserLogin);
                    //Save Service
                    SaveService();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            })).Start();

        }

		//Get files/folder at Path - GetChildren(path) method used
        //Alternative method - GetChildrenPage(path,offset,limit) 
		private void GetChildrenAtPath(string path)
        {
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			  {
				  try
				  {
					  filesFolders = service.GetChildren(path);

					  RunOnUiThread(() =>
					  {
						  ListAdapter = new CloudMetadataAdapter(this, servicePosition, filesFolders);
                            
						  //Save Service
						  SaveService();
					  });
				  }
				  catch (Exception e)
				  {
					  Console.WriteLine(e.Message);
				  }

			  })).Start();
        }

		//Download file at Path - GetChildren(path) method used
		private void DownloadFile(CloudMetaData metaData)
        {
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			  {
				  try
				  {
                      string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                      string localFilename = metaData.Name;
					  string localPath = System.IO.Path.Combine(documentsPath, localFilename);
                      System.IO.Stream stream = service.Download(metaData.Path);  

                      System.IO.File.WriteAllBytes(localPath,ReadFully(stream));
                      
					  RunOnUiThread(() =>
					  {
                            Toast.MakeText(this, "File " + metaData.Name + " downloaded", ToastLength.Long).Show();
					  });
				  }
				  catch (Exception e)
				  {
					  Console.WriteLine(e.Message);
				  }

			  })).Start();
        }


		public static byte[] ReadFully(System.IO.Stream input)
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}

		//Create Share Link for file/folder at Path - CreateShareLink(path) method used
		private void ShareLink(CloudMetaData metaData)
		{
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			  {
				  try
				  {
                      string shareLink = service.CreateShareLink(metaData.Path);

					  RunOnUiThread(() =>
					  {
						    ClipboardManager clipboard = (ClipboardManager)this.GetSystemService(Context.ClipboardService);
                            ClipData clip = ClipData.NewPlainText("Sharable Link", shareLink);  
                            clipboard.PrimaryClip = clip;
                            
                            Toast.MakeText(this, "Copied link to clipboard\n" + shareLink, ToastLength.Long).Show();
					  });
				  }
				  catch (Exception e)
				  {
					  Console.WriteLine(e.Message);
				  }

			  })).Start();
		}

		//Delete file at Path - Delete(path) method used
		private void DeleteFileFolder(CloudMetaData metaData)
        {
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			  {
				  try
				  {
                      service.Delete(metaData.Path);   
					  RunOnUiThread(() =>
					  {
                         Toast.MakeText(this, "File Delete", ToastLength.Short).Show();
                         this.GetChildrenAtPath("/");
					  });
				  }
				  catch (Exception e)
				  {
					  Console.WriteLine(e.Message);
				  }

			  })).Start();
        }

		//Upload file - Upload(path,stream,size,overwrite) method used

		private void UploadImage(System.IO.Stream stream)
        {
			new System.Threading.Thread(new System.Threading.ThreadStart(() =>
			  {
				  try
				  {
					 // Random rand1 = new Random();
					  string fileName = "/image_2.jpg";
                      
                      service.Upload(fileName,stream,1024,true);
					  RunOnUiThread(() =>
					  {
						  Toast.MakeText(this, "File Uploaded", ToastLength.Short).Show();
						  this.GetChildrenAtPath("/");
					  });
				  }
				  catch (Exception e)
				  {
					  Console.WriteLine(e.Message);
				  }

			  })).Start();
        }
    }
}
