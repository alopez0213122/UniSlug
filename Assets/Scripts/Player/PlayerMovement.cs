using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [Header("Health")]
    [SerializeField] private int health;
    [SerializeField] private TextMeshProUGUI healthUI;
    [SerializeField] private bool canTakeDamage;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Shooting shooting;

    private void Awake()
    {
        shooting = GetComponent<Shooting>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        //Flip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    private bool IsGrounded() //Checks for if the player is grounded to determine whther they are allowed to jump
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    private void Flip() //Flips Sprite to look right/left depending on the direction the player is moving in
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) //If bullet collides with player, destroy the bullet and start takenDamage coroutine
        {
            collision.gameObject.GetComponent<Bullet>().ReleaseBullet();
            if (canTakeDamage)
            {
                StartCoroutine(TakenDamage());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heal")) //if player collides with heal tag, heal up to full HP
        {
            Destroy(collision.gameObject);
            health = 10;
            UpdateHealth();
        }
        if (collision.gameObject.CompareTag("PistolAmmo")) //if player collides with PistolAmmo tag, add 24 bullets to pistol reserves
        {
            Destroy(collision.gameObject);
            shooting.pistolReserve += 24;
        }
        if (collision.gameObject.CompareTag("smgAmmo")) //if player collides with smgAmmo tag, add 30 bullets to smg reserves
        {
            Destroy(collision.gameObject);
            shooting.smgReserve += 30;
        }
        if (collision.gameObject.CompareTag("Finish")) //if player collides with Finish Tag, restart game
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    private IEnumerator TakenDamage() //Player loses HP, becomes invulnerable for 1 second, and returns to being able to be hurt
    {
        Debug.Log("Taken Damage");
        canTakeDamage = false;
        health--;
        UpdateHealth();
        if (health <= 0) //If health reaches 0, restart game
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
    }

    public void UpdateHealth() //Updates Health UI
    {
        healthUI.text = "Health: " + health;
    }

}
