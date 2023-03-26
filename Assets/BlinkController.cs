using UnityEngine;

public class BlinkController : MonoBehaviour
{
    
    
    [SerializeField]
    private float blinkTime = 0.15f;
    
    [SerializeField]
    private bool invertBlink = false;
    
    [SerializeField]
    private float maxTimeBetweenBlinks = 1f;
    
    private float _timeSinceLastBlink = 0f;
    
    private float _blinkTimeLeft = 0f;
    
    [SerializeField]
    private GhostController controller;
    
    [SerializeField]
    private VisibilityController visibility;
    
    private void Start() => visibility.SetVisibility(true);
    
    // Update is called once per frame
    private void Update()
    {
        if (!controller.isHuntingOrFakeHunting)
        {
            visibility.SetVisibility(true);
            return;
        }
        
        if (_blinkTimeLeft > float.Epsilon)
        {
            _blinkTimeLeft -= Time.deltaTime;
            if (_blinkTimeLeft <= 0f)
            {
                _blinkTimeLeft = 0f;
            }

            visibility.SetVisibility(invertBlink);
            
        }
        else
        {
            bool addBlink = Random.Range(0, 50) == 0 || _timeSinceLastBlink > maxTimeBetweenBlinks;
            
            _timeSinceLastBlink = addBlink ? 0f : _timeSinceLastBlink + Time.deltaTime;
            _blinkTimeLeft = addBlink ? blinkTime : 0f;
            
            visibility.SetVisibility(addBlink && !invertBlink);
        }
        
    }
}
