using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

    private float runSpeed = 20f;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    private float crouchToRunTimer = 0f;
    private bool crouchToRunTimerStart = false;

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        // If shift is held down, player will speed will increase to 40f 
        // TODO: Add comments here explaining this or refactor to make it more readable
        if (Input.GetButton("Run"))
        {
            runSpeed = 40f;
            if (crouchToRunTimerStart == true)
            {
                if (crouchToRunTimer >= 0.5f)
                {
                    crouchToRunTimer = 0f;
                    crouchToRunTimerStart = false;
                }
                else
                {
                    runSpeed = 20f;
                    crouchToRunTimer += Time.deltaTime;

                }
            }
            if (crouch == true && horizontalMove == 0)
            {
                runSpeed = 20f;
                crouchToRunTimerStart = true;
            }
            else
            {

                crouch = false;
            }
        }
        else if (Input.GetButtonUp("Run"))
        {

            runSpeed = 20f;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = !crouch;
        }

    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        crouch = false;
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }

    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}