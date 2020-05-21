using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject projectile;
    public float projectileSpeed = 10;
    public float health = 150f;
    public float shotsPerSecond = 0.5f;
    public int scoreValue = 150;

    public AudioClip fireSound;
    public AudioClip deathSound;

    private ScoreKeeper scoreKeeper;

    private void Start()
    {
        scoreKeeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
    }

    private void Update()
    {
        float probability = Time.deltaTime * shotsPerSecond;

        if(Random.value < probability)
        {
            Fire();
        }
    }

    private void Fire()
    {
        GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        missile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();

        if(missile)
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
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        Destroy(this.gameObject);
        scoreKeeper.Score(scoreValue);
    }
}
