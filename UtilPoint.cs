using System;
using UnityEngine;

public static class Mathi
{ // int versions of the Math + Mathf class. Duplicate Math methods because we can't derive
	static public int Max(int i1, int i2)
	{
		return Math.Max(i1, i2);
	}

	static public int Min(int i1, int i2)
	{
		return Math.Min(i1, i2);
	}

	static public int Abs(this int i)
	{
		return Math.Abs(i);
	}

	static public int Sign(this int i)
	{
		return Math.Sign(i);
	}

	static public int Clamp(this int v, int min, int max)
	{
		return Min(Max(v, min), max);
	}

	static public int ClampExclusive(this int v, int min, int max)
	{ // excludes the top value
		return Clamp(v, min, max - 1);
	}

	public static int FloorToInt(float f)
	{ // Returns the highest integer equal to the given float.
		var n = (int)f; //truncate
		return (n > f) ? n - 1 : n;
	}

	public static int Mod(int m, int n)
	{ // Mod operator that also works for negative m.
		return (m >= 0) ? (m % n) : ((m - 2 * m * n) % n);
	}

	public static int Div(int m, int n)
	{ // Floor division that also work for negative m.
		if(m >= 0)
			return m / n;

		int t = m / n;
		return (t * n == m) ? t : t - 1;
	}

	public static float Frac(float x)
	{
		return x - FloorToInt(x);
	}

	public static int Signum(this int i)
	{
		return (i == 0) ? 0 : ((i > 0) ? 1 : -1);
	}
}

public struct Point2
{
	public int x, y;

	public int this[int index]
	{
		get
		{
			switch(index)
			{
				case 0:
					return x;
				case 1:
					return y;
				default:
					throw new IndexOutOfRangeException("Invalid Point2 index");
			}
		}

		set
		{
			switch(index)
			{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Point2 index");
			}
		}
	}

	public static readonly Point2 north = new Point2(0, 1);
	public static readonly Point2 northeast = new Point2(1, 1);
	public static readonly Point2 east = new Point2(1, 0);
	public static readonly Point2 southeast = new Point2(1, -1);
	public static readonly Point2 south = new Point2(0, -1);
	public static readonly Point2 southwest = new Point2(-1, -1);
	public static readonly Point2 west = new Point2(-1, 0);
	public static readonly Point2 northwest = new Point2(-1, 1);

	public enum Direction
	{ // handy for iterating through directions. There's a constructor down there that uses 'em.
		Invalid = -1,
		First = 0,
		North = 0,
		NorthEast,
		East,
		SouthEast,
		South,
		SouthWest,
		West,
		NorthWest,
		Length
	};

	private static readonly Point2[] PtCompass = { north, northeast, east, southeast, south, southwest, west, northwest };
	private static readonly string[] StrCompass = new string[] { "North", "NorthEast", "East", "SouthEast", "South", "SouthWest", "West", "NorthWest" };

	public Direction RegionInRect(Point2 Size)
	{ // is this point north-ish, south-ish, east-ish or west-ish in the passed size
		bool NW = Vector3.Cross(new Vector2(x, y), new Vector2(Size.x, Size.y)).z < 0;
		bool NE = Vector3.Cross(new Vector2(x - Size.x, y), new Vector2(Size.x * -1, Size.y)).z > 0;

		Direction RetVal = Direction.West;

		if(NW && NE)
			RetVal = Direction.North;
		else if (!NW && NE)
			RetVal = Direction.East;
		else if (!NW && !NE)
			RetVal = Direction.South;

		return RetVal;
	}

	public static Direction DirectionRotate(Direction d, int delta)
	{
		delta = (delta % (int)Direction.Length) + (int)Direction.Length;
		return (Direction)(((int)d + delta) % (int)Direction.Length);
	}

	public static Direction DirectionOpposite(Direction d)
	{
		return DirectionRotate(d, 4);
	}

	public static Direction DirectionNormalize(Direction d)
	{ // mods a direction into compass directions
		return DirectionRotate(d, 0);
	}

	public static Direction DirectionOf(Point2 p)
	{
		Direction RetVal = Direction.Invalid;
		Point2 pUnit = p.unit;
		for(var D = Direction.First; (D < Direction.Length) && (RetVal == Direction.Invalid); ++D)
			if(PtCompass[(int)D] == pUnit)
				RetVal = D;

		return RetVal;
	}

	public static Direction DirectionOf(String s)
	{
		Direction RetVal = Direction.Invalid;
		for(var D = Direction.First; (D < Direction.Length) && (RetVal == Direction.Invalid); ++D)
			if(StrCompass[(int)D] == s)
				RetVal = D;

		return RetVal;
	}

	public static string NameOf(Direction D)
	{
		return StrCompass[(int)D];
	}

	public static readonly Point2 zero = new Point2(0, 0);
	public static readonly Point2 one = new Point2(1, 1);

	public Point2 abs
	{
		get
		{
			return new Point2(Mathi.Abs(x), Mathi.Abs(y));
		}
	}

	public int max
	{
		get
		{
			return (x > y) ? x : y;
		}
	}

	public Point2 unit
	{
		get
		{
			return new Point2(Mathi.Signum(x), Mathi.Signum(y));
		}
	}

	public float sqrMagnitude
	{
		get
		{
			return (float)x * x + (float)y * y;
		}
	}

	public float magnitude
	{
		get
		{
			return (float)Math.Sqrt(sqrMagnitude);
		}
	}

	public static Point2 operator +(Point2 a, Point2 b)
	{
		return new Point2(a.x + b.x, a.y + b.y);
	}

	public static Point2 operator -(Point2 a)
	{
		return new Point2(-a.x, -a.y);
	}

	public static Point2 operator -(Point2 a, Point2 b)
	{
		return new Point2(a.x - b.x, a.y - b.y);
	}

	public static Point2 operator *(int d, Point2 a)
	{
		return new Point2(d * a.x, d * a.y);
	}

	public static Point2 operator *(Point2 a, int d)
	{
		return d * a;
	}

	public static Point2 operator /(Point2 a, int d)
	{
		return new Point2(a.x / d, a.y / d);
	}

	public static bool operator ==(Point2 p1, Point2 p2)
	{
		return (p1.x == p2.x) && (p1.y == p2.y);
	}

	public static bool operator !=(Point2 p1, Point2 p2)
	{
		return (p1.x != p2.x) || (p1.y != p2.y);
	}

	public override bool Equals(object obj)
	{
		return (obj is Point2) ? (this == (Point2)obj) : false;
	}

	public bool Equals(Point2 other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{ // not an optimal hash, but this class has no immutable members
		return 0x0819023d;
	}

	public override string ToString()
	{
		return "[" + x + "," + y + "]";
	}

	public static float Distance(Point2 p1, Point2 p2)
	{
		return (p1 - p2).magnitude;
	}

	public float Distance(Point2 p)
	{
		return Distance(this, p);
	}

	public static int Dot(Point2 lhs, Point2 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y;
	}

	public int Dot(Point2 rhs)
	{
		return Dot(this, rhs);
	}

	public bool InBounds(int xFrom, int yFrom, int xTo, int yTo)
	{
		return (x >= xFrom) && (x < xTo) && (y >= yFrom) && (y < yTo);
	}

	public bool InBounds(int xTo, int yTo)
	{
		return InBounds(0, 0, xTo, yTo);
	}

	public static explicit operator Point2(Vector2 v)
	{
		return new Point2((int)v.x, (int)v.y);
	}

	public static implicit operator Vector2(Point2 p)
	{
		return new Vector2(p.x, p.y);
	}

	public static explicit operator Point2(Vector3 v)
	{
		return new Point2((int)v.x, (int)v.y);
	}

	public static implicit operator Vector3(Point2 p)
	{
		return new Vector3(p.x, p.y, 0);
	}

	public Point2(int x = 0, int y = 0)
	{
		this.x = x;
		this.y = y;
	}

	public Point2(Vector2 v)
	{
		x = (int)v.x;
		y = (int)v.y;
	}

	public Point2(Direction d)
	{ // returns the x y delta or Point2.Zero if the value is out of range
		this = PtCompass[(int)DirectionNormalize(d)];
	}
}

public struct Point3
{
	public int x, y, z;

	public int this[int index]
	{
		get
		{
			switch(index)
			{
				case 0:
					return x;
				case 1:
					return y;
				case 2:
					return z;
				default:
					throw new IndexOutOfRangeException("Invalid Point3 index");
			}
		}

		set
		{
			switch(index)
			{
				case 0:
					x = value;
					break;
				case 1:
					y = value;
					break;
				case 2:
					z = value;
					break;
				default:
					throw new IndexOutOfRangeException("Invalid Point3 index");
			}
		}
	}

	public static readonly Point3 back = new Point3(0, 0, -1);
	public static readonly Point3 forward = new Point3(0, 0, 1);
	public static readonly Point3 down = new Point3(0, -1, 0);
	public static readonly Point3 up = new Point3(0, 1, 0);
	public static readonly Point3 left= new Point3(-1, 0, 0);
	public static readonly Point3 right = new Point3(1, 0, 0);

	public static readonly Point3 zero = new Point3(0, 0, 0);
	public static readonly Point3 one = new Point3(1, 1, 1);

	public Point3 abs
	{
		get
		{
			return new Point3(Mathi.Abs(x), Mathi.Abs(y), Mathi.Abs(z));
		}
	}

	public int max
	{
		get
		{
			return (x > y) ? ((x > z) ? x : z) : ((y > z) ? y : z);
		}
	}

	public Point3 unit
	{
		get
		{
			return new Point3(Mathi.Signum(x), Mathi.Signum(y), Mathi.Signum(z));
		}
	}

	public float sqrMagnitude
	{
		get
		{
			return (float)x * x + (float)y * y + (float)z * z;
		}
	}

	public float magnitude
	{
		get
		{
			return (float)Math.Sqrt(sqrMagnitude);
		}
	}

	public static Point3 operator +(Point3 p1, Point3 p2)
	{
		return new Point3(p1.x + p2.x, p1.y + p2.y, p1.z + p2.z);
	}

	public static Point3 operator -(Point3 p)
	{
		return new Point3(-p.x, -p.y, -p.z);
	}

	public static Point3 operator -(Point3 p1, Point3 p2)
	{
		return new Point3(p1.x - p2.x, p1.y - p2.y, p1.z - p2.z);
	}

	public static Point3 operator *(int d, Point3 p)
	{
		return new Point3(d * p.x, d * p.y, d * p.z);
	}

	public static Point3 operator *(Point3 p, int d)
	{
		return d * p;
	}

	public static Point3 operator /(Point3 p, int d)
	{
		return new Point3(p.x / d, p.y / d, p.z / d);
	}

	public static bool operator ==(Point3 p1, Point3 p2)
	{
		return (p1.x == p2.x) && (p1.y == p2.y) && (p1.z == p2.z);
	}

	public static bool operator !=(Point3 p1, Point3 p2)
	{
		return (p1.x != p2.x) || (p1.y != p2.y) || (p1.z != p2.z);
	}

	public override bool Equals(object obj)
	{
		return (obj is Point3) ? (this == (Point3)obj) : false;
	}

	public bool Equals(Point3 p)
	{
		return this == p;
	}

	public override int GetHashCode()
	{ // not an optimal hash, but this class has no immutable members
		return 0x081a023e;
	}

	public override string ToString()
	{
		return "[" + x + "," + y + "," + z + "]";
	}

	public static float Distance(Point3 p1, Point3 p2)
	{
		return (p1 - p2).magnitude;
	}

	public float Distance(Point3 p)
	{
		return Distance(this, p);
	}

	public static int Dot(Point3 lhs, Point3 rhs)
	{
		return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z * rhs.z;
	}

	public int Dot(Point3 rhs)
	{
		return Dot(this, rhs);
	}

	public static Point3 Cross(Point3 lhs, Point3 rhs)
	{
		return new Point3(lhs.y * rhs.z - lhs.z * rhs.y, lhs.z * rhs.x - lhs.x * rhs.z, lhs.x * rhs.y - lhs.y * rhs.x);
	}

	public Point3 Cross(Point3 rhs)
	{
		return Cross(this, rhs);
	}

	public bool InBounds(int xFrom, int yFrom, int zFrom, int xTo, int yTo, int zTo)
	{
		return (x >= xFrom) && (x < xTo) && (y >= yFrom) && (y < yTo) && (z >= zFrom) && (z < zTo);
	}

	public bool InBounds(int xTo, int yTo, int zTo)
	{
		return InBounds(0, 0, 0, xTo, yTo, zTo);
	}

	public static explicit operator Point3(Vector3 v)
	{
		return new Point3((int)v.x, (int)v.y, (int)v.z);
	}

	public static implicit operator Vector3(Point3 p)
	{
		return new Vector3(p.x, p.y, p.z);
	}

	public static explicit operator Point2(Point3 p)
	{
		return new Point2(p.x, p.y);
	}

	public static explicit operator Vector2(Point3 p)
	{
		return new Vector2(p.x, p.y);
	}

	public Point3(int x = 0, int y = 0, int z = 0)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Point3(Vector3 v)
	{
		x = (int)v.x;
		y = (int)v.y;
		z = (int)v.z;
	}

	public Point3(Point2 p)
	{
		x = p.x;
		y = p.y;
		z = 0;
	}
}

public static class PointHelpers
{
	public static Point2 Round(this Vector2 v)
	{ // same as the cast, but it rounds instead of truncates. Point2 p = V.Round();
		return new Point2((int)Math.Round(v.x), (int)Math.Round(v.y));
	}

	public static Point3 Round(this Vector3 v)
	{
		return new Point3((int)Math.Round(v.x), (int)Math.Round(v.y), (int)Math.Round(v.z));
	}

	public static T GetValue<T>(this T[,] array, Point2 p)
	{
		return array[p.x, p.y];
	}

	public static void SetValue<T>(this T[,] array, T newValue, Point2 p)
	{
		array[p.x, p.y] = newValue;
	}

	public static Point2 GetSize<T>(this T[,] array)
	{
		return new Point2(array.GetLength(0), array.GetLength(1));
	}

	public static T GetValue<T>(this T[][] array, Point2 p)
	{
		return array[p.x][p.y];
	}

	public static void SetValue<T>(this T[][] array, T newValue, Point2 p)
	{
		array[p.x][p.y] = newValue;
	}

	public static T GetValue<T>(this T[,,] array, Point3 p)
	{
		return array[p.x, p.y, p.z];
	}

	public static void SetValue<T>(this T[,,] array, T newValue, Point3 p)
	{
		array[p.x, p.y, p.z] = newValue;
	}

	public static Point3 GetSize<T>(this T[,,] array)
	{
		return new Point3(array.GetLength(0), array.GetLength(1), array.GetLength(2));
	}

	public static T GetValue<T>(this T[][][] array, Point3 p)
	{
		return array[p.x][p.y][p.z];
	}

	public static void SetValue<T>(this T[][][] array, T newValue, Point3 p)
	{
		array[p.x][p.y][p.z] = newValue;
	}
}

// any new methods we added up there, we'll add to the Unity vectors, just out of convenience
public static class VectorHelpers
{
	public static Point2.Direction RegionInRect(this Vector2 V, Vector2 Size)
	{ // is this point north-ish, south-ish, east-ish or west-ish in a square of the passed size
		bool NW = Vector3.Cross(V, Size).z < 0;
		bool NE = Vector3.Cross(new Vector2(V.x - Size.x, V.y), new Vector2(Size.x * -1, Size.y)).z > 0;

		Point2.Direction RetVal = Point2.Direction.West;

		if(NW && NE)
			RetVal = Point2.Direction.North;
		else if(!NW && NE)
			RetVal = Point2.Direction.East;
		else if(!NW && !NE)
			RetVal = Point2.Direction.South;

		return RetVal;
	}
}
