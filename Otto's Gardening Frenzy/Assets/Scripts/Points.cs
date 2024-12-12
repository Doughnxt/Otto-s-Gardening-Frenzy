using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public float points = 0;
    private TextMeshProUGUI text;
    public TextMeshProUGUI text2;

    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Score: {points}";
        text2.text = $"Your score: {points}";
    }
}
