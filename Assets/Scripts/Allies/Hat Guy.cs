using UnityEngine;

public class HatGuy : MonoBehaviour
{
    private Ally baseScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        baseScript = gameObject.GetComponent<Ally>();
        baseScript.price = -200;
        baseScript.sellPrice = 100;
        baseScript.attackDamage = 1;
        baseScript.attackSpeed = .4f;
        baseScript.bulletTravelSpeed = 4;
    }
}
