using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GoingThoughCast : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        CastUnderneath();
    }

    private void OnDrawGizmos()
    {
        Vector3 originPos = transform.position;
        Vector3 resultPos = originPos;

        resultPos.y -= 1f;
        
        Debug.DrawLine(originPos, resultPos, Color.green, 0.1f);
    }

    void CastUnderneath()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 1f))
        {
            if (hit.collider.name.Equals("Good"))
            {
                hit.collider.transform.parent.gameObject.SetActive(false);
            }
            else if (hit.collider.name.Equals("Bad"))
            {
                if (_rigidbody.velocity.y < -3f)
                {
                    _rigidbody.velocity = new Vector3(0, -3f, 0);
                }
//                gameObject.SetActive(false);
            }
            else
            {
                if (_rigidbody.velocity.y < -3f)
                {
                    _rigidbody.velocity = new Vector3(0, -3f, 0);
                }
            }
        }
    }
}