using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    [Header("Stats")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private int maxHP;
    [SerializeField]
    private int currentHP;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float interactRange;
    [SerializeField]
    private List<string> inventory = new List<string>();

    [Header("Experience")]
    [SerializeField]
    private int curLevel;
    [SerializeField]
    private int curXP;
    [SerializeField]
    private int xpToNextLevel;
    [SerializeField]
    private int previousXPThreshold;
    private int levelXPModifier;


    [Header("Combat")]
    [SerializeField]
    private KeyCode attackKey;
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float attackRate;
    private float lastAttackTime;

    [Header("Sprites")]
    [SerializeField]
    private Sprite downSprite;
    [SerializeField]
    private Sprite upSprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;

    //Components
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;
    private Vector3 facingDirection;
    private ParticleSystem hitEffect;
    private PlayerUI ui;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hitEffect = gameObject.GetComponentInChildren<ParticleSystem>();
        ui = FindObjectOfType<PlayerUI>();

        levelXPModifier = 100;
    }


    void Start() {
        ui.UpdateLevelText();
        ui.UpdateXPBar();
        ui.UpdateHealthBar();
    }


    void Update() {
        Move();
        UpdateFacingDirection();

        if (Input.GetKeyDown(attackKey)) {
            if (Time.time - lastAttackTime >= attackRate) {
                Attack();
            }
        }

        CheckInteract();
    }

    void Move() {
        //Get keyboard inputs
        Vector2 velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (velocity.magnitude != 0) {
            facingDirection = velocity;
        }

        body.velocity = velocity * moveSpeed;
    }

    void UpdateFacingDirection() {
        if (facingDirection.Equals(Vector2.up)) {
            spriteRenderer.sprite = upSprite;
        } else if (facingDirection.Equals(Vector2.down)) {
            spriteRenderer.sprite = downSprite;
        } else if (facingDirection.Equals(Vector2.left)) {
            spriteRenderer.sprite = leftSprite;
        } else if (facingDirection.Equals(Vector2.right)) {
            spriteRenderer.sprite = rightSprite;
        }
    }

    public void TeleportTo(Vector2 location) {
        transform.position = location;
    }


    void Attack() {
        lastAttackTime = Time.time;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, attackRange, 1 << 8);     //only look on layer mask 8
        if (hit.collider != null) {
            hit.collider.GetComponent<Enemy>().TakeDamage(damage);

            //hit effect
            hitEffect.transform.position = hit.collider.transform.position;
            hitEffect.Play();
        }
    }

    public void TakeDamage(int damageTaken) {
        currentHP -= damageTaken;
        if (currentHP <= 0) {
            Die();
        }

        ui.UpdateHealthBar();
    }

    void Die() {
        SceneManager.LoadScene("Game");
    }

    public void AddXP(int xp) {
        curXP += xp;
        ui.UpdateXPBar();
        if (curXP > xpToNextLevel) {
            LevelUp();
        }
    }

    void LevelUp() {
        curLevel++;
        levelXPModifier = curLevel * 100;
        previousXPThreshold = xpToNextLevel;
        xpToNextLevel += levelXPModifier;

        ui.UpdateLevelText();
        ui.UpdateXPBar();
    }

    void CheckInteract() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, interactRange, 1 << 9);

        if (hit.collider != null) {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            ui.SetInteractText(hit.collider.transform.position, interactable.GetInteractDescription());

            if (Input.GetKeyDown(attackKey)) {
                interactable.Interact();
            }
        } else {
            ui.DisableInteractText();
        }
    }


    public int getCurLevel() {
        return curLevel;
    }
    public float getXPRatio() {
        return ((float) curXP - (float) previousXPThreshold) / ((float) xpToNextLevel - (float) previousXPThreshold);
    }
    public float getHealthRatio() {
        return (float) currentHP / (float) maxHP;
    }


    public List<string> GetInventory() {
        return inventory;
    }

    public void AddItemToInventory(string item) {
        inventory.Add(item);
        ui.UpdateInventoryText();
    }
    public void RemoveItemFromInventory(string item) {
        inventory.Remove(item);
        ui.UpdateInventoryText();
    }
    public void RemoveItemAtIndex(int itemIndex) {
        inventory.RemoveAt(itemIndex);
        ui.UpdateInventoryText();
    }
}
