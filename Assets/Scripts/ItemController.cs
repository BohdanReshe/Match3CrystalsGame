using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class ItemController : MonoBehaviour
{
    private static ItemController previousSelectedItem = null;

    public static ItemController instance;

    public static bool isGameOver;

    public bool isSelected = false;
    public bool isMatchFound = false;

    private Vector3[] directions = new Vector3[] { Vector3.up * 4,
                                                   Vector3.down * 4,
                                                   Vector3.left * 4,
                                                   Vector3.right * 4 };

    void Start()
    {
        instance = GetComponent<ItemController>();
    }

    private void OnMouseDown()
    {
        if (!isGameOver)
        {
            if (gameObject == null)
            {
                return;
            }
            if (isSelected)
            {
                Deselect();
            }
            else
            {
                if (previousSelectedItem == null)
                {
                    Select();
                }
                else
                {
                    if (GetListOfNeighbourItemsFromAllDirections(gameObject).Contains(previousSelectedItem.gameObject))
                    {
                        SwapItems();
                        previousSelectedItem.ClearAllMatches();
                        ClearAllMatches();
                        previousSelectedItem.Deselect();
                    }
                    else
                    {
                        Select();
                        previousSelectedItem.GetComponent<ItemController>().Deselect();
                    }
                }
            }
        }
    }

    private void SwapItems()
    {
        Vector3 currentPosition = gameObject.transform.position;
        gameObject.transform.position = previousSelectedItem.gameObject.transform.position;
        previousSelectedItem.gameObject.transform.position = currentPosition;
        AudioManager.instance.PlaySwapSound();
        GameManager.instance.MovesUpdate(-1);
    }

    public void Select()
    {
        isSelected = true;
        previousSelectedItem = gameObject.GetComponent<ItemController>();
        // start animation while game object is selected
        AudioManager.instance.PlaySelectedSound();
    }

    private void Deselect()
    {
        isSelected = false;
        previousSelectedItem = null;

        // return idle animation;
        transform.localScale = new Vector3(1, 1, 1);
    }

    private GameObject GetOneNeighbourFromOneDirection(Vector3 oneDir)
    {
        Physics.Raycast(transform.position, oneDir, out RaycastHit hit);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<GameObject> GetListOfNeighbourItemsFromAllDirections(GameObject gO)
    {
        List<GameObject> neighbourItemsList = new List<GameObject>();
        for (int i = 0; i < directions.Length; i++)
        {
            neighbourItemsList.Add(GetOneNeighbourFromOneDirection(directions[i]));
        }
        return neighbourItemsList;
    }

    private List<GameObject> FindOneMatch(Vector3 oneDir)
    {
        List<GameObject> matchingItems = new List<GameObject>();
        Physics.Raycast(transform.position, oneDir, out RaycastHit hit);
        while (hit.collider != null &&
               hit.collider.gameObject != gameObject &&
               hit.collider.gameObject.name == gameObject.name)
        {
            matchingItems.Add(hit.collider.gameObject);
            Physics.Raycast(hit.collider.transform.position, oneDir, out RaycastHit furtherHit);
            hit = furtherHit;
        }
        return matchingItems;
    }

    private void ClearMatchWithSpecificPath(Vector3[] paths)
    {
        List<GameObject> matchingItems = new List<GameObject>();
        for (int i = 0; i < paths.Length; i++)
        {
            matchingItems.AddRange(FindOneMatch(paths[i]));
        }
        if (matchingItems.Count >= 2)
        {
            foreach (GameObject element in matchingItems)
            {
                if (element != null)
                {
                    BoardManager.instance.InstantiateNewItem(element.transform.position);
                    GameManager.instance.ScoreUpdate(50);
                    Destroy(element);
                }
            }
            isMatchFound = true;
        }
    }

    public void ClearAllMatches()
    {
        if (gameObject == null)
        {
            return;
        }
        else
        {
            ClearMatchWithSpecificPath(new Vector3[2] { Vector2.left * 4, Vector2.right * 4 });
            ClearMatchWithSpecificPath(new Vector3[2] { Vector2.up * 4, Vector2.down * 4 });
            if (isMatchFound)
            {
                BoardManager.instance.InstantiateNewItem(gameObject.transform.position);
                GameManager.instance.ScoreUpdate(50);
                isMatchFound = false;
                Destroy(gameObject);
                AudioManager.instance.PlayMatchSound();
            }
        }
    }
}