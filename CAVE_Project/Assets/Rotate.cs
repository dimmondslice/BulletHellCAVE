using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	
	// Update is called once per frame
    public Vector3 degreesPerSecond;
	void Update () 
	{
        transform.Rotate(degreesPerSecond * Time.deltaTime);
	}
}
