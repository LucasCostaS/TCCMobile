using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController1 : MonoBehaviour
{
    // Start is called before the first frame update
    private bool b1 = false;
    private bool b2 = true;
    public GameObject fase, botao;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Math.Round(fase.transform.GetChild(0).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(1).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(2).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(3).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(4).transform.eulerAngles.z) == 270 &&
        Math.Round(fase.transform.GetChild(5).transform.eulerAngles.z) == 90 &&
        Math.Round(fase.transform.GetChild(6).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(7).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(8).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(9).transform.eulerAngles.z) == 270 &&
        Math.Round(fase.transform.GetChild(10).transform.eulerAngles.z) == 90 &&
        Math.Round(fase.transform.GetChild(11).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(12).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(13).transform.eulerAngles.z) == 0 &&
        Math.Round(fase.transform.GetChild(14).transform.eulerAngles.z) == 180)
            b1 = true;
        else
            b1 = false;

        if (b1 == true && b2 == true)
        {
            for (int i = 0; i < 15; i++)
            {
                fase.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.yellow;
                Destroy(fase.transform.GetChild(i).GetComponent<Rotacao>());
                fase.transform.GetChild(i).localScale = new Vector3(0.75f, 0.75f, 0f);
                fase.transform.GetChild(i).localPosition = new Vector3(fase.transform.GetChild(i).localPosition.x * 0.75f, fase.transform.GetChild(i).localPosition.y * 0.75f, 0f);
            }
            b2 = false;

            botao.SetActive(true);

        }


    }
}
