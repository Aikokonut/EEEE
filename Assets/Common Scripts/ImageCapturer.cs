using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCapturer : MonoBehaviour
{
    [SerializeField] private Camera captureCamera;
    [SerializeField] private Vector2Int imageResolution;
    

    public float CameraSize => this.captureCamera.orthographicSize;
    public Vector3 CameraPosition => this.captureCamera.transform.position;
    
    public Texture2D Capture(){
        int width = imageResolution.x;
        int height = imageResolution.y;
        RenderTexture renderTexture = new RenderTexture(width, height, 24);

        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        var lastActiveRT = RenderTexture.active;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        captureCamera.targetTexture = null;
        RenderTexture.active = lastActiveRT;
#if UNITY_EDITOR
        DestroyImmediate(renderTexture);
#else
        Destroy(renderTexture);
#endif
        
        return texture;
    }

    public void SetCameraSize(float size){
        this.captureCamera.orthographicSize = size;
    }

#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    private void CaptureAndSave(UnityEditor.DefaultAsset folder, string filename = "test.png"){
        var tex = this.Capture();
        var data = tex.EncodeToPNG();

        System.IO.File.WriteAllBytes(UnityEditor.AssetDatabase.GetAssetPath(folder) + "\\" + filename, data);

        DestroyImmediate(tex);
    }
#endif

}
