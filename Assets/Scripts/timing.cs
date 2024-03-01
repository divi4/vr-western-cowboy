using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class timing : MonoBehaviour
{
    public GameObject pistol;
    public GameObject cowboy;

    public TextMeshProUGUI reactionText;
    public GameObject failTextObject;
    public GameObject hitDrawTextObject;
    public GameObject bulletDrawTextObject;

    [Space, SerializeField] private AudioSource bellTollAudioSource;
    [Space, SerializeField] private AudioSource bodyFallAudioSource;
    [Space, SerializeField] private AudioSource gameOverAudioSource;
    [Space, SerializeField] private AudioSource duelAudioSource;

    private bool timeIsRunning = false;
    public bool health = true;
    private bool isEnemyHit = false;
    private float reactionTime = 0; 
    public float countdownTime;


    private bool duelBegun = false;
    private bool failBegun = false;

    // Start is called before the first frame update
    void Start()
    {
        countdownTime = Random.Range(5.0f, 15.0f);
        failTextObject.SetActive(false);
        hitDrawTextObject.SetActive(false);
        bulletDrawTextObject.SetActive(false);
        reactionText.enabled = false;

        pistol.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Nothing");
    }

    // Update is called once per frame
    void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
        } else if (countdownTime <= 0) // BUG: For some reason doesn't equal 0 exactly, check Keke Island's implementation
        {
            DuelStart();
        }


        if (timeIsRunning)
        {
            if (health)
            {
                reactionTime += Time.deltaTime;

                if (cowboy.GetComponent<Target>().bullets == 0 && pistol.GetComponent<GunShoot>().bullets == 0)
                {
                    StartCoroutine(bulletDrawFail());
                }

            }
            else if (isEnemyHit) {
                StartCoroutine(hitDrawFail());
            } else
            {
                StartCoroutine(SceneFail());
            }
        }
    }


    private void DuelStart()
    {
        if (duelBegun == false) {
            StartCoroutine(bellTollAudio());
        }

        pistol.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Interactable");
        timeIsRunning = true;

        cowboy.GetComponent<Target>().isDuelStart = true;

    }


    public IEnumerator enemyHit()
    {
        isEnemyHit = true;

        yield return new WaitForSecondsRealtime(1);

        if (health == true) 
        {
            timeIsRunning = false;

            ShowTime(SetTime(reactionTime));
            reactionText.enabled = true;

            yield return new WaitForSecondsRealtime(7);

            SceneManager.LoadScene("Menu");
        }
    }


    IEnumerator SceneFail()
    {
        if (failBegun == false) {
            bodyFallAudio();
            gameOverAudio();
            duelAudioSource.Stop();
        }

        failTextObject.SetActive(true);

        pistol.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Nothing");

        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadScene("Menu");
    }


    IEnumerator bulletDrawFail() // Refactor with SceneFail()
    {
        yield return new WaitForSecondsRealtime(3);

        if (failBegun == false)
        {
            gameOverAudio();
            duelAudioSource.Stop();
        }

        bulletDrawTextObject.SetActive(true);

        yield return new WaitForSecondsRealtime(10);

        SceneManager.LoadScene("SampleScene");
    }


    IEnumerator hitDrawFail() // Refactor with SceneFail()
    {
        if (failBegun == false)
        {
            bodyFallAudio();
            gameOverAudio();
            duelAudioSource.Stop();
        }

        hitDrawTextObject.SetActive(true);  // Shows Draw! if both AI and player hit

        yield return new WaitForSecondsRealtime(7);

        SceneManager.LoadScene("SampleScene");
    }


    public void bodyFallAudio() 
    { 
        failBegun = true;
        
        var random = Random.Range(0.8f, 1.2f);
        bodyFallAudioSource.pitch = random;
        
        bodyFallAudioSource.Play();
    }


    public void gameOverAudio()
    {
        failBegun = true;

        gameOverAudioSource.Play();
    }


    IEnumerator bellTollAudio() 
    {
        duelBegun = true;

        float length = bellTollAudioSource.clip.length;
        for(int i = 0; i < 3; i++) {

            if (failBegun == true) {
                bellTollAudioSource.Stop();
                break;
            }

            var random = Random.Range(0.6f, 0.8f);
            bellTollAudioSource.pitch = random;
            
            bellTollAudioSource.Play();

            yield return new WaitForSeconds(length);
        }
    }


    private void ShowTime(float[] timeUnits)
    {
        reactionText.text = string.Format("{0:00}:{1:00}", timeUnits[0], timeUnits[1]);
    }


    private float[] SetTime(float time)
    {
        // External code start https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
        float seconds = Mathf.FloorToInt(time - 1);
        float milliseconds = (time % 1) * 100;
        // External code end

        float[] timeUnits = { seconds, milliseconds };
        return timeUnits;
    }


    // ******* Handles UI elements *******
    // External code start https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/
    IEnumerator FadeText()
    {
        for (float i = 0; i <= 1.5f; i += Time.deltaTime)
        {
            reactionText.color = new Color(0.9137f, 0.7686f, 0.1764f, i);
            yield return null;

        }

        for (float i = 1.5f; i >= 0; i -= Time.deltaTime)
        {
            reactionText.color = new Color(0.9137f, 0.7686f, 0.1764f, i);
            yield return null;
        }
    }
    // External code end
}
