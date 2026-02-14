using UnityEngine;

public class NormalGuy : MonoBehaviour
{
    private Ally baseScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        baseScript = gameObject.GetComponent<Ally>();
        baseScript.price = -100;
        baseScript.sellPrice = 50;
        baseScript.attackDamage = 1;
        baseScript.attackSpeed = 1.1f;
        baseScript.bulletTravelSpeed = 4;
    }
}
