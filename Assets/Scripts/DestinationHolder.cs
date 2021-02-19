using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationHolder : MonoBehaviour
{
    public GameObject destination_Object;
    [HideInInspector]
    public Vector3 destination;
    [HideInInspector]
    
    // Start is called before the first frame update
    void Start()
    {
        //sets destination in script
        destination = destination_Object.transform.position;
    }
}
