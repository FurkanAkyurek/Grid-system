using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private void OnEnable()
    {
        GridManager.onMatchCountChanged += this.OnMatchCountChanged;
    }
    private void OnDisable()
    {
        GridManager.onMatchCountChanged -= this.OnMatchCountChanged;
    }

    private void OnMatchCountChanged()
    {
        FindObjectOfType<MatchCountText>().SetMatchCountText();
    }
}
