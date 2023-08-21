using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float minSize, maxSize;
    [SerializeField] Transform sprite;

    private float activeSize;

    void Start()
    {
        sprite.localScale = sprite.localScale * Random.Range(minSize, maxSize);

        activeSize = maxSize;
    }

    void Update()
    {
        sprite.localScale = Vector3.MoveTowards(sprite.localScale, Vector3.one * activeSize, speed * Time.deltaTime);

        if (sprite.localScale.x == activeSize)
        {
            if (activeSize == maxSize)
            {
                activeSize = minSize;
            } else
            {
                activeSize = maxSize;
            }
        }
    }
}
