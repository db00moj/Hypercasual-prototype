using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private PlayerScript playerScript;
    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag== "MoneyMultiplierObject")
        {

            if(playerScript.playerMoneyMultiplier>0) playerScript.ChangePlayerScore(ScoreChangeType.Minus, 150);
            playerScript.playerMoneyMultiplier++;

            if (playerScript.GetPlayerScore() > 0 & playerScript.playerMoneyMultiplier!=5)
            {
                col.gameObject.GetComponentInChildren<ParticleSystem>().Play();
                col.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                col.gameObject.GetComponentInChildren<Canvas>().enabled = false;
            }

            if (playerScript.playerMoneyMultiplier == 5)
            {
                col.gameObject.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
    }
}
