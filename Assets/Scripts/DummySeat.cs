using System.Collections.Generic;
using UnityEngine;

public class DummySeat : MonoBehaviour
{
    [SerializeField] List<Sprite> DummyAwayFan;
    [SerializeField] List<Sprite> DummyHomeFan;

    [SerializeField] SpriteRenderer Fan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int home = Random.Range(0, 4);
        if (home == 1)
        {
            Fan.sprite = DummyHomeFan[Random.Range(0, 4)];
        }
        else if (home == 0)
        {
            Fan.sprite = DummyAwayFan[Random.Range(0, 4)];
        }
    }
}
