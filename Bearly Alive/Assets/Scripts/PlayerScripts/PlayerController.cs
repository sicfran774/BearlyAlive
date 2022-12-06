/***************************************************************
*File: PlayerController.cs
*Author: Radical Cadavical
*Class: CS 4700 – Game Development
*Assignment: Program 4
*Date last modified: 12/5/2022
*
*Purpose: This program handles player movement,
*handles player's health bar, handles collisions and triggers 
*for the player, and stores player's equipped techniques.
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerController))]

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance = null;

    // Player's Movement Speed
    public float walkSpeed = 5f;

    // Player's Health Bar max value
    public int maxHealth = 100;
    private int currentHealth;
    public HealthBar healthBar;


    // HUD Manager for Player's Actions 
    public HudManager hudManager;

    // Keeps tracks of bullets remaining
    int rounds;
    
    // Used for animating
    public bool isMoving {
        get;
        private set;
    }

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
    public float rollSpeed = 50f;
    // Variable to implement dodgeroll function
    float currRollSpeed;

    // Used to implement dodge roll ability
    float time = 0f;
    float startRotation;
    float endRotation;


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
    private Text pressEForUpgradeLabel;

    [SerializeField]
    private Text pressEForTechLabel;

    //Upgrade UI attributes
    public bool canpickupUpgrade;

    public bool canpickupTechnique;

    //public string pickedUpgrade;
    public GameObject pickedUpgrade;

    public GameObject pickedTechnique;



    //public Animation reference
    private AnimatedSprite animating;


    //function: Start()
    //purpose: Initializes player's health bar, toggle menu function,
    //rigidbody, collider, animation sprite, resets player's ability to
    //pickup techniques and upgrades
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
        //LearnTechnique<Slingshot>(2);
        //LearnTechnique<Whip>(1);
        //LearnTechnique<ChiSpit>(2);
        //LearnTechnique<Boomerang>(1);

        //Upgrade pick up attributes 
        pressEForUpgradeLabel.enabled = false;
        canpickupUpgrade = false;
        pickedUpgrade = null;

        canpickupTechnique = false;
        pickedTechnique = null;
        pressEForTechLabel.enabled = false;
    }


    //function: Awake, Unity Function
    //purpose: Initializes player's technique array, and player
    // instance
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
        DoActions();
    }


    //function: OnWeaponMenuToggle
    //purpose: Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {
        //Disable player movement and shooting
        GetComponent<PlayerController>().enabled = !active;

        if (GetComponent<Technique>() != null)
        {
            GetComponent<Technique>().enabled = !active;
        }
    }

    //function: OnWeaponMenuToggle
    //purpose: Handle what happens when weapon swap menu is toggled  
    void OnWeaponMenuToggle(bool active)
    {
        //Disable player movement and shooting
        GetComponent<PlayerController>().enabled = !active;

        if (GetComponent<Technique>() != null)
        {
            GetComponent<Technique>().enabled = !active;
        }
    }


    //function: FixedUpdate, Unity Function
    //purpose: Updates the player's movement every fixed frame-rate
    //frame
    private void FixedUpdate()
    {
        HorizontalMovement();
    }



    //function: DoActions
    //purpose: Checks inputs mapped to actions then calls Act() if all conditions are met
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

        }
        else{
            HandleDodgeRoll();
        }

    }


    //function: HorizontalMovement
    //purpose: Handles Player's movement based on WASD Input Keys Pressed
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
            Vector2 displacement = movement * Time.fixedDeltaTime * walkSpeed;
            Vector2 newPosition =  displacement + currPosition;

            // player is moving if displacement is not zero
            isMoving = (displacement != Vector2.zero);

            if (!Technique.rotationLock)
            {
                // set sprite rotation based on displacement
                if (movement.x > 0f)
                {
                    transform.eulerAngles = Vector3.zero;
                }
                else if (movement.x < 0f)
                {
                    transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }
            }


            //Play walking audio 
            //SoundManager.instance.playWalkSound();

            player.MovePosition(newPosition);
        }
    }


    //function: PlayerRollAbility
    //purpose: Implements the player's horizontal movement by
    //applying the correct speed based on input controls. Checks
    //if player is powered up and applies the powerup's speed
    //mulitplier 
    void PlayerRollAbility()
    {
       
        float jumpAxis = Input.GetAxisRaw("Jump");
        if (jumpAxis > 0f)
        {
            //DO SOMETHING
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement = movement.normalized;
            if (!playerRolled && (movement.x != 0 || movement.y != 0))
            {
                // Do Roll Animation for now teleport 
                currRollSpeed = rollSpeed;
                isInvulnerable = true;
                playerRolling = true; //Prevents movement
                playerRolled = true; // Prevents from other actions to be used\

                time = 0f;
                
                startRotation = transform.eulerAngles.x;
                endRotation = startRotation - 360f;

            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Technique")
        {
            //Display label on UI
            pressEForTechLabel.enabled = true;

            //Player can now pick up upgrade
            canpickupTechnique = true;

            //Assign picked upgrade for upgrade menu UI
            pickedTechnique = collision.gameObject;

        }

        if (collision.gameObject.tag == "Upgrade")
        {
            print("ENTERED Upgrade");

            //Display label on UI
            pressEForUpgradeLabel.enabled = true;

            //Player can now pick up upgrade
            canpickupUpgrade = true;

            //Assign picked upgrade for upgrade menu UI
            pickedUpgrade = collision.gameObject;

        }

        if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.name == "EnemyBullet")
        {
            if (isInvulnerable)
            {
                //NOTHING HAPPENS
            }
            else
            {
                //play audio clip 
                SoundManager.instance.playHitSound();

                healthBar.TookDamage(5);
                if (healthBar.currentHealth <= 0)
                {
                    isDead = true;
                    GetComponent<AnimatedSprite>().enabled = false;
                    // TODO GetComponent<DeathAnimation>().enabled = true;
                    print("YOU HAVE DIED!");

                    //Stop gameplay music
                    GameManager.instance.GetComponent<AudioSource>().Stop();

                    //Load game over scene
                    SceneManager.LoadScene("GameOver");
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Enabel text when player collides with upgrade
        if (collision.gameObject.tag == "Upgrade")
        {
            pressEForUpgradeLabel.enabled = false;

            pickedUpgrade = null;

            canpickupUpgrade = false;

        }
        else if(collision.gameObject.tag == "Technique")
        {
            pressEForTechLabel.enabled = false;

            pickedTechnique = null;

            canpickupTechnique = false;
        }


    }


    //function: HandleDodgeRoll
    //purpose: Handles Players dodge roll action. Player is not allowed to move when 
    // player is rolling. After rollSpeed reaches below 5f, player is allowed
    // to move and the cooldown timer begins.
    void HandleDodgeRoll()
    {
        if (time < dodgeRollDuration)
        {
            Vector2 currPosition = transform.position;
            currPosition += movement * rollSpeed * Time.deltaTime;

            // calculate rotation about z
            float zRotation = Mathf.Lerp(startRotation, endRotation, time / dodgeRollDuration) % 360f;

            // apply rotation about z
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);

            // apply movemnt
            player.MovePosition(currPosition);

            time += Time.deltaTime;
        }
        else
        {
            playerRolling = false;
            isInvulnerable = false;

            // Player has stopped rolling, startTimer to reset cooldown
            Invoke("ResetDodgeRoll", dodgeRollCooldownTimer);
        }

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
