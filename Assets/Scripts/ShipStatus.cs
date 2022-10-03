using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ShipStatus
{
    public int maxLife;
    public int life;
    public float speed;
    public int damage;
    public float shootRate;
    public float rotateSpeed;
    private Image lifeBar;
    private float shootRateCount;
    public void Init(){
        this.life = this.maxLife;
    }
    public bool IsDead => this.life <= 0;
    public void TakeDamage(int value) => this.life -= value;
    public void ShootCooldown() => shootRateCount = shootRate;
    public bool CanShoot => shootRateCount <= 0;
    public void DescreaseShootCooldown(float time) => shootRateCount -= time;
    public void SetLifeBar(Transform go) => lifeBar = go.Find("HealthBar").GetComponent<Image>();
    public void UpdateHealthBar() => lifeBar.fillAmount = (float)this.life / this.maxLife;
}
