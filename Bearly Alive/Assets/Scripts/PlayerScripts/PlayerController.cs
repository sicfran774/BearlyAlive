using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player's Movement Speed
    public float walkSpeed = 5f;

    // Bullet Game Object
    public GameObject projectile;

    // Sword Game Object
    public GameObject sword;

    // HUD Manager for Player's Actions 
    public HudManager hudManager;

    // Spawn Point for Bullet game objects
    //public GameObject projectileSpawner;

    // Bullet Firing Sound
    public AudioSource bulletSource;

    public int _MAX_AMMO = 15;

    // Cooldown Value for Firing Weapon
    public float timeBetweenShots = 1f;

    // Cooldown Value for Using Slash Technique
    public float slashCooldown = 0.7f;
    int rounds;


    // Boolean for any technique being on cooldown
    // If true, prevent player from using other techniques
    bool techniqueCooldown = false;

    // Cooldown Value for DodgeRollAction
    public float dodgeRollCooldownTimer = 1.25f;

    // Boolean to handle players actions when player have used dodgeroll
    bool playerRolled = false;

    // Used to stopped player's movement, helps implementation of the DodgeRoll Action
    bool playerRolling = false;

    // The Speed of the DodgeRoll Action
    public float rollSpeed = 750f;
    // Variable to implement dodgeroll function
    float currRollSpeed;


    // Boolean to stop player object from following the cursor when 
    // player is currently performing slash
    bool slashing = false;

    // Vector used to computer player's new position based on WASD input
    Vector2 movement;

    Rigidbody2D player;
    Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        movement = Vector2.zero;
        rounds = GameManager.instance.bullets;

        //Refresh HUD at the start of the game
        hudManager.refresh();
    }


    // Update is called once per frame
    void Update()
    {
        if (!slashing)
        {
            FollowCursor();
        }

        switch (playerRolling)
        {
            case false:
                PlayerRollAbility();
                HandleSlashTechnique();
                HandleChiSpitTechnique();
                break;
            case true:
                HandleDodgeRoll();
                break;

        }
    }


    private void FixedUpdate()
    {
        HorizontalMovement();
    }


    // Handles Player's movement based on WASD Input Keys Pressed
    // If Player is performing Dodge roll, we prevent player from moving 
    // until action is completed.
    void HorizontalMovement()
    {
        if (!playerRolling)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            Vector2 currPosition = transform.position;
            Vector2 newPosition = movement * Time.deltaTime * walkSpeed + currPosition;

            player.MovePosition(newPosition);
        }
    }


    // Handles Player's aim by having player always follow the
    // position of the mouse cursor
    void FollowCursor()
    {

        //Get the Screen position of the mouse
        Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = (mouseOnScreen - transform.position).normalized;

        //Get the angle between the points
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + -90f));

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
                playerRolling = true; //Prevents movement
                playerRolled = true; // Prevents from other actions to be used
            }
        }
    }


    // Handles Player's slash technique.
    void HandleSlashTechnique()
    {
        if (Input.GetButton("Fire2") && !techniqueCooldown)
        {
            sword.SetActive(true);
            techniqueCooldown = true;
            slashing = true;
            Invoke("ResetTechniqueCooldown", slashCooldown);
            Invoke("ResetFollowCursor", slashCooldown - 0.4f);
        }
    }


    // Handles player's left click input to perform shooting
    // of the Chi Spit Technique
    void HandleChiSpitTechnique()
    {
        if (Input.GetButton("Fire1") && !techniqueCooldown)
        {
            FireWeapon();
            techniqueCooldown = true;
            Invoke("ResetTechniqueCooldown", timeBetweenShots);
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
        currRollSpeed -= currRollSpeed * 10f * Time.deltaTime;
        if(currRollSpeed < 5f)
        {
            playerRolling = false;
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



    // Handles Player's firing, Projectile Sound, reloading and Weapon's Ammo UI.
    // If the weapon has no ammo left, Reload Method is invoke after 2 second delay.
    // Use Left Crtl or Mouse Click to fire weapon
    void FireWeapon()
    {
        if (rounds > 0)
        {
            // Fire Bullet
            rounds--;
            bulletSource.Play();
            GameManager.instance.DecreaseAmmo(1);
            hudManager.refresh();
            Instantiate(projectile, transform.position, transform.rotation);
            print("Bullet's Remaining: " + rounds);
        }

        if(rounds == 0)
        {
            print("Reloading");
            //Reload, wait 2 seconds
            Invoke("Reload", 2f);        
        }

    }


    // Handles Weapon Reload for Basic Weapon
    void Reload()
    {
        rounds = GameManager.instance._MAX_AMMO;
        GameManager.instance.SetAmmo();
        hudManager.refresh();
        CancelInvoke("Reload");
        print("Bullet's Remaining: " + rounds);
    }
}
