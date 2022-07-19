using System;
using UnityEngine;

public class GardenUpdate : MonoBehaviour
{

    public GameObject longWheatMesh;
    public GameObject shortWheatMesh;
    public GameObject stack;

    
 
    public void createStack(Vector3 origin)
    {
        var s = Instantiate(stack, origin, Quaternion.identity);
        s.transform.Translate(Vector3.up);
    }


    public GameObject SetShortMesh(GameObject old)
    {        
        var newgo = Instantiate(shortWheatMesh, old.transform.position, old.transform.rotation, transform);
        Destroy(old);
        return newgo;
    }


    public GameObject SetLongMesh(GameObject old)
    {
        var newgo = Instantiate(longWheatMesh, old.transform.position, old.transform.rotation, transform);
        Destroy(old);
        return newgo;
    }


    public (GameObject, BoxCollider) AddSuppling(float x, float z, bool isCuted)
    {
        GameObject newSuppling = isCuted ? Instantiate(shortWheatMesh, transform) : Instantiate(longWheatMesh, transform); 
        newSuppling.transform.position = new Vector3(transform.position.x + x + 0.5f,
                                                     transform.position.y           ,
                                                     transform.position.z + z + 0.5f);
        var newSupplingCollider = gameObject.AddComponent<BoxCollider>();
        newSupplingCollider.size = newSuppling.transform.localScale;
        newSupplingCollider.center = newSuppling.transform.localPosition;
        newSupplingCollider.isTrigger = false;
        return (newSuppling, newSupplingCollider);
    }

}

