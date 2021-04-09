using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorVertical : MonoBehaviour
{
    //public fields
    [Header("Door")]
    public GameObject door;
    public float speed = 0.5f;
    [Header("Lock Switch")]
    public bool locked = false;
    public GameObject KeySwitch;

    //private fields
    private bool openDoor = false;
    private float closedPos;
    private float openPos;

    // Start is called before the first frame update
    void Start()
    {
        closedPos = door.transform.localPosition.y;
        openPos = closedPos - 5;
        Debug.Log("Sliding Door, closed posY: "+closedPos+", opened posY: "+ openPos);
    }
    // Update is called once per frame
    void Update()
    {
        //unlocks if button is pressed
        if (locked)
        {
            locked = KeySwitch.GetComponent<LockSwitch>().Locked();
        }

        // open the door
        if (openDoor == true && locked == false)     
        {
            if (door.transform.localPosition.y >= openPos) //stops door at open position
            {
                door.transform.localPosition += new Vector3(0f, -(speed * Time.deltaTime), 0f);
            }
        }
        // close the door
        else if (openDoor == false)
        {
            if (door.transform.localPosition.y <= closedPos) // stops door at closed position
            {
                door.transform.localPosition += new Vector3(0f, speed * Time.deltaTime, 0f);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        openDoor = true;
    }
    private void OnTriggerStay(Collider other)
    {
        openDoor = true;
    }
    private void OnTriggerExit(Collider other)
    {
        openDoor = false;
    }
}
