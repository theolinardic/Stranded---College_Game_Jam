using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class radioTuning : MonoBehaviour
{
    public LineRenderer screenLine;
    public int pointCount = 10;
    public Vector3 initalPos;

    private Vector3 secondPosition;
    private Vector3[] points;
    private float segmentWidth;

    public GameObject transform1, transform2;

    // Start is called before the first frame update
    void Start()
    {
        screenLine = this.GetComponent<LineRenderer>();

        screenLine.positionCount = pointCount;

       // screenLine.useWorldSpace = false;

        points = new Vector3[pointCount];
        initalPos = transform1.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        secondPosition = transform2.transform.position;
        //secondPosition.z = 0;

        var dir = secondPosition - initalPos;
        // get the segmentWidth from distance to end position
        segmentWidth = Vector3.Distance(initalPos, secondPosition) / pointCount;



        for (var i = 0; i < points.Length; ++i)
        {
            float x = segmentWidth * i;
            float y = Mathf.Sin(x * Time.time);
            points[i] = new Vector3(x, y, 0);
        }
        screenLine.SetPositions(points);

    }
}
