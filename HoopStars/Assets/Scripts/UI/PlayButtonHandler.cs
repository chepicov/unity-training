﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButtonHandler : MonoBehaviour
{
    [SerializeField] Text text;

    Image img;
    bool isClicked;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        isClicked = false;
    }

    public void OnClick()
    {
        if (isClicked) {
            return;
        }
        GameManager.Instance.LoadLevel();
        isClicked = true;

        float animationTime = 1f;

        StartCoroutine(FadeOut(animationTime));
    }

    IEnumerator FadeOut(float delayTime)
    {
        for (float i = delayTime; i >= 0; i -= Time.deltaTime)
        {
            Color tempColor = img.color;
            tempColor.a = i;
            img.color = tempColor;

            tempColor = text.color;
            tempColor.a = i;
            text.color = tempColor;

            yield return null;
        }
        Destroy(gameObject);
    }
}
