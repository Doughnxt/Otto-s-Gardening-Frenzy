using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    // Variables
    private enum plantType { sunflower, tomato, carrot, lettuce } // Plant types
    [SerializeField] plantType seed; // Variable for plant type
    private Inventory inventory; // Inventory variable

    private SpriteRenderer sRenderer; // Sprite renderer variable
    [SerializeField] private Sprite defaultSprite; // Defailt sprite
    [SerializeField] private Sprite highlightedSprite; // Highlighted sprite
    [SerializeField] private GameObject actionsText; // Action text for planting seed
    [SerializeField] private GameObject actionsText2; // Action text for watering plant
    [SerializeField] private GameObject needSeedsText; // UI text for if the player doesn't have seeds
    [SerializeField] private GameObject needWaterText; // UI text for if the player doesn't have enough water
    [SerializeField] private float displayTime = 2f; // Display time for the different texts
    private bool isColliding; // Boolean for if the player is collding

    [SerializeField] private GameObject[] sunflowerSprites; // An array of all the sunflower sprites
    [SerializeField] private GameObject[] tomatoSprites; // An array of all the tomato sprites
    [SerializeField] private GameObject[] carrotSprites; // An array of all the carrot sprites
    [SerializeField] private GameObject[] lettuceSprites; // An array of all the lettuce sprites

    [SerializeField] private float sunflowerGrowTime = 2f; // The grow time for each stage of sunflowers
    [SerializeField] private float tomatoGrowTime = 2f; // The grow time for each stage of tomatoes
    [SerializeField] private float carrotGrowTime = 1.4f; // The grow time for each stage of carrots
    [SerializeField] private float lettuceGrowTime = 1.2f; // The grow time for each stage of lettuce

    public bool growing = false; // variable for if the plant is growing
    public bool planted = false; // variable for if the plant has been planted
    public bool canWater = false; // variable for if the plant has been planted and can be watered
    public bool isWatering = false; // variable for if the plant is being watered

    [SerializeField] private float wateringTime = 1f; // Watering time
    [SerializeField] private float moveSpeed = 3f; // Speed of moving Otto to the watering position
    [SerializeField] private GameObject waterParticles; // The water particles object
    private PlayerMovement player; // Player script
    [SerializeField] private Vector3 wateringPosition; // Watering position for the player to move to
    public bool atWateringPosition = false; // variable for if Otto is at the watering position

    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>(); // Sets sRenderer to the attached Sprite Renderer component
        inventory = FindObjectOfType<Inventory>(); // Sets inventory variable to the object found in the scene
        player = FindObjectOfType<PlayerMovement>(); // Sets player variable to the object found in the scene
        // Sets all variables to false
        growing = false;
        planted = false;
        canWater = false;
        isWatering = false;
        atWateringPosition = false;
        // Disables all text
        actionsText.SetActive(false);
        actionsText2.SetActive(false);
        needSeedsText.SetActive(false);
        needWaterText.SetActive(false);
        waterParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!growing) // Checks if the plant is NOT growing
        {
            // Loops through each plant sprite loop to disable the sprite
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

        if (isWatering) // Checks if the plant is currently being watered
        {
            if (atWateringPosition) // Checks if Otto is at the watering position
            {
                StartCoroutine(Watering()); // Calls the function that shows the water particles
            }
            else
            {
                if (player.transform.position != wateringPosition) // Checks if Otto's position is NOT at the watering position
                {
                    // Moves the player towards the watering position by using Vector2.MoveTowards
                    player.transform.position = Vector2.MoveTowards(player.transform.position, wateringPosition, moveSpeed * Time.deltaTime);
                }
                else
                {
                    atWateringPosition = true; // Sets this variable to true as Otto's position equals the watering position
                }
            }
        }
        if (isColliding) // Checks if the player is colliding with the plant pot
        {
            if (canWater) // Checks if the plant can be watered
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
                {
                    if (inventory.waterAmount < 10) // Checks if the player has enough water
                    {
                        StartCoroutine(DisplayInventoryText(needWaterText)); // Calls function that shows water is needed
                    }
                    else
                    {
                        WaterPlant(); // Waters the plant
                    }

                }
            }
            else if (!planted) // Checks if the plant has NOT been planted
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Checks if the space key is pressed
                {
                    if (inventory.items.Count == 0) // Checks if the player has ZERO seeds in their inventory
                    {
                        StartCoroutine(DisplayInventoryText(needSeedsText)); // Calls function that shows seeds are needed
                    }
                    else
                    {
                        PlantSeed(); // Plants seeds
                    }
                }
            }
        }
        if (!canWater && planted) // Checks if the plant has been watered and planted
        {
            actionsText2.SetActive(false); // Disables actions test for watering
            sRenderer.sprite = defaultSprite; // Sets the sprite of the plant pot to the default
        }
        if (planted) // Checks if the plant has been planted
        {
            actionsText.SetActive(false); // Disables actions text for planting
            if (isColliding && canWater) // Checks if the player is collidng and the plant can be watered
            {
                actionsText2.SetActive(true); // Shows actions text for watering
            }
        }
    }

    // When called, this method checks the seed type of the last seed collected, setting the plant pot's seed type to that seed.
    // If the inventory is empty, it defaults to sunflower as the seed type
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

    // When called, this method plants a seed into the pot by removing the last seed from the inventory.
    private void PlantSeed()
    {
        GetSeedType(); // Gets the seed type of the last seed
        inventory.items.RemoveAt(inventory.items.Count - 1); // Removes the last seed from the inventory
        canWater = true; // Enables watering
        planted = true; // Marks that the plant has been planted
    }

    // When called, this method waters and grows the plant.
    private void WaterPlant()
    {
        if (canWater) // Checks if watering is enabled
        {
            if (inventory.waterAmount >= 10) // Checks if there's enough water in the inventory
            {
                player.movementEnabled = false; // Disables the player's movement
                player.rb.velocity = Vector2.zero; // Stops the player's potential momentum
                isWatering = true; // Marks that the plant is being watered

                // A switch statement that does different actions depending on the different values of the seed variable.
                // For each of the different plant types, a function is called where they are grown
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

    // This function grows the plants. It takes two paramaters: an array of plant sprites and a float value for the growing time.
    // To grow the plants, the function displays the given plant's grow stage sprite for the growing time
    private IEnumerator GrowSeed(GameObject[] plants, float growTime)
    {
        inventory.waterAmount -= 10; // Removes water from the inventory
        growing = true; // Marks that the plant is growing
        canWater = false; // Disables watering
        yield return new WaitForSeconds(wateringTime + 0.5f); // Waits for the watering particles to finish being shown
        yield return new WaitForSeconds(growTime); // Waits for the given grow time
        plants[0].SetActive(true); // Sets the 1st plant sprite to active
        yield return new WaitForSeconds(growTime); // Waits for the given grow time
        plants[0].SetActive(false); // Disbales the 1st plant sprite
        plants[1].SetActive(true); // Sets the 2nd plant sprite to active
        yield return new WaitForSeconds(growTime); // Waits for the given grow time
        plants[1].SetActive(false);// Disbales the 2nd plant sprite
        plants[2].SetActive(true); // Sets the 3rd plant sprite to active
        HarvestPlant harvestPlant = plants[2].GetComponent<HarvestPlant>(); // Gets the harvest plant component from the plant gameObject
        harvestPlant.harvested = false; // Allows the plant to be harvested
    }

    // Called when a 2D collider enters this game object's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Checks if the collision was the player
        {
            if (!planted) // Checks if the seed hasn't been planted
            {
                sRenderer.sprite = highlightedSprite; // Changes to the highlighted sprite
                actionsText.SetActive(true); // Displays text showing availible actions
            }
            else if (!growing && planted) // Checks if the seed has been planted and not watered
            {
                sRenderer.sprite = highlightedSprite; // Changes to the highlighted sprite
                actionsText2.SetActive(true); // Displays text showing availible actions
            }
            isColliding = true; // Player is colliding
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
            isColliding = false; // Player isn't collding
        }
    }

    // This function displays text that shows if an item is needed for a short amount of time when called
    private IEnumerator DisplayInventoryText(GameObject text)
    {
        text.SetActive(true); // Displays text
        yield return new WaitForSeconds(displayTime); // Waits for display time
        text.SetActive(false); // Disables text
    }

    // This function displays the watering particles when the plant is being watered.
    private IEnumerator Watering()
    {
        isWatering = false; // Sets isWatering to false, marking that the plant is already being watered
        waterParticles.SetActive(true); // Enables the water particles
        yield return new WaitForSeconds(wateringTime); // Waits for the watering time
        waterParticles.SetActive(false); // Disables the water particles
        player.movementEnabled = true; // Enables the player to move again
    }
}
