using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [SerializeField]
    private string itemName;

    public void PickUpItem() {
        FindObjectOfType<Player>().AddItemToInventory(itemName);
        Destroy(gameObject);
    }

}
