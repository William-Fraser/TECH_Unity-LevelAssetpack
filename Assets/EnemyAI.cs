using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject Trigger;
    public GameObject e_Object;
    private Rigidbody e_Rigidbody;
    private Vector3 _direction;
    // Start is called before the first frame update
    void Start()
    {
        
        e_Rigidbody = e_Rigidbody.GetComponent<Rigidbody>();
        //e_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _direction = new Vector3(1, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        e_Object.transform.Translate(_direction * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision col)
    {
        _direction.x = _direction.x * -1;
    }
}
