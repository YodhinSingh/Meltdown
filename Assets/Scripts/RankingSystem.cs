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
    string[] deadGoats = new string[6];             // array of the goats' names that are dead (first in the list will be last in ranking, etc)
    int deadGoatIndex;

    public Text rankings;                           // UI Text that info will be displayed on
    public GameObject winLocation;                  // top of mountain
    string rankingToString;                         // all the info will be put into this string before transferring onto the UI Text

    bool gotItems;                                  // gets all the references to winLocation, etc when new game starts (since old objs were destroyed)

    // Start is called before the first frame update
    void Start()
    {
        gotItems = false;
        deadGoatIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gotItems && SceneManager.GetActiveScene().buildIndex == 1)     // game scene, it finds all needed references  
        {
            winLocation = GameObject.Find("WinLocation");
            rankings = GameObject.Find("Rankings").GetComponent<Text>();
            gotItems = true;
            deadGoatIndex = 0;
            deadGoats = new string[6];
        }

        if (gotItems && SceneManager.GetActiveScene().buildIndex == 0)      // menu scene, it resets values so code works in game scene
        {
            gotItems = false;
            deadGoatIndex = 0;
        }
    }

    public void StartRanking()          // called by PlayerInstanceGenerator Script to let this one know that rankings can start
    {
        rankingToString = "Ranking \n";
        InvokeRepeating("rankGoats", 0.5f, 0.5f);           // call this method every half second. Not needed to call every frame
        aliveGoats = Goats.ToArray();                       // get goat objs and put them in alive goats array for ordering
    }


    void rankGoats()
    {
        if (rankings != null)
        {

            MergeSort(aliveGoats, 0, aliveGoats.Length - 1);        // sort the array based on distance from the win location

            for (int i = 0; i < aliveGoats.Length; i++)
            {
                if (aliveGoats[i] != null)              // go through array and get the names of the goats and put them in the ranking string
                {
                    string name = aliveGoats[i].GetComponentInChildren<MeshRenderer>().material.name;
                    name = name.Substring(0, name.Length - 1 - "(Instance)".Length) + "\n";
                    rankingToString += name;
                }
            }

            for (int i = deadGoats.Length-1; i >= 0; i--)   // go through array backwards for dead ones, since those who died first will be last in ranking
            {
                if (deadGoats[i] != null)
                {
                    rankingToString += deadGoats[i];
                }
            }

            rankings.text = rankingToString;
            rankingToString = "Ranking \n";         // reset string for next iteration
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
        string name = goat.GetComponentInChildren<MeshRenderer>().material.name;
        if (isDead)
            name = name.Substring(0, name.Length - 1 - "(Instance)".Length) + " (DEAD) \n";
        else
            name = name.Substring(0, name.Length - 1 - "(Instance)".Length) + "\n";
        deadGoats[deadGoatIndex++] = name;

        Goats.Remove(goat);
        aliveGoats = Goats.ToArray();
    }
}
