using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    [TextArea(3, 5)]
    public string itemDescription;
    
    // BARU: Kunci jawaban untuk mencocokkan dengan kotak pos
    public string targetHouse; 
}