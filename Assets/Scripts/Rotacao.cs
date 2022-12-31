using UnityEngine;
using UnityEngine.EventSystems;

public class Rotacao : MonoBehaviour
{
  private EventSystem m_EventSystem;

  private void OnEnable()
  {
    //Fetch the current EventSystem. Make sure your Scene has one.
    m_EventSystem = EventSystem.current;
  }

  private Vector3 touchPosWorld;

  //Change me to change the touch phase used.
  private TouchPhase touchPhase = TouchPhase.Ended;

  private void Update()
  {
    //We check if we have more than one touch happening.
    //We also check if the first touches phase is Ended (that the finger was lifted)
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase)
    {
      //We transform the touch position into word space from screen space and store it.
      touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

      Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

      //We now raycast with this information. If we have hit something we can process it.
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