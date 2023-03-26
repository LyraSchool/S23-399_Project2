using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpSpeed = 5f;

    [SerializeField] private float gravity = 9.81f;

    [SerializeField] private float vSpeed;

    //private Rigidbody _rb;

    [FormerlySerializedAs("_playerCameraController")] [SerializeField]
    private PlayerCameraController playerCameraController;

    [FormerlySerializedAs("_characterController")] [SerializeField]
    private CharacterController characterController;

    [FormerlySerializedAs("_visibilityController")] [SerializeField]
    private VisibilityController visibilityController;

    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip whooshSound;
    [SerializeField] private AudioClip hitSound;
    
    [SerializeField] private float xSensitivity = 1f;

    [SerializeField] private float ySensitivity = 1f;
    
    
    private bool _tryInteract = false;
    public bool IsCrouching { get; private set; }
    
    
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();

        HandleInteractions();
    }

    private void HandleInteractions()
    {
        if (!_tryInteract) return;
        _tryInteract = false;

        // See if we are looking at an interactable object within a certain distance
        if (Physics.Raycast(playerCameraController.transform.position, playerCameraController.transform.forward,
                out RaycastHit hit, 5f))
        {
            InteractableObject hitObject = hit.collider.gameObject.GetComponent<InteractableObject>();
            // Check if we are looking at an interactable object
            if (hitObject != null)
            {
                // Interact with the object
                hitObject.Interact();
            }
        }
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(x, 0, z).normalized;
        Vector3 movement = transform.TransformDirection(moveDir);


        if (characterController.isGrounded)
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
            vSpeed = Mathf.Clamp(vSpeed, -100f, 100f);
        }

        movement.y = vSpeed;



        //_rb.velocity = moveDir * speed;
        characterController.Move(speed * Time.deltaTime * movement);



        // Camera rotation

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // First person camera and player rotation
        transform.Rotate(xSensitivity * mouseX * Vector3.up);
        playerCameraController.RotateCamera(-mouseY, ySensitivity);

        if (Input.GetKeyDown(KeyCode.E))
        {
            _tryInteract = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // Crouch
            IsCrouching = true;

            // Reduce the player's height
            characterController.height = 1f;

            // Move the player's center down
            characterController.center = new Vector3(0, 0.5f, 0);

            visibilityController.SetVisibility(false);
        }
        else
        {
            // Stand up
            IsCrouching = false;

            // Reset the player's height
            characterController.height = 1.5f;

            // Reset the player's center
            characterController.center = new Vector3(0, .75f, 0);

            visibilityController.SetVisibility(true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            // Punch
            Punch();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Punch()
    {
        // See if we are looking at ghost within a certain distance
        if (Physics.Raycast(playerCameraController.transform.position, playerCameraController.transform.forward,
            out RaycastHit hit, 5f))
        {
            GhostController hitGhost = hit.collider.gameObject.GetComponent<GhostController>();
            // Check if we are looking at a ghost
            if (hitGhost != null)
            {
                // Punch the ghost
                audioSource.PlayOneShot(hitSound);
                hitGhost.TakeDamage(20);
                return;
            }
        }
        audioSource.PlayOneShot(whooshSound);
    }

    // Start the heartbeat sound
    public void StartHeartbeat() => audioSource.Play();

    // Stop the heartbeat sound
    public void StopHeartbeat() => audioSource.Stop();
}
