using UnityEngine;

public class ItemPickup : Interactable {

    public Item item;

    public override void Interact() {
        base.Interact();
        PickUp();
    }

    public void PickUp() {
        Debug.Log("Picking Up " + item.itemName);
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp) { Destroy(gameObject); }
    }
}
