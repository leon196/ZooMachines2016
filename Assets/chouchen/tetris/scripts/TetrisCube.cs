using UnityEngine;

public class TetrisCube : MonoBehaviour
{	
	private int					_fill		= 0;
	private TetrisColorPalette	_palette	= null;
	public Osciyo osciyo;
	public int[] osciyoColorIndices;
	private Material mat;

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
		if (mat == null) {
			mat = GetComponent<Renderer> ().material;
		}
		mat.color = _palette.GetShapeColor (_fill);
		if (osciyo != null) {
			osciyo.SetColor(osciyoColorIndices, mat.color);
		}
	}
}
