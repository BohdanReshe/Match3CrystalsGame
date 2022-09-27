using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    private GameObject[,] allItemsOnBoard;

    [Header("All used items")]
    [SerializeField] GameObject[] allPrefabs3d;
    [Header("Amount of rows")]
    public int xRows;
    [Header("Amount of columns")]
    public int yColumns;
    [Header("Amount of moves")]

    private List<GameObject> newCreatedItems = new List<GameObject>();
    private float step;
    private Vector3 startPosition;
    private Vector3[] directions;

    private GameObject tempItemWhileInstantiate;



    void Start()
    {

        directions = new Vector3[] { Vector3.up * step,
                                     Vector3.down * step,
                                     Vector3.left * step,
                                     Vector3.right * step };

        instance = GetComponent<BoardManager>();
        // find step between Prefabs
        step = allPrefabs3d[0].GetComponent<BoxCollider>().size.x;

        /* find the position of the first element in board which located in the left bottom corner
        hence the board will be centred for the best view */
        startPosition = new Vector3(-(xRows * step / 2 - step / 2),
                                    -(yColumns * step / 2 - step / 2),
                                    0);
        CreateBoard(startPosition);
    }

    public void CreateBoard(Vector3 vec)
    {
        allItemsOnBoard = new GameObject[xRows, yColumns];

        // set variables for the first condition
        int previousBelow = -1;
        int[] previousLeft = new int[yColumns];
        previousLeft[0] = -1;
        int randomPrefabIndex = -1;

        // instantiate Prefabs on board
        for (int x = 0; x < xRows; x++)
        {
            for (int y = 0; y < yColumns; y++)
            {
                // check for repeating
                while (previousBelow == randomPrefabIndex ||
                       previousLeft[y] == randomPrefabIndex)
                {
                    randomPrefabIndex = Random.Range(0, allPrefabs3d.Length);
                }
                // create Item
                var newItem = Instantiate(allPrefabs3d[randomPrefabIndex],
                                          vec,
                                          allPrefabs3d[randomPrefabIndex].transform.rotation);
                allItemsOnBoard[x, y] = newItem;

                previousBelow = randomPrefabIndex;
                previousLeft[y] = randomPrefabIndex;
                // step up
                vec = new Vector3(
                    vec.x,
                    vec.y + step,
                    vec.z);
                tempItemWhileInstantiate = allItemsOnBoard[x, y];
            }
            // step down to the first row and step right
            vec = new Vector3(
                vec.x + step,
                vec.y - step * yColumns,
                vec.z);
        }
    }

    public void InstantiateNewItem(Vector3 positionToCreate)
    {
        List<GameObject> possibleItems = new List<GameObject>();
        possibleItems.AddRange(allPrefabs3d);
        // check for repeating
        for (int i = 0; i < directions.Length; i++)
        {
            Physics.Raycast(positionToCreate, Vector3.up * 4, out RaycastHit hit);
            if (hit.collider != null)
            {
                possibleItems.Remove(hit.collider.gameObject);
                possibleItems.Remove(tempItemWhileInstantiate);
            }
        }
        int randomIndex = Random.Range(0, possibleItems.Count);
        newCreatedItems.Add(Instantiate(possibleItems[randomIndex],
                                        positionToCreate,
                                        possibleItems[randomIndex].transform.rotation));
        tempItemWhileInstantiate = possibleItems[randomIndex];
    }

    public void ShuffleTheBoard()
    {
        foreach (GameObject gO in allItemsOnBoard)
        {
            Destroy(gO);
        }
        foreach (GameObject gO in newCreatedItems)
        {
            Destroy(gO);
        }
        CreateBoard(startPosition);
        GameManager.instance.MovesUpdate(-1);
        AudioManager.instance.PlayShuffleSound();
    }
}
