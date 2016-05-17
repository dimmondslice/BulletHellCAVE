#define DEBUGS_ENABLED

//uncomment to disable debug messages (optimization)
//#undef DEBUGS_ENABLED

using UnityEngine;

//This script is used to have objects follow the supplied position and rotation from a tracker object
//if the tracker object supplied does not exist there will be a warning thrown
//ER_SyncPosition should be checked from the editor if you want the transform on this game object to set its position every frame
//based off a tracker object
//ER_SyncRotation should be checked from the editor if you want the transform on this game object to set its rotation every frmae
//based off a tracker object
//

public class SyncWithTrackerObj : MonoBehaviour 
{
	public string ER_syncedObjName;
	public bool ER_syncPosition = true;
	public bool ER_syncRotation = false;

	private Tracker trackerRef;
	void Start()
	{
		trackerRef = GameObject.FindObjectOfType<Tracker>();
	}

	void Update()
	{
        ProcessTracker();	
    }

    private void ProcessTracker() {

        if (!trackerRef.objs.ContainsKey(ER_syncedObjName)) {
#if DEBUGS_ENABLED
            Debug.LogWarningFormat("Supplied object is not registered: {0}", ER_syncedObjName);
#endif
            return;
        }

        //sync the transform.position with the desired tracker obj
        if (ER_syncPosition) {
            transform.position = new Vector3(trackerRef.objs[ER_syncedObjName].x,
                                              trackerRef.objs[ER_syncedObjName].y,
                                              trackerRef.objs[ER_syncedObjName].z);
        }
        //sync the transform.rotation with the desired tracker obj
        if (ER_syncRotation) {
            transform.rotation = Quaternion.Euler(trackerRef.objs[ER_syncedObjName].p,
                                                    trackerRef.objs[ER_syncedObjName].h,
                                                    trackerRef.objs[ER_syncedObjName].r);
        }
    }
}
