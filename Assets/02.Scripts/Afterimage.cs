using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Afterimage : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, bool flip, Vector3 position)
    {
        transform.position = position;
        spriteRenderer.flipX = flip;
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        spriteRenderer.sprite = sprite;
        DOTween.ToAlpha(() => spriteRenderer.color, x => spriteRenderer.color = x, 0, 0.3f).OnComplete(() => { gameObject.SetActive(false); });

    }
}
