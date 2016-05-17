using UnityEngine;
using System.Collections;

public class CameraDropdownUIHandler : MonoBehaviour {
	/*
	[System.Serializable]
	public class DisplayBinding {
		public string displayName;



		public int displayIndex;

		//public SetupCanvasController.ComputerConfig
	}
	*/
	public SetupCanvasController setupCanvasController;

	public SetupCanvasController.DisplayConfig screen;

	//public DisplayBinding displayBinding;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendNewDisplay(int displayIndex) {
		setupCanvasController.SetDisplay (screen, displayIndex);
	}
}
