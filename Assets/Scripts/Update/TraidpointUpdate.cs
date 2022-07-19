using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraidpointUpdate : MonoBehaviour
{

    public delegate void OnTraidHit();
    static public event OnTraidHit HitEvent;


    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            HitEvent();
        }
    }
}