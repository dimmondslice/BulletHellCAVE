using UnityEngine;
using System.Collections;

public class moveshit : MonoBehaviour {

	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newForwardVel = Input.GetAxis("Vertical") * transform.forward;
		Vector3 newRightVel = Input.GetAxis("Horizontal") * transform.right;

		transform.Translate(newForwardVel + newRightVel);
	}
}
