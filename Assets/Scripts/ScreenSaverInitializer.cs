using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public class ScreenSaverInitializer : MonoBehaviour {
    ScreenSaverController controller;


    public void SetUp() {
        if (!GetComponentInChildren<Canvas>()) {
            controller = GetComponent<ScreenSaverController>();
            gameObject.name = "ScreenSaver";
            InitRawImage();
        }
    }

    void InitRawImage() {
        GameObject newCanvasObj = new GameObject();
        SetupCanvas(newCanvasObj.AddComponent<Canvas>());
        newCanvasObj.transform.SetParent(transform);
        GameObject newGameObjectRaw = new GameObject();
        RawImage newRawImage = newGameObjectRaw.AddComponent<RawImage>();
        newRawImage.transform.SetParent(newCanvasObj.transform);
        controller.rawImage = newRawImage.GetComponent<RawImage>();
        Rename(newCanvasObj, newRawImage.gameObject);
        SetupCanvas(newCanvasObj.GetComponent<Canvas>());
        SetupRawImageRect(newRawImage.GetComponent<RectTransform>());
        DestroyImmediate(this);

    }

    void SetupRawImageRect(RectTransform rect) {
        rect = controller.rawImage.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition3D = Vector3.zero;
    }

    void SetupCanvas(Canvas canvas) {
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }

    void Rename(GameObject canvasGameObject, GameObject rawImageObject) {
        canvasGameObject.name = "Canvas";
        rawImageObject.name = "RawImage";
    }

}
