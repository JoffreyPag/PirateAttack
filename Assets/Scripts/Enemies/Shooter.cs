using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Enemy
{
    [SerializeField] float shootArea;
    [SerializeField]LayerMask playerMask;
    protected override void EnemyBehaviour()
    {
        if(Player.Instance){
            var close = Physics2D.OverlapCircle(transform.position, shootArea, playerMask);
            if(close){
                Shoot();
            }else{
                Chase();
            }
            RotateTowardsTarget();
        }
        if(!status.CanShoot) status.DescreaseShootCooldown(Time.deltaTime);
    }
    private void Shoot()
    {
        if(status.CanShoot){
            status.ShootCooldown();
            GameObject ball = Instantiate(cannonBall, transform.position, transform.rotation);
            ball.GetComponent<CannonBall>().isEnemy = true;
            ball.GetComponent<CannonBall>().damage = status.damage;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, shootArea);
    }
}
