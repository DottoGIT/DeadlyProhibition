using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStain : MonoBehaviour
{
    public List<Sprite> mySprites = new List<Sprite>();

    void Start()
    {
        SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sprite = mySprites[Random.Range(0, mySprites.Count)];
    }
}
