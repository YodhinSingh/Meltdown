using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArc : MonoBehaviour
{
    public GameObject travelPath;
    public Transform point0, point1, point1V2, point2, point3;
    private Vector3[] positions = new Vector3[4];
    private Vector3[] positions2 = new Vector3[3];


    // Start is called before the first frame update
    void Start()
    {
        travelPath.GetComponent<LineRenderer>().positionCount = positions2.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //DrawCubeCurve();
        DrawQuadCurve();
    }

    public void DrawCubeCurve()
    {
        for (int i = 1; i <= 4; i++)
        {
            float t = i / 4.0f;
            positions[i - 1] = cubeBezier3Line(point0.position, point1.position, point2.position, point3.position, t);
        }
        travelPath.GetComponent<LineRenderer>().SetPositions(positions);
    }

    public void DrawQuadCurve()
    {
        for (int i = 1; i <= 3; i++)
        {
            float t = i / 3.0f;
            positions2[i - 1] = quadBezier2Line(point0.position, point1V2.position, point3.position, t);
        }
        travelPath.GetComponent<LineRenderer>().SetPositions(positions2);
    }

    public static Vector3 cubeBezier3Line(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float r = 1f - t;
        float f0 = r * r * r;
        float f1 = r * r * t * 3;
        float f2 = r * t * t * 3;
        float f3 = t * t * t;
        return new Vector3(
            f0 * p0.x + f1 * p1.x + f2 * p2.x + f3 * p3.x,
            f0 * p0.y + f1 * p1.y + f2 * p2.y + f3 * p3.y,
            f0 * p0.z + f1 * p1.z + f2 * p2.z + f3 * p3.z
        );
    }

    public static Vector3 quadBezier2Line(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return p;
    }
}
