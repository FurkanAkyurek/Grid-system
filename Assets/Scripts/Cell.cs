using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Cell : MonoBehaviour
{
    [SerializeField] private SpriteRenderer cellSprite;

    [HideInInspector] public bool isSelected;

    public SpriteRenderer selectedSprite;
    private Vector3 selectedScale;

    private int posX;
    private int posY;

    private void Start()
    {
        selectedScale = selectedSprite.transform.localScale;
    }

    public IEnumerator ResetCell(float resetTime)
    {
        isSelected = false;

        yield return new WaitForSeconds(resetTime);

        selectedSprite.transform.DOScale(0, 0.2f).SetEase(Ease.InSine).OnComplete(() =>
        {
            selectedSprite.gameObject.SetActive(false);
        });
    }

    public void SelectCell()
    {
        if(!isSelected)
        {
            selectedSprite.gameObject.SetActive(true);

            selectedSprite.transform.localScale = Vector3.zero;

            selectedSprite.transform.DOScale(selectedScale, 0.2f).SetEase(Ease.OutSine);

            isSelected = true;
        }
    }

    public void SetPosition(int x,int y)
    {
        posX = x;

        posY = y;
    }

    public Vector2 GetPosition()
    {
        return new Vector2(posX, posY);
    }
}
