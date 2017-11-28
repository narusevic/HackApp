using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Esri.ArcGISRuntime.UI.Controls;
using System;
using Esri.ArcGISRuntime.UI;
using System.Drawing;
using Esri.ArcGISRuntime.Symbology;

namespace HackApp3
{
	[Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : Activity
	{
		private MapViewModel _mapViewModel = new MapViewModel();
		private MapView _mapView;
		private GraphicsOverlay _sketchOverlay;

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

			btn_Anon.Click += RenderMap;
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
			var btn_Mark = FindViewById<Button>(Resource.Id.Mark);

			btn_Mark.Click += MarkOnMap;

			_sketchOverlay = new GraphicsOverlay();
			_mapView.GraphicsOverlays.Add(_sketchOverlay);

			var markerSymbol = new SimpleMarkerSymbol()
			{
				Color = Color.Red,
				Style = SimpleMarkerSymbolStyle.Cross,
				Size = 30d
			};

			foreach (var geometry in Geometries.GetAll())
			{
				_sketchOverlay.Graphics.Add(new Graphic(geometry, markerSymbol));
			}

			_mapView.Map = _mapViewModel.Map;

			// Listen for changes on the view model
			_mapViewModel.PropertyChanged += MapViewModel_PropertyChanged;
			//_mapView.LocationDisplay.AutoPanMode = LocationDisplayAutoPanMode.Recenter;
			_mapView.LocationDisplay.IsEnabled = true;
		}

		private void MarkOnMap(object sender, System.EventArgs e)
		{
			var btn_Mark = FindViewById<Button>(Resource.Id.Mark);
			//btn_Mark.Enabled = false;

			SketchCreationMode creationMode = SketchCreationMode.Point;
			//Esri.ArcGISRuntime.Geometry.Geometry geometry = await _mapView.SketchEditor.StartAsync(creationMode, true);
			
			//btn_Mark.Enabled = true;
			btn_Mark.Click += MarkOnMap;

			Graphic graphic = new Graphic(_mapView.LocationDisplay.MapLocation, new SimpleMarkerSymbol()
			{
				Color = Color.Red,
				Style = SimpleMarkerSymbolStyle.Cross,
				Size = 30d
			});

			_sketchOverlay.Graphics.Add(graphic);
		}

		private void MapViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Update the map view with the view model's new map
			if (e.PropertyName == "Map" && _mapView != null)
				_mapView.Map = _mapViewModel.Map;
		}
	}
}