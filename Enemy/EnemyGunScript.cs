using System.Collections;
using UnityEngine;


public class EnemyGunScript : MonoBehaviour
{
    [SerializeField] Transform spawner;
    public GameObject bullet;
    private float lifespan = 3f;

    private float bulletSpeed = 25f;
    private Transform player;
    private bool canFire;

    private Gun gun;

    void Start()
    {
        StartCoroutine(spawnTimer());
        gun = GunContainer.badPistol;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        float distancetoPlayer = (player.transform.position - transform.position).magnitude;
        if(canFire == true & distancetoPlayer < 12f)
        {
            StartCoroutine(shootGun());
        }

        Vector3 rot = player.position - transform.position;

        float rotZ = Mathf.Atan2(rot.y,rot.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0,0,rotZ);
        
    }
    void fireBullet(float spread = 0)
    {

        GameObject newBullet = Instantiate(bullet,spawner.position,Quaternion.identity);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

        Vector2 bulletDirection = player.position - transform.position;
        bulletDirection += Vector2.Perpendicular(bulletDirection) * UnityEngine.Random.Range(-spread,spread);
        bulletRb.velocity = new Vector2(bulletDirection.x,bulletDirection.y).normalized * bulletSpeed;
        Destroy(newBullet,lifespan);
    }
    private IEnumerator shootGun()
    {
        canFire = false;
        for(int i = 0; i<gun.bullets; i++)
        {
            fireBullet(gun.spread);
        }
        yield return new WaitForSeconds(gun.fireRate);
        canFire = true;


    }
    private IEnumerator spawnTimer()
    {
        canFire = false;
        yield return new WaitForSeconds(0.5f);
        canFire = true;


    }
}
