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

	public GameObject cam;

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
		Vector3 camF = Camera.main.transform.forward;
		camF = camF.normalized;
		Vector3 camR = Camera.main.transform.right;
		camR = camR.normalized; 


		Vector3 input = new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"),0);
		//input = Vector2.ClampMagnitude (input, 1);
		transform.position += (camR * input.x + camF * input.y) * moveSpeed* Time.deltaTime;
		transform.rotation = Quaternion.RotateTowards (transform.rotation, Camera.main.transform.rotation, Time.deltaTime * 1000f); 
			//Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (cam.transform.position.z,cam.transform.position.y), Time.deltaTime* 5f);
		transform.rotation = Quaternion.Euler (new Vector3 (0, transform.rotation.eulerAngles.y, 0));

		//rb.AddRelativeForce (camR.x * input.x, 0f, camF.y*input.y, ForceMode.Impulse);

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
