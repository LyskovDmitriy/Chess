using System;

[Serializable]
public struct Coordinates
{
	
	public int x;
	public int y;


	public Coordinates (int X, int Y)
	{
		x = X;
		y = Y;
	}
		
	//Direction's x and y values must have the same length or one of them must be equal to 0
	public Coordinates NormalizedDirection
	{
		get{
			if (x == 0 && y == 0)
			{
				return this;
			}
			else if (x == 0)
			{
				return (0 < y) ? Up : Down;
			}
			else if (y == 0)
			{
				return (0 < x) ? Right : Left;
			}
			else if (Math.Abs(x) != Math.Abs(y)) //invalid coordinates
			{
				return Zero;
			}
			else if (x < 0 && 0 < y)
			{
				return LeftUp;
			}
			else if (0 < x && 0 < y)
			{
				return RightUp;
			}
			else if (0 < x && y < 0)
			{
				return RightDown;
			}
			else if (x < 0 && y < 0)
			{
				return LeftDown;
			}
			else //didn't mathch any conditions (should never happen)
			{
				return Zero;
			}
		}
	}

	//Shorthand for writing new Coordinates(-1, 0)
	public static Coordinates Left { get { return new Coordinates(-1, 0); } }

	//Shorthand for writing new Coordinates(-1, 1)
	public static Coordinates LeftUp { get { return new Coordinates(-1, 1); } }

	//Shorthand for writing new Coordinates(0, 1)
	public static Coordinates Up { get { return new Coordinates(0, 1); } }

	//Shorthand for writing new Coordinates(1, 1)
	public static Coordinates RightUp { get { return new Coordinates(1, 1); } }

	//Shorthand for writing new Coordinates(1, 0)
	public static Coordinates Right { get { return new Coordinates(1, 0); } }

	//Shorthand for writing new Coordinates(1, -1)
	public static Coordinates RightDown { get { return new Coordinates(1, -1); } }

	//Shorthand for writing new Coordinates(0, -1)
	public static Coordinates Down { get { return new Coordinates(0, -1); } }

	//Shorthand for writing new Coordinates(-1, -1)
	public static Coordinates LeftDown { get { return new Coordinates(-1, -1); } }

	//Shorthand for writing new Coordinates(0, 0)
	public static Coordinates Zero { get { return new Coordinates(0, 0); } }


	public static Coordinates operator +(Coordinates first, Coordinates second)
	{
		return new Coordinates(first.x + second.x, first.y + second.y);
	}


	public static Coordinates operator -(Coordinates first, Coordinates second)
	{
		return new Coordinates(first.x - second.x, first.y - second.y);
	}


	public static Coordinates operator -(Coordinates coord)
	{
		return new Coordinates(-coord.x, -coord.y);
	}


	public static Coordinates operator *(Coordinates coord, int multiplier)
	{
		return new Coordinates(coord.x * multiplier, coord.y * multiplier);
	}


	public static bool operator ==(Coordinates first, Coordinates second)
	{
		return ((first.x == second.x) && (first.y == second.y));
	}


	public static bool operator !=(Coordinates first, Coordinates second)
	{
		return ((first.x != second.x) || (first.y != second.y));
	}


	public override string ToString()
	{
		return string.Format("({0}, {1})", x, y);
	}
}
