using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Player: MonoBehaviour, IDamageable
{
    public static Player Instance{get; private set;}
    [Tooltip("Ship Status")]
    public ShipStatus status = new ShipStatus();
    public UnityEvent onDie;    
    [SerializeField] GameObject cannonBall;
    [SerializeField] Transform[] sideCannon;
    [SerializeField] GameObject lateralCannons;
    [SerializeField] GameObject Lifebar;
    Transform LifebarInstance;
    private Rigidbody2D mRigidBody;
    private Animator mAnimator;
    private float rotationDirection;
    private float moveDirection;
    private void Awake() {
        if(Player.Instance != null && Player.Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this;
        }
    }
    private void Start() {
        status.Init();
        mRigidBody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        LifebarInstance = Instantiate(Lifebar, Lifebar.transform.position + transform.position, Quaternion.identity).transform;
        status.SetLifeBar(LifebarInstance);
    }
    private void Update() {
        LifebarInstance.position = Lifebar.transform.position + transform.position;
        status.UpdateHealthBar();
        UpdateSprite();
        CheckInputs();
    }
    private void UpdateSprite()
    {
        if(status.IsDead){
            mAnimator.SetTrigger("Dead");
        }
        else if(status.life > status.maxLife/2){
            mAnimator.SetTrigger("High");
        }else if(status.life < status.maxLife/3){
            mAnimator.SetTrigger("Low");
        }else{
            mAnimator.SetTrigger("Mid");
        }
    }
    private void CheckInputs()
    {
        rotationDirection = 0;
        moveDirection = 0;
        if(!status.IsDead){
            rotationDirection = Input.GetAxisRaw("Horizontal");
            moveDirection = Input.GetAxisRaw("Vertical");
            FireInputs();
        }
    }
    private void FireInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootFrontalCannon();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ShootLateralCannon();
        }
    }
    private void ShootFrontalCannon(){
        if(status.CanShoot){
            status.ShootCooldown();
            Shoot(transform,transform.rotation);
        }
    }
    private void Shoot(Transform t,Quaternion rotation){
        GameObject ball =Instantiate(cannonBall, t.position, rotation);
        ball.GetComponent<CannonBall>().isEnemy = false;
        ball.GetComponent<CannonBall>().damage = status.damage;
    }
    private void ShootLateralCannon(){
        if(status.CanShoot){
            status.ShootCooldown();
            foreach(Transform t in sideCannon){
                Quaternion rot = t.rotation;
                rot *= Quaternion.Euler(0,0,-90);
                Shoot(t, rot);
            }
        }
    }
    private void FixedUpdate() {
        transform.Rotate(Vector3.forward*rotationDirection*status.rotateSpeed);
        if(moveDirection > 0) mRigidBody.AddForce((transform.up * moveDirection * status.speed), ForceMode2D.Force); 
        if(!status.CanShoot) status.DescreaseShootCooldown(Time.deltaTime);
    }
    public void TakeDamage(int damage){ 
        status.TakeDamage(damage);
        if(status.IsDead){
            Die();
        }
    }
    public void Die(){
        onDie?.Invoke();
        transform.Find("Explosion").GetComponent<Animator>().SetTrigger("explode");
    }
}