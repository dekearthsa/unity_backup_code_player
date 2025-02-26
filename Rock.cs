using UnityEngine;

public class Rock : MonoBehaviour
{

    public void OnTriggerEnter(Collider other){
        PlayerHeath playerHeath = other.GetComponent<PlayerHeath>();
        if(playerHeath == null){return;}
        playerHeath.Crash();
    }
}


