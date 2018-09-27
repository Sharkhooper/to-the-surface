public enum Orientation {
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3
}

public static class OrientationExtensions {
	/// <summary>
	/// Custom Modulo that does not return negative values
	/// </summary>
	/// <param name="i">Input value</param>
	/// <param name="m">Divisior</param>
	/// <returns>i % m</returns>
	private static int Mod(int i, int m) {
		int tmp = i % m;
		return tmp < 0 ? tmp + m : tmp;
	}

	/// <summary>
	/// Rotates the Orientation clockwise
	/// </summary>
	/// <param name="orientation">The input Orientation</param>
	/// <param name="i">Number of 90 degree clockwise rotation</param>
	/// <returns>orientation rotated 90 degrees clockwise i times</returns>
	public static Orientation RotateClockwise(this Orientation orientation, int i) {
		return (Orientation) Mod((int) orientation + i, 4);
	}
	
	public static Orientation GetRight(this Orientation orientation) {
		return orientation.RotateClockwise(1);
	}

	public static Orientation GetDown(this Orientation orientation) {
		return orientation.RotateClockwise(2);
	}

	public static Orientation GetLeft(this Orientation orientation) {
		return orientation.RotateClockwise(3);
	}
}
