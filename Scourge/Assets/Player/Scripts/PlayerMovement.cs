using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Walking")]
    [Tooltip("The maximum walk speed of the player")]
    public float maxSpeed;      //The maximum movement speed of the player
    [Tooltip("The linear acceleration towards the max speed")]
    public float acceleration;  //The acceleration of the player
    [Tooltip("The linear deceleration towards zero")]
    public float deceleration;  //The deceleration of the player

    [Header("Jump and Gravity")]
    [Tooltip("The initial speed of a jump")]
    public float jumpForce;     //The initial speed of a jump
    [Tooltip("The strength with which gravity pulls the player while in the air")]
    public float gravityStrength;   //The strength of the gravity pulling the player down. High gravity = fast falling and lower jumps
    [Tooltip("The maximum fall speed of the player")]
    public float terminalVelocity;  //The max fall speed of the player

    [Header("Colliders")]
    [Tooltip("The collider which represents the 'body' of the greater object")]
    public BoxCollider2D body;
    [Tooltip("The collider which represents the 'head' of the greater object")]
    public CircleCollider2D head;
    [Tooltip("The collider which represents the 'feet' of the greater object")]
    public CircleCollider2D feet;

    [Header("")]
    [Tooltip("The PlayerHunger script related to the player")]
    public PlayerHunger playerHungerScript;


    //Used to represent direction of input
    private int horizontal_input;   //1 = right, -1 = left
    //private int vertical_input;     //1 = up, -1 = down
    private int directionFacing = 1; //The direction the player is facing, -1 = left, 1 = right

    private bool grounded;  //Is the player touching the ground?

    private Vector2 velocity;    //The player's velocity

    private Rigidbody2D rb;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Resets input values
        horizontal_input = 0;
        //vertical_input = 0;

        Walk();
        JumpNGrav();

        rb.MovePosition(rb.position+(velocity*Time.deltaTime));  //Moves the player based on their velocity
    }

    void Walk()
    {
        //Checks for player input
        if (Input.GetKey(KeyCode.A))
        {
            horizontal_input--;
            directionFacing = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            horizontal_input++;
            directionFacing = 1;
        }


        if (horizontal_input != 0)
        {
            playerHungerScript.UseBlood(0.025f);
            velocity.x = Mathf.MoveTowards(velocity.x, maxSpeed * horizontal_input, acceleration * Time.deltaTime);    //Changes the horizontal velocity according to input
        }
        else
        {
            velocity.x = Mathf.MoveTowards(velocity.x, maxSpeed * horizontal_input, deceleration * Time.deltaTime);    //Changes the horizontal velocity according to input
        }
    }

    void JumpNGrav()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //vertical_input++;
            if (grounded)
            {
                playerHungerScript.UseBlood(30);
                velocity.y = jumpForce;
                grounded = false;
            }
            else
            {
                velocity.y -= gravityStrength * Time.deltaTime;
            }
        }
        else
        {
            if (!grounded)
            {
                velocity.y -= 1.4f*gravityStrength * Time.deltaTime;
            }
        }

        if (velocity.y < -terminalVelocity)
        {
            velocity.y = -terminalVelocity;
        }
    }

    public void InterruptMovement()
    {
        velocity = Vector3.zero;
    }

    public int GetDirectionFacing()
    {
        return directionFacing;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider == feet)
        {
            grounded = true;

        }
    }
}