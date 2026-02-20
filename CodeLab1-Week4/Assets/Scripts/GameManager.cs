using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //game object with the timer text
    public GameObject timerTextObject;
    //timer display as text
    TextMeshPro textMeshComponent;
    //game manager singleton!
    public static GameManager instance;
    //elapsed time
    float time = 0;
    //time until game over
    public int gameTime = 5;
    //is game over or not
    bool isGameOver;
    //private variable for the score
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

  
    //list for the high scores
  List<int> highScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //create a basic property for score
    void Start()
    {
        textMeshComponent = timerTextObject.GetComponent<TextMeshPro>(); //gets the text displays
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
        //calls this function every 1 second after one second
        InvokeRepeating("IncreaseTime", 1f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        //time += Time.deltaTime;
        //sets the timer display
        textMeshComponent.SetText("Time: " + Math.Floor(time));
    
        if (time >= gameTime) //condition for game over
        {
            Debug.Log("Game Over"); 
            //textMeshComponent.SetText("Game Over");
            //calls the function to save the high scores
            if (!isGameOver)
            {
                UpdateHighScores(); //but if game is not over then update high scores file
            }

            isGameOver = true;  //if condition is met then game is over
        }
    }
    void IncreaseTime()
    {
        //literally adds time to the timer so will jump up faster
        time++; 
        //if you just want to use Invoke to repeatedly call something
        Invoke("IncreaseTime", 1f);
    }
    //function for updating high scores and saving them to a file
    void UpdateHighScores()
    {
        //TODO: review diff between data Path and persistent data Path
        string filePath = Application.dataPath + "/Resources/HighScores.txt";
        
        if (highScores == null) //we don't have the data yet from the file
        {
            highScores = new List<int>(); //calls new list of high scores
            //if the file does not exist
            if (!File.Exists(filePath))
            {
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
                    //TODO: read value from the file and put them into the list
                string scores = File.ReadAllText(filePath);
                //scores = scores.TrimEnd(',');
                //string[] scoresArray = scores.Split(new [] {","}, StringSplitOptions.None);
                string[] scoresArray = scores.Split(","); //but trailing comma seems to break this
                //must fix comma!

                for (int i = 0; i < scoresArray.Length; i++)
                {
                    highScores.Add(int.Parse(scoresArray[i]));
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
