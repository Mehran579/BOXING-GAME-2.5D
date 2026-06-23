using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    Vector2 movement;       //for reading keyboard input
    Vector3 playermovement;  //for reading and writing player's current velocity
    float verticalvelocity; //to keep player grounded and applying jump.
    [Header("Movement Stats")] //Header for the movement stats in the inspector
    public float speed;
    public float jump;
    public float gravity = -9.8066f;

    void Start()
    {
        controller = GetComponent<CharacterController>(); //Reference to the player's character controller component
        animator = GetComponent<Animator>(); //Reference to the player's animator component
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();  //takes input from keyboard (x-z direction)
        if (context.performed)
        {
            animator.SetBool("IsWalking", true); //Triggers walking animation
        }
        if (context.canceled)
        {
            animator.SetBool("IsWalking", false); //Stops walking animation
        }
    }
    public void OnJump(InputAction.CallbackContext context) 
    {
        if(context.performed && controller.isGrounded)  //checks if the jump button is pressed and the player is on the ground
        {
            animator.SetTrigger("IsJumping"); //Triggers jump animation
            verticalvelocity = Mathf.Sqrt(jump * -2f * gravity);  //calculates the vertical velocity needed to jump using 3rd equation of motion
        }
    }

    void Update()
    {
        if(controller.isGrounded && verticalvelocity < 0) //checks if the player is on the ground and if the vertical velocity is less than 0
        {
            verticalvelocity = -2f; //sets the vertical velocity to a small negative value to keep the player grounded
        }
        verticalvelocity += gravity * Time.deltaTime;  //applies gravity to player;
        Vector3 move = new Vector3(movement.x * speed, verticalvelocity, 0); //final calculated movement vector;
        controller.Move(move * Time.deltaTime); //Applies the calculated movement to the player;

        //incase player collides with the walls of the ring
        if(controller.velocity.x == 0 && controller.isGrounded) 
        {
            animator.SetBool("IsWalking", false); 
        }
    }
}
