using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public float SmoothTime = 0.3f;
    public float MaxSpeed = 100.0f;

    private bool _isDragging = false;
    private RaycastHit2D _desiredObject;
    private float _gravity;
    private Vector3 _velocity;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            _desiredObject = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);  // Check for objects at mouse position by raycasting
            if (_desiredObject.transform == null || !_desiredObject.transform.CompareTag("Slice")) return;  // If no object was found OR the found object has the tag "Slice", return
            _isDragging = true; // Player is now dragging

            // Set gameobjects gravity to 0 to prevent constant acceleration while moving it in air
            _gravity = _desiredObject.rigidbody.gravityScale;
            _desiredObject.rigidbody.gravityScale = 0;
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            Vector3 objectPosition;
            if (_desiredObject.transform.GetComponent<MeshRenderer>() != null)  // Does the gameobject contain a "MeshRenderer" ?
            {
                // Yes -> Use "MeshRenderer"
                objectPosition = _desiredObject.transform.gameObject.GetComponent<MeshRenderer>().bounds.center;    // Object position is NOT at center -> recalculate center of object
            }
            else
            {
                // No -> Than it HAS to contain a "SpriteRenderer" -> Use "SpriteRenderer"
                objectPosition = _desiredObject.transform.gameObject.GetComponent<SpriteRenderer>().bounds.center;    // Object position is NOT at center -> recalculate center of object
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // Convert mouse position from screen to world coordinates
            Vector3.SmoothDamp(objectPosition, mousePosition, ref _velocity, SmoothTime, MaxSpeed);  // Calculate next position
            _desiredObject.rigidbody.velocity = _velocity;  // Apply calculated velocity
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _desiredObject.rigidbody.gravityScale = _gravity;   // Reset gravity to original
            _isDragging = false;    // Player is not dragging gameobject anymore
        }
    }
}
