using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public List<Transform> goats;
    public Vector3 offset;
    public float smoothTime = 0.8f;

    public float minZoom = 99f;
    public float maxZoom = 80f;
    private float zoomLimit = 25f;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }



    private void LateUpdate()
    {
        if (goats.Count == 0)
            return;

        Bounds goatBounds = GetBoundsGoats();

        Move(goatBounds);
        Zoom(goatBounds);
       
    }

    private void Zoom(Bounds bounds)
    {
        float maxDistance = Mathf.Max(bounds.size.x, bounds.size.y);

        //float newZoom = Mathf.Lerp(maxZoom, minZoom, maxDistance / zoomLimit);
        float newZoom = 100f;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);

    }

    private void Move(Bounds bounds)
    {
        Vector3 centerPoint = GetCenterPoint(bounds);

        Vector3 newPos = new Vector3(0, centerPoint.y + 5f, 0) + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint(Bounds bounds)
    {
        if (goats.Count == 1)
        {
            return goats[0].position;
        }

        return bounds.center;
    }

    private Bounds GetBoundsGoats()
    {
        var bounds = (goats[0] == null) ? new Bounds() : new Bounds(goats[0].position, Vector3.zero);
        for (int i = 0; i < goats.Count; i++)
        {
            bounds.Encapsulate(goats[i].position);
        }

        return bounds;
    }


    public void addGoat(GameObject goat)
    {
        goats.Add(goat.transform);
    }
    public void RemoveGoat(GameObject goat)
    {
        goats.Remove(goat.transform);
    }

   
}
