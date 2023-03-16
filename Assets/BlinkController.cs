using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkController : MonoBehaviour
{
    
    
    [SerializeField]
    private float blinkTime = 0.5f;
    
    [SerializeField]
    private bool invertBlink = false;
    
    private float blinkTimeLeft = 0f;
    
    [SerializeField]
    private GhostController controller;
    
    [SerializeField]
    private VisibilityController visibility;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(controller.isHuntingOrFakeHunting);
        
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
                blinkTimeLeft = Random.Range(0, 100) == 0 ? blinkTime : 0f;
            }
        }
        else
        {
            visibility.SetVisibility(false);
        }
    }
}
