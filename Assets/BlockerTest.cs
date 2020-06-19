using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Package;
using UnityEngine;
using UnityEngine.UI;

public class BlockerTest : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        void OnClickAction(Button button)
        {
            button.enabled = false;
            var blockerData = Utilities.Blocker.CreateBlockerForRectTransform((RectTransform) button.transform);
            blockerData.Image.color = new Color32(0, 0, 0, 128);
            blockerData.Button.onClick.AddListener(() =>
            {
                Destroy(blockerData.RectTransform.gameObject);
                button.enabled = true;
            });
        }

        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => OnClickAction(button));
        }
    }
}