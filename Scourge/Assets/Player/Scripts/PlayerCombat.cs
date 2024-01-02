using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Dash")]
    [Tooltip("The speed at which the player dashes")]
    public float dashSpeed; //The speed at which the player dashes
    [Tooltip("The damage the dash inflicts")]
    public float dashStrength;  //The damage the dash does
    [Tooltip("The distance the dash travels given no obstacles")]
    public float dashDistance;  //The distance the dash travels, given no obstacles
    //public float dashPiercing; //The number of creatures which the dash can pass through
    //public float dashImpactOfObstacles; //The impact that piercing through a creature has on the distance of your dash
    [Tooltip("The game object used as a body during a dash")]
    public GameObject dashBody;

    [Header("")]
    [Tooltip("The PlayerMovement script related to the player")]
    public PlayerMovement playerMovementScript;
    [Tooltip("The PlayerHunger script related to the player")]
    public PlayerHunger playerHungerScript;
    [Tooltip("The default, non-attacking body")]
    public GameObject baseBody;

    //Dash attack
    private bool playerFreeToDash;  //Whether or not the player should be allowed to perform a dash
    private bool dashing;   //Whether or not the player is currently dashing
    private float distanceDashed; //The distance dashed in an active dash
    private int dashInput;  //The direction of the dash: -1 for left, 1 for right


    private float currentAttackStrength;
    private Rigidbody2D rb;

    private List<Collider2D> exemptColliders = new();   //The colliders which should be exempt from action upon collision. Usually just colliders which have already been hit, i.e. preventing an attack from hurting the same target twice

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFreeToDash = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Dash();
    }

    private void Dash()
    {
        if (Input.GetKey(KeyCode.Slash) && playerFreeToDash)    //If able to dash and pressing the dash key
        {
            exemptColliders = new();
            currentAttackStrength = dashStrength;
            dashInput = playerMovementScript.GetDirectionFacing();   //The direction the player will dash in
            playerMovementScript.InterruptMovement();   //Resets the velocity of the player, so any motion they had before the dash wont be continued.
            playerMovementScript.enabled = false;   //Disables other player movement: dashes must always be seen through
            playerHungerScript.UseBlood(30);
            baseBody.SetActive(false);
            dashBody.SetActive(true);
            dashing = true;
            playerFreeToDash = false;   //Prevents player from dashing again mid-dash
            distanceDashed = 0; //Resets distance dashed
        }
        if (dashing)
        {
            if(distanceDashed + dashSpeed*Time.deltaTime < dashDistance)  //If an additional movement would not exceed the maximum travel distance of a dash
            {
                rb.MovePosition(rb.position + new Vector2(dashSpeed * Time.deltaTime, 0)*dashInput);  //Moves the player during a dash
                distanceDashed += dashSpeed * Time.deltaTime;
            }
            else    //End of dash
            {
                rb.MovePosition(rb.position + new Vector2(dashDistance-(distanceDashed+dashSpeed * Time.deltaTime), 0)*dashInput);  //Will occur on the final frame of a dash, this moves the player to exactly one dashDistance from their starting location
                playerMovementScript.enabled = true;
                baseBody.SetActive(true);
                dashBody.SetActive(false);
                dashing = false;
                playerFreeToDash = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Human" && !exemptColliders.Contains(collision.collider))
        {
            exemptColliders.Add(collision.collider);
            collision.collider.GetComponentInParent<HumanHealth>().Hurt(currentAttackStrength);
            playerHungerScript.DrinkBlood(currentAttackStrength);
        }
    }
}
