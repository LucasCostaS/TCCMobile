using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicaoSnap : MonoBehaviour
{

    public float[] gradeX = new float[4];
    public float[] gradeY = new float[4];
    private Vector2[] posicao = new Vector2[16];
    public GameObject sombra;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        setarSnap();
    }

    public void setarSnap()
    {
        for (int i = 0; i < gradeY.Length; i++)
        {
            gradeX[i] = sombra.transform.GetChild(i).transform.position.x;
            gradeY[i] = sombra.transform.GetChild(4 * i).transform.position.y;
        }
        for (int i = 0; i < gradeY.Length; i++)
        {
            for (int j = 0; j < gradeY.Length; j++)
            {
                posicao[j + (4 * i)] = new Vector2(gradeX[j], gradeY[gradeY.Length - (1 + i)]);

            }
        }
    }
}
