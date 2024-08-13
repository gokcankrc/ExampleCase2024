using UnityEngine;

namespace Gokcan.Helpers
{
	public static class VectorExtensions
	{
		public static Vector2 FromXY(this Vector3 v)
		{
			return (Vector2)v;
		}

		public static Vector2 FromXZ(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		public static Vector2 FromYZ(this Vector3 v)
		{
			return new Vector2(v.y, v.z);
		}

		public static Vector3 ToXY(this Vector2 v)
		{
			return (Vector3)v;
		}

		public static Vector3 ToXZ(this Vector2 v)
		{
			return new Vector3(v.x, 0, v.y);
		}

		public static Vector3 ToYZ(this Vector2 v)
		{
			return new Vector3(0, v.x, v.y);
		}

		public static Vector3 OmitX(this Vector3 v)
		{
			return new Vector3(0, v.y, v.z);
		}

		public static Vector3 OmitY(this Vector3 v)
		{
			return new Vector3(v.x, 0, v.z);
		}

		public static Vector3 OmitZ(this Vector3 v)
		{
			return new Vector3(v.x, v.y, 0);
		}

		public static Vector2Int[] GetNeighbors(this Vector2Int v)
		{
			return new Vector2Int[]
			{
				v + new Vector2Int(1, 0),
				v + new Vector2Int(-1, 0),
				v + new Vector2Int(0, 1),
				v + new Vector2Int(0, -1),
			};
		}

		public static Vector3Int[] GetNeighbors(this Vector3Int v)
		{
			return new Vector3Int[]
			{
				v + new Vector3Int(1 ,0, 0),
				v + new Vector3Int(-1 ,0, 0),
				v + new Vector3Int(0 ,1, 0),
				v + new Vector3Int(0 ,-1, 0),
				v + new Vector3Int(0 ,0, 1),
				v + new Vector3Int(0 ,0, -1),
			};
		}
	}

	public static class GridExtensions
	{
		public static Bounds GetTotalBounds(this Grid g, Vector3Int dimensions)
		{
			Vector3 bounds = g.cellSize;
			bounds.Scale(dimensions);
			Vector3 gapTotal = g.cellGap;
			Vector3Int gapDimensions = dimensions - Vector3Int.one;
			gapTotal.Scale(gapDimensions);
			return new Bounds(Vector3.zero, bounds + gapTotal);
		}

		public static Bounds GetTotalBounds(this Grid g, Vector2Int dimensions)
		{
			return g.GetTotalBounds(new Vector3Int(dimensions.x, dimensions.y, 0));
		}
	}
}