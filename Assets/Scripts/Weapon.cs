using UnityEngine;
using System.Collections;
using TeamUtility.IO;

public class Weapon : MonoBehaviour {

    public GameObject bullet, hitPrefab, bloodPrefab;
    public LayerMask whatToHit;
    public Transform bulletTrail, muzzleFlash, rayPathStart, rayPathEnd;

    public WeaponType weapon;
    public enum WeaponType {
        BASIC,
        ROCKET,
        GRENADELAUNCHER
    }

    private float fireRate = 0, timeToFire = 0; // fireRate is the number of times the gun will fire per second
    private Transform firePoint;
    private Player player;
    private float timeToSpawnEffect = 0;

	void Start () {
        firePoint = transform.FindChild("FirePoint");
        player = GetComponent<Player>();

        switch(weapon) {
            case WeaponType.BASIC:
                fireRate = 15;
                break;
            case WeaponType.ROCKET:
                fireRate = 0;
                break;
            case WeaponType.GRENADELAUNCHER:
                fireRate = 0;
                break;
        }
	}
	
	void Update () {
        // single-burst weapons
        if (fireRate == 0) {
            if (InputManager.GetButtonDown("Fire")) {
                Fire();
            }
        }
        // automatic weapons
        else {
            if (InputManager.GetButton("Fire") && Time.time > timeToFire) {
                timeToFire = Time.time + (1 / fireRate);
                Fire();
            }
        }
	}

    private void Fire() {

        if (player.stats.ammo <= 0)
            return;
        
        player.stats.ammo--;

        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Quaternion mouseRotation = Quaternion.LookRotation(Vector3.forward, mousePos - firePoint.position);
        Vector3 dir = mousePos - firePoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        SimplePool.Spawn(bullet, firePoint.position, PlayerController.playerRotation, true);
        */

        //if (!OptionsMenuHandler.controllerMode) {
            Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            Vector2 firePointPosition = new Vector2(transform.position.x, transform.position.y);
            RaycastHit2D hit = Physics2D.Raycast(rayPathStart.position, (rayPathEnd.position - rayPathStart.position), 100, whatToHit);
            Debug.DrawLine(rayPathStart.position, (rayPathEnd.position - rayPathStart.position) * 100, Color.magenta); //(mousePosition - firePointPosition)
        //}
        //else {
            
        //}

        if (hit.collider != null) {
            //Debug.DrawLine(firePointPosition, hit.point, Color.red);
            if (hit.collider.tag == "Enemy") {
                hit.collider.gameObject.GetComponentInChildren<Enemy>().TakeDamage(10);
            }
        }

        if (Time.time >= timeToSpawnEffect) {
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null) {
                hitPos = (mousePosition - firePointPosition) * 100; // If we didn't hit anything, set the hit point to far off in space so we don't see the animation
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else {
                hitPos = hit.point; // Else, we hit something and want that to be the animation position
                hitNormal = hit.normal;
            }

            CastEffect(hitPos, hitNormal, hit);
            timeToSpawnEffect = Time.time + (1 / fireRate);
        }
    }

    private void CastEffect(Vector3 hitPos, Vector3 hitNormal, RaycastHit2D hit) {
        //SimplePool.Spawn(bulletTrail.gameObject, firePoint.position, firePoint.rotation, true);
        /*Transform trail = Instantiate(bulletTrail, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null) {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(trail.gameObject, 0.05f);
        */

        if (hitNormal != new Vector3(9999, 9999, 9999)) {
            if (hit.collider.tag == "Enemy") {
                GameObject hitEffect = Instantiate(bloodPrefab, hitPos, Quaternion.identity) as GameObject; // Quaternion.FromToRotation(Vector3.forward, hitNormal)
                Destroy(hitEffect, 0.2f);
            }
            else {
                GameObject hitEffect = Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.forward, hitNormal)) as GameObject; // Quaternion.FromToRotation(Vector3.forward, hitNormal)
                Destroy(hitEffect, 1f);
            }
        }

        //Transform clone = SimplePool.Spawn(muzzleFlash.gameObject, firePoint.position, firePoint.rotation, true).transform;
        Transform clone = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float muzzleSize = Random.Range(0.7f, 1f);
        clone.localScale = new Vector3(muzzleSize, muzzleSize, clone.localScale.z);
        clone.localPosition = new Vector3(clone.localPosition.x, clone.localPosition.y, clone.localPosition.z);
        Destroy(clone.gameObject, (1f / fireRate) / 2f);
        //SimplePool.Despawn(clone.gameObject);
    }
}