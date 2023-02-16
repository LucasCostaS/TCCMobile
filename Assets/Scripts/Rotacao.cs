using UnityEngine;
using UnityEngine.EventSystems;

public class Rotacao : MonoBehaviour
{
  private EventSystem m_EventSystem;

  private Vector3 touchPosWorld;
  private Vector2 touchPosWorld2D;

  private void OnEnable()
  {
    //Fetch the current EventSystem. Make sure your Scene has one.
    m_EventSystem = EventSystem.current;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {

      touchPosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
    }
    if (Input.GetMouseButtonUp(0))
    {
      RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

      if (hitInformation.collider != null)
      {
        //We should have hit something with a 2D Physics collider!
        GameObject touchedObject = hitInformation.transform.gameObject;
        //touchedObject should be the object someone touched.
        touchedObject.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
      }
    }

    }
  
}