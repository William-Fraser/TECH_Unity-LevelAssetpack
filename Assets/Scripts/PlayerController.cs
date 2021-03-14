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
    private MeshRenderer m_MeshRemover;
    public float _movementSpeed = 7f;
    public float _jumpHeight = 10f;

    //fields for respawn
    public GameObject RespawnFog;
    private Image fog;
    private bool respawning;

    //camera
    private Rigidbody c_Rigidbody;
    private Quaternion c_Rotation;

    public GameObject viewCamera;
    public float _mouseXSpeed = 1f;
    public float _mouseYSpeed = 1f;
    public float _MaxLookHeight = 60;
    public float _MinLookHeight = -60;


    // Start is called before the first frame update
    void Start()
    {
        //save players origin as spawn
        m_playerSpawn = this.transform.position;
        m_Rotation = this.transform.rotation;

        //initializes the Player and Camera
        m_Rigidbody = GetComponent<Rigidbody>();
        m_MeshRemover = GetComponent<MeshRenderer>();
        c_Rigidbody = viewCamera.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        c_Rotation = viewCamera.transform.rotation;

        //initialization for respawn
        RespawnFog.SetActive(true);
        fog = RespawnFog.GetComponent<Image>();
        respawning = false;
        fog.canvasRenderer.SetAlpha(0);


        //StopFall(); // stops model from falling over
    }
    // Update is called once per frames
    void Update()
    {

        //move the player
        MoveController();

        //player view control X, Y
        ViewController();

        if (respawning)
        {
            fog.CrossFadeAlpha(1, .5f, false);
            Debug.Log("Run this Once");
            StartCoroutine("RespawnTimer");
        }
        //StopFall();
    }
    private void OnCollisionStay(Collision collision)
    {
        m_isGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Killbox")
        { // polish with game over screen and possible restart button / needs state machine
            //m_MeshRemover.enabled = false;
            respawning = true;
            //m_MeshRemover.enabled = true;
        }
        if (other.tag == "Checkpoint")
        {
            m_playerSpawn = this.transform.position;
        }
        if (other.tag == "Teleport")
        {
            this.transform.localPosition = other.GetComponent<DestinationHolder>().destination + (Vector3.forward * 2);
        }
        if (other.tag == "Pickup")
        {
            Debug.Log("PICKUP initiated");
            other.gameObject.GetComponent<Pickup>().pickedUp = true;
        }
        if (other.tag == "Enemy")
        {
            other.GetComponentInParent<Collider>().enabled = false;
            other.GetComponentInParent<MeshRenderer>().enabled = false;
        }
    }
    //private void StopFall()//Stops the player from falling over 
    //{
    //    m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
    //    m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    //    c_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
    //}
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
        viewCamera.transform.rotation = Quaternion.Euler(c_Rotation.x, c_Rotation.y, c_Rotation.z);
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
    IEnumerator RespawnTimer()
    {
        Debug.Log("Starting respawntimer");
        yield return new WaitForSeconds(1);
        Debug.Log("Respawning");
        gameObject.transform.position = m_playerSpawn;
        fog.CrossFadeAlpha(0, .7f, false);
        respawning = false;
    }//// detect when screen is black
}
