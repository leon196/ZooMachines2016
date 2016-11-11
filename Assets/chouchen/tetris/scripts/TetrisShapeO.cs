using System.Collections.Generic;

public class TetrisShapeO : TetrisShape
{
	public TetrisShapeO()
	{
		_p1RotationKey = (int)0x34;
		_p2RotationKey = (int)0x45;

		_shapeEnum 	   = TetrisShapeEnum.ShapeO;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 1, 1 },
				new int[]{ 1, 1 },
			},

			new int[][] {
				new int[]{ 1, 1 },
				new int[]{ 1, 1 },
			}
		};

		SetColor ();
	}
}

