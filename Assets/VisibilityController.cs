using UnityEngine;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] meshRenderers;

    [SerializeField]
    private bool isVisible = false;

    private bool _lastVal = false;
    
    public void SetVisibility(bool val) => isVisible = val;

    private void Update()
    {
        if (isVisible == _lastVal) return;
        
        _lastVal = isVisible;
        
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers) meshRenderer.enabled = isVisible;
    }
}
