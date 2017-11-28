using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Esri.ArcGISRuntime.UI.Controls;

namespace HackApp3
{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		MapViewModel _mapViewModel = new MapViewModel();
		MapView _mapView;

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

		private void Initialize()
		{
			SetContentView(Resource.Layout.Main);

			var btn_Register = FindViewById<Button>(Resource.Id.RegisterButton);
			var btn_Anon = FindViewById<Button>(Resource.Id.AnonymousButton);

			btn_Register.Click += Btn_Register_Click;

			btn_Anon.Click += (s, arg) =>
			{
				SetContentView(Resource.Layout.Map);
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

		private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Update the map view with the view model's new map
			if (e.PropertyName == "Map" && _mapView != null)
				_mapView.Map = _mapViewModel.Map;
		}
	}
}