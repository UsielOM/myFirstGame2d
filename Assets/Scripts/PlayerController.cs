using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private float extraJumpForce = 2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;// Mejora la experiencia de salto apra el usuario 

    private float x = 0f;
    private bool jumpPressed;
    private int currentExtraJumps;

    private float coyoteTimeCounter;
    private bool inCoyoteTime => coyoteTimeCounter > 0;//Getter  este tendra el valor envace al resultado de la funcion flecha 
    private float jumpBufferCounter;
    private bool inJumpBuffer => jumpBufferCounter > 0;//Getter  este tendra el valor envace al resultado de la funcion flecha 



    private Rigidbody2D rb;
    private Animator anim;
   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

     void FixedUpdate()//Motor de fisicas
    {
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

     void OnDrawGizmos()//al usar gismos // se va a dinuajr una esfera 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);// En este punto se dibujo la esfera 
    }

    // Update is called once per frame
    void Update()
    {
        DetectInput();
        JumpPlayer();
        FlipPlayer();
        UpdateAnimatorParameters();
    }


    private void DetectInput()
    {
        x = Input.GetAxis("Horizontal");
        jumpPressed = Input.GetButtonDown("Jump");
    }


    private void FlipPlayer()//flipep mediante la posision
    {
        Vector3 scale = transform.localScale;
        if(x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);//Se hace uso de valor absoluto 
        } else if (x < 0) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);// se puso un menos para tranformar la escal a anegativo sea el valor que sea
        }
    }

    private void JumpPlayer () // salto mediante addForce 
    {
        if (jumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        } else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if(inJumpBuffer)
        {
            if (inCoyoteTime || currentExtraJumps < extraJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * (inCoyoteTime ? jumpForce : extraJumpForce), ForceMode2D.Impulse);
                if (!inCoyoteTime)
                {
                    currentExtraJumps++;//COYOTE TIME PERMITE SALTAR MIENTRAS ESTAS EN AL AIRE POR UNOS SEGUNDOS PEQUEÑOS 
                    anim.SetTrigger("dobleJump");
                } else anim.SetTrigger("Jump");
                
                    coyoteTimeCounter = 0;
                jumpBufferCounter = 0;
               
            }
        }
        if (IsGrounded() && rb.linearVelocity.y < 0.1f)
        {
            currentExtraJumps = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
       
    }

    private bool IsGrounded ()
    {
        bool isGroundedPlayer = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);//Este va a dectectar si la esfera esta tocando la capa que le indiquemos se pasa la referencia de la esfera
        return isGroundedPlayer;
    }


    private void UpdateAnimatorParameters()
    {
        anim.SetFloat("HorVelocity", Mathf.Abs(rb.linearVelocity.x));// seteamos el valor de  velocidad de x 
        anim.SetFloat("VerVelocity", rb.linearVelocity.y);// seteamos el valor de  velocidad de y
        anim.SetBool("isGrounded", IsGrounded());//seteamos el valor falso o verdadero
    }
}
