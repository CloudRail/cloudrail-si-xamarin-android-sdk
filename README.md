[![Android Arsenal](https://img.shields.io/badge/Android%20Arsenal-CloudRail-brightgreen.svg?style=flat-square)](https://android-arsenal.com/details/1/3604)

<p align="center">
  <img width="200px" src="http://cloudrail.github.io/img/cloudrail_logo_github.png"/>
</p>

# CloudRail SI for Xamarin Android
Integrate Multiple Services With Just One API

<p align="center">
  <img width="300px" src="http://cloudrail.github.io/img/cloudrail_si_github.png"/>
</p>

CloudRail is an API integration solution which abstracts multiple APIs from different providers into a single and universal interface.

**Current Interfaces:**
<p align="center">
  <img width="800px" src="http://cloudrail.github.io/img/available_interfaces_v2.png"/>
</p>

---
---

Full documentation can be found at our [website](https://cloudrail.com/integrations).

Learn more about CloudRail on https://cloudrail.com

---
---
With CloudRail, you can easily integrate external APIs into your application.
CloudRail is an abstracted interface that takes several services and then gives a developer-friendly API that uses common functions between all providers.
This means that, for example, Upload() works in exactly the same way for Dropbox as it does for Google Drive, OneDrive, and other Cloud Storage Services, and GetEmail() works similarly the same way across all social networks.

## Download and Basic Setup

Just add download and the DLL `cloudrail-si-xamarin-android-sdk.dll` file to your project reference
and starting using it

```groovy
using Com.Cloudrail.SI;

CloudRail.AppKey = "{Your_License_Key}";
```


## Current Interfaces
Interface | Included Services
--- | ---
Cloud Storage | Dropbox, Google Drive, OneDrive, Box, Egnyte, OneDrive Business
Business Cloud Storage | AmazonS3, Microsoft Azure, Rackspace, Backblaze, Google Cloud Platform
Social Profiles | Facebook, GitHub, Google+, LinkedIn, Slack, Twitter, Windows Live, Yahoo, Instagram, Heroku
Social Interaction | Facebook, FacebookPage, Twitter
Payment | PayPal, Stripe
Email | Maljet, Sendgrid
SMS | Twilio, Nexmo
Point of Interest | Google Places, Foursquare, Yelp
---
### Cloud Storage Interface:

* Dropbox
* Box
* Google Drive
* Microsoft OneDrive
* Egnyte
* OneDrive Business

#### Features:

* Download files from Cloud Storage.
* Upload files to Cloud Storage.
* Get Meta Data of files, folders and perform all standard operations (copy, move, etc) with them.
* Retrieve user and quota information.
* Generate share links for files and folders.

[Full Documentation](https://cloudrail.com/integrations/interfaces/CloudStorage;platformId=XamarinAndroid)
#### Code Example:

```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};

// ICloudStorage cs = new Box(context, "[clientIdentifier]", "[clientSecret]");
// ICloudStorage cs = new OneDrive(context, "[clientIdentifier]", "[clientSecret]");

// Google Drive:
// GoogleDrive drive = new GoogleDrive(context, "[clientIdentifier]", "", "[redirectUri]", "[state]");
// drive.UseAdvancedAuthentication();
// ICloudStorage cs = drive;

ICloudStorage cs = new Dropbox(context, "[clientIdentifier]", "[clientSecret]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
        IList<CloudMetaData> filesFolders = cs.GetChildren("/");
        //IList<CloudMetaData> filesFolders = cs.GetChildrenPage("/", 1, 4);  // Path, Offet, Limit
        //cs.Upload(/image_2.jpg,stream,1024,true);   // Path and Filename, Stream (data), Size, overwrite (true/false)
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
})).Start();
````
---

### Business Cloud Storage Interface:

* Amazon S3
* Microsoft Azure
* Rackspace
* Backblaze
* Google Cloud Platform

#### Features:

* Create, delete and list buckets
* Upload files
* Download files
* List files in a bucket and delete files
* Get file metadata (last modified, size, etc.)

[Full Documentation](https://cloudrail.com/integrations/interfaces/BusinessCloudStorage;platformId=XamarinAndroid)
#### Code Sample
```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// IBusinessCloudStorage cs = new MicrosoftAzure(this, "[accountName]", "[accessKey]");
// IBusinessCloudStorage cs = new Rackspace(this, "[username]", "[apiKey]", "[region]");
// IBusinessCloudStorage cs = new Backblaze(this, "[accountId]", "[appKey]");
// IBusinessCloudStorage cs = new GoogleCloudPlatform(this, "[clientEmail]", "[privateKey]", "[projectId]");
IBusinessCloudStorage cs = new AmazonS3(this, "[accessKeyId]", "[secretAccessKey]", "[region]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
        Bucket bucket = cs.CreateBucket("");
        AssetManager assetManager = this.Assets;
        System.IO.Stream stream = assetManager.Open("UserData.csv");
        long size = assetManager.OpenFd("UserData.csv").Length;
        cs.UploadFile(bucket,"Data.csv",stream,size);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````

---

### Social Media Profiles Interface:

* Facebook
* Github
* Google Plus
* LinkedIn
* Slack
* Twitter
* Windows Live
* Yahoo
* Instagram
* Heroku

#### Features

* Get profile information, including full names, emails, genders, date of birth, and locales.
* Retrieve profile pictures.
* Login using the Social Network.

[Full Documentation](https://cloudrail.com/integrations/interfaces/Profile;platformId=Android)
#### Code Example:

```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// Google Plus:
// GooglePlus gPlus = new GooglePlus(this, "[clientIdentifier]", "", "[redirectUri]", "[state]");
// gPlus.UseAdvancedAuthentication();
// IProfile profile = gPlus;


// IProfile profile = new GitHub(this, "[clientIdentifier]", "[clientSecret]");
// IProfile = new Slack(this, "[clientIdentifier]", "[clientSecret]");
// IProfile = new Instagram(this, "[clientIdentifier]", "[clientSecret]");
// ...
IProfile profile = new Facebook(this, "[clientIdentifier]", "[clientSecret]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       string email = profile.Email;
       string fullname = profile.FullName;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---

### Social Media Interaction Interface:

* Facebook
* FacebookPage
* Twitter

#### Features

* Get a list of connections.
* Make a post for the user.
* Post images and videos.

[Full Documentation](https://cloudrail.com/integrations/interfaces/Social;platformId=XamarinAndroid)
#### Code Example:

```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// ISocial social = new Twitter(this, "[clientID]", "[clientSecret]");
// ISocial social = new Facebook(this, "[pageName]", "[clientID]", "[clientSecret]");
ISocial social = new Facebook(this, "[clientID]", "[clientSecret]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       social.PostUpdate("Hey there! I'm using CloudRail.");
       IList<String> connections = social.Connections;
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---

### Payment Interface:

* PayPal
* Stripe

#### Features

* Perform charges
* Refund previously made charges
* Manage subscriptions

[Full Documentation](https://cloudrail.com/integrations/interfaces/Payment;platformId=XamarinAndroid)
#### Code Example

```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// IPayment payment = new Stripe(this, "[secretKey]");
IPayment payment = new PayPal(this, true, "[clientIdentifier]", "[clientSecret]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       CreditCard source = new CreditCard(null, (Java.Lang.Long)6, (Java.Lang.Long)2021, "xxxxxxxxxxxxxxxx", "visa", "<FirstName>", "<LastName>", null);
       Charge charge = payment.CreateCharge((Java.Lang.Long)500, "USD", source);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---
### Email Interface:

* Mailjet
* Sendgrid

#### Features

* Send Email

[Full Documentation](https://cloudrail.com/integrations/interfaces/Email;platformId=XamarinAndroid)

#### Code Example

````java
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// IEmail email = new MailJet(this, "[clientIdentifier]", "[clientSecret]");
IEmail email = new SendGrid(this, "API Key");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       IList<string> toAddresses = new List<string>();
       toAddresses.Add("foo@bar.com");
       toAddresses.Add("bar@foo.com");
       email.SendEmail("info@cloudrail.com", "CloudRail", toAddresses, "Welcome", "Hello from CloudRail", null, null, null);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---
### SMS Interface:

* Twilio
* Nexmo
* Twizo

#### Features

* Send SMS

[Full Documentation](https://cloudrail.com/integrations/interfaces/SMS;platformId=Android)

#### Code Example

````csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// ISMS sms = new Nexmo(this, "[clientIdentifier]", "[clientSecret]");
ISMS sms = new Twilio(this, "[clientIdentifier]", "[clientSecret]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       sms.SendSMS("CloudRail", "+4912345678", "Hello from CloudRail");
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---
### Points of Interest Interface:

* Google Places
* Foursquare
* Yelp

#### Features

* Get a list of POIs nearby
* Filter by categories or search term

[Full Documentation](https://cloudrail.com/integrations/interfaces/PointsOfInterest;platformId=XamarinAndroid)
#### Code Example

```` csharp
using Com.Cloudrail.SI;
using Com.Cloudrail.SI.Interfaces;
using Com.Cloudrail.SI.Exceptions;
using Com.Cloudrail.SI.Services;
using Com.Cloudrail.SI.Types;

CloudRail.AppKey = "{Your_License_Key};


// IPointsOfInterest poi = new Foursquare(this, "[clientID]", "[clientSecret]");
// IPointsOfInterest poi = new Yelp(this, "[consumerKey]", "[consumerSecret]", "[token]", "[tokenSecret]");
IPointsOfInterest poi = new GooglePlaces(this, "[apiKey]");

new System.Threading.Thread(new System.Threading.ThreadStart(() =>
{
    try
    {
       IList<POI> res = poi.GetNearbyPOIs((Java.Lang.Double)49.4557091, (Java.Lang.Double)8.5279138, (Java.Lang.Long)1000, "restaurant", null);
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

})).Start();
````
---

More interfaces are coming soon.

## Advantages of Using CloudRail

* Consistent Interfaces: As functions work the same across all services, you can perform tasks between services simply.

* Easy Authentication: CloudRail includes easy ways to authenticate, to remove one of the biggest hassles of coding for external APIs.

* Switch services instantly: One line of code is needed to set up the service you are using. Changing which service is as simple as changing the name to the one you wish to use.

* Simple Documentation: There is no searching around Stack Overflow for the answer. The CloudRail documentation at https://cloudrail.com/integrations is regularly updated, clean, and simple to use.

* No Maintenance Times: The CloudRail Libraries are updated when a provider changes their API.

* Direct Data: Everything happens directly in the Library. No data ever passes a CloudRail server.

## Sample Applications

If you don't know how to start or just want to have a look at how to use our SDK in a real use case, we created a few sample application which you can try out:

* Sample using the CloudStorage interface: [UnifiedCloudStorage](https://github.com/CloudRail/cloudrail-si-xamarin-android-sdk/tree/master/Examples/UnifiedCloudStorage)

## License Key

CloudRail provides a developer portal which offers usage insights for the SDKs and allows you to generate license keys.

It's free to sign up and generate a key.

Head over to https://cloudrail.com/signup

## Pricing

Learn more about our pricing on https://cloudrail.com/pricing

## Other Platforms

CloudRail is also available for other platforms like iOS, Java and NodeJS. You can find all libraries on https://cloudrail.com

## Questions?

Get in touch at any time by emailing us: support@cloudrail.com

or

Tag a question with cloudrail on [StackOverflow](http://stackoverflow.com/questions/tagged/cloudrail)
