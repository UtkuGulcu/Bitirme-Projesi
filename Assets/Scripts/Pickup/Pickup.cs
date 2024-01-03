using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] [Range(0, 2f)] private float distance;
    
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        RaycastHit2D[] hitArray = Physics2D.RaycastAll(transform.position, Vector2.down, 0.7f);
        
        foreach (var hit in hitArray)
        {
            if (hit.transform.name != transform.name)
            {
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
                Destroy(this);
            }
        }
        
        Debug.DrawRay(transform.position, Vector3.down * distance, Color.blue);
    }
}
