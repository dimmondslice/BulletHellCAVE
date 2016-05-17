using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MotionController : MonoBehaviour {

	public string name;
	public bool loop, resetPosition, playing;
	public List<Orientation> frames;
	public char hotkey;

	private int currentFrame;
	private Orientation startOrientation;
	public int selectionIndex;

	public _LevelData savedMotions;

	public void Start() {
		// Execute Update in the editor
		#if UNITY_EDITOR
			EditorApplication.update += Update;
		#endif

		currentFrame = 0;
		loop = playing = false;
		resetPosition = true;
	}

	// Load a motion from the serialized object
	public void Load(SerializedMotion _motion) {
		name = _motion.name;
		frames = _motion.orientations;
	}

	public void Clear() {
		name = null;
		loop = false;
		resetPosition = true;
		frames = null;
	}

	// Unbind the editor Update calls
	void OnDestroy() {
		#if UNITY_EDITOR
			EditorApplication.update -= Update;
		#endif
	}

	public void Update() {
		if (playing) {
			gameObject.transform.position = startOrientation.pos - frames[currentFrame].pos;
			gameObject.transform.rotation = startOrientation.rot * frames[currentFrame].rot;

			currentFrame += 1;
			if (currentFrame == frames.Count) {
				if (!loop) {
					Stop();
				}
				else { 
					Stop();
					Play();
				}
			}
		}
	}

	public void Play() {
		playing = true;
		startOrientation = new Orientation(transform.position, transform.rotation);
	}

	public void Pause() {
		playing = false;
	}

	public void Stop() {
		if (resetPosition) {
			gameObject.transform.position = startOrientation.pos;
			gameObject.transform.rotation = startOrientation.rot;
		}
		startOrientation = null;
		currentFrame = 0;
		Pause();
	}
}

[CustomEditor(typeof(MotionController))]
public class MotionControllerEditor : Editor {

	public MotionController motionController;
	public _LevelData savedMotions;

	private string[] motionNames;
	private float minFrame, maxFrame;

	private string hotkey_string;

	public void OnEnable () {
		Reload();
	}

	public void Reload() {
		string path = "Assets/Resources/_LevelData.asset";
		savedMotions = (_LevelData)AssetDatabase.LoadAssetAtPath(path, typeof(_LevelData));

		motionController = (MotionController)target;
		motionController.Clear();

		if (motionController.selectionIndex > 0 && motionController.selectionIndex < savedMotions.motions.Count) {
			motionController.Load(savedMotions.motions[motionController.selectionIndex-1]);
			minFrame = 1;
			maxFrame = motionController.frames.Count - 1;
		}
		else {
			motionController.selectionIndex = 0;
		}

		motionNames = new string[savedMotions.motions.Count + 1];
		motionNames[0] = "[Select a Motion]";
		for (int i = 0; i < savedMotions.motions.Count; i++) {
			motionNames[i+1] = savedMotions.motions[i].name;
		}
	}

	public override void OnInspectorGUI() {
		GUILayout.Space(10.0f);
		GUILayout.BeginHorizontal();

		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.LabelField("Selected Motion", GUILayout.Width(115.0f));
		motionController.selectionIndex = EditorGUILayout.Popup(motionController.selectionIndex, motionNames, GUILayout.MaxWidth(140.0f));

		if (EditorGUI.EndChangeCheck ()) {
			if (motionController.selectionIndex == 0) {
				motionController.Clear();
				minFrame = -1;
				maxFrame = -1;
			}
			else {
				motionController.Load(savedMotions.motions[motionController.selectionIndex-1]);
				minFrame = 1;
				maxFrame = motionController.frames.Count - 1;
			}
		}

		GUILayout.EndHorizontal();
		if (motionController.selectionIndex > 0) {

			GUILayout.BeginHorizontal();

			motionController.loop = EditorGUILayout.Toggle("Enable Looping", motionController.loop);

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			motionController.resetPosition = EditorGUILayout.Toggle("Reset Position", motionController.resetPosition);

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			EditorGUILayout.LabelField("Hotkey", GUILayout.Width(115.0f));
			EditorGUILayout.LabelField("CTR + ", GUILayout.Width(37.5f));

			GUI.SetNextControlName("MOTION CONTROLLER HOTKEY");
			hotkey_string = GUILayout.TextField(motionController.hotkey.ToString(), 1, GUILayout.Width(18.0f));

			if (hotkey_string != null && hotkey_string.Length > 0 && char.IsLetterOrDigit(hotkey_string, 0)) {
				hotkey_string = hotkey_string.ToUpper();
				motionController.hotkey = hotkey_string[0];
			}
			else {
				hotkey_string = "";
				motionController.hotkey = '\0';
			}
			

			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();

			if (!motionController.playing) {
				if (GUILayout.Button("Play", GUILayout.Width(70.0f))) {
					motionController.Play();
				}
			}
			else {
				if (GUILayout.Button("Pause", GUILayout.Width(70.0f))) {
					motionController.Pause();
				}

				if (GUILayout.Button("Stop", GUILayout.Width(70.0f))) {
					motionController.Stop();
				}
			}

			GUILayout.EndHorizontal();
		}
		Repaint();
	}

	/*void OnSceneGUI() {

		if (hotkey_string.Length > 0) {
			KeyCode hotkey_code = (KeyCode)System.Enum.Parse(typeof(KeyCode), hotkey_string);
			if (Event.current.type == EventType.keyDown && Event.current.keyCode == hotkey_code) {
				Debug.Log("HOTKEY");
			}
		}
	}*/

}