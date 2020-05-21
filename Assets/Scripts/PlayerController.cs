using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 15.0f;
    public float padding = 1.0f;
    public GameObject projectile;
    public float projectileSpeed;
    public float firingRate = 0.2f;
    public float health = 250f;

    private float xMin;
    private float xMax;

    public AudioClip fireSound;

    private void Start()
    {
        float distance = this.transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 RightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xMin = leftMost.x + padding;
        xMax = RightMost.x - padding;
    }

    private void Fire()
    {
        Vector3 offset = new Vector3(0, 1, 0);
        GameObject beam = Instantiate(projectile, this.transform.position + offset, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }

    // Update is called once per frame
    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // Time.deltaTime - time between frame updates (keep speed consistant)           
            this.transform.position += Vector3.left * speed * Time.deltaTime;

        } else if (Input.GetKey(KeyCode.RightArrow)) {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }

        // Restrict the player to the game space
        float newX = Mathf.Clamp(this.transform.position.x, xMin, xMax);
        this.transform.position = new Vector3(newX, this.transform.position.y, this.transform.position.z);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();

        if (missile)
        {
            health -= missile.GetDamage();

            missile.Hit();

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        SceneLoader sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
        sceneLoader.LoadLastScene();
        Destroy(this.gameObject);
    }
}
