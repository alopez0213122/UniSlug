using UnityEngine;
using UnityEngine.Pool;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [SerializeField] private Bullet bulletPrefab;
    private IObjectPool<Bullet> bulletPool;
    [SerializeField] private int defaultBulletPoolCapacity = 50;
    [SerializeField] private int bulletPoolMaxSize = 100;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetFromBulletPool, OnReleaseToBulletPool,
            OnDestroyPooledObject, true, defaultBulletPoolCapacity, bulletPoolMaxSize);
    }

    public Bullet GetBullet()
    {
        Bullet bullet = bulletPool.Get();
        return bullet;
    }
    
    private Bullet CreateBullet()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab);
        bulletInstance.BulletPool = bulletPool;
        return bulletInstance;
    }

    private void OnGetFromBulletPool(Bullet pooledBullet)
    {
        pooledBullet.SetIsReleased(false);
        pooledBullet.gameObject.SetActive(true);
    }
    
    private void OnReleaseToBulletPool(Bullet pooledBullet)
    {
        pooledBullet.gameObject.SetActive(false);
    }
    
    private void OnDestroyPooledObject(Bullet pooledBullet)
    {
        Destroy(pooledBullet.gameObject);
    }
}