using TMPro;
using UnityEngine;

public class CounterDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    public void Display(int value)
    {
        text.text = value.ToString();
    }

    public void Reset() {
        text.text = "0";
    }
}
