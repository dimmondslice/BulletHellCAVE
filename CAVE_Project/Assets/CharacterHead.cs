using UnityEngine;
using System.Collections;

public class CharacterHead : MonoBehaviour 
{
    public int health = 100;
    public GUIStyle stylin;

	public int level;

    void Update()
    {
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2, 10, 200, 100), "Health:" + health, stylin);
    }
    public void Hit(int damage)
    {
        health -= damage;
		transform.Translate(new Vector3(0f,6f,0f));
		Invoke("MoveBack", 1);
	}

	void MoveBack()
	{
		transform.Translate(new Vector3(0f,-6f,0f));
	}
    
}
