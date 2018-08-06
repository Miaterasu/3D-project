using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject weapon;

    public float moveSpeed = 5f;
    public float jumpForce = 100f;
    public float attackCooldown = 0.1f;

    public int numberOfJumps = 1;

    Rigidbody rb;

    float attackCooldownTimer = 0;

    bool attacking = false;
	
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
        //Movimenta o personagem com as setas e WASD
        rb.AddRelativeForce (Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"), ForceMode.Impulse);

        //Clampa a velocidade pra não sair Sanicando por aí
        rb.velocity = new Vector3 (Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed),
                                   Mathf.Clamp(rb.velocity.y, -moveSpeed, moveSpeed),
                                   Mathf.Clamp(rb.velocity.z, -moveSpeed, moveSpeed));

        //Pula
        if(Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0)
        {
            rb.AddRelativeForce(0f, jumpForce, 0f, ForceMode.Impulse);
        }

        //Ataca
        Attack();
        

    }

    void Attack()
    {
        Debug.Log(attacking);

        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            attacking = true;
        }
        
        if(attacking)
        {
            weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, transform.position + (transform.forward + transform.right) * 0.8f, Time.deltaTime * 10);
            attackCooldownTimer += Time.deltaTime;
        }
        else
        {
            weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, transform.position + transform.right * 0.8f, Time.deltaTime * 100);
        }

        if (attackCooldownTimer >= attackCooldown)
        {
            attacking = false;
            attackCooldownTimer = 0f;
        }
    }
}
