using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Stats")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int damage;

    [Header("Target")]
    [SerializeField]
    private float chaseRange;
    [SerializeField]
    private float attackRange;
    private Player player;

    [Header("Combat")]
    [SerializeField]
    private float attackRate;
    private float lastAttackTime;
    [SerializeField]
    private int xpWorth;

    //Components
    private Rigidbody2D body;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();    //expensive fonctions : it should not be put in Update function
    }

    // Update is called once per frame
    void Update() {
        float playerDist = Vector2.Distance(transform.position, player.transform.position);

        if (playerDist <= chaseRange) {
            Chase();
        } else {
            body.velocity = Vector2.zero;
        }

        if (playerDist <= attackRange) {
            body.velocity = Vector2.zero;
            if (Time.time - lastAttackTime >= attackRate) {
                Attack();
            }
        }
    }

    void Chase() {
        Vector2 direction = (player.transform.position - transform.position).normalized;    //make vector magnitude equal to 1

        body.velocity = direction * moveSpeed;
    }

    void Attack() {
        lastAttackTime = Time.time;

        player.TakeDamage(damage);
    }

    public void TakeDamage(int damageTaken) {
        currentHP -= damageTaken;
        if (currentHP <= 0) {
            Die();
        }
    }

    void Die() {
        player.AddXP(xpWorth);
        Destroy(gameObject);
    }

}
