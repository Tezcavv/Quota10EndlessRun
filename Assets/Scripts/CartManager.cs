using System;
using System.Collections.Generic;
using UnityEngine;

public class CartManager : MonoBehaviour
{
    public static event Action<Passant> OnCartCollided = delegate { };
    public static event Action OnEnteringTheatre = delegate { };

    [SerializeField, Min(0)] private int bodies;
    [Tooltip("How many points should be reached each time to show more bodies on cart?")]
    [SerializeField, Min(1)] private int showBodyFactor = 1;
    [SerializeField] private List<MeshRenderer> bodyRenderers;

    private void Awake()
    {
        ScoreManager.OnScoreChanged += HandleBodyRenderers;
        //theater.onPlayerEnterTheatre +=  enetringTheatre;
    }

    private void Start()
    {
        HandleBodyRenderers(0);
    }

    private void OnDestroy()
    {
        ScoreManager.OnScoreChanged -= HandleBodyRenderers;
        //theater.onPlayerEnterTheatre -=  enetringTheatre;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sbattuto con " +  collision.gameObject.name);
        Passant hitPassant = collision.gameObject.GetComponentInParent<Passant>();
        if(hitPassant )
        { 
            OnCartCollided?.Invoke(hitPassant);
            hitPassant.gameObject.SetActive(false);

            if (hitPassant.PassInfo.type is PassInfo.PassType.actor)
            {
                bodies++;
                Debug.Log("Corpo aggiunto al carrello. Totale corpi: " + bodies);
            }
        }


    }

    private void HandleBodyRenderers(int newScore)
    {
        int amountToShow = newScore % showBodyFactor;
        for(int i = 0; i < bodyRenderers.Count; i++)
        {
            bodyRenderers[i].enabled = i < amountToShow;
        }
    }
    private void EnteringTheatre()
    {
        OnEnteringTheatre?.Invoke();
        bodies = 0;
        Debug.Log("Entrando nel teatro. Corpi azzerati.");
    }
}
