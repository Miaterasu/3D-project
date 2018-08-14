using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public GameObject weapon;

    public Text uiText;

    public float moveSpeed = 5f;
    public float jumpForce = 100f;
    public float attackCooldown = 0.1f;
    public float knockBackPower = 150f;
    public float immuneDuration = 0.5f;

    public int hp = 100;
    public int power = 5;
    public int numberOfJumps = 1;

    Rigidbody rb;

    float attackCooldownTimer = 0;
    float immuneTimer = 0;

    bool attacking = false;
    bool immune = false;
	
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        uiText.text = "Hp Player: " + hp;

        MovementController();
        //Ataca
        Attack();
        Immunity();
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
        //Debug.Log(hp);

    }

    void Attack()
    {
        
        if (Input.GetMouseButtonDown(0) && !attacking)
        {
            attacking = true;
        }
        
        if(attacking)
        {
            weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, transform.position + (transform.forward + transform.right) * 0.8f, Time.deltaTime * 10);
            attackCooldownTimer += Time.deltaTime;
            weapon.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, transform.position + transform.right * 0.8f, Time.deltaTime * 100);
            weapon.GetComponent<BoxCollider>().enabled = false;
        }

        if (attackCooldownTimer >= attackCooldown)
        {
            attacking = false;
            attackCooldownTimer = 0f;
        }

    }

    void Immunity()
    {
        if (!immune)
        {
            return;
        }

        immuneTimer += Time.deltaTime;
        if(immuneTimer >= immuneDuration)
        {
            immuneTimer = 0f;
            immune = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyAttack"))
        {
            if (!immune)
            { 
                hp -= other.gameObject.GetComponentInParent<EnemyController>().power;
                rb.AddForce(transform.forward * -knockBackPower, ForceMode.Impulse);
                immune = true;
            }
        }
    }
}
