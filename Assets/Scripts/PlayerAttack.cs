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

            // Check if the player is facing left, and adjust angle calculation if true
            if (transform.eulerAngles.y == 180)
            {
                armTransform.localScale = new Vector3(-1, 1, 1);
                lineRenderer.transform.localScale = new Vector3(-1, 1, 1);
                angle = Mathf.Clamp(angle + 180, 230, 90);
            }
            else
            {
                armTransform.localScale = new Vector3(1, 1, 1);
                lineRenderer.transform.localScale = new Vector3(1, 1, 1);
                angle = Mathf.Clamp(angle, -50, 90);
            }

            armTransform.rotation = Quaternion.Euler(0, 0, angle);

            // Direction for the raycast
            Vector3 raycastDirection = lineRenderer.transform.right;

            // Check if the player is facing left and adjust raycast direction if true
            if (transform.eulerAngles.y == 180)
            {
                raycastDirection = -raycastDirection;
                armTransform.gameObject.GetComponentInChildren<SpriteRenderer>().flipY = true;
            }

            RaycastHit2D hit = Physics2D.Raycast(lineRenderer.transform.position, raycastDirection, 1000f);
            lineRenderer.positionCount = 2;

            if (hit)
            {
                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(0, lineRenderer.transform.position);
                lineRenderer.SetPosition(1, lineRenderer.transform.position + direction.normalized * 0.5f); // Line length when nothing is hit
            }
            // OnGunFire();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            OnGunFire();
        }
    }


    private void OnGunFire()
    {
        lineRenderer.positionCount = 0;
        playerMovementScript.enabled = true;
        selfSpriteRenderer.enabled = true;
        foreach (SpriteRenderer renderer in childRenderers)
        {
            renderer.enabled = false;
        }
    }
}
