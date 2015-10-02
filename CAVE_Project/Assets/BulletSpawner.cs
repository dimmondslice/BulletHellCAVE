using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour 
{
    public float timer = 0f;
    public float coolDown = 3f;
    public float chance = .5f;

    public Transform Bullet;

	void Update () 
    {
        timer += Time.deltaTime;
        if(timer >= coolDown)
        {
            timer = 0;
            if(Random.Range(0, 3) == 0)
            {
                Instantiate(Bullet, transform.position, Quaternion.identity);

                 if(coolDown > 1.3 ) coolDown -= .1f;
            }
        }
	}
}
