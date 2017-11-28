using System.ComponentModel;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Esri.ArcGISRuntime.UI.Controls;
using Android.Widget;

namespace HackApp3
{
	[Activity (Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        MapViewModel _mapViewModel = new MapViewModel();
        MapView _mapView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set the view from the "Main" layout resource
            SetContentView(Resource.Layout.Main);

			var btn_Register = FindViewById<Button>(Resource.Id.RegisterButton);
			var btn_Anon = FindViewById<Button>(Resource.Id.AnonymousButton);

			btn_Register.Click += (s, arg) =>
			{
				SetContentView(Resource.Layout.Register);
			};

			btn_Anon.Click += (s, arg) =>
			{
				SetContentView(Resource.Layout.Map);
				//RenderMap();

				////Get MapView from the view and assign map from view-model
				//_mapView = FindViewById<MapView>(Resource.Id.MyMapView);
				//_mapView.Map = _mapViewModel.Map;

				//// Listen for changes on the view model
				//_mapViewModel.PropertyChanged += MapViewModel_PropertyChanged;
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