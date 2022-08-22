using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public Action OnMatchCountChanged;

    public Action<int> OnRebuildButtonClicked;

    [SerializeField] private TextMeshProUGUI matchCountText;

    [SerializeField] private TMP_InputField sizeInputField;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;

        OnMatchCountChanged += SetMatchCountText;
    }

    private void Start()
    {
        SetMatchCountText();
    }

    private void SetMatchCountText()
    {
        matchCountText.text = "Match Count : " + PlayerPrefs.GetInt("MatchCount").ToString();
    }

    public void RebuildButton()
    {
        if (int.TryParse(sizeInputField.text, out int size))
        {
            if (size > 1 && size < 15)
            {
                OnRebuildButtonClicked(size);
            }
            else if(size > 15)
            {
                OnRebuildButtonClicked(15);
            }
            else if(size < 1)
            {
                OnRebuildButtonClicked(2);
            }
        }

        sizeInputField.text = "";
    }
}
