using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackUpdate : MonoBehaviour
{

    public delegate void OnStackHit(GameObject stack);
    static public event OnStackHit HitEvent;

    private Rigidbody rb;
    private BoxCollider boxCollider;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    void Update()
    {
        if (rb.IsSleeping())
        {
            rb.isKinematic = true;
            boxCollider.enabled = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {        
        if(collider.gameObject.name == "Player")
        {            
            HitEvent(gameObject);
        }
    }
}
