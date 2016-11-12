using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TetrisTutorial : MonoBehaviour 
{	
	[SerializeField]
	private GameObject _midiControllerPrefab	= null;
	[SerializeField]
	private Transform  _midiControllerSpawn		= null;

	[SerializeField]
	private ShapeDecoration	_playerOneShapeT	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeI	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeL	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeJ	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeS	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeZ	= null;
	[SerializeField]
	private ShapeDecoration	_playerOneShapeO	= null;

	[SerializeField]
	private ShapeDecoration	_playerTwoShapeT	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeI	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeL	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeJ	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeS	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeZ	= null;
	[SerializeField]
	private ShapeDecoration	_playerTwoShapeO	= null;
	[SerializeField]
	private Text			_movementText 		= null;
	[SerializeField]
	private Text			_rotationText 		= null;
	[SerializeField]
	private Text			_fallText			= null;
	[SerializeField]
	private Text			_outSyncText 		= null;

	private MidiController	_midiController		= null;

	public bool isinit { get; set; }

	public void Awake ()
	{
		_movementText.gameObject.SetActive (false);
		_rotationText.gameObject.SetActive (false);
		_fallText.gameObject.SetActive (false);
	}

	// Use this for initialization
	public void ConstructMidiController () 
	{
		GameObject midiControllerGo				 = GameObject.Instantiate (_midiControllerPrefab);
		midiControllerGo.transform.parent 		 = _midiControllerSpawn;
		midiControllerGo.transform.localPosition = Vector3.zero;
		midiControllerGo.transform.localRotation = Quaternion.identity;

		_midiController = midiControllerGo.AddComponent<MidiController> ();
	}

	public void ConstructTetrisShape (PlayerID playerId, TetrisShapeEnum shapeEnum, TetrisCube cube, TetrisColorPalette palette)
	{
		if (playerId == PlayerID.PlayerOne) {
			if (shapeEnum == TetrisShapeEnum.ShapeI)
				_playerOneShapeI.Constuct (new TetrisShapeI (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeJ)
				_playerOneShapeJ.Constuct (new TetrisShapeJ (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeL)
				_playerOneShapeL.Constuct (new TetrisShapeL (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeO)
				_playerOneShapeO.Constuct (new TetrisShapeO (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeS)
				_playerOneShapeS.Constuct (new TetrisShapeS (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeT)
				_playerOneShapeT.Constuct (new TetrisShapeT (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeZ)
				_playerOneShapeZ.Constuct (new TetrisShapeZ (), cube, palette);
		} else if (playerId == PlayerID.PlayerTwo) {
			if (shapeEnum == TetrisShapeEnum.ShapeI)
				_playerTwoShapeI.Constuct (new TetrisShapeI (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeJ)
				_playerTwoShapeJ.Constuct (new TetrisShapeJ (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeL)
				_playerTwoShapeL.Constuct (new TetrisShapeL (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeO)
				_playerTwoShapeO.Constuct (new TetrisShapeO (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeS)
				_playerTwoShapeS.Constuct (new TetrisShapeS (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeT)
				_playerTwoShapeT.Constuct (new TetrisShapeT (), cube, palette);
			else if (shapeEnum == TetrisShapeEnum.ShapeZ)
				_playerTwoShapeZ.Constuct (new TetrisShapeZ (), cube, palette);			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LaunchTutorial ()
	{
		StartCoroutine (PlayTutorial());
	}

	IEnumerator PlayTutorial ()
	{
		_rotationText.gameObject.SetActive (false);
		_movementText.gameObject.SetActive (false);
		_fallText.gameObject.SetActive (false);
		_outSyncText.gameObject.SetActive (false);

		yield return null;
		_rotationText.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		_rotationText.gameObject.SetActive (false);
		_movementText.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		_movementText.gameObject.SetActive (false);
		_fallText.gameObject.SetActive (true);
		yield return new WaitForSeconds (2f);
		_fallText.gameObject.SetActive (false);

		GameObject.FindObjectOfType<TetrisGame> ().TutorialIsOver ();
	}

	public void LaunchSpecialTutorial ()
	{
		StartCoroutine (PlaySpecialTutorial ());
	}

	IEnumerator PlaySpecialTutorial ()
	{
		_rotationText.gameObject.SetActive (false);
		_movementText.gameObject.SetActive (false);
		_fallText.gameObject.SetActive (false);
		_outSyncText.gameObject.SetActive (true);
		yield return new WaitForSeconds (4f);

		GameObject.FindObjectOfType<TetrisGame> ().SpecialTutorialIsOver ();
	}
}
