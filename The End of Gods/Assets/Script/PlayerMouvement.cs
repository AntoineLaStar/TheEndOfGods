using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour {
    protected float targetVelocityX;
    protected float targetVelocityY;
    [SerializeField] public float maxSpeed = 10;
    [SerializeField] public float maxHeight = 10;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected Rigidbody2D rigidBody2D;
    protected ContactFilter2D contactFilter;
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected bool InTheAir = false;
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    // Use this for initialization
    private void Awake()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 velocityX = new Vector2();
        velocityX.x = targetVelocityX;
        Vector2 deltaPositionX = velocityX * Time.deltaTime;
        Movement(deltaPositionX, true);
    }
    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rigidBody2D.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                hitBufferList.Add(hitBuffer[i]);
            }

            //for (int i = 0; i < hitBufferList.Count; i++)
            //{
            //    Vector2 currentNormal = hitBufferList[i].normal;
            //    float modifiedDistance = hitBufferList[i].distance - shellRadius;
            //    distance = modifiedDistance < distance ? modifiedDistance : distance;
            //}
        }
        rigidBody2D.position = rigidBody2D.position + move.normalized * distance;
    }
    // Update is called once per frame
    void Update () {
        ComputeVelocity();
        
	}

    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        if (Input.GetAxis("Jump") != 0 && InTheAir == false)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, maxHeight), ForceMode2D.Impulse);
            InTheAir = true;
        }
        targetVelocityX = move.x * maxSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 collPosition = collision.transform.position;
        if (collPosition.y > transform.position.y)
        {
            InTheAir = false;
        }
        //if (Mathf.Approximately(1.0F, 10.0F / 10.0F))
    }



}
