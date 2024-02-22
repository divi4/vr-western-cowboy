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
    private bool m_isDisabled;

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

        var bulletPrefab = Instantiate(bullet, bulletPosition.position, bulletPosition.rotation);
        var bulletRB = bulletPrefab.GetComponent<Rigidbody>();

        var direction = bulletPrefab.transform.TransformDirection(Vector3.up);
        bulletRB.AddForce(direction * bulletSpeed);
        Destroy(bulletPrefab, 5f);
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
     if (!m_isDisabled && other.gameObject.CompareTag("Bullet"))
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
        m_Renderer.enabled = m_isDisabled;
        m_Collider.enabled = m_isDisabled;

        canShoot = false;
    }

    private void TargetDestroyEffect()
    {
        m_ParticleSystem.Play();
    }
}
