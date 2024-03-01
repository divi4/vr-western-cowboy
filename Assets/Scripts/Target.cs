using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject reactionText;
    
    private SkinnedMeshRenderer m_Renderer;
    private BoxCollider m_Collider;
    private AudioSource m_AudioSource;
    private ParticleSystem m_ParticleSystem;
    private Animator cowbodyAnimator;


    private Vector3 m_RandomRotation;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    [Range(0, 3000), SerializeField] private float bulletSpeed;

    [Space, SerializeField] private AudioSource shootAudioSource;
    [Space, SerializeField] private AudioSource noBulletsAudioSource;
    [Space, SerializeField] private AudioSource bodyFallAudioSource;


    private float timer;
    private float shootTime;
    public bool isDuelStart = false;
    public bool canShoot = true;


    private float lastShot;
    public int bullets = 6;


    void Awake()
    {
        m_Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_Collider = GetComponent<BoxCollider>();
        // m_ParticleSystem = GetComponentInChildren<ParticleSystem>();
    }


    void Start()
    {
        cowbodyAnimator = GetComponent<Animator>();

        shootTime = Random.Range(2f, 2.9f); // Change to 3-11 after testing
        timer = 0.0f;
    }


    void Update() 
    {        
        if (isDuelStart == true) {
            timer += Time.deltaTime;

            if (timer >= shootTime)
            {
                shootPlayer();
                timer = 0.0f;
                shootTime = Random.Range(2.5f, 4.2f);
            } else
            {
                cowbodyAnimator.SetBool("isShoot", false);
            }
        }
    }


    public void shootPlayer() 
    {
        if (canShoot == false) return;
        if (bullets == 0) {
            NoBulletsAudio();
            return;
        }

        cowbodyAnimator.SetBool("isShoot", true);

        GunShotAudio();

        bullets -= 1;



        // 9/10 probability of hitting player
        float[] horizontalAim = {0f, 0.5f, 0.75f, 1f, 1.25f, -0.5f, 0f, 0.5f, 0.75f, -1f}; // First nine are trajectories that'll hit the target, 9/10, -int moves horizontal right relative to player
        float[] verticalAim = { 0f, -1f, 1, 0f, -1f, 1, 0f, -1f, 1, -2.5f}; // First nine are trajectories that'll hit the target, 9/10, -int moves vertical up relative to player

        Quaternion aim = Quaternion.Euler(verticalAim[Random.Range(0,9)], 0f, horizontalAim[Random.Range(0, 9)]); // vertical is x as the transform object x rotation is altered by 90degrees
        
        var bulletPrefab = Instantiate(bullet, bulletPosition.position, bulletPosition.rotation * aim);
        var bulletRB = bulletPrefab.GetComponent<Rigidbody>();

        var direction = bulletPrefab.transform.TransformDirection(Vector3.up);
        bulletRB.AddForce(direction * bulletSpeed);
        Destroy(bulletPrefab, 3f);
    }


    public void GunShotAudio() 
    { 
        var random = Random.Range(0.8f, 1.2f);
        shootAudioSource.pitch = random;
        
        shootAudioSource.Play();
    }


    public void NoBulletsAudio() 
    { 
        var random = Random.Range(0.8f, 1.2f);
        noBulletsAudioSource.pitch = random;
        
        noBulletsAudioSource.Play();
    }


    public void bodyFallAudio() 
    {   
        var random = Random.Range(0.8f, 1.2f);
        bodyFallAudioSource.pitch = random;
        
        bodyFallAudioSource.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
     if (isDuelStart && other.gameObject.CompareTag("Bullet"))
     {      
            bodyFallAudio();

            Destroy(other.gameObject);
            ToggleTarget();

            StartCoroutine(reactionText.GetComponent<timing>().enemyHit());

            // TODO add particle system
            //TargetDestroyEffect();
        
            //add a ReactionTimerStop() function to stop reaction timer
        }
    }

    private void ToggleTarget()
    {
        m_Renderer.enabled = false;
        m_Collider.enabled = false;

        canShoot = false;
    }

    private void TargetDestroyEffect()
    {
        m_ParticleSystem.Play();
    }
}
