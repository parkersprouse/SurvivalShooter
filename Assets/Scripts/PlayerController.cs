/*
using UnityEngine;
using System.Collections;
using TeamUtility.IO;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour {

    public static Quaternion playerRotation;
    public Animator animator;
    public float speed = 3;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public LayerMask collisionLayer;
    public CollisionInfo collisionInfo;
    [HideInInspector]
    public BoxCollider2D boxCollider;
    [HideInInspector]
    public Vector2 playerInput;

    float horizontalRaySpacing, verticalRaySpacing;
    private RaycastOrigins raycastOrigins;
    private Vector3 velocity;
    private float velocityXsmoothing, velocityYsmoothing;
    private float accelerationTime = .05f;
    private float rotationSpeed = 30f;

    const float skinWidth = .015f;

    struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

    public struct CollisionInfo {
        public bool above, below, left, right;

        public void Reset() {
            above = below = left = right = false;
        }
    }

    void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        CalculateRaySpacing();
    }

    void Update() {

        /*
         * Moving the player
         /
        Vector2 input = new Vector2(InputManager.GetAxisRaw("Horizontal"), InputManager.GetAxisRaw("Vertical"));

        animator.SetFloat("Speed", Mathf.Abs(input.x) + Mathf.Abs(input.y));

        float targetVelX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelX, ref velocityXsmoothing, accelerationTime);
        //velocity.x = input.x * speed;

        float targetVelY = input.y * speed;
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelY, ref velocityYsmoothing, accelerationTime);
        //velocity.y = input.y * speed;

        Move(velocity * Time.deltaTime, input);


        /*
         * Controlling the player looking at the mouse location
         /
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Quaternion mouseRotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, mouseRotation, rotationSpeed * Time.deltaTime);

        playerRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void Move(Vector3 vel, Vector2 input) {
        UpdateRaycastOrigins();
        collisionInfo.Reset();
        playerInput = input;

        if (vel.x != 0)
            HorizontalCollisions(ref vel);

        if (vel.y != 0)
            VerticalCollisions(ref vel);

        transform.Translate(vel, Space.World);
    }

    void VerticalCollisions(ref Vector3 vel) {
        float directionY = Mathf.Sign(vel.y);
        float rayLength = Mathf.Abs(vel.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + vel.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                vel.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }
    }

    void HorizontalCollisions(ref Vector3 vel) {
        float directionX = Mathf.Sign(vel.x);
        float rayLength = Mathf.Abs(vel.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionLayer);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit) {
                vel.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }
    }

    void UpdateRaycastOrigins() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
}
*/

using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class PlayerController : MonoBehaviour {

    public static Quaternion playerRotation;
    public Animator animator;
    public float speed = 3;

    private TopDownCollider2D tdc;
    private Vector3 velocity;
    private float velocityXsmoothing, velocityYsmoothing;
    private float accelerationTime = .05f;
    private float rotationSpeed = 30f;
    private Transform torso;

    void Start() {
        tdc = GetComponent<TopDownCollider2D>();
        torso = transform.GetChild(0).transform;

        /*foreach (Transform t in transform) {
            if (t.name == "Torso") {
                torso = t.transform;
            }
        }*/
        //torso = transform.Find("PlayerSprite/Torso");
        torso = transform;
    }

    void Update() {

        /*
         * Moving the player
         */
        Vector2 input = new Vector2(InputManager.GetAxisRaw("Horizontal"), InputManager.GetAxisRaw("Vertical"));

        animator.SetFloat("Speed", Mathf.Abs(input.x) + Mathf.Abs(input.y));

        float targetVelX = input.x * speed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelX, ref velocityXsmoothing, accelerationTime);

        float targetVelY = input.y * speed;
        velocity.y = Mathf.SmoothDamp(velocity.y, targetVelY, ref velocityYsmoothing, accelerationTime);

        tdc.Move(velocity * Time.deltaTime);


        /*
         * Controlling the player looking at the mouse location
         */
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        torso.rotation = Quaternion.Slerp(torso.rotation, rot, rotationSpeed * Time.deltaTime);

        //playerRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        playerRotation = new Quaternion(torso.rotation.x, torso.rotation.y, torso.rotation.z, torso.rotation.w);
    }
}