using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorVertical : MonoBehaviour
{

    public float speed = 1f;
    public GameObject door;
    private bool openDoor = false;
    private float closedPos;
    private float openPos;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = door.transform.localPosition.y;
        openPos = closedPos - 5;
        Debug.Log("Sliding Door "+closedPos+" "+ openPos);
    }
    // Update is called once per frame
    void Update()
    {
        if (openDoor == true) // open the door
        {
            Debug.Log($"Y pos : {door.transform.localPosition.y} open pos : {openPos}");
            if (door.transform.localPosition.y >= openPos)
            {
                door.transform.localPosition += new Vector3(0f, -(speed * Time.deltaTime), 0f);
            }
            
        }
        else if (openDoor == false) // close the door
        {

            if (door.transform.localPosition.y <= closedPos)
            {
                door.transform.localPosition += new Vector3(0f, (speed * Time.deltaTime), 0f);
            }
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        openDoor = true;
    }
    private void OnTriggerExit(Collider other)
    {
        openDoor = false;
    }
}
