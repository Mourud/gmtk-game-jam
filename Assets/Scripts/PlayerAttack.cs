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
        playerMovementScript = GetComponent<PlayerMovement>();
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
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - armTransform.position;
            direction.z = 0;  // Keep the direction in the X-Y plane

            // Calculate the rotation needed to point the object at the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -50, 90);
            armTransform.rotation = Quaternion.Euler(0, 0, angle);

            RaycastHit2D hit = Physics2D.Raycast(lineRenderer.transform.position, direction);
            lineRenderer.positionCount = 2;


            if (hit)
            {
                // If raycast hits something, set end point to hit point
                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                // If raycast hits nothing, draw line towards mouse position
                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, lineRenderer.transform.position + direction.normalized * 0.5f); // Line length when nothing is hit
            }
            // OnGunFire();
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
}
