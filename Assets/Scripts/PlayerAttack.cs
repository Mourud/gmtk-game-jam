using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private Animator animator;

    private SpriteRenderer[] childRenderers;

    private SpriteRenderer selfSpriteRenderer;

    private PlayerMovement playerMovementScript;

    private Transform armTransform;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        childRenderers = GetComponentsInChildren<SpriteRenderer>().Where(x => x.gameObject != gameObject).ToArray();
        selfSpriteRenderer = GetComponent<SpriteRenderer>();
        armTransform = transform.Find("Arm");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            playerMovementScript.enabled = false;
            selfSpriteRenderer.enabled = false;
            foreach (SpriteRenderer renderer in childRenderers)
            {
                renderer.enabled = true;
            }
            AimAtMouse();
            StartRaycast();
            OnGunFire();
        }

    }

    private void OnGunFire()
    {
        playerMovementScript.enabled = true;
        selfSpriteRenderer.enabled = true;
        foreach (SpriteRenderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }
    }

    private void AimAtMouse()
    {
       
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - armTransform.position;
        direction.z = 0;  // Keep the direction in the X-Y plane

        // Calculate the rotation needed to point the object at the target

        float angle = Mathf.Clamp(calculateAngleofVector(direction), -50, 90);
        armTransform.rotation = Quaternion.Euler(0, 0, angle);

    }

    private float calculateAngleofVector(Vector3 vector)
    {
        return  Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
    }
    void StartRaycast()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
    }
    void DrawTrajectory()
    {
        lineRenderer.positionCount = 2;
        Vector3 mousePosition = Input.mousePosition;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);


        Vector3 gunPosition = lineRenderer.transform.position;
        gunPosition.z = 0; // For 2D, we force z position to be 0
        // Calculate direction from gun to mouse position
        Vector3 direction = worldMousePosition - gunPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Restrict angle to be between -50 and 90 degrees
        angle = Mathf.Clamp(angle, -50, 90);
        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        // RaycastHit2D hit = Physics2D.Raycast(gunPosition, direction);

        // if (hit.collider != null)
        // {
        //     // If raycast hits something, set end point to hit point
        //     lineRenderer.SetPosition(0, gunPosition);
        //     lineRenderer.SetPosition(1, hit.point);
        // }
        // else
        // {
        // If raycast hits nothing, draw line towards mouse position
        lineRenderer.SetPosition(0, gunPosition);
        lineRenderer.SetPosition(1, gunPosition + direction.normalized * 0.5f); // Line length when nothing is hit
        // }
    }
}
