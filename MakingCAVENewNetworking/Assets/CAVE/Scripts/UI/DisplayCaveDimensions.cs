//#if DEBUGS_ENABLED

//#undef DEBUGS_ENABLED

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DisplayCaveDimensions : MonoBehaviour {

	public Transform left_TopLeft;
	public Transform left_BottomLeft;
	public Transform left_TopRight;
	public Transform left_BottomRight;

	public Transform middle_TopLeft;
	public Transform middle_BottomLeft;
	public Transform middle_TopRight;
	public Transform middle_BottomRight;

	public Transform right_TopLeft;
	public Transform right_BottomLeft;
	public Transform right_TopRight;
	public Transform right_BottomRight;


	public bool displayCaveBounds = false;

	public Color leftWallOutlineColor = Color.blue;
	public Color leftWallColor;

	public Color middleWallOutlineColor = Color.grey;
	public Color middleWallColor;

	public Color rightWallOutlineColor = Color.red;
	public Color rightWallColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//**************COME BACK AND FINISH IMPLEMENTING CAVE BONDS BASED ON WHAT'S IN THE INSPECTOR GUI**********
	void OnGizmos() {
		Color originalGizmoColor = Gizmos.color;

		Gizmos.color = leftWallColor;
		Gizmos.DrawCube (CalcAvgPos (left_TopLeft.position,
									 left_TopRight.position,
									 left_BottomLeft.position,
									 left_BottomRight.position),
			new Vector3(0.2f,
						Mathf.Abs ((left_TopLeft.position.y - left_BottomLeft.position.z) / 2.0f),
						Mathf.Abs (left_TopLeft.position.z - left_TopRight.position.z) / 2.0f));

		Gizmos.color = middleWallColor;


		Gizmos.color = rightWallColor;


		Gizmos.color = originalGizmoColor;
	}

	Vector3 CalcAvgPos(params Vector3[] positions) {
		Vector3 avgPos = new Vector3(0.0f, 0.0f, 0.0f);

		foreach (Vector3 vec in positions) {
			avgPos += vec;
		}

		avgPos /= positions.Length;

		return avgPos;
	}
	/*
	Vector3 CalcCubeSize(Vector3 topLeft, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight) {
		//Vector3 middleLeft = CalcAvgPos (topLeft, bottomLeft);
		//Vector3 middleRight = CalcAvgPos (topRight, bottomRight);



	}
	*/
}

#if UNITY_EDITOR

[CustomEditor(typeof(DisplayCaveDimensions))]
public class DisplayCaveDimensions_Editor : Editor {

	private DisplayCaveDimensions selfScript;

	void OnEnable() {
		selfScript = (DisplayCaveDimensions)target;

	}


	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		//selfScript.displayCaveBounds = GUILayout.Toggle (selfScript.displayCaveBounds, "Display CAVE Bounds");
		if (GUILayout.Button ("DisplayCaveBounds")) {
			ToggleDisplayBounds();
		}
	}

	void ToggleDisplayBounds() {
		if (selfScript.displayCaveBounds == true) {
			//SceneView.onSceneGUIDelegate -= OnScene;
			selfScript.displayCaveBounds = false;
		}
		else {
			//SceneView.onSceneGUIDelegate += OnScene;
			selfScript.displayCaveBounds = true;
		}
	}

	void OnScene(SceneView sceneView) {

		//if (!selfScript.displayCaveBounds) {return;}
		//if (selfScript.displayCaveBounds)

		Color originalHandleColor = Handles.color;

		Rect positioningBrush = new Rect();
		//DRAW LEFT WALL
		Handles.color = selfScript.leftWallColor;

		//positioningBrush.Set (selfScript.CloseTopLeftCorner.position.x,);

		Handles.DrawSolidRectangleWithOutline (new Vector3 [] {selfScript.left_TopLeft.position,
																selfScript.left_BottomLeft.position,
																selfScript.left_BottomRight.position,
																selfScript.left_TopRight.position
															   },
											   selfScript.leftWallColor,
											   selfScript.leftWallOutlineColor);

		//DRAW MIDDLE WALL
		Handles.color = selfScript.middleWallColor;

		Handles.DrawSolidRectangleWithOutline (new Vector3 [] {selfScript.middle_TopLeft.position,
			selfScript.middle_BottomLeft.position,
			selfScript.middle_BottomRight.position,
			selfScript.middle_TopRight.position
		},
			selfScript.middleWallColor,
			selfScript.middleWallOutlineColor);

		//DRAW RIGHT WALL
		Handles.color = selfScript.rightWallColor;
		Handles.DrawSolidRectangleWithOutline (new Vector3 [] {selfScript.right_TopLeft.position,
																selfScript.right_BottomLeft.position,
																selfScript.right_BottomRight.position,
																selfScript.right_TopRight.position
															  },
			selfScript.rightWallColor,
			selfScript.rightWallOutlineColor);
		


		Handles.color = originalHandleColor;
	}

}
#endif
