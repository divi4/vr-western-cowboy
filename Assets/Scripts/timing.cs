using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class timing : MonoBehaviour
{
    public TextMeshProUGUI reactionText;
    public GameObject failTextObject;

    private bool timeIsRunning = false;   // Set back to false if enemy hit
    private float countdown = 5;  // Make it randon range
    private bool health = true;
    private float reactionTime;

    // Start is called before the first frame update
    void Start()
    {
        reactionTime = Random.Range(5.0f, 15.0f);
        failTextObject.SetActive(false);
        reactionText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        } else if (countdown == 0) 
        {
            DuelStart();
        }


        if (timeIsRunning)
        {
            // if (health != false)
            // {
                reactionTime += Time.deltaTime;
                
            // }
            // else
            // {
            //     StartCoroutine(SceneFail());
            // }
        } else {
            ShowTime(SetTime(reactionTime));
            reactionText.enabled = true;
        }
    }


    private void DuelStart()
    {
        // GetComponent<__get script of pistol__>().enabled = true;
        timeIsRunning = true;
    }


    public void enemyHit()
    {
        timeIsRunning = false;
    }




    IEnumerator SceneFail()
    {
        failTextObject.SetActive(true);

        // GetComponent<__get script of pistol__>().enabled = false;

        yield return new WaitForSecondsRealtime(5);

        SceneManager.LoadScene("SampleScene");
    }


    private void ShowTime(float[] timeUnits)
    {
        reactionText.text = string.Format("{0:0}:{1:00}:{2:000}", timeUnits[0], timeUnits[1], timeUnits[2]);
    }


    private float[] SetTime(float time)
    {
        // External code start https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time % 1) * 1000;
        // External code end

        float[] timeUnits = { minutes, seconds, milliseconds };
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
