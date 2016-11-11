public class Coord
{
	public int x = 0;
	public int y = 0;

	public Coord ()
	{
		this.x = 0;
		this.y = 0;
	}

	public Coord (int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override string ToString ()
	{
		return string.Format ("{0}/{1}",x,y);
	}
}
