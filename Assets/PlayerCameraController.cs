using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Camera _mainCamera;

    private float _rotY;
    
    private readonly Vector3 _cameraOffset = new Vector3(0, 1.5f, 0);
    private readonly Vector3 _crouchCameraOffset = new Vector3(0, 0.75f, 0);
    
    // Start is called before the first frame update
    private void Start()
    {
        // Initialize Fields
        _mainCamera = Camera.main;

        _rotY = 0;
        
        InitializeCamera();
    }

    // Update is called once per frame
    private void Update()
    {
        _mainCamera.transform.localPosition = playerController.IsCrouching ? _crouchCameraOffset : _cameraOffset;
    }
    
    public void InitializeCamera()
    {
        _mainCamera.transform.localPosition = _cameraOffset;
    }

    public void RotateCamera(float y, float ySens)
    {
        _rotY += y * ySens;
        _rotY = Mathf.Clamp(_rotY, -90f, 90f); 
        _mainCamera.transform.localRotation = Quaternion.Euler(_rotY, 0, 0);
    }
}
