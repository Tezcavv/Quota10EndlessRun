using System;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public static event Action<Passant> OnCartCollided = delegate { };

    [SerializeField, Min(0)] private int bodies;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sbattuto con " +  collision.gameObject.name);
        if(TryGetComponent(out Passant passant))
        {
            OnCartCollided?.Invoke(passant);
            passant.gameObject.SetActive(false);
        }
        
    }
}
