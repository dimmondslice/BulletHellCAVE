﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float force;
    public Rigidbody rb;
    public Transform player;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = FindObjectOfType<CharacterHead>().transform;


        speed *= Random.Range(5, 10) / 7.5f;
        //the forward vector is setafter its instantiated, change this later
        rb.velocity = transform.forward * speed;
    }
    void Update()
    {
        //sorta hard codey, just destroy the cube if it's gone past the end of it's track
        if (transform.localPosition.magnitude > 26.5)
            Destroy(gameObject);

        if(rb.velocity.magnitude < maxSpeed )
            rb.AddForce(transform.forward * force);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
