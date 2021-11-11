using DG.Tweening;
using UnityEngine;

public class Afterimage : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, Vector3 scale, Vector3 position)
    {
        transform.position = position;
        transform.localScale = scale;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sprite = sprite;
        DOTween.ToAlpha(() => spriteRenderer.color, x => spriteRenderer.color = x, 0, 0.3f).OnComplete(() => { gameObject.SetActive(false); });

    }
}
