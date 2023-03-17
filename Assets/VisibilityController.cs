using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class VisibilityController : MonoBehaviour
{
    private bool lastVal = false;
    [SerializeField]
    private bool isVisible = false;

    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;

    public void SetVisibility(bool val)
    {
        //Debug.Log("Set to " + val + "");
        isVisible = val;
    }


    // Start is called before the first frame update
    private void Start() { }
    
    // Update is called once per frame
    private void Update()
    {
        if (isVisible == lastVal) return;
        
        lastVal = isVisible;
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = isVisible;
        }
    }
}
