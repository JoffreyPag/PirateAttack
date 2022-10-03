using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_sprite;
    public bool isEnemy = false;
    public int damage;
    [SerializeField] float timeToDisapear;
    [SerializeField] float speed;
    [SerializeField] Animator explosionAnimator;
    private float timeToDisapearCount;
    private void Start() {
        m_sprite = GetComponent<SpriteRenderer>();
        m_rigidBody = GetComponent<Rigidbody2D>();
        timeToDisapearCount = timeToDisapear;
        m_rigidBody.velocity = transform.up*speed;
    }
    private void Update() {
        Fade();   
    }
    private void Fade(){
        Color color = m_sprite.color;
        color.a = timeToDisapearCount/timeToDisapear;
        timeToDisapearCount -= Time.deltaTime;
        if(timeToDisapearCount <= 0f){
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if((isEnemy && other.CompareTag("Player")) || (!isEnemy && other.CompareTag("Enemy"))){
            other.GetComponent<IDamageable>().TakeDamage(damage);
            explosionAnimator.SetTrigger("explode");
            GetComponent<CircleCollider2D>().enabled = true;
            m_rigidBody.velocity = Vector2.zero;
            Invoke(nameof(DestroySelf), .5f);
        }
    }
    public void DestroySelf() => Destroy(gameObject);
}