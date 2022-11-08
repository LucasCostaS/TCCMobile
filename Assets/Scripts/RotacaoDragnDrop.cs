using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoDragnDrop : MonoBehaviour
{


    Vector3 touchPosWorld;
    Vector2 iniTouchPosWorld2D, endTouchPosWorld2D;

    //Change me to change the touch phase used.
    TouchPhase touchPhase = TouchPhase.Ended;

    void Update()
    {

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 iniTouchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                Vector2 endTouchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            }
            else
            {
                endTouchPosWorld2D = new Vector2(0f, 0f);
            }
            if (iniTouchPosWorld2D == endTouchPosWorld2D)
            {
                //We should have hit something with a 2D Physics collider!
                //GameObject touchedObject = hitInformation.transform.gameObject;
                //touchedObject should be the object someone touched.
                gameObject.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
                //Debug.Log(touchedObject.transform.eulerAngles.z);
            }
        }
            //We now raycast with this information. If we have hit something we can process it.
            //RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

            //if (hitInformation.collider != null)
            
         
    }
}
