using System.Collections.Generic;

public class TetrisShapeI : TetrisShape
{
	public TetrisShapeI()
	{
		_p1RotationKey = (int)0x32;
		_p2RotationKey = (int)0x47;

		_shapeEnum 		= TetrisShapeEnum.ShapeI;

		_rotations = new List<int[][]> () {
			new int[][] {
				new int[]{ 0, 1, 0, 0 },
				new int[]{ 0, 1, 0, 0 },
				new int[]{ 0, 1, 0, 0 },
				new int[]{ 0, 1, 0, 0 }
			},

			new int [][] {
				new int[]{ 0, 0, 0, 0 },
				new int[]{ 0, 0, 0, 0 },
				new int[]{ 1, 1, 1, 1 },
				new int[]{ 0, 0, 0, 0 }
			},

			new int[][] {
				new int[]{ 0, 0, 1, 0 },
				new int[]{ 0, 0, 1, 0 },
				new int[]{ 0, 0, 1, 0 },
				new int[]{ 0, 0, 1, 0 }
			},

			new int [][] {
				new int[]{ 0, 0, 0, 0 },
				new int[]{ 0, 0, 0, 0 },
				new int[]{ 1, 1, 1, 1 },
				new int[]{ 0, 0, 0, 0 }
			}
		};

		SetColor ();
	}
}

