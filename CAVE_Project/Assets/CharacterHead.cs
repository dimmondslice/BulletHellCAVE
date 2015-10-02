using UnityEngine;
using System.Collections;

public class CharacterHead : MonoBehaviour 
{
    public int health = 100;
    public GUIStyle stylin;

    void Update()
    {
        if(health <= 0)
        {
            Application.LoadLevel(0);
        }
    }
    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 130, 10, 50, 30), "Health:" + health, stylin);
    }
    public void Hit(int damage)
    {
        health -= damage;
    }
    
}
