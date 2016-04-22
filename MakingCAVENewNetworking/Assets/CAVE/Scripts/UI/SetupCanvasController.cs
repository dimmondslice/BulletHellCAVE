using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetupCanvasController : MonoBehaviour {

	[System.Serializable]
	public class CameraCluster {


		public ComputerConfig config;

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

	[System.Serializable]
	public class ScreenDropdown {
		public ScreenConfig config;

		public Dropdown dropdown;
	}

	[System.Serializable]
	public class CameraDisplayManager {

		public Camera terminalCam;

		public ComputerConfig curComputerConfig;

		public List<CameraCluster> camClusterList;
		public Dictionary<ComputerConfig, CameraCluster> camClusterDict;

		public List<ScreenDropdown> screenDropdownList;
		public Dictionary<ScreenConfig, ScreenDropdown> screenDropdownDict;

		public void Init() {
			//create cam cluster lookup table
			camClusterDict = new Dictionary<ComputerConfig, CameraCluster> ();
			foreach (CameraCluster cluster in camClusterList) {
				camClusterDict.Add (cluster.config, cluster);
			}

			//create screen configuration lookup table
			screenDropdownDict = new Dictionary<ScreenConfig, ScreenDropdown> ();
			foreach(ScreenDropdown dropdown in screenDropdownList) {
				screenDropdownDict.Add(dropdown.config, dropdown);
			}

			//initialize UI elements
			camClusterDict [curComputerConfig].toggleButton.isOn = true;

			screenDropdownDict [ScreenConfig.Terminal].dropdown.value = terminalCam.targetDisplay;
			screenDropdownDict [ScreenConfig.LeftCam].dropdown.value = camClusterDict [curComputerConfig].leftCam.targetDisplay;
			screenDropdownDict [ScreenConfig.RightCam].dropdown.value = camClusterDict [curComputerConfig].rightCam.targetDisplay;

			/*foreach (ScreenConfig config in screenDropdownDict.Keys) {
				screenDropdownDict[key].dropdown.value = camClusterDict[curComputerConfig].	
			}*/
			/*
			foreach (KeyValuePair<ScreenConfig, ScreenDropdown> kvp in screenDropdownDict) {
				
			}
			*/
		}
	}

	public enum ComputerConfig {
		Middle = 0,
		Left = 1,
		Right = 2,
	}

	public enum ScreenConfig {
		Terminal = 0,
		LeftCam = 1,
		RightCam = 2,
	}

	private Canvas setupCanvas;

	public KeyCode enableUIKey = KeyCode.F1;
	public GameObject setupUI;
	public bool startActive = false;


	public CameraDisplayManager displayManager;


	void Awake() {
		setupCanvas = GetComponent<Canvas> ();

		setupUI.SetActive (startActive);

		displayManager.Init ();
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
	//Toggle the Configuration UI
		if (Input.GetKeyDown(enableUIKey)) {
			setupUI.SetActive (!setupUI.activeInHierarchy);
		}
	}
	/*
	public void HandleIsEnabled () {
		
	}
	*/
	public void SetComputer(ComputerConfig newConfig) {

		//Debug.Log (newConfig);
		if (newConfig == displayManager.curComputerConfig) {return;}

		displayManager.camClusterDict [displayManager.curComputerConfig].enabled = false;

		displayManager.curComputerConfig = newConfig;

		displayManager.camClusterDict [newConfig].enabled = true;
	}


	public void SetDisplay(ScreenConfig screen, int displayIndex) {
		switch (screen) {
		case ScreenConfig.Terminal:
			displayManager.terminalCam.targetDisplay = displayIndex;
			//setupCanvas.targetDisplay = displayIndex;
			break;

		case ScreenConfig.LeftCam:
			displayManager.camClusterDict [displayManager.curComputerConfig].leftCam.targetDisplay = displayIndex;
			break;
		case  ScreenConfig.RightCam:
			displayManager.camClusterDict [displayManager.curComputerConfig].rightCam.targetDisplay = displayIndex;
			break;

		default:
			goto case ScreenConfig.Terminal;
		}

 
	}

}
