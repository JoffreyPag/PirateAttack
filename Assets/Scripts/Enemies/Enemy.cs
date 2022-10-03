using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public ShipStatus status = new ShipStatus();
    public int point;
    [SerializeField] protected GameObject cannonBall;
    [SerializeField] int points;
    [SerializeField] protected Animator explosionAnimator;
    [SerializeField] GameObject LifeBar;
    Transform LifeBarTransform;
    protected Rigidbody2D mrigidBody;
    private Animator mAnimator;
    void Start()
    {
        status.Init();
        mrigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        LifeBarTransform = Instantiate(LifeBar, LifeBar.transform.position + transform.position, Quaternion.identity).transform;
        status.SetLifeBar(LifeBarTransform);
    }
    private void Update() {
        LifeBarTransform.position = LifeBar.transform.position + transform.position;
        status.UpdateHealthBar();
        CheckLife();
    }
    private void CheckLife()
    {
        status.UpdateHealthBar();
        if(status.IsDead){
            mAnimator.SetTrigger("Dead");
        }else if( status.life > status.maxLife/2){
            mAnimator.SetTrigger("High");
        }else if(status.life <= status.maxLife/3){
            mAnimator.SetTrigger("Low");
        }else{
            mAnimator.SetTrigger("Mid");
        }
    }
    void FixedUpdate()
    {
        if(!status.IsDead && !Player.Instance.status.IsDead) EnemyBehaviour();
    }
    protected virtual void EnemyBehaviour(){}
    protected virtual void Chase(){
        transform.position = Vector2.MoveTowards(transform.position, Player.Instance.transform.position, status.speed*Time.deltaTime);
    }
    protected void RotateTowardsTarget()
    {
        float offset = -90f;
        Vector2 direction = Player.Instance.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;       
        transform.rotation = Quaternion.Euler(Vector3.forward * status.rotateSpeed * (angle + offset));
    }
    public void TakeDamage(int damage)
    {
        status.TakeDamage(damage);
        if(status.IsDead){
            GivePlayerPoint();
            explosionAnimator.SetTrigger("explode");
            GetComponent<BoxCollider2D>().enabled = false;
            Invoke(nameof(Die), 1);
        }
    }
    public void Die()
    {
        Destroy(LifeBarTransform.gameObject);
        Destroy(gameObject);
    }
    protected void GivePlayerPoint(){
        GameManager.Instance.AddPoints(point);
    }
}
