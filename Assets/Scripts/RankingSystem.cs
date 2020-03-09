using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RankingSystem : MonoBehaviour
{
    List<GameObject> Goats = new List<GameObject>();        // stores list of all active goats (size changes so List better than array)
    GameObject[] aliveGoats;                            // array of alive goats that will be ordered to give ranking
    string[] deadGoats = new string[8];             // array of the goats' names that are dead (first in the list will be last in ranking, etc)
    int deadGoatIndex;

    public GameObject winLocation;                  // top of mountain

    bool gotItems;                                  // gets all the references to winLocation, etc when new game starts (since old objs were destroyed)

    float[] positionsOfNames = new float[8];
    Vector2 screenDimensions;
    GameObject[] rankNames = new GameObject[16];
    private Vector3 topGoatPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        screenDimensions = new Vector2(Screen.width, Screen.height);
        /*
        positionsOfNames[0] = 1040;
        positionsOfNames[1] = 1000;
        positionsOfNames[2] = 960;
        positionsOfNames[3] = 920;
        positionsOfNames[4] = 880;
        positionsOfNames[5] = 840;
        */

        positionsOfNames[0] = screenDimensions.y - 50;
        positionsOfNames[1] = screenDimensions.y -90;
        positionsOfNames[2] = screenDimensions.y -130;
        positionsOfNames[3] = screenDimensions.y -170;
        positionsOfNames[4] = screenDimensions.y -210;
        positionsOfNames[5] = screenDimensions.y -250;
        positionsOfNames[6] = screenDimensions.y - 290;
        positionsOfNames[7] = screenDimensions.y - 330;
        gotItems = false;
        deadGoatIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gotItems && SceneManager.GetActiveScene().buildIndex == 1)     // game scene, it finds all needed references  
        {
            winLocation = GameObject.Find("WinLocation");
            gotItems = true;
            deadGoatIndex = 0;
            deadGoats = new string[8];
            rankNames = GetComponentInParent<PlayerInstanceGenerator>().goatNameGraphics;
        }

        if (gotItems && SceneManager.GetActiveScene().buildIndex == 0)      // menu scene, it resets values so code works in game scene
        {
            gotItems = false;
            deadGoatIndex = 0;
            rankNames = GetComponentInParent<PlayerInstanceGenerator>().goatNameGraphics;
        }

        
    }

    public void StartRanking()          // called by PlayerInstanceGenerator Script to let this one know that rankings can start
    {
        InvokeRepeating("rankGoats", 0.5f, 0.5f);           // call this method every half second. Not needed to call every frame
        aliveGoats = Goats.ToArray();                       // get goat objs and put them in alive goats array for ordering
    }


    void rankGoats()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && gotItems && rankNames != null)
        {
            MergeSort(aliveGoats, 0, aliveGoats.Length - 1);        // sort the array based on distance from the win location

            for (int i = 0; i < aliveGoats.Length; i++)
            {
                if (aliveGoats[i] != null)              // go through array and get the names of the goats and put them in the ranking string
                {
                    if (i == 0)
                    {
                        topGoatPos = aliveGoats[i].transform.position;
                    }
                    Transform goatNamePos = rankNames[aliveGoats[i].GetComponent<GoatSlingShot>().playerIndex * 2 - 2].transform;

                    goatNamePos.position = new Vector3(goatNamePos.position.x, positionsOfNames[i], goatNamePos.position.z);

                }
            }
            int pos = 0;
            for (int i = deadGoats.Length -1; i >= 0; i--)   // go through array backwards for dead ones, since those who died first will be last in ranking
            {
                
                if (deadGoats[i] != null)
                {
                    int index = int.Parse(deadGoats[i].Substring(0, 1));
                    Transform goatNamePos = rankNames[index * 2 - 1].transform;
                    if (!deadGoats[i].Contains("D"))
                    {
                        goatNamePos = rankNames[index * 2 - 2].transform;
                    }

                    goatNamePos.position = new Vector3(goatNamePos.position.x, positionsOfNames[aliveGoats.Length + pos++], goatNamePos.position.z);

                }
            }
        }
    }



    void MergeSort(GameObject[] input, int startIndex, int endIndex)    // merge sort method, sorts in n-Log(n) time, faster than regular sort method
    {
        int mid;

        if (endIndex > startIndex)
        {
            mid = (endIndex + startIndex) / 2;
            MergeSort(input, startIndex, mid);
            MergeSort(input, (mid + 1), endIndex);
            Merge(input, startIndex, (mid + 1), endIndex);
        }
    }

    private void Merge(GameObject[] input, int left, int mid, int right)    // helper method to merge sort
    {
        GameObject[] temp = new GameObject[input.Length];
        int i, leftEnd, lengthOfInput, tmpPos;
        leftEnd = mid - 1;
        tmpPos = left;
        lengthOfInput = right - left + 1;

        //selecting smaller element from left sorted array or right sorted array and placing them in temp array.
        while ((left <= leftEnd) && (mid <= right))
        {
            if (CompareDistance(input[left], input[mid]) <= 0)   // compares 2 items based on distance from win location
            {
                temp[tmpPos++] = input[left++];
            }
            else
            {
                temp[tmpPos++] = input[mid++];
            }
        }
        //placing remaining element in temp from left sorted array
        while (left <= leftEnd)
        {
            temp[tmpPos++] = input[left++];
        }

        //placing remaining element in temp from right sorted array
        while (mid <= right)
        {
            temp[tmpPos++] = input[mid++];
        }

        //placing temp array to input
        for (i = 0; i < lengthOfInput; i++)
        {
            input[right] = temp[right];
            right--;
        }
    }


    int CompareDistance(GameObject first, GameObject second)    // compares distance from win location for each obj
    {
        if (winLocation != null)
        {
            float distanceForFirst = int.MaxValue;
            float distanceForSecond = int.MaxValue;
            if (first != null)
                distanceForFirst  = Vector3.Distance(first.transform.position, winLocation.GetComponent<Transform>().position);
            if (second != null)
                distanceForSecond = Vector3.Distance(second.transform.position, winLocation.GetComponent<Transform>().position);

            if (distanceForFirst > distanceForSecond)   // if first is closer to win location, return 1
                return 1;
            else if (distanceForFirst < distanceForSecond)  // if second is closer, return -1
                return -1;
            else
                return 0;                                   // other wise return 0 if they are equal
        }
        else
            return 0;
    }



    public void addGoat(GameObject goat)        // called by PlayerInstanceGenerator script to add goat to list
    {
        Goats.Add(goat);
        aliveGoats = Goats.ToArray();
    }
    /* Called by goatSlingShot script. If isDead is true: 
             called when goat dies. remove from alive goat list, and add to dead goat array
       if isDead is false:
             called when 1 player has won the game, do the same thing as when isDead is true, but dont write dead on it.
             (goats are anyways killed when 1 player has won, so that it can reset for the next round, so if it is not added to dead goats array
             then it will simply disappear from the ranking system, this way it is still there)
    */
    public void RemoveGoat(GameObject goat, bool isDead)     
    {
        int index = goat.GetComponentInChildren<GoatSlingShot>().playerIndex;
        string name = index.ToString();
        if (isDead)
        {
            name += "D";
            rankNames[index * 2 - 2].GetComponent<RawImage>().enabled = false;
            rankNames[index * 2 - 1].GetComponent<RawImage>().enabled = true;
        }
        deadGoats[deadGoatIndex++] = name;

        Goats.Remove(goat);
        aliveGoats = Goats.ToArray();
    }

    public Vector3 TopGoatPosition()
    {
        return topGoatPos;
    }
}
