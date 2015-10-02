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
        rb.velocity = transform.forward;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
