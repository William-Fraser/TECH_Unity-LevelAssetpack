using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSwitch : MonoBehaviour
{
    private bool locked = true;
    private bool pressed = false;
    private Color pressedButton = Color.grey; // colour is dim red
    private Renderer buttonRend; // to set the colour of material

    public bool Locked() //get
    {
        return locked;
    }

    private void Start()
    {
        buttonRend = GetComponent<Renderer>();
    }
    // when switch is triggered, press down and unlock
    private void OnTriggerEnter(Collider other)
    {
        if (!pressed)
        {
            transform.Translate(Vector3.down / 8);
            buttonRend.material.SetColor("_Color", pressedButton); 
            pressed = true;
        }
        locked = false;
    }

}
