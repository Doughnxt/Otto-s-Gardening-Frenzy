using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    private enum plantType { sunflower, tomato, carrot, lettuce } // Plant types
    [SerializeField] plantType seed; // Variable for plant type
    private Inventory inventory; // Inventory variable

    private SpriteRenderer sRenderer;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite highlightedSprite;
    [SerializeField] private GameObject actionsText;
    [SerializeField] private GameObject actionsText2;
    [SerializeField] private GameObject needSeedsText;
    [SerializeField] private GameObject needWaterText;
    [SerializeField] private float displayTime = 2f;
    private bool isColliding;

    [SerializeField] private GameObject[] sunflowerSprites;
    [SerializeField] private GameObject[] tomatoSprites;
    [SerializeField] private GameObject[] carrotSprites;
    [SerializeField] private GameObject[] lettuceSprites;

    [SerializeField] private float sunflowerGrowTime = 2f;
    [SerializeField] private float tomatoGrowTime = 2f;
    [SerializeField] private float carrotGrowTime = 1.4f;
    [SerializeField] private float lettuceGrowTime = 1.2f;

    private bool growing = false;
    private bool planted = false;
    private bool canWater = false;

    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        inventory = FindObjectOfType<Inventory>(); // Sets inventory variable to the object found in the scene
        growing = false;
        planted = false;
        canWater = false;
        actionsText.SetActive(false);
        actionsText2.SetActive(false);
        needSeedsText.SetActive(false);
        needWaterText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!growing)
        {
            foreach (var f in sunflowerSprites)
            {
                f.SetActive(false);
            }
            foreach (var f in tomatoSprites)
            {
                f.SetActive(false);
            }
            foreach (var f in carrotSprites)
            {
                f.SetActive(false);
            }
            foreach (var f in lettuceSprites)
            {
                f.SetActive(false);
            }
        }
        if (isColliding)
        {
            if (canWater)
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
                {
                    if (inventory.waterAmount < 10)
                    {
                        StartCoroutine(DisplayInventoryText(needWaterText));
                    }
                    else
                    {
                        WaterPlant(); // Waters the plant
                    }

                }
            }
            else if (!planted)
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
                {
                    if (inventory.items.Count == 0)
                    {
                        StartCoroutine(DisplayInventoryText(needSeedsText));
                    }
                    else
                    {
                        PlantSeed(); // Plants seeds
                    }
                }
            }
        }
        if (!canWater && planted)
        {
            actionsText2.SetActive(false);
            sRenderer.sprite = defaultSprite;
        }
        if (planted)
        {
            actionsText.SetActive(false);
            if (isColliding && canWater)
            {
                actionsText2.SetActive(true);
            }
        }
    }

    private void GetSeedType()
    {
        if (inventory.items[inventory.items.Count - 1] == "sunflower")
        {
            seed = plantType.sunflower;
        }
        else if (inventory.items[inventory.items.Count - 1] == "tomato")
        {
            seed = plantType.tomato;
        }
        else if (inventory.items[inventory.items.Count - 1] == "carrot")
        {
            seed = plantType.carrot;
        }
        else if (inventory.items[inventory.items.Count - 1] == "lettuce")
        {
            seed = plantType.lettuce;
        }
        else if (inventory.items.Count == 0)
        {
            seed = plantType.sunflower;
        }
    }

    private void PlantSeed()
    {
        GetSeedType();
        inventory.items.RemoveAt(inventory.items.Count - 1); // Removes the last seed from the inventory
        canWater = true;
        planted = true;
    }
    private void WaterPlant()
    {
        if (canWater)
        {
            if (inventory.waterAmount >= 10)
            {
                switch (seed)
                {
                    case plantType.sunflower:
                        StartCoroutine(GrowSeed(sunflowerSprites, sunflowerGrowTime));
                        break;


                    case plantType.tomato:
                        StartCoroutine(GrowSeed(tomatoSprites, tomatoGrowTime));
                        break;


                    case plantType.carrot:
                        StartCoroutine(GrowSeed(carrotSprites, carrotGrowTime));
                        break;

                    case plantType.lettuce:
                        StartCoroutine(GrowSeed(lettuceSprites, lettuceGrowTime));
                        break;


                    default:
                        break;
                }
            }
        }
    }

    private IEnumerator GrowSeed(GameObject[] plants, float growTime)
    {
        inventory.waterAmount -= 10;
        growing = true;
        canWater = false;
        yield return new WaitForSeconds(growTime);
        plants[0].SetActive(true);
        yield return new WaitForSeconds(growTime);
        plants[0].SetActive(false);
        plants[1].SetActive(true);
        yield return new WaitForSeconds(growTime);
        plants[1].SetActive(false);
        plants[2].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            if (!planted)
            {
                sRenderer.sprite = highlightedSprite; // Changes to the highlighted sprite
                actionsText.SetActive(true); // Displays text showing availible actions
            }
            else if (!growing && planted)
            {
                sRenderer.sprite = highlightedSprite; // Changes to the highlighted sprite
                actionsText2.SetActive(true); // Displays text showing availible actions
            }
            isColliding = true;
        }
    }

    // Called when a 2D collider leaves this game object's collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            sRenderer.sprite = defaultSprite; // Changes to the regular sprite
            actionsText.SetActive(false); // Disables text showing availible actions
            actionsText2.SetActive(false); // Disables text showing availible actions
            isColliding = false;
        }
    }

    // This function displays text that shows if an item is needed for a shot amount of time when called
    private IEnumerator DisplayInventoryText(GameObject text)
    {
        text.SetActive(true); // Displays text
        yield return new WaitForSeconds(displayTime); // Waits for display time
        text.SetActive(false); // Disables text
    }
}
