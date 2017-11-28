using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Esri.ArcGISRuntime.UI.Controls;
using Android.Widget;
using System;
using Esri.ArcGISRuntime.UI;

namespace HackApp3
{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		MapViewModel _mapViewModel = new MapViewModel();
		MapView _mapView;

		private ViewState m_CurrentView = ViewState.Main;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set the view from the "Main" layout resource
			SetContentView(Resource.Layout.Main);

			var btn_Register = FindViewById<Button>(Resource.Id.RegisterButton);
			var btn_Anon = FindViewById<Button>(Resource.Id.AnonymousButton);

			btn_Register.Click += RegisterClicked;
			btn_Anon.Click += AnonClicked;
		}

		public override void OnBackPressed()
		{
			switch (m_CurrentView)
			{
				case ViewState.Map:
					SetContentView(Resource.Layout.Main);
					break;
				case ViewState.Register:
					SetContentView(Resource.Layout.Main);
					break;
				default:
					break;
			}
		}

		private void RegisterClicked(object sender, EventArgs ea)
		{
			SetContentView(Resource.Layout.Register);
			m_CurrentView = ViewState.Register;
		}

		private void AnonClicked(object sender, EventArgs ea)
		{
			SetContentView(Resource.Layout.Map);
			m_CurrentView = ViewState.Register;
			RenderMap();
		}

		private void RenderMap()
		{
			//Get MapView from the view and assign map from view-model
			_mapView = FindViewById<MapView>(Resource.Id.MyMapView);
			_mapView.Map = _mapViewModel.Map;

			// Listen for changes on the view model
			_mapViewModel.PropertyChanged += MapViewModel_PropertyChanged;

			_mapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Recenter;
			_mapView.LocationDisplay.IsEnabled = true;

		}

		private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Update the map view with the view model's new map
			if (e.PropertyName == "Map" && _mapView != null)
				_mapView.Map = _mapViewModel.Map;
		}
	}
}