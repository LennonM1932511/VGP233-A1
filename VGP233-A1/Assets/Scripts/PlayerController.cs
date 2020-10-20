using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum JumpState
    {
        Grounded, Jumping, DoubleJumping
    }

    public float speed = 20.0f;
    public float jumpForce = 200.0f;
    public int playerScore = 0;
    public int playerLives = 3;

    private Vector3 startPosition;
    private Rigidbody rb;
    public GameObject uiManager;
    private JumpState _jumpState = JumpState.Grounded;

    private void Awake()
    {
        UIManager uiMngr = uiManager.GetComponent<UIManager>();
        ServiceLocator.Register<UIManager>(uiMngr);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = new Vector3();
        startPosition = rb.position;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();        
    }

    private void Move()
    {
        // Set local vars to axis values
        // Create V3 and assign X and Z to horizontal and vertical

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);        

        rb.AddForce(movement * speed);
    }

    private void Jump()
    {   
        float jumpVal = Input.GetButtonDown("Jump") ? jumpForce : 0.0f;

        switch (_jumpState)
        {
            case JumpState.Grounded:
                if (jumpVal > 0.0f)
                {
                    _jumpState = JumpState.Jumping;
                }
                break;
            case JumpState.Jumping:
                if (jumpVal > 0.0f)
                {                    
                    _jumpState = JumpState.DoubleJumping;
                }
                break;
            case JumpState.DoubleJumping:
                jumpVal = 0.0f;
                break;
            default:
                break;
        }

        Vector3 jumping = new Vector3(0.0f, jumpVal ,0.0f);
        rb.AddForce(jumping);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_jumpState == JumpState.Jumping || _jumpState == JumpState.DoubleJumping) 
            && collision.gameObject.CompareTag("Ground"))
        {
            _jumpState = JumpState.Grounded;
        }
        
        // Deduct a life and reset player position/velocity
        if (collision.gameObject.CompareTag("Wall"))
        {
            playerLives -= 1;
            ServiceLocator.Get<UIManager>().UpdateLivesDisplay(playerLives);
            
            rb.position = startPosition;
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            PickUp pickup = other.gameObject.GetComponent<PickUp>();
            if (pickup != null)
            {
                playerScore += pickup.Collect();
                ServiceLocator.Get<UIManager>().UpdateScoreDisplay(playerScore);
            }
        }
    }
}
