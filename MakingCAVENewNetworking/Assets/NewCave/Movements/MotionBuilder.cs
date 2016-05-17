using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Orientation {
	public Vector3 pos;
	public Quaternion rot;
	public Orientation() {
		pos = new Vector3();
		rot = Quaternion.identity;
	}
	public Orientation(Vector3 _pos, Quaternion _rot) {
		pos = _pos;
		rot = _rot;
	}
	public Orientation(Transform transform) {
		pos = transform.position;
		rot = transform.rotation;
	}
}

[System.Serializable]
public class SerializedMotion {
	public string name;
	public List<Orientation> orientations;
	public bool loop;
	public bool reset;

	public SerializedMotion() {
		loop = reset = true;
		name = "";
		orientations = new List<Orientation>();
	}

	public SerializedMotion(string _name) {
		loop = reset = true;
		name = _name;
		orientations = new List<Orientation>();
	}

	public SerializedMotion (string _name, List<Orientation> _orientations) {
		loop = reset = true;
		name = _name;
		orientations = _orientations;
	}

	public void Trim(int start, int end) {
		Debug.Log(end);
		Debug.Log(end - orientations.Count - 1);
		Debug.Log(orientations.Count);
		orientations.RemoveRange(end, orientations.Count - end - 1);
		orientations.RemoveRange(0, start);
	}
}
	
[ExecuteInEditMode]
public class MotionBuilder : MonoBehaviour {

	public string name;
	public bool loop, resetPosition, playing;
	public List<Orientation> frames;
	public char hotkey;

	private int currentFrame, startingFrame, endingFrame;
	private Orientation startOrientation;
	public int selectionIndex;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
		EditorApplication.update += Update;
		#endif

		currentFrame = 0;
		selectionIndex = 0;
		startingFrame = endingFrame = -1;
		playing = false;
	}

	void OnDestroy() {
		#if UNITY_EDITOR
		EditorApplication.update -= Update;
		#endif
	}

	// Load a motion from the serialized object
	public void Load(SerializedMotion _motion) {
		name = _motion.name;
		frames = _motion.orientations;
	}

	// Update is called once per frame
	public void Update() {
		if (playing) {
			if (currentFrame < startingFrame)
				currentFrame = startingFrame;

			gameObject.transform.position = startOrientation.pos - frames[currentFrame].pos;
			gameObject.transform.rotation = startOrientation.rot * frames[currentFrame].rot;

			currentFrame += 1;
			if (currentFrame == frames.Count || currentFrame == endingFrame) {
				Reset();
				Stop();
				Play();
			}
		}
	}

	public void Play() {
		playing = true;
		startOrientation = new Orientation(transform.position, transform.rotation);
	}

	public void Play(int _startingFrame, int _endingFrame) {
		startingFrame = _startingFrame;
		endingFrame = _endingFrame;
		Play();
	}

	public void Pause() {
		playing = false;
	}

	public void Reset() {
		gameObject.transform.position = startOrientation.pos;
		gameObject.transform.rotation = startOrientation.rot;
	}

	public void Stop() {
		Pause();
		startOrientation = null;
		currentFrame = Math.Max(0, startingFrame);
	}

	public void Clear() {
		name = null;
		loop = false;
		resetPosition = false;
		frames = null;
	}
}

[CustomEditor(typeof(MotionBuilder))]
public class MotionBuilderEditor : Editor {

	public MotionBuilder motionBuilder;
	_LevelData savedMotions;

	SerializedMotion motion; 
	string rename_field;
	string[] motionNames;
	bool isRecording;

	private float minFrame, maxFrame;

	public void OnEnable() {
		isRecording = false;
		motionBuilder = (MotionBuilder)target;
		if (rename_field == null) {
			rename_field = "";
		}
			
		string path = "Assets/Resources/_LevelData.asset";
		savedMotions = (_LevelData)AssetDatabase.LoadAssetAtPath(path, typeof(_LevelData));

		motionNames = new string[savedMotions.motions.Count + 1];
		motionNames[0] = "[Select a Motion]";
		for (int i = 0; i < savedMotions.motions.Count; i++) {
			motionNames[i+1] = savedMotions.motions[i].name;
		}

		if (motionBuilder.selectionIndex > 0) {
			motionBuilder.Load(savedMotions.motions[motionBuilder.selectionIndex-1]);
			minFrame = 1;
			maxFrame = motionBuilder.frames.Count - 1;
		}
	}

	public void enableControllers() {
		MotionController[] controllers = motionBuilder.GetComponents<MotionController>();
		for (int i = 0; i < controllers.Length; i++) {
			//controllers[i].enabled = false;
			//controllers[i].enabled = true;
		}
	}

	public override void OnInspectorGUI() {
		
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Record New", GUILayout.Width(80.0f)))
			NewMotionWindow.Show(this);
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.LabelField("Selected Motion", GUILayout.Width(115.0f));
		motionBuilder.selectionIndex = EditorGUILayout.Popup(motionBuilder.selectionIndex, motionNames, GUILayout.MaxWidth(140.0f));

		if (EditorGUI.EndChangeCheck ()) {
			if (motionBuilder.selectionIndex == 0) {
				motionBuilder.Clear();
				minFrame = 0;
				maxFrame = 0;
			}
			else {
				motionBuilder.Load(savedMotions.motions[motionBuilder.selectionIndex-1]);
				minFrame = 1;
				maxFrame = motionBuilder.frames.Count - 1;
			}
		}

		GUILayout.EndHorizontal();

		if (motionBuilder.selectionIndex != 0) {
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Rename", GUILayout.Width(115.0f));
			rename_field = GUILayout.TextField(rename_field);
			GUILayout.EndHorizontal();

			GUILayout.Space(10.0f);
			GUILayout.BeginHorizontal();

			EditorGUILayout.LabelField( ((int)minFrame).ToString(), GUILayout.MaxWidth(28.0f));
			EditorGUILayout.MinMaxSlider(ref minFrame, ref maxFrame, 1, motionBuilder.frames.Count - 1, new GUILayoutOption[] {GUILayout.Width(180.0f)} );
			EditorGUILayout.LabelField(((int)maxFrame).ToString());

			GUILayout.EndHorizontal();
			GUILayout.Space(10.0f);

			GUILayout.BeginHorizontal();

			if (!motionBuilder.playing) {
				if(GUILayout.Button("Preview", GUILayout.Width(80.0f))) {
					motionBuilder.Play((int)minFrame, (int)maxFrame);
				}
			}
			else {
				if(GUILayout.Button("Stop", GUILayout.Width(80.0f))) {
					motionBuilder.Reset();
					motionBuilder.Stop();
				}
			}

			if (GUILayout.Button("Save", GUILayout.Width(80.0f))) {	
				if (motionBuilder.playing)
					motionBuilder.Reset();
				motionBuilder.Stop();
				if (rename_field != "")
					savedMotions.Rename(motionBuilder.selectionIndex - 1, rename_field);
				
				savedMotions.Trim(motionBuilder.selectionIndex - 1, (int)minFrame, (int)maxFrame);
				motionBuilder.selectionIndex = 0;		
				OnEnable();
				enableControllers();
			}

			if (GUILayout.Button("Delete", GUILayout.Width(80.0f))) {
				savedMotions.RemoveMotion(motionBuilder.selectionIndex - 1);
				motionBuilder.selectionIndex = 0;
				OnEnable();
				enableControllers();
			}

			GUILayout.EndHorizontal();
		}

	}
}

public class NewMotionWindow : EditorWindow {

	MotionBuilder motionBuilder;
	MotionBuilderEditor editor;
	_LevelData levelData;

	Orientation startingOrientation;
	SerializedMotion motion; 
	string name_field;
	bool isRecording;

	void OnEnable() {
		isRecording = false;
		maxSize = new Vector2(225, 50);
		levelData = (_LevelData)Resources.Load("_LevelData");
		levelData.Initialize();
	}

	void Update() {

		// Record the frames of motion
		if (isRecording) {
			Orientation frame = new Orientation();
			frame.pos = startingOrientation.pos - motionBuilder.gameObject.transform.position;
			frame.rot = Quaternion.FromToRotation(startingOrientation.rot * Vector3.forward, motionBuilder.gameObject.transform.forward);
			Debug.Log(frame.pos);
			motion.orientations.Add(frame);
		}
	}

	void OnGUI() {
		GUI.Label(new Rect(5, 4, 80, 18), "Enter a name:");

		// Assign name from text field
		name_field = EditorGUI.TextField(new Rect(85, 4, 125, 15), name_field);

		if (!isRecording) {
			// Begin isRecording
			if (GUI.Button(new Rect(5, 28, 50, 18), "Record")) {
				motion = new SerializedMotion();
				startingOrientation = new Orientation(motionBuilder.gameObject.transform);
				isRecording = true;
			}
			// Allow Submission of a isRecording
			if (!isRecording && motion != null && motion.orientations.Count > 0) {
				if (GUI.Button(new Rect(60, 28, 50, 18), "Submit")) {
					motion.name = name_field;
					levelData.AddMotion(motion);
					motionBuilder.Stop();
					editor.OnEnable();
					this.Close();
				}
			}
		}
		else {
			// End isRecording
			if (GUI.Button(new Rect(5, 28, 50, 18), "Stop")) {
				isRecording = false;
				motionBuilder.Load(motion);
				motionBuilder.transform.position = startingOrientation.pos;
				motionBuilder.transform.rotation = startingOrientation.rot;
				motionBuilder.Play();
			}
		}
		// Cancel Motion Creation
		if (GUI.Button(new Rect(170, 28, 50, 18), "Cancel")) {
			motionBuilder.Stop();
			this.Close();
		}
	}

	public static void Show(MotionBuilderEditor editor) {
		NewMotionWindow window = (NewMotionWindow)EditorWindow.GetWindow(typeof(NewMotionWindow));
		window.editor = editor;
		window.motionBuilder = editor.motionBuilder;
	}
}