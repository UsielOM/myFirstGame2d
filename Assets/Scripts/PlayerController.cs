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
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;
    [SerializeField] private ParticleSystem dashParticles;

    // =====  Properties privates =====
    private float x = 0f;
    private Vector2 playerVelocity;
    private bool jumpPressed;
    private int currentExtraJumps;
    private bool justLand;

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

    private Vector2 externalForce;

    private Platform platform;


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
            playerVelocity = new Vector2(x * speed + externalForce.x, rb.linearVelocity.y + externalForce.y);
            Vector2 platformVelocity = platform != null ? platform.Velocity : Vector2.zero;

            float horizontalVelocity = playerVelocity.x  + platformVelocity.x;
            float verticalVelocity = slidingWall ? -wallSlideSpeed : rb.linearVelocity.y + externalForce.y  + platformVelocity.y;

            rb.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
        }

        externalForce = Vector2.zero;

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

    // ===== Functions Public =====
    public void Inpulse(Vector2 dir, float force, bool resetExtraJumps)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(dir *  force, ForceMode2D.Impulse);
        anim.SetBool("DoubledJumping", false);

        currentExtraJumps = resetExtraJumps ? 0 : extraJumps;
        coyoteTimeCounter = 0;
        jumpBufferCounter = 0;
    }

    public void AddExternalForce (Vector2 force)
    {
        externalForce += force;

    }

    public void  SetPlatform(Platform platform)
    {
        this.platform = platform; // se usa this para refrencias varibles qeu creamos como tienen el mismo nombre
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
                dashParticles.transform.localScale = Vector3.one; 
            }
            else if (x < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);// se puso un menos para tranformar la escal a anegativo sea el valor que sea
                dashParticles.transform.localScale = new Vector3(-1,1,1);
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
                    anim.SetBool("DoubledJumping", true);
                    anim.SetTrigger("dobleJump");
                }
                jumpParticles.Play();
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
            anim.SetBool("DoubledJumping", false);

            if (justLand)
            {
                landParticles.Play();
                justLand = false;
            }
       
        }
        else  
        {
            justLand = true;
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
        anim.SetBool("DoubledJumping", false);
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(direction * Mathf.Abs(scale.x), scale.y, scale.z);// Esto va a rotar al personaje  a la direccion del salto
        jumpParticles.Play();
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
      
            if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && !runParticles.isPlaying && IsGrounded() )  runParticles.Play();

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
        anim.SetFloat("HorVelocity", Mathf.Abs(playerVelocity.x));// seteamos el valor de  velocidad de x 
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
        dashParticles.Play();
        // Espera de 0.5f
        yield return new WaitForSeconds(dashDuration);
       

        //Desactivar el dash
        rb.gravityScale = originalGravity;
        dashing = false;
        dashParticles.Stop();
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

}
