using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private int health;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private int direction = -1; 
    [SerializeField] private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ChangeDirection());
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y); //Move in current direction
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) //If bullet collides with enemy, take damage and destroy the bullet
        {
            collision.gameObject.GetComponent<Bullet>().ReleaseBullet();
            TakenDamage();
        }
        else //if enemy collides with anything, change direction
        {
            direction *= -1;
        }
    }


    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator ChangeDirection() //Every 1-3.5 seconds, Enemy changes move direction
    {
        while (true)
        {
            float directionTime = Random.Range(1f, 3.5f);
            yield return new WaitForSeconds(directionTime);

            direction *= -1;
        }
    }

    private void TakenDamage() //Enemy loses health
    {
        Debug.Log("Enemy Taken Damage");
        health--;
        if (health <= 0)
            Destroy(gameObject);
    }
}
