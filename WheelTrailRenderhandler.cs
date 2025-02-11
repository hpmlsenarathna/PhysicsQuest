using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRender : MonoBehaviour
{
    // Components
    TopDownCarController topDownCarController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        // Get the top down car controller
        topDownCarController = GetComponent<TopDownCarController>();
        if (topDownCarController == null)
        {
            Debug.LogError("TopDownCarController not found on this GameObject.");
        }

        // Get the trail render component
        trailRenderer = GetComponent<TrailRenderer>();
        if (trailRenderer == null)
        {
            Debug.LogError("TrailRenderer not found on this GameObject.");
        }

        // Set the trail render to not emit in the start
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (topDownCarController != null && trailRenderer != null)
        {
            // If the car tires are screeching then we'll emit a trail.
           // trailRenderer.emitting = topDownCarController.IsTireScreeching(out _, out _);
        }
    }
}
