using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotacaoDragnDrop : MonoBehaviour
{


    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1))
        {
            transform.Rotate(0.0f, 0.0f, -45.0f, Space.Self);
        }
    }


}
