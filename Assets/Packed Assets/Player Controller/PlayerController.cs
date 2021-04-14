using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // ----- variables
    //model
    private Rigidbody m_Rigidbody;
    private Quaternion m_Rotation;
    private bool m_isGrounded = true;
    private Vector3 m_playerSpawn;
    
    [Header("Player Movement")]
    public float _movementSpeed = 7f;
    public float _jumpHeight = 7f;

    //camera
    private Quaternion c_Rotation;

    [Header("Camera")]
    new public GameObject camera;
    public float _mouseXSpeed = 1f;
    public float _mouseYSpeed = 1f;
    [Range(1f, 60f)]
    public float _MaxLookHeight = 60;
    [Range(-1f, -60f)]
    public float _MinLookHeight = -60;

    //fields for respawn
    private Image fog;
    private bool respawning;
    private bool respawnOnce;
    
    [Header("Respawn")]
    public GameObject RespawnFog;


    // Start is called before the first frame update
    void Start()
    {
        //save players origin as spawn
        m_playerSpawn = this.transform.position;
        m_Rotation = this.transform.rotation;

        //initializes the Player and Camera
        m_Rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        c_Rotation = camera.transform.rotation;

        //initialization for respawn
        RespawnFog.SetActive(true);
        fog = RespawnFog.GetComponent<Image>();
        respawning = true;
        respawnOnce = false;
        //fog.canvasRenderer.SetAlpha(0);

    }
    // Update is called once per frames
    void Update()
    {
        //move the player
        MoveController();

        //player view control X, Y
        ViewController();

        if (respawning && respawnOnce == false)
        {
            respawnOnce = true;
            fog.CrossFadeAlpha(1, .5f, false);
            Debug.Log("Run this Once");
            StartCoroutine("Respawn");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        m_isGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Killbox")
        { 
            respawning = true;
        }
        if (other.tag == "Checkpoint")
        {
            m_playerSpawn = this.transform.position;
        }
        if (other.tag == "Teleport")
        {
            this.transform.position = other.GetComponent<DestinationHolder>().destination;
        }
        if (other.tag == "Pickup")
        {
            Debug.Log("PICKUP initiated");
            other.gameObject.GetComponent<Collectable>().pickedUp = true;
        }
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<Collider>().enabled = false;
            other.GetComponentInParent<MeshRenderer>().enabled = false;
        }
    }
    private void ViewController()
    {
        //sets rotation on the x axis
        float xRotation = Input.GetAxis("Mouse X") * _mouseXSpeed;
        m_Rotation.y += xRotation;
        c_Rotation.y += xRotation;

        //sets rotation on the y axis
        c_Rotation.x += Input.GetAxis("Mouse Y") * _mouseYSpeed * (-1); //rotates on the x axis to look up and down
        c_Rotation.x = Mathf.Clamp(c_Rotation.x, _MinLookHeight, _MaxLookHeight); // clamp locks range to stop neck from breaking

        //rotates view point
        transform.Rotate(0, xRotation, 0);
        camera.transform.rotation = Quaternion.Euler(c_Rotation.x, c_Rotation.y, c_Rotation.z);
        //Debug.Log(c_Rotation.x);
    }
    private void MoveController()
    {
        float translateForwardBack = Input.GetAxis("Vertical");
        float translateSidetoSide = Input.GetAxis("Horizontal");
        gameObject.transform.Translate(0, 0, translateForwardBack * _movementSpeed * Time.deltaTime);
        gameObject.transform.Translate(translateSidetoSide * _movementSpeed * Time.deltaTime, 0, 0);

        //player jumps
        if (Input.GetKeyDown("space") && m_isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.up * _jumpHeight, ForceMode.Impulse);
            m_isGrounded = false;
        }
        if (m_isGrounded == false)
        {
            float gravityBoost = 1 * 2 * Time.deltaTime;
            gameObject.transform.Translate(0, gravityBoost * Time.deltaTime, 0);
        }
    }

    //handles respawn animations
    IEnumerator Respawn()
    {
        Debug.Log("Starting respawntimer");
        yield return new WaitForSeconds(1);
        Debug.Log("Respawning");

        //reposition
        gameObject.transform.position = m_playerSpawn;

        //remove respawn fog
        fog.CrossFadeAlpha(0, .7f, false);
        respawning = false;
        respawnOnce = false; // set once to false to respawn once again
    }
}
