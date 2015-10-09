using UnityEngine;
using System.Collections;

public class BulletSpawner : MonoBehaviour 
{
    public float timer = 0f;
    public float coolDown = 3f;
    public float chance = .5f;

    public Transform BulletPrefab;

	public string side;

	void Update () 
    {
        timer += Time.deltaTime;
        if(timer >= coolDown)
        {
            timer = 0;
            if(Random.Range(0, 3) == 0)
            {
                GameObject b = Instantiate(BulletPrefab, transform.position, Quaternion.Euler(transform.forward)) as GameObject;
                 if(coolDown > 1.15 )
					coolDown -= .15f;
            }
        }
	}
	public void Reset()
	{
		timer= 0;
		coolDown=3f;
	}
}
