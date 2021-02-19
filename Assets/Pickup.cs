using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //enable if pickup respawns
    public bool enableRespawn = false;

    public GameObject pickUp;
    public int timeUntilRespawn = 2;

    private float rotSpeed = 50f;
    public bool pickedUp;
    private bool respawning;
    private float timeStampCollectedTime;
    private float respawnTimeElapsed;
    //private float timer = 0.0f;
    private Vector3 spinpoint = new Vector3(0, 1, 0);    
    

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(spinpoint * rotSpeed * Time.deltaTime); // rotates pick up in realtime
        if (pickedUp == true)
        {
            if (respawning == false) 
            {
                Debug.Log("Picked up");
                pickUp.GetComponent<MeshRenderer>().enabled = false;
                pickUp.GetComponent<SphereCollider>().enabled = false;
                timeStampCollectedTime = Time.time;
                respawning = true;
            }

                pickedUp = false;
        }
        if (enableRespawn == false)
        {
            pickUp.SetActive(false);
        }
        else
        {
            if (respawning == true)
            {
                respawnTimeElapsed = Time.time - timeStampCollectedTime;
                if (respawnTimeElapsed >= timeUntilRespawn)
                {
                    Debug.Log("Respawning");
                    pickUp.GetComponent<MeshRenderer>().enabled = true;
                    pickUp.GetComponent<SphereCollider>().enabled = true;
                    respawning = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            pickedUp = true;
        }
    }
}