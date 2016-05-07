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

    [HideInInspector]
	public bool showCaveBounds = false;

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
	void OnDrawGizmos() {

        if (!showCaveBounds) { return; }

        float wallThickness = 0.0f;
		Color originalGizmoColor = Gizmos.color;

        //LEFT WALL
		Gizmos.color = leftWallColor;
		Gizmos.DrawCube (CalcAvgPos (left_TopLeft.position,
									 left_TopRight.position,
									 left_BottomLeft.position,
									 left_BottomRight.position),
			new Vector3(wallThickness,
						Mathf.Abs (left_TopLeft.position.y - left_BottomLeft.position.y),
						Mathf.Abs (left_TopLeft.position.z - left_TopRight.position.z))
                        );

        Gizmos.color = leftWallOutlineColor;
        Gizmos.DrawLine(left_TopLeft.position, left_BottomLeft.position);
        Gizmos.DrawLine(left_TopLeft.position, left_TopRight.position);
        Gizmos.DrawLine(left_TopLeft.position, left_BottomRight.position);
        Gizmos.DrawLine(left_TopRight.position, left_BottomRight.position);
        Gizmos.DrawLine(left_BottomLeft.position, left_BottomRight.position);
        Gizmos.DrawLine(left_TopRight.position, left_BottomLeft.position);

        //MIDDLE WALL
		Gizmos.color = middleWallColor;
        Gizmos.DrawCube(CalcAvgPos(middle_TopLeft.position,
                                   middle_TopRight.position,
                                   middle_BottomLeft.position,
                                   middle_BottomRight.position),
            new Vector3(Mathf.Abs(middle_TopLeft.position.x - middle_TopRight.position.x),
                        Mathf.Abs(middle_TopLeft.position.y - middle_BottomLeft.position.y),
                        wallThickness)
                        );

        Gizmos.color = middleWallOutlineColor;
        Gizmos.DrawLine(middle_TopLeft.position, middle_BottomLeft.position);
        Gizmos.DrawLine(middle_TopLeft.position, middle_TopRight.position);
        Gizmos.DrawLine(middle_TopLeft.position, middle_BottomRight.position);
        Gizmos.DrawLine(middle_TopRight.position, middle_BottomRight.position);
        Gizmos.DrawLine(middle_BottomLeft.position, middle_BottomRight.position);
        Gizmos.DrawLine(middle_TopRight.position, middle_BottomLeft.position);

        //RIGHT WALL
        Gizmos.color = rightWallColor;
        Gizmos.DrawCube(CalcAvgPos(right_TopLeft.position,
                                   right_TopRight.position,
                                   right_BottomLeft.position,
                                   right_BottomRight.position),
            new Vector3(wallThickness,
                        Mathf.Abs(right_TopLeft.position.y - right_BottomLeft.position.y),
                        Mathf.Abs(right_TopLeft.position.z - right_TopRight.position.z))
                        );

        Gizmos.color = rightWallOutlineColor;
        Gizmos.DrawLine(right_TopLeft.position, right_BottomLeft.position);
        Gizmos.DrawLine(right_TopLeft.position, right_TopRight.position);
        Gizmos.DrawLine(right_TopLeft.position, right_BottomRight.position);
        Gizmos.DrawLine(right_TopRight.position, right_BottomRight.position);
        Gizmos.DrawLine(right_BottomLeft.position, right_BottomRight.position);
        Gizmos.DrawLine(right_TopRight.position, right_BottomLeft.position);


        //RESET
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

    public override void OnInspectorGUI() {

        bool prevShowSetting = selfScript.showCaveBounds;

        selfScript.showCaveBounds = GUILayout.Toggle(selfScript.showCaveBounds, "Show Screens", EditorStyles.radioButton);

        if (selfScript.showCaveBounds != prevShowSetting) {
            SceneView.RepaintAll();
        }

        base.OnInspectorGUI();

    }


}
#endif
