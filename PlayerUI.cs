using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI inventoryText;
    [SerializeField]
    private TextMeshProUGUI interactText;
    [SerializeField]
    private Image healthBarFill;
    [SerializeField]
    private Image xpBarFill;

    [SerializeField]
    private Player player;

    void Awake() {
        player = FindObjectOfType<Player>();
    }



    public void UpdateLevelText() {
        levelText.text = "Level\n" + player.getCurLevel();
    }
    public void UpdateHealthBar() {
        healthBarFill.fillAmount = player.getHealthRatio();
    }
    public void UpdateXPBar() {
        xpBarFill.fillAmount = player.getXPRatio();
    }


    public void SetInteractText(Vector3 pos, string text) {
        interactText.gameObject.SetActive(true);
        interactText.text = text;

        interactText.transform.position = Camera.main.WorldToScreenPoint(pos + Vector3.up);
    }
    public void DisableInteractText() {
        if (interactText.gameObject.activeInHierarchy) {
            interactText.gameObject.SetActive(false);
        }
    }


    public void UpdateInventoryText() {
        inventoryText.text = "";

        foreach(string item in player.GetInventory()) {
            inventoryText.text += item + "\n";
        }
    }

}
