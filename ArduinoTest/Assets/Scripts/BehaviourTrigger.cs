using System;
using UnityEngine;

public class BehaviourTrigger : MonoBehaviour
{
    public event Action<int> OnBehaviourTriggered;
    public void TriggerBehaviour(int behaviourStrength)
    {
        OnBehaviourTriggered(behaviourStrength);
    }
}
