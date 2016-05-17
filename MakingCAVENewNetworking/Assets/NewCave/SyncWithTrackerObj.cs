#define DEBUGS_ENABLED

//uncomment to disable debug messages (optimization)
//#undef DEBUGS_ENABLED

using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(NetworkIdentity))]
//[RequireComponent(typeof(NetworkTransform))]
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
