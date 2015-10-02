using UnityEngine;
using System.Collections;

public class Hurtbox : MonoBehaviour 
{
    public CharacterHead charHead;

    void Awake()
    {
        charHead = GetComponentInParent<CharacterHead>();
    }
    void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Bullet")
        {
            print("hit");
            charHead.Hit(34);
        }
    }
}
