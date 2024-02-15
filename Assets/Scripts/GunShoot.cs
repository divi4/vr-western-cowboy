using UnityEngine;

public class GunShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float shootDelay = 0.2f;
    [Range(0, 3000), SerializeField] private float bulletSpeed;

    [Space, SerializeField] private AudioSource audioSource;

    private float lastShot;
    public int bullets = 6;
    public bool canShoot = true;

    public void Shoot() 
    {
        if(canShoot) {
            if (lastShot > Time.time) return;

            if(bullets == 0) {
                // NoBulletsAudio();
                return;
            }

            lastShot = Time.time + shootDelay;

            // TODO add audio
            // GunShotAudio();

            bullets -= 1;

            var bulletPrefab = Instantiate(bullet, bulletPosition.position, bulletPosition.rotation);
            var bulletRB = bulletPrefab.GetComponent<Rigidbody>();

            var direction = bulletPrefab.transform.TransformDirection(Vector3.forward); 
            bulletRB.AddForce(direction * bulletSpeed);
            Destroy(bulletPrefab, 5f);
        }
    }


    public void GunShotAudio() 
    { 
        var random = Random.Range(0.8f, 1.2f);
        audioSource.pitch = random;
        
        audioSource.Play();
    }


    public void NoBulletsAudio() 
    { 
        var random = Random.Range(0.8f, 1.2f);
        audioSource.pitch = random;  // Change source
        
        audioSource.Play();
    }
}
