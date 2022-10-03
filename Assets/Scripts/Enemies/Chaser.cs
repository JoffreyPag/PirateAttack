using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Enemy
{
    protected override void EnemyBehaviour()
    {
        if(Player.Instance){
            Chase();
        }
    }
    protected override void Chase()
    {
        base.Chase();
        RotateTowardsTarget();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<IDamageable>().TakeDamage(status.damage);
            explosionAnimator.SetTrigger("explode");
            GetComponent<BoxCollider2D>().enabled = false;
            status.life = 0;
            Invoke(nameof(Die), 1);
        }
    }
}
