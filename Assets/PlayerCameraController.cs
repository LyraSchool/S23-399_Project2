using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    // Renderers
    [SerializeField] private SkinnedMeshRenderer[] headRenderers;
    [SerializeField] private MeshRenderer[] moreHeadRenderers;
    [SerializeField] private MeshRenderer cylinderRenderer;
    
    
    private Camera _mainCamera;

    private float _rotY;
    
    private readonly Vector3 _cameraOffset = new Vector3(0, 1.5f, 0);
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize Fields
        _mainCamera = Camera.main;

        _rotY = 0;
        
        InitializeCamera();
    }

    // Update is called once per frame
    void Update() { }
    
    public void InitializeCamera()
    {
        _mainCamera.transform.localPosition = _cameraOffset;
        
        // Disable all renderers of the head
        foreach (SkinnedMeshRenderer headRenderer in headRenderers) headRenderer.enabled = false;
        foreach (MeshRenderer headRenderer in moreHeadRenderers) headRenderer.enabled = false;
        
        // Hide the neck
        cylinderRenderer.enabled = true;
        
        // Vector3 cameraPos = transform.position;
        // cameraPos.y += 1.5f;
        // _mainCamera.transform.localPosition = cameraPos;
        
    }

    public void RotateCamera(float y, float ySens)
    {
        _rotY += y * ySens;
        _rotY = Mathf.Clamp(_rotY, -90f, 90f); 
        _mainCamera.transform.localRotation = Quaternion.Euler(_rotY, 0, 0);
    }
}
