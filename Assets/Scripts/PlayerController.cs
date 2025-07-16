using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // ===== Serialized Field =====
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckRadius = 1f;

    [Header("Wall Check")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask whatIsWall;

    [Header("Movement")]
    [SerializeField] private float speed = 3f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private int extraJumps = 1;
    [SerializeField] private float extraJumpForce = 2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;// Mejora la experiencia de salto apra el usuario 

    [Header("Wall Slide")]
    [SerializeField] private float wallSlideSpeed = 0.5f;

    [Header("Wall Jump")]
    [SerializeField] private Vector2 wallJumpForce = new Vector2(5f, 8f);
    [SerializeField] private float wallJumpDuration;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.35f;
    [SerializeField] private float dashCooldown = 1f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem runParticles; //Se crea una varibale para almacenar particulas 


    // =====  Properties privates =====
    private float x = 0f;
    private bool jumpPressed;
    private int currentExtraJumps;

    private float coyoteTimeCounter;
    private bool inCoyoteTime => coyoteTimeCounter > 0;//Getter  este tendra el valor envace al resultado de la funcion flecha 
    private float jumpBufferCounter;
    private bool inJumpBuffer => jumpBufferCounter > 0;//Getter  este tendra el valor envace al resultado de la funcion flecha 

    private Rigidbody2D rb;
    private Animator anim;

    private bool slidingWall;
    private bool wallJumping;

    private bool dashing;
    private bool canDash = true;


    // =====  Unity Events =====
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
        if (!wallJumping && !dashing)
        {
            rb.linearVelocity = new Vector2(x * speed, slidingWall ? -wallSlideSpeed : rb.linearVelocity.y);
        }

    }

    void OnDrawGizmos()//al usar gismos // se va a dibujar una esfera 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);// En este punto se dibujo la esfera
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheck.position, wallCheck.localScale);
    }

    void Update()
    {
        DetectInput();
        JumpPlayer();
        HandelWallSlide();
        FlipPlayer();
        HandelDash();
        UpdateAnimatorParameters();
        HandleParticles();
    }



    // ===== Functions Private =====
    private void DetectInput()
    {
        x = Input.GetAxisRaw("Horizontal");// devuelve el valor en crudso en valores enteros
        jumpPressed = Input.GetButtonDown("Jump");
    }

    private void FlipPlayer()//flipep mediante la posision
    {
        if (!wallJumping)
        {
            Vector3 scale = transform.localScale;
            if (x > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);//Se hace uso de valor absoluto 
            }
            else if (x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);// se puso un menos para tranformar la escal a anegativo sea el valor que sea
            }
        }

    }

    private void JumpPlayer() // salto mediante addForce 
    {
        if (jumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        } else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (inJumpBuffer)
        {
            if ((inCoyoteTime || currentExtraJumps < extraJumps) && !slidingWall)
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
            } else if (slidingWall) {
                WallJump();
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

    private void WallJump()
    {
        wallJumping = true;
        int direction = transform.localScale.x > 0 ? -1 : 1;// esto nos va a permitir saltar a la direccion que corresponde 
        rb.linearVelocity = new Vector2(wallJumpForce.x * direction, wallJumpForce.y);// sE ESTA DANDO LA FUERZA DEL SALTO A LA DIRECCION QUE ESTE VIENDO
        currentExtraJumps = 0;


        Invoke(nameof(StopWallJump), wallJumpDuration);//invoked va ejecutar una fucnion despeus de cierto tiempo que lo indiquemos

        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(direction * -Mathf.Abs(scale.x), scale.y, scale.z);// Esto va a rotar al personaje  a la direccion del salto

        coyoteTimeCounter = 0;
        jumpBufferCounter = 0;
    }

    private void StopWallJump() {
        wallJumping = false;
    }
    private void HandelWallSlide() {
        if (IsWalled() && !IsGrounded() && x != 0 && rb.linearVelocity.y < -0.1f) {
            slidingWall = true;
        } else slidingWall = false;
    }

    private void HandelDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !slidingWall)
        {
            StartCoroutine(Dash());
        }
    }

    private void HandleParticles()
    {
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && !runParticles.isPlaying && IsGrounded()) runParticles.Play();

        else if ((Mathf.Abs(rb.linearVelocity.x) < 0.1f || !IsGrounded()) && runParticles.isPlaying) runParticles.Stop();

    }

    private bool IsGrounded ()
    {
        bool isGroundedPlayer = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);//Este va a dectectar si la esfera esta tocando la capa que le indiquemos se pasa la referencia de la esfera
        return isGroundedPlayer;
    }

    private bool IsWalled ()
    {
        bool isWalled = Physics2D.OverlapBox(wallCheck.position, wallCheck.localScale, 0, whatIsWall);
        return isWalled;
    }

    private void UpdateAnimatorParameters()
    {
        anim.SetFloat("HorVelocity", Mathf.Abs(rb.linearVelocity.x));// seteamos el valor de  velocidad de x 
        anim.SetFloat("VerVelocity", rb.linearVelocity.y);// seteamos el valor de  velocidad de y
        anim.SetBool("isGrounded", IsGrounded());//seteamos el valor falso o verdadero
        anim.SetBool("wallJump", slidingWall);
    }

    // ==== Corutina ====

    private IEnumerator Dash()
    {
        // Activar el dash
        dashing = true;
        canDash = false;

        //desactivar la gravedad para poder hacer el dash
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        //direccion
        int direction = transform.localScale.x > 0 ? 1 : -1;
        rb.linearVelocity = new Vector2 (direction * dashSpeed, 0);
        // Espera de 0.5f
        yield return new WaitForSeconds(dashDuration);
       

        //Desactivar el dash
        rb.gravityScale = originalGravity;
        dashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
