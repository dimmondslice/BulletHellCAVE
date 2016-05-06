using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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
		public DisplayConfig config;

		public Dropdown dropdown;
	}

	[System.Serializable]
	public class CameraDisplayManager {

		public Camera terminalCam;

		public ComputerConfig curComputerConfig;

		public List<CameraCluster> camClusterList;
		public Dictionary<ComputerConfig, CameraCluster> camClusterDict;

		public List<ScreenDropdown> screenDropdownList;
		public Dictionary<DisplayConfig, ScreenDropdown> screenDropdownDict;

		public void Init() {
			//create cam cluster lookup table
			camClusterDict = new Dictionary<ComputerConfig, CameraCluster> ();
			foreach (CameraCluster cluster in camClusterList) {
				camClusterDict.Add (cluster.config, cluster);
			}

			//create screen configuration lookup table
			screenDropdownDict = new Dictionary<DisplayConfig, ScreenDropdown> ();
			foreach(ScreenDropdown dropdown in screenDropdownList) {
				screenDropdownDict.Add(dropdown.config, dropdown);
			}

			//initialize UI elements
			camClusterDict [curComputerConfig].toggleButton.isOn = true;

			screenDropdownDict [DisplayConfig.Terminal].dropdown.value = terminalCam.targetDisplay;
			screenDropdownDict [DisplayConfig.LeftCam].dropdown.value = camClusterDict [curComputerConfig].leftCam.targetDisplay;
			screenDropdownDict [DisplayConfig.RightCam].dropdown.value = camClusterDict [curComputerConfig].rightCam.targetDisplay;

			/*foreach (ScreenConfig config in screenDropdownDict.Keys) {
				screenDropdownDict[key].dropdown.value = camClusterDict[curComputerConfig].	
			}*/
			/*
			foreach (KeyValuePair<ScreenConfig, ScreenDropdown> kvp in screenDropdownDict) {
				
			}
			*/
		}

        public void SetComputer(ComputerConfig newConfig) {

            //Debug.Log (newConfig);
            if (newConfig == curComputerConfig) { return; }

            //disable the last Cam Cluster we were on
            camClusterDict[curComputerConfig].enabled = false;

            //switch tracking to using the new Cam Cluster
            curComputerConfig = newConfig;

            //Actually enable the new Cam Cluster
            camClusterDict[newConfig].enabled = true;


        }

        public void SetDisplay(DisplayConfig screen, int displayIndex) {
            switch (screen) {
                case DisplayConfig.Terminal:
                    terminalCam.targetDisplay = displayIndex;
                    //setupCanvas.targetDisplay = displayIndex;
                    break;

                case DisplayConfig.LeftCam:
                    camClusterDict[curComputerConfig].leftCam.targetDisplay = displayIndex;
                    break;
                case DisplayConfig.RightCam:
                    camClusterDict[curComputerConfig].rightCam.targetDisplay = displayIndex;
                    break;

                default:
                    goto case DisplayConfig.Terminal;
            }


        }
    }

	public enum ComputerConfig {
		Middle = 0,
		Left = 1,
		Right = 2,
	}

	public enum DisplayConfig {
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

	void Start () {
		
	}
	
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
	
	public void SetComputer(ComputerConfig newConfig) {

        int terminalIndex = displayManager.terminalCam.targetDisplay;
        int leftCamIndex = displayManager.camClusterDict[displayManager.curComputerConfig].leftCam.targetDisplay;
        int rightCamIndex = displayManager.camClusterDict[displayManager.curComputerConfig].rightCam.targetDisplay;

        displayManager.SetComputer(newConfig);

        displayManager.terminalCam.targetDisplay = terminalIndex;
        displayManager.camClusterDict[displayManager.curComputerConfig].leftCam.targetDisplay = leftCamIndex;
        displayManager.camClusterDict[displayManager.curComputerConfig].rightCam.targetDisplay = rightCamIndex;
    }


	public void SetDisplay(DisplayConfig screen, int displayIndex) {
        displayManager.SetDisplay(screen, displayIndex);
	}

}
