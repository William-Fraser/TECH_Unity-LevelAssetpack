using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ----- variables
    //object
    private Rigidbody m_Rigidbody;
    public float _movement_speed = 7f;
    public float _jump_speed = 50f;
    private bool _isGrounded = true;
    private Vector3 _playerSpawn;

    //camera
    public GameObject view_camera;
    private Rigidbody c_Rigidbody;
    public float _mouseX_speed = 2f;
    public float _mouseY_speed = 2f;


    // Start is called before the first frame update
    void Start()
    {
        //save players origin as spawn
        _playerSpawn = this.transform.position;

        //stop player from falling over
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        //stop camera rotating on z axis (view turns upsidedown)
        c_Rigidbody = view_camera.GetComponent<Rigidbody>();
        c_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;

        // locks cursor to centre screen and turns invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //move the player
        float translateForwardBack = Input.GetAxis("Vertical");
        float translateSidetoSide = Input.GetAxis("Horizontal");
        gameObject.transform.Translate(0, 0, translateForwardBack * _movement_speed * Time.deltaTime);
        gameObject.transform.Translate(translateSidetoSide * _movement_speed * Time.deltaTime, 0, 0);

        //player view move
        float mouseX = _mouseX_speed * Input.GetAxis("Mouse X");
        gameObject.transform.Rotate(0, mouseX, 0);

        //player jumps
        if (Input.GetKeyDown("space") && _isGrounded) {
            m_Rigidbody.AddForce(Vector3.up * _jump_speed, ForceMode.Impulse);
            _isGrounded = false;
        }

        //holds Y view move, stops neck from breaking --- FIX
        float neckCheck = view_camera.transform.rotation.x;
        if (neckCheck < 60.0f && neckCheck > -60.0f) {
            float mouseY = _mouseY_speed * Input.GetAxis("Mouse Y");
            view_camera.transform.Rotate(-mouseY, 0, 0);
        }
        else { }
    }
    private void OnCollisionStay(Collision collision)
    {
        _isGrounded = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Killbox"){
            this.transform.position = _playerSpawn;
        }
        if (other.tag == "Checkpoint") {
            _playerSpawn = this.transform.position;
        }
        if (other.tag == "Teleport")
        {
            this.transform.Rotate(0,+180,0);
            this.transform.position = other.GetComponent<DestinationHolder>().destination;
        }
    }
}
