using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private Rigidbody _rb;
    
    [SerializeField]
    private PlayerCameraController _playerCameraController;
    
    
    public bool IsThirdPerson { get; private set; }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        Vector3 moveDir = new Vector3(x, 0, z).normalized;
        
        _rb.velocity = moveDir * speed;
        
        // Use 'T' to toggle third person
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (IsThirdPerson)
            {
                _playerCameraController.GoFirstPerson();
            }
            else
            {
                _playerCameraController.GoThirdPerson();
            }
        }
        
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        // First person camera and player rotation
        if (!IsThirdPerson)
        {
            transform.Rotate(Vector3.up * mouseX);
            _playerCameraController.RotateCamera(mouseX, mouseY);
        }
        else
        {
            
            _playerCameraController.RotateCamera(mouseX, mouseY);
        }
    }
}
