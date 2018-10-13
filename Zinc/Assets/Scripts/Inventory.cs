﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Singleton
    public static Inventory instance;
    private void Awake() {
        if( instance != null) {
            Debug.LogWarning("More than one instance of Inventory Found! ");
            return;
        }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int capacity = 20;
    public List<Item> items = new List<Item>();

    public bool Add(Item item) {
        if(items.Count >= capacity) {
            Debug.Log("Inventory Full!");
            return false;
        }
        items.Add(item);
        if (onItemChangedCallback != null) { onItemChangedCallback.Invoke(); }
        
        return true;
    }
    public void Remove(Item item) {
        items.Remove(item);
        if (onItemChangedCallback != null) { onItemChangedCallback.Invoke(); }
    }
}
