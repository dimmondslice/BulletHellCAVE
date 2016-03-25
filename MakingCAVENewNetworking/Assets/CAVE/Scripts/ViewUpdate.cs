using UnityEngine;
using System.Collections;


public class ViewUpdate : MonoBehaviour {
    public Tracker ER_Tracker;

	//public Vector3 trackerPosition = new Vector3(0f,1.5f,0f);
	public Transform trackerTransform;
	public bool stereo = false;
	public float interpupillaryDistance = 0.1f;

	bool menuToggle = false;
	private string ip = "129.161.12.147";
	public int port = 12345;
	
	public enum viewType{center, left, right};
	public viewType whichCameraToViewFrom = viewType.center;
	
	void Start()
    {
        

        if (ER_Tracker == null) {
            ER_Tracker = FindObjectOfType<Tracker>();
        }
    }

    void Update()
    {
        trackerTransform.localPosition = new Vector3(ER_Tracker.objs["Visor"].x, ER_Tracker.objs["Visor"].y, ER_Tracker.objs["Visor"].z);
        
        if (Input.GetKeyDown(KeyCode.M)){
			if (menuToggle){
				menuToggle = false;
			}
			else {
				menuToggle = true;

			}
		}
	}
	
	public void OnGUI(){
		if (menuToggle){
			ip = GUI.TextField(new Rect(20, 60, 150, 20), ip, 15);
			if (GUI.Button(new Rect(105,10,150,20), "Start server as center")){
				//Debug.Log("Not actually starting server yet");
				Debug.Log ("SET CENTER TO TRUE");
				Settings.position = Settings.Position.Center;
				transform.Find("Main Camera-centerL").gameObject.SetActive(true);
				transform.Find("Main Camera-centerR").gameObject.SetActive(true);
				transform.Find("Main Camera-leftL").gameObject.SetActive(false);
				transform.Find("Main Camera-leftR").gameObject.SetActive(false);
				transform.Find("Main Camera-rightL").gameObject.SetActive(false);
				transform.Find("Main Camera-rightR").gameObject.SetActive(false);
				whichCameraToViewFrom = viewType.center;
                //Settings.HideAndLockCursor();
                //Network.InitializeServer(4, 12345, false);
                menuToggle = false;
			}
			if (GUI.Button(new Rect(180, 50, 150, 20), "Listen as left")){
				//Debug.Log("Not left yet");
				Settings.position = Settings.Position.Left;
				transform.Find("Main Camera-centerL").gameObject.SetActive(false);
				transform.Find("Main Camera-centerR").gameObject.SetActive(false);
				transform.Find("Main Camera-leftL").gameObject.SetActive(true);
				transform.Find("Main Camera-leftR").gameObject.SetActive(true);
				transform.Find("Main Camera-rightL").gameObject.SetActive(false);
				transform.Find("Main Camera-rightR").gameObject.SetActive(false);
				whichCameraToViewFrom = viewType.left;
                //Settings.HideAndLockCursor();
                //Network.Connect(ip, port);
                menuToggle = false;
			}
			if (GUI.Button(new Rect(180,75,150,20), "Listen as right")){
				//Debug.Log("Not right yet");
				Settings.position = Settings.Position.Right;
				transform.Find("Main Camera-centerL").gameObject.SetActive(false);
				transform.Find("Main Camera-centerR").gameObject.SetActive(false);
				transform.Find("Main Camera-leftL").gameObject.SetActive(false);
				transform.Find("Main Camera-leftR").gameObject.SetActive(false);
				transform.Find("Main Camera-rightL").gameObject.SetActive(true);
				transform.Find("Main Camera-rightR").gameObject.SetActive(true);
				whichCameraToViewFrom = viewType.right;
                menuToggle = false;
				//Settings.HideAndLockCursor();
				//Network.Connect(ip, port);
			}
		}
	}
}
