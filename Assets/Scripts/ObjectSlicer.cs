using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using cakeslice;
using UnityEditor;
using UnityEngine;

public class ObjectSlicer : MonoBehaviour
{
    private List<SpriteSlicer2DSliceInfo> _slicedSpriteInfo = new List<SpriteSlicer2DSliceInfo>();
    private LineRenderer _lineRenderer;

    private Vector2 _startPos;
    private Vector2 _endPos;

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
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // Set Starposition for slicingray + convert screen to world coordinates
        }

        if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.enabled = false;
            Slice();
        }

        if (Input.GetMouseButton(0))
        {
            _endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //Set Endposition for slicingray + convert screen to world coordinates
            DrawSlicingray();
        }
    }

    private void DrawSlicingray()
    {
        RaycastHit2D raycastHit2D = Physics2D.Linecast(_startPos, _endPos);
        RaycastHit2D raycastHit2Dback = Physics2D.Linecast(_endPos, _startPos);
        if (raycastHit2D.collider == null || raycastHit2Dback.collider == null) return;  // Return if no object was found
        if (!raycastHit2D.transform.CompareTag("slice")) return;    // Return if object is not a slice
        _lineRenderer.enabled = true;
        _startPos = raycastHit2D.point;

        _lineRenderer.SetPositions(new Vector3[] { _startPos, raycastHit2Dback.point });
    }

    private void Slice()
    {
        // Slicing the sprite requirs 2 vectors which are NOT on/over the sprite
        // Thats why we calculate a offset for the start position
        Vector2 elevatedStartPos = _startPos - _endPos;
        elevatedStartPos.Normalize();
        _startPos += elevatedStartPos;

        SpriteSlicer2D.SliceAllSprites(_startPos, _endPos, false, ref _slicedSpriteInfo, "slice");
    }
}
