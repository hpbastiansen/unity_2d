// https://forum.unity.com/threads/how-to-detect-if-mouse-is-over-ui.1025533/#post-6642844

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// This script checks whether the cursor is over an UI element or not.
public class UITest : MonoBehaviour
{
    private int _UILayer;

    private void Start()
    {
        _UILayer = LayerMask.NameToLayer("UI");
    }

    /// Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    /// Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverUIElement(List<RaycastResult> _eventSystemRaycastResults)
    {
        for (int _index = 0; _index < _eventSystemRaycastResults.Count; _index++)
        {
            RaycastResult _curRaycastResult = _eventSystemRaycastResults[_index];
            if (_curRaycastResult.gameObject.layer == _UILayer)
                return true;
        }
        return false;
    }


    /// Gets all event system raycast results of current mouse or touch position.
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData _eventData = new PointerEventData(EventSystem.current);
        _eventData.position = Input.mousePosition;
        List<RaycastResult> _raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventData, _raycastResults);
        return _raycastResults;
    }
}
