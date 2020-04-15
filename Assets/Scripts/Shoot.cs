using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject _bombPrefab;

    [SerializeField]
    private GameObject _signalParticles;

    [SerializeField]
    private Transform _cursor;

    [SerializeField]
    private Transform _nozzle;


    //BEZIER
    //public Transform[] controlPoints;
    public LineRenderer lineRenderer;

    private int curveCount = 1;
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;
    private Vector3[] points;

    private void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        //curveCount = (int)controlPoints.Length / 3;
        points = new Vector3[SEGMENT_COUNT];
    }


    private void Update()
    {
        LaunchProjectile();
    }

    Vector3 LaunchProjectile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            lineRenderer.enabled = true;
            DrawCurve(this.transform.position, hit.point);
            _cursor.gameObject.SetActive(true);
            _cursor.position = hit.point + Vector3.up * 0.1f;

            //Vector3 Vo = CalculateVelocity(hit.point, _nozzle.position, 1f);

            transform.rotation = Quaternion.LookRotation(hit.point);

            if (Input.GetMouseButtonDown(0) && hit.collider.gameObject.name == "Ground")
            {
                var go = Instantiate(_bombPrefab, _nozzle.transform.position, Quaternion.identity);
                StartCoroutine(FollowPath(go));
            }
        }
        else
        {
            _cursor.gameObject.SetActive(false);
            lineRenderer.enabled = false;
        }
        return Vector3.zero;
    }

    IEnumerator FollowPath(GameObject go)
    {
        foreach (var item in points)
        {
            go.transform.position = item;
            //FollowPath(go, item);
            yield return new WaitForSeconds(0.000001f);
        }
        go.SetActive(false);
        Instantiate(_signalParticles, go.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GameObject gameObject = Instantiate(_bombPrefab, go.transform.position + Vector3.up * 3, Quaternion.identity);
        gameObject.AddComponent<Rigidbody>().useGravity = true;
    }

    void DrawCurve(Vector3 origin, Vector3 destination)
    {
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;
                Vector3 midpoint = MidPoint(origin, destination);
                Vector3 pixel = CalculateCubicBezierPoint(t, origin, new Vector3(midpoint.x, Vector2.Distance(transform.position, destination), midpoint.z), destination);
                points[i - 1] = pixel;
                lineRenderer.positionCount = (((j * SEGMENT_COUNT) + i));
                lineRenderer.SetPosition(i - 1, pixel);
            }
        }
    }

    Vector3 MidPoint(Vector3 v1, Vector3 v2)
    {
        float x = (v1.x + v2.x) / 2;
        float y = (v1.y + v2.y) / 2;
        float z = (v1.z + v2.z) / 2;

        return new Vector3(x, y, z);
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2/*, Vector3 p3*/)
    {
        Vector3 P0 = (Mathf.Pow((1 - t), 2) * p0);
        Vector3 P1 = 2 * (1 - t) * t * p1;
        Vector3 P2 = Mathf.Pow(t, 2) * p2;

        return P0 + P1 + P2;
    }
}
