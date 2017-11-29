using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace HackApp3
{
	internal class GeometryHelper
	{
		internal static List<MapPoint> GetAll()
		{
			return new List<MapPoint>()
			{
				new MapPoint(25.267789, 54.692581, new SpatialReference(37001)),
				new MapPoint(25.276225, 54.693954, new SpatialReference(37001)),
				new MapPoint(25.290403, 54.686743, new SpatialReference(37001))
			};
		}

		internal static SimpleMarkerSymbol GetMarker()
		{
			return new SimpleMarkerSymbol()
			{
				Color = Color.FromArgb(128, 255, 0, 0),
				Style = SimpleMarkerSymbolStyle.Square,
				Outline = new SimpleLineSymbol()
				{
					Color = Color.Black,
					Width = 2
				},
				Size = 20d
			};
		}

			
	}
}