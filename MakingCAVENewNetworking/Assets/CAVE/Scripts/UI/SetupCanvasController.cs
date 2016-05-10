#define DEBUGS_ENABLED

//uncomment to disable debugs
//#undef DEBUGS_ENABLED

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
//using System.Linq;

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

		public int GetIndexOfDisplayConfig(DisplayConfig display) {

			switch (display) {
				case DisplayConfig.Terminal:
					return terminalCam.targetDisplay;

				case DisplayConfig.LeftCam:
					return camClusterDict[curComputerConfig].leftCam.targetDisplay;

				case DisplayConfig.RightCam:
					return camClusterDict[curComputerConfig].rightCam.targetDisplay;

				default: goto case DisplayConfig.Terminal;
			}
		}

		public DisplayConfig GetDisplayConfigFromIndex(int displayIndex) {
			//CHECK TERMINAL CAM
			if (terminalCam.targetDisplay == displayIndex) {
				return DisplayConfig.Terminal;
			}

			//CHECK LEFT CAM
			else if (camClusterDict[curComputerConfig].leftCam.targetDisplay == displayIndex) {
				return DisplayConfig.LeftCam;
			}

			//CHECK RIGHT CAM
			else if (camClusterDict[curComputerConfig].rightCam.targetDisplay == displayIndex) {
				return DisplayConfig.RightCam;
			}

			//else {
			#if DEBUGS_ENABLED
			Debug.LogErrorFormat("Supplied display index of: {0} was not acceptable, returning the Terminal config as default", displayIndex);
			#endif

			return DisplayConfig.Terminal;
			//}
		}

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

            AssignDropdownUIValues();

			/*foreach (ScreenConfig config in screenDropdownDict.Keys) {
				screenDropdownDict[key].dropdown.value = camClusterDict[curComputerConfig].	
			}*/
			/*
			foreach (KeyValuePair<ScreenConfig, ScreenDropdown> kvp in screenDropdownDict) {
				
			}
			*/
		}

        public void AssignDropdownUIValues() {
            screenDropdownDict[DisplayConfig.Terminal].dropdown.value = terminalCam.targetDisplay;
            screenDropdownDict[DisplayConfig.LeftCam].dropdown.value = camClusterDict[curComputerConfig].leftCam.targetDisplay;
            screenDropdownDict[DisplayConfig.RightCam].dropdown.value = camClusterDict[curComputerConfig].rightCam.targetDisplay;
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

			//int prevDisplayIndex

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

		//int displayToSwapIndex = displayManager.GetDisplayConfigFromIndex (displayIndex);

		//grab the DisplayConfig of the destination display
		DisplayConfig displayToSwapConfig = displayManager.GetDisplayConfigFromIndex ((displayIndex));
		//grab the current Display Index of the display we're moving, before we move it
		int oldDisplayConfig = displayManager.GetIndexOfDisplayConfig (screen);

		if (screen == DisplayConfig.Terminal) {

			//displayManager.GetIndexOfDisplayConfig (DisplayConfig.Terminal);

			//setupCanvas.targetDisplay = displayIndex;
			//setupCanvas.worldCamera = 
			//setupCanvas.worldCamera = displayManager.terminalCam;
		}

        displayManager.SetDisplay(screen, displayIndex);
		displayManager.SetDisplay (displayToSwapConfig, oldDisplayConfig);



        setupCanvas.targetDisplay = displayManager.terminalCam.targetDisplay;
        setupCanvas.worldCamera = displayManager.terminalCam;

        displayManager.AssignDropdownUIValues();

    }

}
