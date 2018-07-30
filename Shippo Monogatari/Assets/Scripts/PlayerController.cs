using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 5f;
    public float jumpForce = 100f;

    public int numberOfJumps = 1;

    Rigidbody rb;
	
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        MovementController();
	}
    
    /*
     *  Funções sobre comandos ficam aqui 
     */

    void MovementController()
    {
        //Eixos do Vector3
        //X -> Esquerda e direita
        //Y -> Pulo
        //Z -> Frente e trás

        rb.AddRelativeForce ((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical")), ForceMode.Impulse);

        rb.velocity = new Vector3 (Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed),
                                   Mathf.Clamp(rb.velocity.y, -moveSpeed, moveSpeed),
                                   Mathf.Clamp(rb.velocity.z, -moveSpeed, moveSpeed));
    }
}
