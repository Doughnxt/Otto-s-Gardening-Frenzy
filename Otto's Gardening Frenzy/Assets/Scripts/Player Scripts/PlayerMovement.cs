using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement variables
    private float verDirection; // Variable for vertical direction
    private float horDirection; // Variable for horizontal direction
    [SerializeField] private float speed = 5; // Variable for speed; can be changed in the editor
    public bool movementEnabled = true; // Variable for if movement is enabled or not

    // Component variables
    public Rigidbody2D rb; // Rigidbody2D variable (controls physics)

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Sets rb to the RigidBody2D attached to this object
    }

    // Fixed Update is called every fixed framerarte frame
    // Used to make movement speed fixed across different framerates
    void FixedUpdate()
    {
        if (movementEnabled) // Checks if movement is enabled
        {
            Move(); // Calls move method
        }
    }

    // This method makes the player move around the screen based on their input
    private void Move()
    {
        verDirection = Input.GetAxis("Vertical"); // Uses input manager to set the vertical direction to a value between -1 and 1 depending on the vertical imput
        horDirection = Input.GetAxis("Horizontal"); // Uses input manager to set the horizontal direction to a value between -1 and 1 depending on the horizontal imput

        if (verDirection != 0 || horDirection != 0) // chceks if the player is trying to move in any direction, either horizontal or vertical
        {
            rb.velocity = new Vector2(horDirection * speed, verDirection * speed); // Moves player by setting their velocity to the speed defined earlier and direction in both the x and y axes
        }

        if (verDirection == 0 && horDirection == 0) // Checks if the player is not trying to move in any direction
        {
            rb.velocity = Vector2.zero; // Stops player by setting velocity to zero
        }
    }
}
