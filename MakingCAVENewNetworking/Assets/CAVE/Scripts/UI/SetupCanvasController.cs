using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetupCanvasController : MonoBehaviour {

	[System.Serializable]
	public class CameraCluster {

		public Toggle toggleButton;

		public Camera leftCam;
		public Camera rightCam;

		public bool enabled{
			get {
				if (leftCam.enabled == rightCam.enabled) {// both cams are the same, we can return the setting of either one
					return leftCam.enabled;
				}
				else {
					Debug.LogWarningFormat ("Left Cam {0} and Right Cam {1} do not share the same setting. Returning false as a failsafe", leftCam.name, rightCam.name);
					return false;
				}
			}

			set {
				toggleButton.isOn = value;

				leftCam.enabled = value;
				rightCam.enabled = value;
			}
		}
	}


	public enum ComputerConfig {
		Middle = 0,
		Left = 1,
		Right = 2,
	}
	/*
	public enum ScreenConfig {
		Middle = 0,
		Left = 1,
		Right = 2,
	}
	*/

	public KeyCode enableUIKey = KeyCode.F1;
	public GameObject setupUI;
	public bool startActive = false;

	public CameraCluster leftCamCluster;
	public CameraCluster middleCamCluster;
	public CameraCluster rightCamCluster;


	public ComputerConfig curComputerConfig;
	//public ScreenConfig curScreenConfig;


	void Awake() {
		setupUI.SetActive (startActive);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput();
	}

	public void HandleInput() {
		//HandleIsEnabled ();
		if (Input.GetKeyDown(enableUIKey)) {
			setupUI.SetActive (!setupUI.activeInHierarchy);
		}
	}
	/*
	public void HandleIsEnabled () {
		
	}
	*/
	public void SetComputer(ComputerConfig newConfig) {

		Debug.Log (newConfig);

		switch (newConfig) {
		case ComputerConfig.Left:
				leftCamCluster.enabled = true;

				middleCamCluster.enabled = false;
				rightCamCluster.enabled = false;
				break;

			case ComputerConfig.Middle:
				middleCamCluster.enabled = true;

				leftCamCluster.enabled = false;
				rightCamCluster.enabled = false;
				break;

			case ComputerConfig.Right:
				rightCamCluster.enabled = true;

				leftCamCluster.enabled = false;
				middleCamCluster.enabled = false;
				break;

			default:
				goto case ComputerConfig.Middle;
		}
	}

	/*
	public void SetComputer_Middle() {
		SetComputer (ComputerConfig.Middle);
	}
	public void SetComputer_Left() {
		SetComputer (ComputerConfig.Left);
	}
	public void SetComputer_Right() {
		SetComputer (ComputerConfig.Right);
	}
	*/
}
