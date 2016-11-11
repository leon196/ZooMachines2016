using UnityEngine;

public class TetrisCube : MonoBehaviour
{	
	private int					_fill		= 0;
	private TetrisColorPalette	_palette	= null;

	public int fill
	{
		get { return _fill; }
		set 
		{
			if (_fill != value) {
				_fill = value;
			}
			UpdateColor ();
		}
	}

	public void SetPalette (TetrisColorPalette palette)
	{
		_palette = palette;
		UpdateColor ();
	}

	private void UpdateColor ()
	{
		Material mat = GetComponent<Renderer> ().material;
		mat.color = _palette.GetShapeColor (_fill);
	}
}
