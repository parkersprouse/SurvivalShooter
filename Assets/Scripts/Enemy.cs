/*
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public float speed = 5;
    public int health = 100;
    [HideInInspector]
    public BoxCollider2D boxCollider;

    private float rotationSpeed = 10f, nextTimeToSearch = 0, timeUntilNextAttack = 0;
    // The player object, used to give the enemy its path
    private GameObject player;
    // Used for determining movement direction
    private Vector3 previousLocation = Vector3.zero;
    
    // raycast stuff
    public LayerMask collisionLayer;
    public int distanceFromCenter;
    private float rayDistance = 1;

    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {

        if (player == null) {
            FindPlayer();
            return;
        }

        Vector3 currentVelocity = (transform.position - previousLocation) / Time.deltaTime; // Calculates the current velocity of the enemy, used to determine the direction the enemy is moving in

        /////////////////////////////////////////////////////////////////////////////////////
        //Vector3 transformUp = transform.up;
        //Vector3 transformRight = transform.right;
        //RaycastHit leftRayHitInfo;
        //RaycastHit rightRayHitInfo;

        Vector3 leftRayOrigin = transform.position + distanceFromCenter * transform.right;
        Vector3 rightRayOrigin = transform.position - distanceFromCenter * transform.right;

        Debug.DrawRay(leftRayOrigin, Vector2.left * rayDistance, Color.red);
        Debug.DrawRay(rightRayOrigin, Vector2.right * rayDistance, Color.red);

        RaycastHit2D hitLeft = Physics2D.Raycast(leftRayOrigin, Vector2.left, rayDistance, collisionLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(rightRayOrigin, Vector2.right, rayDistance, collisionLayer);

        if (hitLeft) {
            if (timeUntilNextAttack <= Time.time) {
                if (hitLeft.collider.tag == "Player") {
                    player.GetComponent<Player>().DamagePlayer(10);
                }
                timeUntilNextAttack = Time.time + 1; // check every second if the enemy is touching the player
            }
            
        }
        else if (hitRight) {
            if (timeUntilNextAttack <= Time.time) {
                if (hitRight.collider.tag == "Player") {
                    player.GetComponent<Player>().DamagePlayer(10);
                }
                timeUntilNextAttack = Time.time + 1;
            }
            
        }
        /////////////////////////////////////////////////////////////////////////////////////

        previousLocation = transform.position;

        Rotate();
        transform.Translate(Vector3.up * Time.deltaTime * speed); // Move the character forwards along its Y-axis, which is representative of the direction it is facing
    }

    private void Rotate() {
        Vector3 difference = player.transform.position - transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    private void FindPlayer() {
        if (nextTimeToSearch <= Time.time) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
                player = searchResult;
            nextTimeToSearch = Time.time + 0.5f;
        }
    }

}
*/

using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private bool AnimationTesting = false;

    public float speed = 2;
    public int health = 100;
    public int damageDealt = 10;
    public Sprite[] bloodSpots;
    public GameObject blood, playerBlood;

    private float rotationSpeed = 10f, nextTimeToSearch = 0, timeUntilNextAttack = 0;
    private GameObject player; // The player object, used to give the enemy its path
    private Vector3 previousLocation = Vector3.zero; // Used for determining movement direction
    private TopDownCollider2D tdc;
    private Animator animator;

    //private Collider2D hit;
    private RaycastHit2D ray;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        tdc = GetComponentInParent<TopDownCollider2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!AnimationTesting) {

            if (player == null) {
                animator.SetBool("Attack1", false);
                animator.SetFloat("Speed", 0);
                FindPlayer();
                return;
            }

            else if (Vector2.Distance(transform.position, player.transform.position) < 20) {
                Vector3 currentVelocity = (transform.parent.transform.position - previousLocation) / Time.deltaTime; // Calculates the current velocity of the enemy, used to determine the direction the enemy is moving in
                previousLocation = transform.parent.transform.position;
                Rotate();
                tdc.Move(transform.parent.transform.up * Time.deltaTime * speed); // Move the character forwards along its Y-axis, which is representative of the direction it is facing

                animator.SetFloat("Speed", Mathf.Abs(currentVelocity.x) + Mathf.Abs(currentVelocity.y));


                //hit = Physics2D.OverlapCircle(transform.parent.transform.position, tdc.radius, 1 << LayerMask.NameToLayer("Player"));
                ray = Physics2D.Raycast(transform.parent.transform.position, transform.parent.transform.up, 2, 1 << LayerMask.NameToLayer("Player"));
                if (ray.collider != null) {
                    if (timeUntilNextAttack <= Time.time) {
                        animator.SetBool("Attack1", true);
                        //HurtPoorPlayer();
                    }
                }
                else {
                    animator.SetBool("Attack1", false);
                }
            }

        }
        else {
            if (Input.GetKeyDown(KeyCode.Q)) {
                animator.SetTrigger("Attack 1");
            }
            else if (Input.GetKeyDown(KeyCode.W)) {
                animator.SetFloat("Speed", 1);
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                animator.SetFloat("Speed", 0);
            }
        }
    }

    public void HurtPoorPlayer() {
        if (ray != null) {
            ray.collider.SendMessage("DamagePlayer", damageDealt, SendMessageOptions.DontRequireReceiver);
            GameObject hitEffect = Instantiate(playerBlood, ray.point, Quaternion.identity) as GameObject; // Quaternion.FromToRotation(Vector3.forward, hitNormal)
            timeUntilNextAttack = Time.time + 1.5f;
            Destroy(hitEffect, 0.2f);
        }
    }

    private void Rotate() {
        Vector3 difference = player.transform.position - transform.parent.transform.position;
        difference.Normalize();
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Quaternion rot = Quaternion.Euler(0f, 0f, rotZ - 90);
        transform.parent.transform.rotation = Quaternion.Slerp(transform.parent.transform.rotation, rot, rotationSpeed * Time.deltaTime);
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
        if (health <= 0) {
            blood.GetComponent<SpriteRenderer>().sprite = bloodSpots[Random.Range(0, bloodSpots.Length)];
            GameObject b = Instantiate(blood, transform.parent.transform.position, transform.parent.transform.rotation) as GameObject;
            b.GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.8f, 0.8f, 1);
            animator.SetTrigger("BloodFade");
            Destroy(transform.parent.gameObject);
            Destroy(b, 5f);
        }
    }

    private void FindPlayer() {
        if (nextTimeToSearch <= Time.time) {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null) {
                player = searchResult;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }

}