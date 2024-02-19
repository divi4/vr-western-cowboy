using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : MonoBehaviour
{
    public GameObject reactionText;
    private bool m_isDisabled;

    public void OnTriggerEnter(Collider other)
    {
     if (!m_isDisabled && other.gameObject.CompareTag("Bullet"))
     {
        reactionText.GetComponent<timing>().health = false;
     }
    }
}
