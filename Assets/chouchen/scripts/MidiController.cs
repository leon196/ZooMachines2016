using UnityEngine;
using System.Collections.Generic;

public class MidiController : MonoBehaviour 
{
	private Dictionary<int, MeshRenderer> _midiKeys				=	new Dictionary<int, MeshRenderer>();
	private Dictionary<int, MeshRenderer> _midiKnobsRotations	=	new Dictionary<int, MeshRenderer>();
	private Dictionary<int, MeshRenderer> _midiKnobSliders		=	new Dictionary<int, MeshRenderer>();

	// Use this for initialization
	void Awake () 
	{	
		int childCount = transform.childCount;

		for (int i = 0; i < childCount; i++) 
		{
			MeshRenderer child = transform.GetChild (i).GetComponent<MeshRenderer> ();

			switch (child.name) 
			{
				//Player One - Keyboard touch
				case "Cube.001": _midiKeys [(int)0x30] = child; 	break;
				case "Cube.092": _midiKeys [(int)0x32] = child; 	break;
				case "Cube.091": _midiKeys [(int)0x34] = child; 	break;
				case "Cube.090": _midiKeys [(int)0x35] = child; 	break;
				case "Cube.089": _midiKeys [(int)0x37] = child; 	break;
				case "Cube.088": _midiKeys [(int)0x39] = child; 	break;
				case "Cube.087": _midiKeys [(int)0x3B] = child; 	break;
				//Player One - Force Fall touch
				case "Cube.036": _midiKeys [(int)0x18] = child; 	break;
				//Player One - Move Block Horizontally
				case "Cube.068": _midiKnobsRotations [(int)0x39] = child; 	break;


				//Player Two - Keyboard touch
				case "Cube.079": _midiKeys [(int)0x48] = child; 	break;
				case "Cube.080": _midiKeys [(int)0x47] = child; 	break;
				case "Cube.081": _midiKeys [(int)0x45] = child; 	break;
				case "Cube.082": _midiKeys [(int)0x43] = child; 	break;
				case "Cube.083": _midiKeys [(int)0x41] = child; 	break;
				case "Cube.084": _midiKeys [(int)0x40] = child; 	break;
				case "Cube.085": _midiKeys [(int)0x3F] = child; 	break;
					//Player One - Force Fall touch
				case "Cube.029": _midiKeys [(int)0x1F] = child; 	break;
					//Player One - Move Block Horizontally
				case "Cube.061": _midiKnobsRotations [(int)0x39] = child; 	break;
			}
		}
	}
}
