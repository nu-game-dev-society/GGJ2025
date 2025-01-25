using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour
{
    [SerializeField] private float force = 1.0f;
    [SerializeField] private float rotForce = 1.0f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(GetRandom() * force);
        rb.AddTorque(GetRandom() * rotForce);
    }
    Vector3 GetRandom()
    {
        return new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
    }

    [ContextMenu("Explode")]
    public void Explode()
    {
        rb.AddForce(GetRandom() * force * 10);
        rb.AddTorque(GetRandom() * rotForce * 2);

    }

}
