using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
public class MenuDeath : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        StartCoroutine(waitAndSetActiveFalse());
    }
    private IEnumerator waitAndSetActiveFalse()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
        spriteRenderer.enabled = false;
    }

}
