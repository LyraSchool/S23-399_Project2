using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{

    [SerializeField]
    private SkinnedMeshRenderer[] headRenderers;
    
    [SerializeField]
    private MeshRenderer[] moreHeadRenderers;
    
    [SerializeField]
    private MeshRenderer cylinderRenderer;
    
    public bool IsThirdPerson { get; private set; }
    
    private Camera mainCamera;
    
    
    private Vector3 firstPersonCameraOffset = new Vector3(0, 1.5f, 0);
    private Vector3 thirdPersonCameraOffset = new Vector3(0, 1f, -2.5f);
    
    
    
    private bool lastVal = false;
    [SerializeField]
    private bool tp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        
        GoFirstPerson();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsThirdPerson)
        {
            mainCamera.transform.localPosition = transform.position + firstPersonCameraOffset;
        }
    }
    
    public void GoFirstPerson()
    {
        foreach (SkinnedMeshRenderer headRenderer in headRenderers)
        {
            headRenderer.enabled = false;
        }
        
        foreach (MeshRenderer headRenderer in moreHeadRenderers)
        {
            headRenderer.enabled = false;
        }
        
        cylinderRenderer.enabled = true;
        
        Vector3 cameraPos = transform.position;
        cameraPos.y += 1.5f;
        mainCamera.transform.localPosition = cameraPos;
        
    }
    
    public void GoThirdPerson()
    {
        foreach (SkinnedMeshRenderer headRenderer in headRenderers)
        {
            headRenderer.enabled = true;
        }
        
        foreach (MeshRenderer headRenderer in moreHeadRenderers)
        {
            headRenderer.enabled = true;
        }
        
        cylinderRenderer.enabled = false;
        
        Vector3 cameraPos = transform.position;
        Vector3 cameraOffset = new Vector3(0, 1f, -2.5f);
        mainCamera.transform.localPosition = cameraPos + cameraOffset;
    }
    
    public void RotateCamera(float x, float y)
    {
        Transform mainCameraTransform = mainCamera.transform;
        mainCameraTransform.Rotate(Vector3.right, y);
        
        Vector3 eulerAngles = mainCameraTransform.eulerAngles;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, 0, 180);
        mainCamera.transform.eulerAngles = eulerAngles;
        
    }
}
