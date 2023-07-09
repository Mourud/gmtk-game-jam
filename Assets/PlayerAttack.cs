using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    public LineRenderer lineRenderer;
    bool isAiming = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        lineRenderer.positionCount = 2; // Start and end points
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>().Where(x => x.gameObject != gameObject).ToArray();
        if (Input.GetButton("Fire1") && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || animator.GetCurrentAnimatorStateInfo(0).IsName("GunFireArmlessAim")))
        {

            isAiming = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = true;
            }
            // Set animation trigger
            animator.SetBool("GunAim", true);
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            AimAtMouse();


        }
        else if (Input.GetButtonUp("Fire1"))
        {
            gameObject.GetComponent<PlayerMovement>().enabled = true;
            // Shoot gun
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.enabled = false;
            }
            if (isAiming)
            {
                animator.SetBool("GunAim", false);
                animator.SetTrigger("GunShoot");
                isAiming = false;
            }
            //animator.SetTrigger("GunShoot");
        }


    }

    private void AimAtMouse()
    {
        Transform arm = gameObject.transform.Find("Arm");
        GameObject attachedGameObject = arm.gameObject;
        Vector2 spriteSize = attachedGameObject.GetComponent<SpriteRenderer>().bounds.size;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); ;
        mousePos.z = 0;
        Vector3 direction = mousePos - arm.position;
        direction.z = 0;  // Keep the direction in the X-Y plane

        // Calculate the rotation needed to point the object at the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the object
        arm.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        DrawTrajectory();



    }
    void DrawTrajectory()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldMousePosition.z = 0; // For 2D, we force z position to be 0


        Vector3 gunPosition = lineRenderer.transform.position;
        gunPosition.z = 0; // For 2D, we force z position to be 0

        // Calculate direction from gun to mouse position
        Vector3 direction = worldMousePosition - gunPosition;

        RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction);

        if (hit.collider != null)
        {
            // If raycast hits something, set end point to hit point
            lineRenderer.SetPosition(0, gunPosition);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // If raycast hits nothing, draw line towards mouse position
            lineRenderer.SetPosition(0, gunPosition);
            lineRenderer.SetPosition(1, gunPosition + direction.normalized * 1000); // Line length when nothing is hit
        }
    }
}
