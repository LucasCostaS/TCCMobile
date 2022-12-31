using UnityEngine;

public class Enunciado : MonoBehaviour
{
  public GameObject objeto, objeto2;

  // Start is called before the first frame update
  private void Start()
  {
  }

  // Update is called once per frame
  private void Update()
  {
  }

  private void OnMouseDown()
  {
    gameObject.SetActive(false);
    objeto.SetActive(true);
    objeto2.SetActive(true);
  }
}