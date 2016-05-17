using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class _LevelData : ScriptableObject {

	[SerializeField]
	public List<SerializedMotion> motions;

	[SerializeField]
	public int active_index;

	[MenuItem("Assets/Create/_LevelData")]
	public static void CreateAsset() {
		ScriptableObjectUtility.CreateAsset<_LevelData>();
	}

	public void Initialize() {
		if (motions == null) {
			active_index = -1;
			motions = new List<SerializedMotion>();
		}
	}

	public void AddMotion(SerializedMotion _motion) {
		motions.Add(_motion);
		Save();
	}

	public void RemoveMotion(int _motion_index) {
		motions.RemoveAt(_motion_index);
		Save();
	}

	public void SetActive(int _active_index) {
		active_index = _active_index;
		Save();
	}

	public void Rename (int _rename_index, string _name) {
		motions[_rename_index].name = _name;
		Save();
	}

	public void Trim (int _trim_index, int _start_frame, int _end_frame) {
		motions[_trim_index].Trim(_start_frame, _end_frame);
		Save();
	}

	public void Save() {
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssets();
	}

	public List<SerializedMotion> Motions() {
		return motions;
	}
}

