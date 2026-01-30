using System;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public static event Action<Passant> OnCartCollided = delegate { };

    [SerializeField, Min(0)] private int bodies;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sbattuto con " +  collision.gameObject.name);
        Passant hitPassant = collision.gameObject.GetComponentInParent<Passant>();
        if(hitPassant)
        {
            OnCartCollided?.Invoke(hitPassant);
            hitPassant.gameObject.SetActive(false);
        }
        
    }
}
