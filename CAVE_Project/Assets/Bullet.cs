using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    public float speed;
    public Rigidbody rb;
    public Transform player;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<CharacterHead>().transform;


        speed *= Random.Range(5, 10) / 7.5f;
        rb.velocity = transform.forward * speed;
    }
    void Update()
    {
        //sorta har codey, just destroy the cube if it's gone past the end of it's track
        if (transform.localPosition.magnitude > 26.5)
            Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
