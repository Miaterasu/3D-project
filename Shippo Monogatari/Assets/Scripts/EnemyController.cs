using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public GameObject player;
    public GameObject weapon;

    public Text uiText;

    public float speed = 5f;
    public float attackDistance = 3f;
    public float attackCooldown = 0.5f;
    public float knockBackPower = 5f;
    public float immuneDuration = 0.5f;
    public bool knockback = false;  

    public int hp = 50;
    public int power = 3;

    NavMeshAgent agent;
    Rigidbody rb;

    float attackCooldownTimer = 0f;

    float immuneTimer = 0;

    bool attacking = false;
    bool immune = false;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        uiText.text = "Hp Enemy: " + hp;

        if(attacking)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }

        agent.destination = player.transform.position;

        float distanceToPlayer = Mathf.Abs((transform.position - player.transform.position).magnitude);
        if(distanceToPlayer <= attackDistance && !attacking)
        {
            attacking = true;
        }

        Attack();
        Immunity();
        Debug.Log(hp);
	}

    void Attack()
    {
        if (attacking)
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
        if (immuneTimer >= immuneDuration)
        {
            immuneTimer = 0f;
            immune = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAttack"))
        {
            if (!immune)
            {   
                hp -= other.gameObject.GetComponentInParent<PlayerController>().power;
                knockback = false;
                rb.AddForce(transform.forward * -knockBackPower, ForceMode.Acceleration);
                immune = true;
            }
        }
    }
}
