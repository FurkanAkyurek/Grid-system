using UnityEngine;
using TMPro;

public class MatchCountText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public TextMeshProUGUI Text
    {
        get { return text; }
        set { text = value; }
    }

    public void SetMatchCountText()
    {
        Text.text = "Match Count : " + PlayerPrefs.GetInt("MatchCount").ToString();
    }
}
