using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingUIManager : MonoBehaviour
{
    private TimeStamp best, cur;
    private bool gameFinished;
    public Text resultMessage, curResult,curResultText, bestResult;
    // Start is called before the first frame update
    void Start()
    {
        if (ReadTimeAndCompare())
        {


            if (PlayerPrefs.GetInt("result") == 1)
                gameFinished = true;
            else
                gameFinished = false;
            bestResult.text = best.hourCount + "h:" + best.minuteCount + "m:" + (int)best.secondsCount + "s";


            if (gameFinished)
            {
                resultMessage.text = "Congrats!! You Win!";
                curResultText.text = "Your result this time:";
                curResult.text = cur.hourCount + "h:" + cur.minuteCount + "m:" + (int)cur.secondsCount + "s";
            }
            else
            {
                resultMessage.text = "Don't Give Up! Try again!";
                curResultText.text = "Your result was not saved.";
                curResult.text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //read in the data file and compare result
    public bool ReadTimeAndCompare()
    {
        string bestTime;
        string curTime;
        if (File.Exists(Application.dataPath + "/SaveData/curTimeStamp.json"))
        {
             curTime = File.ReadAllText(Application.dataPath + "/SaveData/curTimeStamp.json");
            cur = JsonUtility.FromJson<TimeStamp>(curTime);
        }
        else
        {
            return false;
        }

        if (File.Exists(Application.dataPath + "/SaveData/bestTimeStamp.json"))
        {
            bestTime = File.ReadAllText(Application.dataPath + "/SaveData/bestTimeStamp.json");
            best = JsonUtility.FromJson<TimeStamp>(bestTime);
            //compare the two time stamp, if the current timestamp is sooner than the best one,
            //current become best
            if (TimeCompare(cur, best))
            {
                best = cur;
                bestTime = curTime;
            }

        }
        else
        {
            best = cur;
            bestTime = curTime;
        }

        File.WriteAllText(Application.dataPath + "/SaveData/bestTimeStamp.json", bestTime);
        return true;
    }
    public bool TimeCompare(TimeStamp a, TimeStamp b)
    {
        if (b == null)
            return true;
        if (a == null)
            return false;
        if (a.hourCount > b.hourCount)
            return false;
        else if (a.hourCount == b.hourCount && a.minuteCount > b.hourCount)
            return false;
        else if (a.hourCount == b.hourCount && a.minuteCount == b.minuteCount && a.secondsCount > b.secondsCount)
            return false;

        return true;
    }

}
