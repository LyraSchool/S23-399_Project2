using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpSpeed = 5f;
    
    [SerializeField] private float gravity = 9.81f;
    
    [SerializeField] private float vSpeed;
    
    //private Rigidbody _rb;
    
    [SerializeField]
    private PlayerCameraController _playerCameraController;
    
    [SerializeField]
    private CharacterController _characterController;
    
    [SerializeField]
    private float xSensitivity = 1f;
    
    [SerializeField]
    private float ySensitivity = 1f;
    
    
    
    private bool tryInteract = false;
    
    
    private void Awake()
    {
        //_rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (!tryInteract) return;
        tryInteract = false;
        
        // See if we are looking at an interactable object within a certain distance
        RaycastHit hit;
        if (Physics.Raycast(_playerCameraController.transform.position, _playerCameraController.transform.forward, out hit, 5f))
        {
            // Check if we are looking at an interactable object
            if (hit.collider.gameObject.GetComponent<InteractableObject>() != null)
            {
                // Interact with the object
                hit.collider.gameObject.GetComponent<InteractableObject>().Interact();
            }
        }
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 moveDir = new Vector3(x, 0, z).normalized;
        Vector3 movement = transform.TransformDirection(moveDir);

        
        if (_characterController.isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = jumpSpeed;
            }
        }
        else
        {
            vSpeed -= gravity * Time.deltaTime;
        }
        
        movement.y = vSpeed;
        
        
        
        //_rb.velocity = moveDir * speed;
        _characterController.Move(speed * Time.deltaTime * movement);
        
        
        
        // Camera rotation
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        // First person camera and player rotation
        transform.Rotate(xSensitivity * mouseX * Vector3.up);
        _playerCameraController.RotateCamera(-mouseY, ySensitivity);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            tryInteract = true;
        }
        
    }
}
