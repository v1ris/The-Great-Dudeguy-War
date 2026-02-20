using UnityEngine;

public class CoolGuy : MonoBehaviour
{
    private Ally baseScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        baseScript = gameObject.GetComponent<Ally>();
        baseScript.price = -400;
        baseScript.sellPrice = 200;
        baseScript.attackDamage = 2;
        baseScript.attackSpeed = .3f;
        baseScript.bulletTravelSpeed = 5;
    }
}
