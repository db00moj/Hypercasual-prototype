using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreChangeObjectScript : MonoBehaviour
{
    [Space(10)]
    [Header("==OnPlayersTouch==")]
    [SerializeField] private ScoreChangeType myScoreChangeType;
    [SerializeField] private int amountOfChange = 50;

    private ParticleSystem myParticleSystem;

    void Start()
    {
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        if (gameObject.tag=="Gate") RefreshingGateColorAndText();
    }

    void RefreshingGateColorAndText()
    {
        Image gateImage = GetComponentInChildren<Image>();
        Text gateInteractAmountText = GetComponentInChildren<Text>();

        switch (myScoreChangeType)
        {
            case ScoreChangeType.Plus:
                gateImage.color = Color.green;
                gateInteractAmountText.text = "+" + amountOfChange;
                myParticleSystem.startColor = Color.green;
                break;
            case ScoreChangeType.Minus:
                gateImage.color = Color.red;
                gateInteractAmountText.text = "-" + amountOfChange;
                myParticleSystem.startColor = Color.red;
                break;
            case ScoreChangeType.Multiply:
                gateImage.color = Color.green;
                gateInteractAmountText.text = "×" + amountOfChange;
                myParticleSystem.startColor = Color.green;
                break;
            case ScoreChangeType.Divide:
                gateImage.color = Color.red;
                gateInteractAmountText.text = "÷" + amountOfChange;
                myParticleSystem.startColor = Color.red;
                break;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().ChangePlayerScore(myScoreChangeType, amountOfChange);
            myParticleSystem.Play();

            foreach (BoxCollider boxCollider in GetComponentsInParent<BoxCollider>())
            {
                StartCoroutine(DestroyBeforeActivate(boxCollider.gameObject));
            }

            if (gameObject.tag == "LittleScoreChange")
            {
                GetComponentInChildren<MeshRenderer>().enabled = false;
            }
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
