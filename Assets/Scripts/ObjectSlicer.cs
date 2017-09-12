using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSlicer : MonoBehaviour
{
    private List<SpriteSlicer2DSliceInfo> _slicedSpriteInfo = new List<SpriteSlicer2DSliceInfo>();
    private LineRenderer _lineRenderer;

    private Vector3 _startPos;
    private Vector3 _endPos;

    // Start is called at initialization
    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lineRenderer.enabled = true;
            _startPos = Input.mousePosition;    // Set Starposition for slicingray
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.enabled = false;
        }

        if (Input.GetMouseButton(0))
        {
            _endPos = Input.mousePosition;
            DrawSlicingray();
        }
    }

    private void DrawSlicingray()
    {
        float cutLenght = Vector3.Distance(_startPos, _endPos);

        Vector3[] linePositions = new Vector3[] { Camera.main.ScreenToWorldPoint(_startPos), Camera.main.ScreenToWorldPoint(_endPos)};

        //Set z-axis of position to -5 else line would be on camera and not visible
        linePositions[0].z = -5;
        linePositions[1].z = -5;

        _lineRenderer.SetPositions(linePositions);
    }
}
