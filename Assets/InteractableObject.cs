using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Action OnInteract;
    public void Interact() => OnInteract?.Invoke();
}
