using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //game object with the timer text
    public GameObject timerTextObject;

    TextMeshPro textMeshComponent;
    
    public static GameManager instance;
    
    float time = 0;

    public int gameTime = 5;
    
    bool isGameOver;

    private int score = 0;
//with a property you don't want the functions inside to refer to the property
    public int Score
    {
        set
        {
            score = value;
        }
        get
        {
            return score;
        }
    }

  

  List<int> highScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //create a basic property for score
    void Start()
    {
        textMeshComponent = timerTextObject.GetComponent<TextMeshPro>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //but if you try to randomize the rate it will assign
        //a random number the first time but repeat with that number
        InvokeRepeating("IncreaseTime", 1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        textMeshComponent.SetText("Time: " + Math.Floor(time));

        if (time >= gameTime)
        {
            Debug.Log("Game Over");
            //textMeshComponent.SetText("Game Over");
            //calls the function to save the high scores
            if (!isGameOver)
            {
                UpdateHighScores();
            }

            isGameOver = true;
        }
    }
    void IncreaseTime()
    {
        //literally adds time to the timer so will jump up faster
        time++; 
        //if you just want to use Invoke to repeatedly call something
        Invoke("IncreaseTime", 1f);
    }

    void UpdateHighScores()
    {
        //TODO: review diff between data Path and persistent data Path
        string filePath = Application.dataPath + "/Resources/HighScores.txt";
        
        if (highScores == null) //we don't have the data yet from the file
        {
            
            //if the file does not exist
            if (!File.Exists(filePath))
            {
                highScores = new List<int>();
                //add some default values
                //(var to change over time; do we stay in the loop; what to change with each loop)
                //this for loop makes new default high scores
                for (int i = 0; i < 10; i++)
                {
                    highScores.Add(i * 10);
                }

            }
            else
            {
                {
                    //TODO: read value from the file and put them into the list
                }
            }
        }
        //check to see if there is a new high score
        
        //compare score to each part of the list
        for (int i = 0; i < highScores.Count; i++)
        {
            if (highScores[i] > score) 
            {
                //put new high score in the list
                ////insert high score into list at this point
                highScores.Insert(i, score);
                //this will break the loop at this point
                break;
            }
            
        }
        //remove the high score that is no longer in the list
        //will always remove the score in the 0 spot in the list
        //this is not in the for loop because it happens one time
        highScores.RemoveAt(0);
        
        //TODO: save new high scores
        
        string fileContents = "";
        
        //go through every high score in the list
        //and make it a string and add it to the fileContents
        //with a delimiter, which is a string that separates the values added to the list
        for (int i = 0; i < highScores.Count; i++)
        {
            //adds current score to file contents
            fileContents += highScores[i] + ","; //or "|"
        }

        File.WriteAllText(filePath, fileContents);
     
    }
}
