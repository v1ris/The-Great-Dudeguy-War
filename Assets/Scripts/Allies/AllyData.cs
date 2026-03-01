using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewAlly", menuName = "Data/AllyData")]
public class AllyData : ScriptableObject
{
    //visual
    public Sprite allySprite;
    
    // stats
    public int attackDamage;
    public float attackSpeed;
    public int bulletTravelSpeed;
    public int price;
    public int sellPrice;
}
