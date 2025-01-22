using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnnamedItem", menuName = "InventoryItem")]
public class ItemSO : ScriptableObject
{
   [SerializeField] private string itemName;
   [SerializeField] private string description;
   [SerializeField] private float itemWeight;
   
   public string ItemName => itemName;
   public string Description => description;
   public float ItemWeight => itemWeight;
}
