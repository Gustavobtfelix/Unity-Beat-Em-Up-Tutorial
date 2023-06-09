using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAdam : MonoBehaviour
{
    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;
    public int maxHealth = 10;
    public string playerName;
    public Sprite playerImage;

    private int currentHealth;
    private float currentSpeed;
    private Rigidbody rb;
    private Animator anim;
    private Transform groundCheck;
    private bool onGround;
    private bool isDead = false;
    private bool facingRight = true;
    private bool jump = false;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
    }

    
    void Update()
    {
        //valida se empty object GroundCheck esta em contato com o layer do chao
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //certifica que animacao vai rodar quando variavel for alterada
        anim.SetBool("OnGround", onGround);
        anim.SetBool("Dead", isDead);

        if(Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if(!onGround) //trava vertical durante pulo
                z = 0;

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed );

            if(onGround)
                anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

            if(h > 0 && !facingRight) 
            {
                Flip();
            }
            else if (h < 0 && facingRight)
            {
                Flip();
            }

            if(jump)
            {
                jump = false;
                rb.AddForce(Vector3.up * jumpForce);
            }
            //fixa posicao do jogador dentro do range da Camera main nas 4 direcoes
            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0,0,10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 1, maxWidth - 1),
                rb.position.y,
                Mathf.Clamp(rb.position.z, minHeight, maxHeight)
                );
        }
    }

    void Flip ()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }

    public void TookDamage(int damage)
    {
        if(!isDead)
        {
            currentHealth -= damage;
            anim.SetTrigger("HitDamage");
            FindObjectOfType<UIManager>().UpdateHealth(currentHealth);
        }
    }

}
