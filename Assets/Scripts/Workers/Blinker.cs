﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    private TextMeshPro _renderer;
    private void Awake()
    {
        _renderer = GetComponent<TextMeshPro>();
    }
    private void OnEnable()
    {
        StartCoroutine(Blink());
    }
    IEnumerator Blink()
    {
        while (true)
        {
            _renderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
            _renderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
