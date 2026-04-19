using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    private IObjectPool<Bullet> bulletPool;
    public IObjectPool<Bullet> BulletPool { set => bulletPool = value; }
    private bool bIsReleased = false;
    
    private void OnCollisionEnter2D(Collision2D collision) //Destroys if bullet hits a wall
    {
        if (collision.gameObject.CompareTag("Obstruction"))
        {
            ReleaseBullet();
        }
    }

    public void SetIsReleased(bool value)
    {
        bIsReleased = value;
    }

    public void ReleaseBullet()
    {
        if (bIsReleased) return;
        bIsReleased = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
        bulletPool.Release(this);
    }
}
