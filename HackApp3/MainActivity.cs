using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Widget;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.UI.Controls;
using Java.IO;
using System.Drawing;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.Geometry;

namespace HackApp3
{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		private MapViewModel _mapViewModel = new MapViewModel();
		private MapView _mapView;
		private GraphicsOverlay _sketchOverlay;

		private ImageView _imageView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Initialize();

			// Get MapView from the view and assign map from view-model
			//_mapView = FindViewById<MapView>(Resource.Id.MyMapView);
			//_mapView.Map = _mapViewModel.Map;

			//// Listen for changes on the view model
			//_mapViewModel.PropertyChanged += MapViewModel_PropertyChanged;
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// Make it available in the gallery

			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// Display in ImageView. We will resize the bitmap to fit the display.
			// Loading the full sized image will consume to much memory
			// and cause the application to crash.

			int height = Resources.DisplayMetrics.HeightPixels;
			int width = _imageView.Height;
			App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
			if (App.bitmap != null)
			{
				_imageView.SetImageBitmap(App.bitmap);
				App.bitmap = null;
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
		}

		private void Initialize()
		{
			SetContentView(Resource.Layout.Main);

			var btn_Register = FindViewById<Button>(Resource.Id.RegisterButton);
			var btn_Anon = FindViewById<Button>(Resource.Id.AnonymousButton);

			btn_Register.Click += Btn_Register_Click;
			btn_Anon.Click += RenderMap;
		}

		private void Btn_RegisterProblemTest_Click(object sender, EventArgs e)
		{
			SetContentView(Resource.Layout.RegisterProblem);
			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				Button problemPhotoButton = FindViewById<Button>(Resource.Id.photoButton);
				_imageView = FindViewById<ImageView>(Resource.Id.imageView1);
				problemPhotoButton.Click += TakeAPicture;
			}
			Button problemBackButton = FindViewById<Button>(Resource.Id.RegisterProblemBackButton);
			problemBackButton.Click += (sender2, e2) => {
				_mapViewModel = new MapViewModel();

				RenderMap(sender2, e2);
			};

			Button problemButton = FindViewById<Button>(Resource.Id.RegisterProblemButton);
			problemButton.Click += (sender2, e2) => { 				
				_mapViewModel = new MapViewModel();

				RenderMap(sender2, e2);

				var location = _mapView.LocationDisplay.MapLocation;

				Graphic graphic = new Graphic(new MapPoint(location.X, location.Y, new SpatialReference(37001)), GeometryHelper.GetMarker());
				_sketchOverlay.Graphics.Add(graphic);
			};
		}

		private void Btn_Register_Click(object sender, System.EventArgs e)
		{
			SetContentView(Resource.Layout.Register);

			var btn_RegisterBack = FindViewById<Button>(Resource.Id.RegisterBackButton);
			btn_RegisterBack.Click += (s2, arg2) =>
			{
				Initialize();
			};
		}

		private void RenderMap(object sender, System.EventArgs e)
		{
			SetContentView(Resource.Layout.Map);

			//Get MapView from the view and assign map from view-model
			_mapView = FindViewById<MapView>(Resource.Id.MyMapView);
			_mapView.Map = _mapViewModel.Map;

			_sketchOverlay = new GraphicsOverlay();

			_mapView.GraphicsOverlays.Add(_sketchOverlay);

			var btn_RegisterIssue = FindViewById<Button>(Resource.Id.RegisterIssue);
			btn_RegisterIssue.Click += Btn_RegisterProblemTest_Click;
			
			foreach (var geometry in GeometryHelper.GetAll())
			{
				_sketchOverlay.Graphics.Add(new Graphic(geometry, GeometryHelper.GetMarker()));
			}

			// Listen for changes on the view model
			_mapViewModel.PropertyChanged += MapViewModel_PropertyChanged;
			//_mapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Recenter;
			_mapView.LocationDisplay.IsEnabled = true;
		}

		private void RegisterIssue(object sender, System.EventArgs e)
		{
			var btn_RegisterIssue = FindViewById<Button>(Resource.Id.RegisterIssue);

			SetContentView(Resource.Layout.RegisterProblem);

			btn_RegisterIssue.Click += RegisterIssue;

			Graphic graphic = new Graphic(_mapView.LocationDisplay.MapLocation, GeometryHelper.GetMarker());

			_sketchOverlay.Graphics.Add(graphic);
		}

		private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Update the map view with the view model's new map
			if (e.PropertyName == "Map" && _mapView != null)
				_mapView.Map = _mapViewModel.Map;
		}

		private void CreateDirectoryForPictures()
		{
			App._dir = new File(
				Android.OS.Environment.GetExternalStoragePublicDirectory(
					Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
				PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		private void TakeAPicture(object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new File(App._dir, String.Format("problemPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}
	}

	public static class App
	{
		public static File _file;
		public static File _dir;
		public static Bitmap bitmap;
	}

	public static class BitmapHelpers
	{
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
								   ? outHeight / height
								   : outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}
	}
}