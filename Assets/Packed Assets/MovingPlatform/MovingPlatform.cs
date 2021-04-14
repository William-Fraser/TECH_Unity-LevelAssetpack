using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    //public fields
    [Header("Platform")]
    public GameObject platformObject;
    public float speed;
    [Space]
    public GameObject destinationObject;

    //private fields
    private Vector3 returnPosition;
    private bool travelingTo = true;
    
    // Start is called before the first frame update
    void Start()
    {
        this.transform.parent = platformObject.transform;
        returnPosition = platformObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (travelingTo)
        {
            if (platformObject.transform.localPosition.x >= destinationObject.transform.localPosition.x) // stops platfom at destination
            {
                platformObject.transform.localPosition += new Vector3(-(speed * Time.deltaTime), 0, 0);
            }
            else
            {
                travelingTo = false; // once platform reaches destination it's no longer traveling to it, instead it returns
            }
        }
        else
        {
            if (platformObject.transform.localPosition.x <= returnPosition.x) // stops platfom at return position
            {
                platformObject.transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            else
            {
                travelingTo = true; // once platform reaches the return position it should travel again
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        other.transform.parent = transform;
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
