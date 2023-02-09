using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Space(10)]
    [SerializeField] private int playerScore = 0;
    [SerializeField] private int playerLevel = 0;
    [Space(10)]
    [SerializeField] private int playerMoney = 0;
    public int playerMoneyMultiplier = 0;
    [Space(10)]
    [SerializeField] private float playerMovingSpeed = 2;
    [Space(10)]
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private ParticleSystem bulletShootParticle;
    [SerializeField] private ParticleSystem bulletBreakParticle;
    [Space(10)]
    [SerializeField] private GameObject[] playerLevelObjects;
    [SerializeField] private GameObject gameEndUI;

    private bool gameEnded=false;

    private Transform cameraTransform;
    private ParticleSystem myParticleSystem;
    private Text scoreText;

    private Vector2 touchStartPos = Vector2.zero;
    private Vector3 playerTouchPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("MainCamera").GetComponent<Transform>();
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded)
        {
            MovingCharacter();
            cameraTransform.transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, transform.position.z - 8); //camera following character
        }
    }

    void MovingCharacter()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * playerMovingSpeed);



        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            playerTouchPos = transform.position;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Input.mousePosition;
            float touchDifference = touchPos.x - touchStartPos.x;

            transform.position = new Vector3 (Mathf.Lerp(transform.position.x, Mathf.Clamp(playerTouchPos.x + touchDifference / 80, -2f, 2f), Time.deltaTime * playerMovingSpeed * 1.5f), transform.position.y, transform.position.z); //moving character to left/right
        }
    }

    public void ChangePlayerScore(ScoreChangeType scoreChangeType, int amountOfChange)
    {
        int oldScore = playerScore;
        switch (scoreChangeType)
        {
            case ScoreChangeType.Plus:
                playerScore += amountOfChange;
                break;
            case ScoreChangeType.Minus:
                playerScore = Mathf.Clamp(playerScore - amountOfChange,0,9999);
                break;
            case ScoreChangeType.Multiply:
                playerScore = playerScore*amountOfChange;
                break;
            case ScoreChangeType.Divide:
                playerScore = (int)playerScore/amountOfChange;
                break;
        }
        if(!gameEnded)CheckLevel();

        //StartCoroutine(ScoreTextSmoothChange(oldScore, playerScore));
        scoreText.text = "" + playerScore;
    }

    public void ChangePlayerMoney(int amountOfMoney)
    {
        playerMoney += amountOfMoney;
    }

    void CheckLevel()
    {
        int newLevel = 0;
        if (playerScore < 250) newLevel = 0;
        else if (playerScore >= 250 & playerScore < 750) newLevel = 1;
        else if (playerScore >= 750) newLevel = 2;

        if (newLevel != playerLevel)
        {
            playerLevel = newLevel;
            for(int i=0; i < playerLevelObjects.Length; i++)
            {
                playerLevelObjects[i].SetActive(false);
            }
            playerLevelObjects[playerLevel].SetActive(true);

            myParticleSystem.Play();
        }

    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "GatesParent")
        {
            foreach(BoxCollider boxCollider in col.gameObject.GetComponentsInChildren<BoxCollider>())
            {
                boxCollider.enabled = false;
            }
        }

        if (col.gameObject.tag == "WinZoneTrigger")
        {
            StartCoroutine(FinishingGame(col.gameObject));
        }
    }

    IEnumerator ScoreTextSmoothChange(int oldScore, int newScore)
    {
        float secondsToChange = 1.5f;
        while (secondsToChange>0.05f)
        {
            secondsToChange -= Time.deltaTime;
            oldScore = (int)Mathf.Lerp(oldScore,newScore,5*Time.deltaTime);
            scoreText.text = "" + oldScore;
            yield return null;
        }
        scoreText.text = "" + newScore;
        yield return null;
    }

    IEnumerator FinishingGame(GameObject finishingTrigger)
    {
        gameEnded = true;
        float s = 0;
        Vector3 finishingPos = new Vector3(0, 1.2f, finishingTrigger.transform.position.z);
        Vector3 camFinishingPos = new Vector3(5, 4, finishingTrigger.transform.position.z - 4.5f);
        while (s < 2)
        {
            s += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(transform.position, finishingPos, 5 * Time.deltaTime);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, camFinishingPos, 5 * Time.deltaTime);
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, Quaternion.Euler(16,-45,0), 5 * Time.deltaTime);
            yield return null;
        }

        gameObject.transform.position = finishingPos;
        cameraTransform.position = camFinishingPos;
        cameraTransform.rotation = Quaternion.Euler(16, -45, 0);

        yield return new WaitForSeconds(1.5f);
        bulletShootParticle.Play();
        bulletObject.SetActive(true);

        while (playerScore > 0 & playerMoneyMultiplier!=5)
        {
            bulletObject.transform.Translate(Vector3.forward * 2.5f * Time.deltaTime);
            cameraTransform.position = new Vector3(5, 4, bulletObject.transform.position.z - 4.5f);
            yield return null;
        }

        foreach (MeshRenderer meshRenderer in bulletObject.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
        
        bulletBreakParticle.Play();

        yield return new WaitForSeconds(1.5f);

        gameEndUI.SetActive(true);
        GameObject.Find("ScoreText").SetActive(false);
        GameObject.Find("TextYouEarnedAmount").GetComponent<Text>().text = "" + playerMoney;
        GameObject.Find("TextYouGetAmount").GetComponent<Text>().text = "" + (playerMoney*playerMoneyMultiplier);
        GameObject.Find("TextMultiplier").GetComponent<Text>().text = "X" + playerMoneyMultiplier;


    }
    
}
