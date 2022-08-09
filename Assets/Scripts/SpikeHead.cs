using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    public Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && rig.bodyType != RigidbodyType2D.Kinematic)
        {
            rig.bodyType = RigidbodyType2D.Kinematic;
            InvertPath();
        }
    }

    void InvertPath() {
        rig.gravityScale = -rig.gravityScale;
        rig.bodyType = RigidbodyType2D.Dynamic;
    }
}
