using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxSpeed;
    public float minHeight, maxHeight;
    public float damageTime = 0.5f;
    public int maxHealth;
    public float attackRate = 1f;
    public string enemyName;
    public Sprite enemyImage;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool facingRight = false;
    private Transform target;
    private bool isDead = false;
    private float zForce;
    private float walkTimer;
    private bool damaged = false;
    private float damageTimer;
    private float nextAttack;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<PlayerAdam>().transform;
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        //valida se empty object GroundCheck esta em contato com o layer do chao
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("Grounded", onGround);
        anim.SetBool("Dead", isDead);

        //valida se inimigo esta antes ou depois do jogador
        facingRight = (target.position.x < transform.position.x) ? false: true;
        if(facingRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if(damaged && !isDead)
        {
            //Tempo que um inimigo fica atordoado
            damageTimer += Time.deltaTime;
            if(damageTimer >= damageTime)
            {
                damaged = false;
                damageTimer = 0;
            }
        }

        walkTimer += Time.deltaTime;
    }

    private void FixedUpdate() 
    {
        if(!isDead)
        {
            Vector3 targetDistance = target.position - transform.position;
            float hForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            // a cada 1 e 2 segundos altera movimentacao
            if(walkTimer >= Random.Range(1f, 2f))
            {
                //baixo cima ou zerado
                zForce = Random.Range(-1, 3);
                walkTimer = 0;
            }

            //quando inimigo esta perto do player
            if(Mathf.Abs(targetDistance.x) <= 1.5f)
            {
                hForce = 0;
            }
            
            //inimigo nao se move quando esta tomando dano
            if(!damaged)
                rb.velocity = new Vector3(hForce * currentSpeed, 0, zForce * currentSpeed);

            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));


            //verifica se esta proximo do jogador e valida tempo do ultimo ataque
            if(Mathf.Abs(targetDistance.x) < 1.5f && Mathf.Abs(targetDistance.z) < 1.5f && Time.time > nextAttack)
            {
                anim.SetTrigger("Attack");
                currentSpeed = 0;
                nextAttack = Time.time + attackRate;
            }
        }

        //fixa posicao do jogador dentro do range da Camera main nas 4 direcoes
        rb.position = new Vector3
            (
            rb.position.x,
            rb.position.y,
            Mathf.Clamp(rb.position.z, minHeight, maxHeight)
            );
    }

    public void TookDamage(int damage)
    {
        if(!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            //mostra vida do inimigo no Canvas
            FindObjectOfType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyName, enemyImage);
            if(currentHealth <= 0)
            {
                //muda status e empurra inimigo
                isDead = true;
                rb.AddRelativeForce(new Vector3(3,5,0), ForceMode.Impulse);
            }
        }
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }
}
