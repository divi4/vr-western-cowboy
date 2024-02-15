using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : MonoBehaviour
{
    public GameObject reactionText;

    public void OnTriggerEnter()
    {
        reactionText.GetComponent<timing>().health = false;
    }
}
