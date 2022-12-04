using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    // Player's Movement Speed
    public float walkSpeed = 5f;

    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;



    // HUD Manager for Player's Actions 
    public HudManager hudManager;


   // Cooldown Value for Using Slash Technique
    public float slashCooldown = 0.7f;
    int rounds;
    
    // Used for animating
    public bool isMoving {
        get;
        private set;
    }


    // Boolean for any technique being on cooldown
    // If true, prevent player from using other techniques
    bool techniqueCooldown = false;

    // Cooldown Value for DodgeRollAction
    public float dodgeRollCooldownTimer = 1.25f;

    // Boolean to handle players actions when player have used dodgeroll
    bool playerRolled = false;

    // Used to stopped player's movement, helps implementation of the DodgeRoll Action
    bool playerRolling = false;

    // Used to prevent player from taking damage when performing DodgeRoll Action
    bool isInvulnerable = false;

    public float dodgeRollDuration = 1f;

    // The Speed of the DodgeRoll Action
    public float rollSpeed = 200f;
    // Variable to implement dodgeroll function
    float currRollSpeed;


    // Boolean to stop player object from following the cursor when 
    // player is currently performing slash
    bool slashing = false;

    // Boolean to mark death for animation
    // player is currently alive
    public bool isDead = false;

    //CHANGED TO PUBLIC SO I CAN USE IN GAMEMANAGER
    // Variables to hold the two known player actions
    public Technique[] techniques {
            get ; 
            private set;
            }

    // Vector used to computer player's new direction based on WASD input
    Vector2 movement;

    // physics components
    Rigidbody2D player;
    Collider2D coll;

    //Text object when upgrade is collided 
    [SerializeField]
    private Text pressFLabel;

    [SerializeField]
    private Text pressTLabel;

    //Upgrade UI attributes
    public bool canpickupUpgrade;

    public bool canpickupTechnique;

    //public string pickedUpgrade;
    public GameObject pickedUpgrade;

    public GameObject pickedTechnique;


    //public Animation reference
    private AnimatedSprite animating;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animating = GetComponent<AnimatedSprite>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);


        movement = Vector2.zero;
        rounds = GameManager.instance.bullets;

        //Method called when delegate is invoked 
        UpgradeUI.instance.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        TechniqueUIManager.instance.onToggleWeaponMenu += OnWeaponMenuToggle;

        //Refresh HUD at the start of the game
        hudManager.refresh();


        //For testing: give player starting moves
        //LearnTechnique<Slingshot>(1);
        //LearnTechnique<Slash>(1);
        LearnTechnique<Slingshot>(2);
        LearnTechnique<ChiSpit>(1);
        //LearnTechnique<Boomerang>(2);

        //Upgrade pick up attributes 
        pressFLabel.enabled = false;
        canpickupUpgrade = false;
        pickedUpgrade = null;

        canpickupTechnique = false;
        pickedTechnique = null;
        pressTLabel.enabled = false;
    }

    private void Awake()
    {
        techniques = new Technique[2];
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        print(isMoving);
        if (!slashing)
        {
            FollowCursor();
        }

        DoActions();
    }

    //Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {
        //Disable player movement and shooting
        GetComponent<PlayerController>().enabled = !active;

        if (GetComponent<Technique>() != null)
        {
            GetComponent<Technique>().enabled = !active;
        }
    }


    void OnWeaponMenuToggle(bool active)
    {
        //Disable player movement and shooting
        GetComponent<PlayerController>().enabled = !active;

        if (GetComponent<Technique>() != null)
        {
            GetComponent<Technique>().enabled = !active;
        }
    }


    private void FixedUpdate()
    {
        HorizontalMovement();
    }

    // Checks inputs mapped to actions then calls Act() if all conditions are met
    private void DoActions() {
        if (!playerRolling)
        {
            if (!Technique.cursorLock && !Technique.moveLock)
            {
                PlayerRollAbility();

            }
            if (Input.GetButton("Fire1") && techniques[0] != null)
            {
                techniques[0].Act();
            }
            if (Input.GetButton("Fire2") && techniques[1] != null)
            {
                techniques[1].Act();
            }
            //if (!Technique.techsCooling)
            //{
            //    PlayerRollAbility();
            //}

        }
        else{

        }

    }

    // Handles Player's movement based on WASD Input Keys Pressed
    // If Player is performing Dodge roll, we prevent player from moving 
    // until action is completed.
    void HorizontalMovement()
    {
        if (!playerRolling && !Technique.moveLock)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            if (Mathf.Abs(movement.x) > 0)
            {
                animating.enabled = true;
            }

            Vector2 currPosition = transform.position;
            Vector2 displacement = movement * Time.deltaTime * walkSpeed;
            Vector2 newPosition =  displacement + currPosition;

            // player is moving if displacement is not zero
            isMoving = (displacement != Vector2.zero);

            player.MovePosition(newPosition);
        }
    }


    // Handles Player's aim by having player always follow the
    // position of the mouse cursor
    void FollowCursor()
    {
        if (!Technique.cursorLock) {
            //Get the Screen position of the mouse
            Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 rotation = (mouseOnScreen - transform.position).normalized;

            //Get the angle between the points
            float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + -90f));
            }
    }


    // Handles the implementation of the player's ability: Dodge Roll
    // 2 Second Cooldown between uses
    void PlayerRollAbility()
    {
       
        float jumpAxis = Input.GetAxisRaw("Jump");
        if (jumpAxis > 0f)
        {
            //DO SOMETHING
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (!playerRolled)
            {
                // Do Roll Animation for now teleport 
                currRollSpeed = rollSpeed;
                isInvulnerable = true;
                playerRolling = true; //Prevents movement
                playerRolled = true; // Prevents from other actions to be used
                StartCoroutine(PerformDodgeRoll(dodgeRollDuration));
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Technique")
        {
            //Display label on UI
            pressTLabel.enabled = true;

            //Player can now pick up upgrade
            canpickupTechnique = true;

            //Assign picked upgrade for upgrade menu UI
            pickedTechnique = collision.gameObject;

        }

        if (collision.gameObject.tag == "Upgrade")
        {
            print("ENTERED Upgrade");

            //Display label on UI
            pressFLabel.enabled = true;

            //Player can now pick up upgrade
            canpickupUpgrade = true;

            //Assign picked upgrade for upgrade menu UI
            pickedUpgrade = collision.gameObject;

        }

        if (collision.gameObject.tag == "EnemyBullet")
        {
            if (isInvulnerable)
            {
                //NOTHING HAPPENS
            }
            else
            {
                healthBar.TookDamage(5);
                if (healthBar.currentHealth <= 0)
                {
                    isDead = true;
                    GetComponent<AnimatedSprite>().enabled = false;
                    // TODO GetComponent<DeathAnimation>().enabled = true;
                    print("YOU HAVE DIED!");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Enabel text when player collides with upgrade
        if (collision.gameObject.tag == "Upgrade")
        {
            pressFLabel.enabled = false;

            pickedUpgrade = null;

            canpickupUpgrade = false;

        }
        else if(collision.gameObject.tag == "Technique")
        {
            pressTLabel.enabled = false;

            pickedTechnique = null;

            canpickupTechnique = false;
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if(other.gameObject.tag == "Technique")
        {
            if (Input.GetKey(KeyCode.E))
            {
                LearnTechnique<Slash>(1);
                Destroy(other.gameObject);
            }
        }

    }


    // Resets the cooldown boolean variable. Called by Invoke Function
    // from specific time.
    void ResetTechniqueCooldown()
    {
        techniqueCooldown = false;
    }


    
    // Resets the slashing boolean variable to have player object follow the cursor.
    // Called by Invoke Function inside HandleSlashTechnique
    void ResetFollowCursor()
    {
        slashing = false;
    }



    // Handles Players dodge roll action. Player is not allowed to move when 
    // player is rolling. After rollSpeed reaches below 5f, player is allowed
    // to move and the cooldown timer begins.
    void HandleDodgeRoll()
    {
        Vector2 currPosition = transform.position;
        currPosition += movement * currRollSpeed * Time.deltaTime;
        player.MovePosition(currPosition);
        currRollSpeed -= currRollSpeed * dodgeRollDuration * Time.deltaTime;
        if(currRollSpeed < 5f)
        {
            playerRolling = false;
            isInvulnerable = false;
            // Player has stopped rolling, startTimer to reset cooldown
            Invoke("ResetDodgeRoll", dodgeRollCooldownTimer);
        }
    }



    IEnumerator PerformDodgeRoll(float duration)
    {
        float startRotation = transform.eulerAngles.x;
        float endRotation = startRotation - 360f;

        float yRot = transform.eulerAngles.y;
        float yEndRot = yRot - 360f;


        float t = 0.0f;


        movement = movement.normalized;

        while (t < duration)
        {
            Vector2 currPosition = transform.position;
            currPosition += movement * rollSpeed * Time.deltaTime;

            float xRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360f;
            float yRotation = Mathf.Lerp(yRot, yEndRot, t / duration) % 360f;

            if(Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            }
            else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
            {
                transform.eulerAngles = new Vector3(xRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else
            {
                transform.eulerAngles = new Vector3(xRotation, yRotation, transform.eulerAngles.z);
            }



            player.MovePosition(currPosition);

            t += Time.deltaTime;

            yield return null;
        }

        isInvulnerable = false;
        playerRolling = false;
        Invoke("ResetDodgeRoll", dodgeRollCooldownTimer);
    }




    // Called by Invoke Method to allow player
    // to perform dodge roll
    void ResetDodgeRoll()
    {
        playerRolled = false;
    }
///////////////////////////////////////////////
// these methods teach the player different techniques and upgrades. Use when collecting loot.
/////////////////////////////////////////////////

    // places an instance of the perameterized technique into a technique slot. slot can be 1 or 2.
    public void LearnTechnique<T>(int slot) where T : Technique {
        techniques[slot-1] = gameObject.AddComponent<T>() as T;
        techniques[slot-1].Initialize();
    }


    // gives upgrade to technique in slot. Can override old upgrades.
    // upgrade string can be: "none", "poison", "fire", "reflect",
    public void setUpgrade(int slot, string upgrade) {
        if (slot == 1 || slot == 2) {
            techniques[slot-1].SetUpgrade(upgrade);
        }
    }


}
