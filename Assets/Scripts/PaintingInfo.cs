using TMPro;
using UnityEngine;

public class PaintingInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_name;
    [SerializeField] private TextMeshProUGUI text_author;
    [SerializeField] private TextMeshProUGUI text_description;
    
    public void InitData(string _name, string author, string _description)
    {
        text_name.text = _name;
        text_author.text = author;
        text_description.text = _description;
    }
}