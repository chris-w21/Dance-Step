using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera ))]
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;

    [SerializeField] private Vector3 offset;

    [SerializeField] private float smoothness = 0.5f, multiplier = 0.001f;

    [SerializeField] private float minZoom = 40f, maxZoom = 10f, zoomLimiter = 50f;

    private Camera cam
    {
        get
        {
            return Camera.main;
        }
    }

    private float zoom;

    private Vector3 velocity;

    private Vector3 centrePoint;

    private Bounds bounds, bounds1;

    private void LateUpdate()
    {
        if (renderers.Length == 0)
        {
            return;
        }
        Move();
        Zoom();
    }

    private void Zoom()
    {
        zoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter) + bounds.size.magnitude;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        bounds1 = new Bounds(new Vector3(renderers[0].bounds.center.x, renderers[0].bounds.center.y, 0f), new Vector2(renderers[0].bounds.size.x, renderers[0].bounds.size.y));
        for (int i = 0; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds.size.magnitude;
    }

    private void Move()
    {
        centrePoint = GetCentrepoint();

        transform.position = Vector3.SmoothDamp(transform.position, centrePoint + offset, ref velocity, smoothness);
    }

    private Vector3 GetCentrepoint()
    {
        if (renderers.Length == 1)
        {
            return renderers[0].transform.position;
        }

        bounds = new Bounds(new Vector3(renderers[0].bounds.center.x, renderers[0].bounds.center.y, 0f), new Vector2(renderers[0].bounds.size.x, renderers[0].bounds.size.y));

        for (int i = 0; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        return bounds.center;
    }
}