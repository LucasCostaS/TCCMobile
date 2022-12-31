using UnityEngine;
using UnityEngine.EventSystems;

public class RotacaoDragnDrop : MonoBehaviour
{
  private EventSystem m_EventSystem;

  private void OnEnable()
  {
    //Fetch the current EventSystem. Make sure your Scene has one.
    m_EventSystem = EventSystem.current;
  }

  private Touch touch;
  private Vector3 touchPosWorld;
  private float touchDuration;

  private void Update()
  {
    if (Input.touchCount > 0)
    {
      touchDuration += Time.deltaTime;
      touch = Input.GetTouch(0);
      if (touch.phase == TouchPhase.Ended && touchDuration < 0.2f)
      {
        touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

        Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);

        RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

        if (hitInformation.collider != null)
        {
          GameObject touchedObject = hitInformation.transform.gameObject;

          // possibilidade de ter que usar try-catch
          if (touchedObject.transform.parent.transform.parent.name == "pecas")
            touchedObject.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
        }
      }
    }
    else
      touchDuration = 0.0f;
  }
}