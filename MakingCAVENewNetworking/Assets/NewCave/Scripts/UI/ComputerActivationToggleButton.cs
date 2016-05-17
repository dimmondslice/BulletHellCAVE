using UnityEngine;
using System.Collections;

public class ComputerActivationToggleButton : MonoBehaviour {

	public SetupCanvasController setupCanvasControl;

	public SetupCanvasController.ComputerConfig buttonConfig;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SendConfigModeToControl(bool setting) {
		if (setting == true) {
			setupCanvasControl.SetComputer (buttonConfig);
		}	
	}
}
