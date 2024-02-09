using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject reactionText;
    
    
    private SkinnedMeshRenderer m_Renderer;
    private BoxCollider m_Collider;
    private AudioSource m_AudioSource;
    private ParticleSystem m_ParticleSystem;

    private Vector3 m_RandomRotation;
    private bool m_isDisabled;

    private void Awake()
    {
        m_Renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        m_Collider = GetComponent<BoxCollider>();
        m_AudioSource = GetComponent<AudioSource>();
        m_ParticleSystem = GetComponentInChildren<ParticleSystem>();

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!m_isDisabled && other.gameObject.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            ToggleTarget();

            reactionText.GetComponent<timing>().enemyHit();

            // TODO add audio and particle system
            //TargetDestroyEffect();
            
            Invoke("ToggleTarget", 3f); // Respawn target for testing

            //ReactionTimerStop() Stop reaction timer
        }
    }

    private void ToggleTarget()
    {
        m_Renderer.enabled = m_isDisabled;
        m_Collider.enabled = m_isDisabled;

        m_isDisabled = !m_isDisabled;
    }

    private void TargetDestroyEffect()
    {
        var random = Random.Range(0.8f, 1.0f);

        m_AudioSource.pitch = random;

        m_AudioSource.Play();
        m_ParticleSystem.Play();
    }
}
