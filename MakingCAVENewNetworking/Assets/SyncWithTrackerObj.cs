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
		//sync the transform.position with the desired tracker obj
		if(ER_syncPosition)
		{
			transform.position = new Vector3(trackerRef.objs[ER_syncedObjName].x,
											  trackerRef.objs[ER_syncedObjName].y,
											  trackerRef.objs[ER_syncedObjName].z);
        }
		//sync the transform.rotation with the desired tracker obj
		if (ER_syncRotation)
		{
			transform.rotation = Quaternion.Euler(trackerRef.objs[ER_syncedObjName].p,
													trackerRef.objs[ER_syncedObjName].h,
													trackerRef.objs[ER_syncedObjName].r);
		}
    }
}
