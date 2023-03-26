using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private InteractableObject interactableObject;
    
    [SerializeField]
    private AudioSource audioSource;
    
    [SerializeField]
    private AudioClip openSound;
    
    [SerializeField]
    private AudioClip closeSound;
    
    public bool isLocked = false;
    
    [SerializeField]
    private float swingSpeed = 10f;
    
    private float _targetRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        interactableObject.OnInteract += Interact;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate door to target rotation
        
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-90f, _targetRotation, 0f), Time.deltaTime * swingSpeed);
    }

    private void Interact()
    {
        if (transform.localRotation.eulerAngles.y > 20f)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }


    private void OpenDoor()
    {
        if (isLocked)
        {
            // Play locked sound
            return;
        }
        
        // Play open sound
        audioSource.PlayOneShot(openSound);
        
        // Set Rotation target to 90 degrees
        _targetRotation = 90f;
    }

    private void CloseDoor()
    {
        // Set Rotation target to 0 degrees
        _targetRotation = 0f;
        
        // Play close sound
        audioSource.PlayOneShot(closeSound);
    }
}
