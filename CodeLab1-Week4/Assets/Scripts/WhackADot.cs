using UnityEngine;
using System.Collections;

public class WhackADot : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("hello mouse is down");
        //use the property to change the score
        GameManager.instance.Score++;
        
        //make the circle relocate to a random position
        Vector2 newPos = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
        transform.position = newPos;
    }
}