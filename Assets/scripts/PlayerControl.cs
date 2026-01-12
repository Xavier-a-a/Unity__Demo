using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float jumpForce;
    public float probeLen;
    public bool grounded; //Only public for debugging puroses
    public LayerMask whatIsGround;
    public float walkSpeed;  
    public float maxWalk;
    public float turnSpeed;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Rigidbody rigi;
    private IA_PlayerInputs ctrl;
    private int KeyCount;
    public int minKeys;
    public TextMeshProUGUI displayKeys;
    public TextMeshProUGUI displayTime;
    public TextMeshProUGUI displayAmmo;
    private float timer;
    public bool isAlive;
    public GameObject restartBtn;
    public GameObject projectile;
    public float spawnDist;
    public int ammo;
    public int refillAmount;

    void Jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (isAlive)
        {
            if (grounded)
            {
                rigi.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void Fire(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (ammo > 0 && isAlive)
        {
        Instantiate(projectile, this.transform.position + this.transform.forward 
        * spawnDist, this.transform.rotation);
        ammo--;
        UpdateAmmo();
        }
    }

    void UpdateKeys()
    {
        displayKeys.text = KeyCount.ToString("00") + " / " + minKeys.ToString("00") + "keys";
    }

    void UpdateTimer()
        {
            displayTime.text = timer.ToString("000.00");
        }
    
    void UpdateAmmo()
    {
        displayAmmo.text = new string('|', ammo);
    }

    //awake is called when the object instantiates in the scene
    void Awake()
    {
            rigi = GetComponent<Rigidbody>();
            ctrl = new IA_PlayerInputs();
            ctrl.Enable();
            ctrl.Player.Jump.started += Jump;
            ctrl.Player.Attack.started += Fire;
            KeyCount = 0;
            UpdateKeys();
            timer = 0f;
            UpdateTimer();
            isAlive = true;
            restartBtn.SetActive(false);
            UpdateAmmo();
    }

    void OnDisable()
    {
            ctrl.Disable();
    }

    //fixedupdate is called at a regular interval (50 per second)
    void FixedUpdate()
    {
        if (isAlive)
        {
            grounded = Physics.Raycast(this.transform.position, Vector3.down, probeLen, whatIsGround);
        
            //read input from user
            moveInput = ctrl.Player.Move.ReadValue<Vector2>();
            rotateInput = ctrl.Player.Rotate.ReadValue<Vector2>();

            timer += Time.deltaTime;
            UpdateTimer();

            if (rotateInput.magnitude > 0.1f)
            {
                Vector3 angleVelocity = new Vector3(0f, rotateInput.x * turnSpeed, 0f);
                Quaternion deltaRot = Quaternion.Euler(angleVelocity * Time.deltaTime);
                rigi.MoveRotation(rigi.rotation * deltaRot);
            }

            if (moveInput.magnitude > 0.1f) 
            {
                Vector3 moveForward = moveInput.y * this.transform.forward;
                Vector3 moveRight = moveInput.x * this.transform.right;
                Vector3 moveVector = moveForward + moveRight;
                rigi.AddForce(moveVector * walkSpeed * Time.deltaTime);

                rigi.linearVelocity = Vector3.ClampMagnitude(rigi.linearVelocity, maxWalk);
            }
        }
    }
    //if movement input, put together vector and move player

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "key")
        {
            KeyCount++;
            //Debug.Log(KeyCount);
            UpdateKeys();
            Destroy(other.gameObject);
        }

        else if (other.transform.tag == "Refill")
        {
            ammo += refillAmount;
            Destroy(other.gameObject);
            UpdateAmmo();
        }

        else if (other.transform.tag == "Finish")
        {
            if (KeyCount < minKeys)
            {
                Debug.Log("Collet more keys to exit");
            }
            else
            {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision");
        if(other.transform.tag == "Enemy")
        {
            Debug.Log("hit");
            isAlive = false;
            restartBtn.SetActive(true);
        }
    }
}
