using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    
    
    [SerializeField]
    private float blinkTime = 0.15f;
    
    [SerializeField]
    private bool invertBlink = false;
    
    [SerializeField]
    private float maxTimeBetweenBlinks = 1f;
    
    private float timeSinceLastBlink = 0f;
    
    private float blinkTimeLeft = 0f;
    
    [SerializeField]
    private GhostController controller;
    
    [SerializeField]
    private VisibilityController visibility;
    
    private void Start() => visibility.SetVisibility(false);
    
    // Update is called once per frame
    private void Update()
    {
        if (controller.isHuntingOrFakeHunting)
        {
            
            if (blinkTimeLeft > float.Epsilon)
            {
                blinkTimeLeft -= Time.deltaTime;
                if (blinkTimeLeft <= 0f)
                {
                    blinkTimeLeft = 0f;

                    visibility.SetVisibility(invertBlink);
                }
            }
            else
            {
                bool addBlink = Random.Range(0, 50) == 0 || timeSinceLastBlink > maxTimeBetweenBlinks;
                
                timeSinceLastBlink = addBlink ? 0f : timeSinceLastBlink + Time.deltaTime;
                blinkTimeLeft = addBlink ? blinkTime : 0f;
                
                visibility.SetVisibility(addBlink && !invertBlink);
            }
        }
        else
        {
            visibility.SetVisibility(false);
        }
    }
}
