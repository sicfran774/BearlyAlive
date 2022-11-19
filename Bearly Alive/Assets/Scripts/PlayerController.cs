using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;

    public GameObject projectile;
    public HudManager hudManager;

    public AudioSource bulletSource;

    public int _MAX_AMMO = 15;
    int rounds;


    bool playerShot = false;
    bool playerRolled = false;
    bool playerRolling = false;
    public float rollSpeed = 750f;


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

        playerShot = true;
        StartCoroutine(FireRate());
    }


    // Update is called once per frame
    void Update()
    {
        FollowCursor();
        switch (playerRolling)
        {
            case false:
                //
                //HorizontalMovement();
                PlayerRollAbility();
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


    // Handles Player's movement and rotates player's sprite in direction of Input Axis
    // Vertical Direction is prioritized if player is holding down both vertical and horizontal
    // input
    void HorizontalMovement()
    {
        //float jumpAxis = Input.GetAxis("Jump");
        //if (jumpAxis > 0)
        //{
        //    PlayerRollAbility();
        //}

        if (!playerRolling)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            Vector2 currPosition = transform.position;
            Vector2 newPosition = movement * Time.deltaTime * walkSpeed + currPosition;

            player.MovePosition(newPosition);
        }

    }


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
        //Vector2 direction;
       
        float jumpAxis = Input.GetAxisRaw("Jump");
        if (jumpAxis > 0f)
        {
            //DO SOMETHING
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (!playerRolled)
            {
                print(movement);
                // Do Roll Animation for now teleport 
                rollSpeed = 750f;
                playerRolling = true;
                playerRolled = true;
            }
        }
    }

    // Handles Players dodge roll action. Player is not allowed to move when 
    // player is rolling. After rollSpeed reaches below 5f, player is allowed
    // to move and the cooldown timer begins.
    void HandleDodgeRoll()
    {
        Vector2 currPosition = transform.position;
        currPosition += movement * rollSpeed * Time.deltaTime;
        player.MovePosition(currPosition);

        rollSpeed -= rollSpeed * 10f * Time.deltaTime;
        if(rollSpeed < 5f)
        {
            playerRolling = false;
            Invoke("ResetDodgeRoll", 1.25f);
        }


    }


    // No in use
    void StartMovement()
    {
        playerRolling = false;
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
            Instantiate(projectile, SpawnProjectile(), transform.rotation);
            print("Bullet's Remaining: " + rounds);
        }

        if(rounds == 0)
        {
            print("Reloading");
            //Reload, wait 2 seconds
            Invoke("Reload", 2f);        
        }

    }

    // Computers Projectile's Spawn and direction to simulate bullets firing
    // out of player object. 
    Vector3 SpawnProjectile()
    {
        Vector3 projectileSpawn = transform.position;
        Vector3 direction = transform.rotation.eulerAngles;
        print(direction);
        if (direction.z == 90f)
        {
            projectileSpawn.x += -2f;
        }
        else if (direction.z == 270f)
        {
            projectileSpawn.x += 2f;
        }
        else if (direction.z == 180f)
        {
            projectileSpawn.y += -2f;
        }
        else if (direction.z == 0f)
        {
            projectileSpawn.y += 2f;
        }

        return projectileSpawn;
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


    // A Coroutine that Handles the basic weapon's firerate and weapon's functionality4
    // Input Fire1 responds to Left Ctrl and Mouse Click
    IEnumerator FireRate()
    {
        while (playerShot)
        {
            if (Input.GetButton("Fire1"))
            {
                FireWeapon();
                yield return new WaitForSeconds(0.7f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    // 
    void TurnOffFiring()
    {
        playerShot = false;
    }


}
