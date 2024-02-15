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

    private bool timeIsRunning = false;
    public bool health = true;
    private float reactionTime = 0; 
    public float countdownTime;
    

    // Start is called before the first frame update
    void Start()
    {
        countdownTime = Random.Range(5.0f, 15.0f);
        failTextObject.SetActive(false);
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
            // SoundBell();
            DuelStart();
        }


        if (timeIsRunning)
        {
            if (health != false)
            {
                reactionTime += Time.deltaTime;

            }
            else
            {
                StartCoroutine(SceneFail());
            }
        }
    }


    private void DuelStart()
    {
        pistol.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Interactable");
        timeIsRunning = true;

        cowboy.GetComponent<Target>().isDuelStart = true;
    }


    public void enemyHit()
    {
        timeIsRunning = false;
        pistol.GetComponent<GunShoot>().canShoot = false;

        ShowTime(SetTime(reactionTime));
        reactionText.enabled = true;

    }



    IEnumerator SceneFail()
    {
        failTextObject.SetActive(true);

        pistol.GetComponent<XRGrabInteractable>().interactionLayers = InteractionLayerMask.GetMask("Nothing");

        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadScene("SampleScene");
    }


    private void ShowTime(float[] timeUnits)
    {
        reactionText.text = string.Format("{0:00}:{1:000}", timeUnits[0], timeUnits[1]);
    }


    private float[] SetTime(float time)
    {
        // External code start https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
        float seconds = Mathf.FloorToInt(time);
        float milliseconds = (time % 1) * 1000;
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
