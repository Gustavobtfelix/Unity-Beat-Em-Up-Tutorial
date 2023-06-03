using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxSpeed;
    public float minHeight, maxHeight;

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
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        target = FindObjectOfType<PlayerAdam>().transform;
    }

    
    void Update()
    {
        //valida se empty object GroundCheck esta em contato com o layer do chao
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        anim.SetBool("Grounded", onGround);

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

            rb.velocity = new Vector3(hForce * currentSpeed, 0, zForce * currentSpeed);

            anim.SetFloat("Speed", Mathf.Abs(currentSpeed));
        }

        //fixa posicao do jogador dentro do range da Camera main nas 4 direcoes
        rb.position = new Vector3
            (
            rb.position.x,
            rb.position.y,
            Mathf.Clamp(rb.position.z, minHeight, maxHeight)
            );
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }
}
