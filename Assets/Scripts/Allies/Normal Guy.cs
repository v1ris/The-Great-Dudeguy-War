using UnityEngine;

public class NormalGuy : MonoBehaviour
{
    private Ally baseScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        baseScript = gameObject.GetComponent<Ally>();
        baseScript.price = 100;
        baseScript.sellprice = 50;
        baseScript.attackDamage = 1;
        baseScript.attackSpeed = 1;
        baseScript.bulletTravelSpeed = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
