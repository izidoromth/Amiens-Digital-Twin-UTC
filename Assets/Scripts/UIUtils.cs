using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public static class UIUtils
    {
        public static bool MouseoverObject(GameObject gameObject)
        {
            RaycastHit hit;
            Ray ray = GameObject.FindAnyObjectByType<Camera>().ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out hit) && hit.collider.gameObject.Equals(gameObject);
        }

        ///Returns 'true' if we touched or hovering on Unity UI element.
        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }

        ///Returns 'true' if we touched or hovering on Unity UI element.
        public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }

        public static bool IsPointerOverUIElementByName(string name)
        {
            var UIElem = GetEventSystemRaycastResults().FirstOrDefault(r => r.gameObject.name == name).gameObject;
            return UIElem != null && UIElem.name.Contains(name);
        }

        ///Gets all event systen raycast results of current mouse or touch position.
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }        
    }
}
