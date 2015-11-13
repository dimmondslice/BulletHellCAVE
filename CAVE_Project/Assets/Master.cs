using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Master : MonoBehaviour
{
    public List<Transform> spawners;
    public Transform bulletPrefab;
    public float spawnRate;
    public float spawnRateLimit;

    void Start()
    {
        //populate spawners
        GameObject[] spawnersGO = GameObject.FindGameObjectsWithTag("BulletSpawner");
        spawners = new List<Transform>();
        foreach (GameObject g in spawnersGO) spawners.Add(g.transform);

        print(spawners.Count);
    }
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            print("started spawning bullets");
            //begin spawning coroutine
            StartCoroutine(BulletSpawning());
        }
    }
	IEnumerator BulletSpawning ()
    {
        while (true)
        {
            if (spawnRate > spawnRateLimit) spawnRate -= .1f;
            Spawn();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void Spawn()
    {
        int ran = Random.Range(0, spawners.Count);
        print(ran);
        Transform spawn = spawners[ran];
        Transform bullet = Instantiate(bulletPrefab, spawn.position, Quaternion.identity) as Transform;
        //NetworkServer.Spawn(bullet.gameObject);
        bullet.transform.forward = spawn.forward;
        bullet.parent = spawn;
        bullet.GetComponent<Bullet>().speed *= (3 / spawnRate);
    }
}
