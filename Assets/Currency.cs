using UnityEngine;

[CreateAssetMenu(fileName = "NewCurrency", menuName = "Currency/CurrencyData", order = 1)]
public class Currency : ScriptableObject
{
    
    public static float amount ;

    public static bool canaffoard(float cost)
    {
        Debug.Log("Ammount : " + amount);
        if(cost >= amount)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
}
