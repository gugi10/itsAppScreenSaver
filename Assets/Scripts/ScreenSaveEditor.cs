using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class ScreenSaveEditor : MonoBehaviour {
#if(UNITY_EDITOR)
    ScreenSaverController controller;

    void Start() {
        if (!EditorApplication.isPlaying && !GetComponentInChildren<Canvas>()) {
            controller = GetComponent<ScreenSaverController>();
            AddRawImage();
        }
    }

    void AddRawImage() {
        GameObject newCanvasObj = new GameObject();
        SetupCanvas(newCanvasObj.AddComponent<Canvas>());
        newCanvasObj.transform.SetParent(transform);
        GameObject newRawImage = new GameObject();
        newRawImage.AddComponent<RawImage>();
        newRawImage.transform.SetParent(newCanvasObj.transform);
        controller.rawImage = newRawImage.GetComponent<RawImage>();
        Rename(newCanvasObj, newRawImage);
        SetupCanvas(newCanvasObj.GetComponent<Canvas>());
        SetupRawImageRect(newRawImage.GetComponent<RectTransform>());

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
#endif
}
