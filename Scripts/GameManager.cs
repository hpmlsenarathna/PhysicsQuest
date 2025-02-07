using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public int maxShots = 3;

    private int usedShots = 0;

    private IconHandler iconHandler;

    [SerializeField] private float secsToWaitForDeath = 3f;
    [SerializeField] private GameObject restartObj;
    [SerializeField] private SlingshotHandler slingshotHandler;
    [SerializeField] private Image nextLevelImage;

    private List<Baddie> Baddies = new List<Baddie>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        iconHandler = FindObjectOfType<IconHandler>();

        Baddie[] allBaddies = FindObjectsOfType<Baddie>();

        for(int i = 0; i< allBaddies.Length; i++) {
            Baddies.Add(allBaddies[i]);
        }
        nextLevelImage.enabled = false;
    }

    public void useShot()
    {
        iconHandler.UseShot(usedShots);
        usedShots++;

        CheckForLastShot();
    }

    public bool hasEnoughShots()
    {
        if (usedShots < maxShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if(usedShots >= maxShots)
        {
            StartCoroutine(CheckForShotWaitTime());
        }
    }

    private IEnumerator CheckForShotWaitTime()
    {
        yield return new WaitForSeconds(secsToWaitForDeath);

        //have all baddies been killed
        if(Baddies.Count == 0 ) {
            WinGame();
}
        else
        {
            LoseGame();
        }
    }

    public void removeBaddie(Baddie baddie)
    {
        Baddies.Remove(baddie);
        CheckForAllBaddiesDead();
    }

    //check before timer to see if won
    public void CheckForAllBaddiesDead()
    {
        if(Baddies.Count == 0)
        {
            WinGame();
        }
}

    #region Win/Lost
    
    private void WinGame()
    {
        Debug.Log("Win");
        restartObj.SetActive(true);
        slingshotHandler.enabled = false;

        int currentScenceIndex = SceneManager.GetActiveScene().buildIndex;
        int maxLevel = SceneManager.sceneCountInBuildSettings;
        if(currentScenceIndex + 1 < maxLevel )
        {
            nextLevelImage.enabled = true;
           
        }
    }
    

    public void LoseGame()
    {
        DOTween.Clear();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion
}
