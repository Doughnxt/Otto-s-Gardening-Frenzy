using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<string> items = new List<string>(); // List of seeds in inventory
    public List<string> itemsLastFrame; // List to compare values
    public int maxSeedCarryAmount = 4; // Max inventory capacity

    [SerializeField] private TextMeshProUGUI text; // Text values
    [SerializeField] private UnityEngine.UI.Slider waterAmountSlider;
    public Gradient waterFill; // Gradient for water fill color
    public UnityEngine.UI.Image waterImage;
    private List<int> itemCounts = new List<int>() { 0, 0, 0, 0 }; // List of counts of each type of seed
    private bool counting = false; // Boolean for if the counting method is running
    public float waterAmount; // Amount of water in inventory
    public float maxWaterAmount = 20; // Maximum amount of water that can be stored in the inventory

    // Start is called before the first frame update
    void Start()
    {
        waterAmount = 0;
        itemsLastFrame = new List<string>(items); // Sets the compare list to the items list
    }

    // Update is called once per frame
    void Update()
    {
        waterImage.color = waterFill.Evaluate(waterAmountSlider.normalizedValue);
        waterAmountSlider.value = waterAmount; // Sets the UI water container to the amount of water collected
        text.text = $"x{itemCounts[0]} \nx{itemCounts[1]} \nx{itemCounts[2]} \nx{itemCounts[3]}"; // Updates text with counts of each seed

        if (items != itemsLastFrame) // Checks if seed values have updated since last frame by looking at the compare list
        {
            if (!counting) // Checks if counting started
                CountSeeds(); // Counts seeds
        }

        itemsLastFrame = new List<string>(items); // Sets compare list to the items list
    }

    // This method counts the number of each type of seed in the inventory
    private void CountSeeds()
    {
        counting = true; // Sets counting to true, as counting has started
        itemCounts = new List<int>() { 0, 0, 0, 0 }; // Resets count list to 0
        foreach (var item in items) // Loops through each item in the seed list and updates the count of each type of seed
        {
            if (item == "sunflower")
            {
                itemCounts[0]++;
            }
            else if (item == "tomato")
            {
                itemCounts[1]++;
            }
            else if (item == "carrot")
            {
                itemCounts[2]++;
            }
            else if (item == "lettuce")
            {
                itemCounts[3]++;
            }
        }
        counting = false; // Sets counting to false, as counting has ended
    }
}
