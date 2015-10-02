using UnityEngine;
using System.Collections;

public class Hurtbox : MonoBehaviour 
{
    public CharacterHead charHead;

	public BulletSpawner[] spawners;

    void Awake()
    {
        charHead = GetComponentInParent<CharacterHead>();
		spawners = FindObjectsOfType<BulletSpawner>();
    }
    void OnTriggerEnter(Collider other)
	{   
        if(other.tag == "Bullet")
        {
			charHead.Hit(100);
            foreach(var spawn in spawners)
			{
				spawn.Reset();
			}
        }
    }
}
