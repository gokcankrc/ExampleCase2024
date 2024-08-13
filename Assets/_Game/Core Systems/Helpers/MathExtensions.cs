namespace Gokcan.Helpers
{
	public static class MathExtensions
	{
		public static float MapClamped(this float value, float unmappedFrom, float unmappedTo, float mappedFrom, float mappedTo)
		{
			if (value <= unmappedFrom)
				return mappedFrom;
			else if (value >= unmappedTo)
				return mappedTo;
			return value.MapUnclamped(unmappedFrom, unmappedTo, mappedFrom, mappedTo);
		}

		public static float MapUnclamped(this float value, float unmappedFrom, float unmappedTo, float mappedFrom, float mappedTo)
		{
			return ((mappedTo - mappedFrom) * ((value - unmappedFrom) / (unmappedTo - unmappedFrom))) + mappedFrom;
		}
	}
}