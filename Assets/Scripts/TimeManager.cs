using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TimeManager : Singleton<TimeManager>
{

    public Text timerText;

    private TimeStamp time;

    


    // Start is called before the first frame update
    void Start()
    {
        time = new TimeStamp();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimerUI();
    }
    //call this on update
    public void UpdateTimerUI()
    {
        //set timer UI
        time.secondsCount += Time.deltaTime;
        timerText.text = time.hourCount + "h:" + time.minuteCount + "m:" + (int)time.secondsCount + "s";
        if (time.secondsCount >= 60)
        {
            time.minuteCount++;
            time.secondsCount = 0;
        }
        else if (time.minuteCount >= 60)
        {
            time.hourCount++;
            time.minuteCount = 0;
        }
    }
    //save current time stamp as current played time stamp
    public void SaveTimeStampToJson() 
    {

        string currentTimeStamp = JsonUtility.ToJson(time);
        File.WriteAllText(Application.dataPath + "/SaveData/curTimeStamp.json", currentTimeStamp);
    }

}
