using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyChangeObjectScript : MonoBehaviour
{
    [Space(10)]
    [Header("==OnPlayersTouch==")]
    [SerializeField] private int amountOfMoneyChange = 50;

    private ParticleSystem myParticleSystem;

    void Start()
    {
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().ChangePlayerMoney(amountOfMoneyChange);
            myParticleSystem.Play();
            StartCoroutine(DestroyBeforeActivate(gameObject));   
            GetComponentInChildren<MeshRenderer>().enabled = false;

        }
        else if (col.gameObject.tag == "DestroyObjectsZone")
        {
            Destroy(gameObject);
        }


    }

    IEnumerator DestroyBeforeActivate(GameObject objToDestroy)
    {
        yield return new WaitForSeconds(4);
        Destroy(objToDestroy);
        yield return null;
    }
}
