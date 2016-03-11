using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Notes : MonoBehaviour {

    public string notesString;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(Notes))]
public class NotesEditor : Editor {

    Notes selfScript;

    void OnEnable() {
        selfScript = (Notes)target;
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();

        selfScript.notesString = EditorGUILayout.TextArea(selfScript.notesString);

    }
}
#endif
