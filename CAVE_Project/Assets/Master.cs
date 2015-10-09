using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master : MonoBehaviour 
{
    public List<Transform> spawners;
    public Transform bulletPrefab;
    public float spawnRate;

    void Start()
    {
        //populate spawners
        GameObject[] spawnersGO = GameObject.FindGameObjectsWithTag("BulletSpawner");
        spawners = new List<Transform>();
        foreach (GameObject g in spawnersGO) spawners.Add(g.transform);

        //begin spawning coroutine
        StartCoroutine(BulletSpawning());
    }
	IEnumerator BulletSpawning ()
    {
        while (true)
        {
            if (spawnRate > 1.5) spawnRate -= .1f;
            Spawn();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void Spawn()
    {
        int ran = Random.Range(0, spawners.Count-1);
        Transform spawn = spawners[ran];
        Transform bullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity) as Transform;
        bullet.transform.forward = spawn.forward;
        bullet.parent = spawn;
        bullet.GetComponent<Bullet>().speed *= (3 / spawnRate);
    }
}
