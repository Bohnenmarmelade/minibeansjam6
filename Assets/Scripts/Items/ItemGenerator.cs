using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemGenerator : MonoBehaviour
{
    public LevelController LevelController;
    public Transform generationPoint;
    public List<GameObject> foodItemPrefabs = new List<GameObject>();
    public List<GameObject> garbageItemPrefabs = new List<GameObject>();
    public int foodItemsToBeGenerated = 5;
    public int numberOfFoodSpawnIntervals = 5;
    public float distanceToBorders = 7f;
    public float minHorizontalDistanceBetweenItems = 2f;
    public float maxHorizontalDistanceBetweenItems = 5f;
    public bool multipleItemsAtOnce = false;
    
    private float levelTime = 100f;                //time value
    private float timeOfFoodSpawnInterval = 0f;    //time value
    private float timeUntilNextFoodSpawn = 0f;     //time stamp
    private const int amountItemsAtOnce = 2;
    private bool generationAllowed = true;
    private GameObject generatedItem;
    private float itemWidth;
    private float itemHeight;
    private bool firstGeneration = true;
    private float generatedAdditionalHorizontalDistance = 0f;
    private bool foodHasToBeSpawned = false;
    private int foodItemsPerInterval = 0;
    private int foodItemsGeneratedInInterval = 0;
    private bool generateFood = true;

    // Start is called before the first frame update
    void Start()
    {
        itemWidth = foodItemPrefabs[0].GetComponent<BoxCollider2D>().size.x;
        levelTime = LevelController.LevelDuration();
        timeOfFoodSpawnInterval = levelTime / numberOfFoodSpawnIntervals;
        timeUntilNextFoodSpawn = Time.time + timeOfFoodSpawnInterval;
        foodItemsPerInterval = Mathf.CeilToInt(foodItemsToBeGenerated / numberOfFoodSpawnIntervals);

        EventManager.Instance.OnLevelFinished.AddListener(OnLevelFinished);
        EventManager.Instance.OnLevelStarted.AddListener(OnLevelStarted);
    }

    // Update is called once per frame
    void Update()
    {
        if (generationAllowed && CheckHorizontalItemDistance())
        {
            GenerateItems();
        }
    }

    private void OnLevelFinished()
    {
        generationAllowed = false;
    }

    private void OnLevelStarted()
    {
        generationAllowed = true;
        firstGeneration = true;
    }

    private void GenerateItems()
    {
        // if the interval is over and not enough food has been spawned, force food spawn until enough food is spawned
        if (CheckIfFoodHasToBeSpawned())
        {
            foodItemsGeneratedInInterval++;
            // spawn an item anywhere between lower and upper bounds
            generatedItem = SpawnItem(foodItemPrefabs[Random.Range(0, foodItemPrefabs.Count)], 0, distanceToBorders);
            return;
        }
        
        // calculate the time stamp for the next interval
        CalculateTimeOfNextFoodSpawnInterval();
        
        // generate additional horizontal distance
        generatedAdditionalHorizontalDistance = Random.Range(0f,
            maxHorizontalDistanceBetweenItems - minHorizontalDistanceBetweenItems);
        
        // check if multiple items should be spawned
        if (multipleItemsAtOnce)
        {
            GenerateMultipleItems();
        }
        else
        {
            GenerateSingleItem();
        }
    }
    
    private void CalculateTimeOfNextFoodSpawnInterval()
    {
        if (Time.time >= timeUntilNextFoodSpawn)
        {
            timeUntilNextFoodSpawn = timeOfFoodSpawnInterval + Time.time;
        }
    }

    private void GenerateMultipleItems()
    {
        GameObject prevGeneratedItem = null;
        
        // generate 2 items
        for (int k = 0; k >= amountItemsAtOnce; k++)
        {
            // if food is allowed to be generated add it to the randomizer
            int i = 1;
            if (generateFood)
            {
                i = 2;
            }

            float lowerBound = 0;
            float upperBound = distanceToBorders;
            // calculate lower and upper bounds based on previous generated item
            if (prevGeneratedItem != null)
            {
                // calculate bounds based on previous generated item
                float upperDistance = (generationPoint.position.y + distanceToBorders) -
                                      prevGeneratedItem.transform.position.y;

                float lowerDistance = prevGeneratedItem.transform.position.y -
                                      (generationPoint.position.y - distanceToBorders);

                // get upper half bounds
                if (upperDistance >= lowerDistance)
                {
                    lowerBound = prevGeneratedItem.transform.position.y + itemHeight;
                }
                // get lower half bounds
                else
                {
                    upperBound = prevGeneratedItem.transform.position.y - itemHeight;
                }
            }
            
            // generate food or garbage
            int food = Random.Range(0, i);
            switch (food)
            {
                // generate food
                case 1:
                {
                    foodItemsGeneratedInInterval++;
                    UpdateFoodGeneration();
                    prevGeneratedItem = SpawnItem(foodItemPrefabs[Random.Range(0, foodItemPrefabs.Count)], lowerBound, upperBound);
                    break;
                }

                // generate garbage
                case 0:
                {
                    prevGeneratedItem = SpawnItem(garbageItemPrefabs[Random.Range(0, garbageItemPrefabs.Count)], lowerBound, upperBound);
                    break;
                }
            }
        }

        generatedItem = prevGeneratedItem;
    }

    private void GenerateSingleItem()
    {
        // if food is allowed to be generated add it to the randomizer
        int i = 1;
        if (generateFood)
        {
            i = 2;
        }
        
        // generate food or garbage
        int food = Random.Range(0, i);
        switch (food)
        {
            // generate food
            case 1:
            {
                foodItemsGeneratedInInterval++;
                UpdateFoodGeneration();
                // spawn a food item anywhere between lower and upper bounds
                generatedItem = SpawnItem(foodItemPrefabs[Random.Range(0, foodItemPrefabs.Count)], 0, distanceToBorders);
                break;
            }

            // generate garbage
            case 0:
            {
                // spawn a garbage item anywhere between lower and upper bounds
                generatedItem = SpawnItem(garbageItemPrefabs[Random.Range(0, garbageItemPrefabs.Count)], 0, distanceToBorders);
                break;
            }
        }
    }

    private bool CheckHorizontalItemDistance()
    {
        // check minimal horizontal distance to before spawned items
        if (firstGeneration)
        {
            firstGeneration = false;
            return true;
        }

        if (!generatedItem)
        {
            return true;
        }
        return generatedItem.transform.position.x < generationPoint.transform.position.x - itemWidth - minHorizontalDistanceBetweenItems - generatedAdditionalHorizontalDistance;
    }

    private void UpdateFoodGeneration()
    {
        if (foodItemsPerInterval >= foodItemsGeneratedInInterval)
        {
            generateFood = false;
        }
    }

    private bool CheckIfFoodHasToBeSpawned()
    {
        return (foodItemsGeneratedInInterval < foodItemsPerInterval) && (Time.time >= timeUntilNextFoodSpawn);
    }

    // spawn an item anywhere between lower and upper bounds
    private GameObject SpawnItem(GameObject itemPrefab, float lowerBounds, float upperBounds)
    {
        //spawn item
        GameObject go = Instantiate(itemPrefab, generationPoint.position, itemPrefab.transform.rotation);
        
        //move item up or down depending on previously spawned platform
        go.transform.position += new Vector3(0, Random.Range(lowerBounds, upperBounds), 0);

        return go;
    }
}
