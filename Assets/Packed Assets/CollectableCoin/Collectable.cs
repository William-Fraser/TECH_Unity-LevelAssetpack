using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Collectable")]
    public GameObject pickUpObject;
    [Tooltip("rotates around this objects Y axis")]
    public GameObject rotationAxis;
    public float rotationSpeed = 50f;

    [Header("Respawn")]
    public bool enableRespawn = false;
    public int timeUntilRespawn = 2;

    [HideInInspector]
    public bool pickedUp;

    //private fields
    private bool respawning;
    private float timeStampCollectedTime;
    private float respawnTimeElapsed;
    private Vector3 spinpoint = new Vector3(0, 1, 0);


    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        rotationAxis.transform.Rotate(spinpoint * rotationSpeed * Time.deltaTime); // rotates pick up in realtime
        if (pickedUp == true)
        {
            if (respawning == false)
            {
                if (enableRespawn == false)
                {
                    pickUpObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Picked up");
                    pickUpObject.GetComponent<MeshRenderer>().enabled = false;
                    pickUpObject.GetComponent<SphereCollider>().enabled = false;
                    timeStampCollectedTime = Time.time;
                    respawning = true;
                }
            }

            pickedUp = false;
        }
        else
        {

            if (enableRespawn && respawning == true)
            {
                respawnTimeElapsed = Time.time - timeStampCollectedTime;
                if (respawnTimeElapsed >= timeUntilRespawn)
                {
                    Debug.Log("Respawning");
                    pickUpObject.GetComponent<MeshRenderer>().enabled = true;
                    pickUpObject.GetComponent<SphereCollider>().enabled = true;
                    respawning = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pickedUp = true;
        }
    }
}