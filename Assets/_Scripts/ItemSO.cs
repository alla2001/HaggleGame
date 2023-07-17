using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName ="itemData",menuName ="Item/Create Item", order =0)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public int minPrice = 5;
    public int maxPrice = 200;
    public Sprite icon;

}
