using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float floorHeigth;
    
    public float[] xPos;
    private int xPosIndex = 1;

    Animator _animator;
    public AnimationCurve jumpCurve;
    private float jumpTimer;
    private float yPos = 0f;
    private bool _jumping;
    private bool jumping
    {
        get { return _jumping; }
        set
        {
            _jumping = value;
            _animator.SetBool("Jumping", value);
        }
    }

    public bool hasDie;

    public Transform attackPos;
    public float attackrange;
    public LayerMask whatIsObstacle;

    void Start()
    {
        _animator = GetComponent<Animator>();
        Begin();
    }

    private void Begin()
    {
        _animator.SetBool("Running", true);
    }

    void Update()
    {
        if (hasDie)
            return;
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") > 0)
            MoveRigth();
        if (Input.GetButtonDown("Horizontal") && Input.GetAxisRaw("Horizontal") < 0)
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.Q))
            Attack();
        if (!jumping && (Input.GetAxisRaw("Vertical") > 0 || Input.GetKeyDown(KeyCode.Space)))
            jumping = true;
        
        if (jumping)
        {
            yPos = jumpCurve.Evaluate(jumpTimer);
            jumpTimer += Time.deltaTime;

            if (jumpTimer > 1f)
            {
                jumpTimer = 0f;
                jumping = false;
            }
        }
        
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos[xPosIndex], floorHeigth + yPos, transform.position.z), Time.deltaTime * speed);
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
        hasDie = true;
        Debug.Log("Ouch");
    }

    private void MoveRigth()
    {
        xPosIndex++;
        if (xPosIndex > xPos.Length - 1)
            xPosIndex = xPos.Length - 1;
    }

    private void MoveLeft()
    {
        xPosIndex--;
        if (xPosIndex < 0)
            xPosIndex = 0;
    }

    private void Attack()
    {
        _animator.SetTrigger("Kick");
        Vector3 pos = attackPos.position;
        pos.z += 2f;
        Collider[] obstacleToKick = Physics.OverlapCapsule(attackPos.position, pos, attackrange, whatIsObstacle);
        if (obstacleToKick.Length > 0)
            Destroy(obstacleToKick[0].gameObject);
        //foreach (Collider c in obstacleToKick)
        //{
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
            Die();
    }
}
