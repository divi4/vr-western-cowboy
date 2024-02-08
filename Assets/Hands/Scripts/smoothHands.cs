using System;
using UnityEngine.InputSystem;
using UnityEngine;

public class smoothHands : MonoBehaviour
{
    [SerializeField] private Animator handAnimator;
    [SerializeField] private InputActionReference triggerActionRef;
    [SerializeField] private InputActionReference gripActionRef;


    private static readonly int TriggerAnimation = Animator.StringToHash("Trigger");
    private static readonly int GripAnimation = Animator.StringToHash("Grip");

    private void Update()
    {
        float triggerValue = triggerActionRef.action.ReadValue<float>();
        handAnimator.SetFloat("Trigger", triggerValue);

        float gripValue = gripActionRef.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }

}
