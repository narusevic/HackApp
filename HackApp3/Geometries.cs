using Esri.ArcGISRuntime.Geometry;
using System;
using System.Collections.Generic;

namespace HackApp3
{
	internal class Geometries
	{
		internal static List<MapPoint> GetAll()
		{
			return new List<MapPoint>()
			{
				new MapPoint(582320, 6062668, new SpatialReference(4669))
			};
		}
	}
}