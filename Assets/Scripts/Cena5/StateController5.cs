using UnityEngine;

public class StateController5 : MonoBehaviour
{
  public bool spawn = true;
  public GameObject pecasCriadas;

  private void Start()
  {
  }

  private void Update()
  {
    spawn = (Physics2D.OverlapCircle(new Vector2(pecasCriadas.transform.localPosition.x, pecasCriadas.transform.localPosition.y), 0.1f, (1 << 6)) == null);
  }
}