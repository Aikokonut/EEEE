using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SessionBound : MonoBehaviour
{
    [SerializeField] private BoxCollider box;
    [SerializeField] private Texture2D mapTex;
    private UIMiniMap uiMiniMap => UIManager.Instance.UIMiniMap;


    private Vector3 center, size;

#if UNITY_EDITOR
    [SerializeField] private ImageCapturer imageCapturer;
    [Sirenix.OdinInspector.Button]
    private void CaptureSessionTexture(UnityEditor.DefaultAsset folder, string filename = "test.png"){
        //imageCapturer.SetCameraSize(this.size.x / 2);
        var tex = imageCapturer.Capture();
        var data = tex.EncodeToPNG();

        var savePath = UnityEditor.AssetDatabase.GetAssetPath(folder) + "\\" + filename;
        System.IO.File.WriteAllBytes(savePath, data);
        DestroyImmediate(tex);

        this.mapTex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
        var imageCamSize = imageCapturer.CameraSize * 2;
        var boxSize = new Vector3(imageCamSize, 50, imageCamSize);
        transform.position = this.imageCapturer.CameraPosition.Set(y: 0);
        box.size = boxSize;
        box.center = Vector3.zero;
        EditorUtility.SetDirty(mapTex);
    }
#endif

    private void Awake(){
        center = transform.position + VectorUtils.Multiply(box.center, transform.lossyScale);
        size = VectorUtils.Multiply(box.size, transform.lossyScale);
    }

    public void InitializeSessionMap(){
        var discPos = ThrowStateController.Instance.Thrower.Disc.transform.position.XZ();
        var targetPos = LevelManager.Instance.CurrentSessionInfo.throwTarget.transform.position.XZ();

        var discMinimapCoord = ConvertPointToMiniMapCoord(discPos);
        var targetMinimapCoord = ConvertPointToMiniMapCoord(targetPos);

        uiMiniMap.SetTargetMarkerPosition(targetMinimapCoord);
        uiMiniMap.SetDiscMarkerPosition(discMinimapCoord);

        uiMiniMap.SetMapTexture(this.mapTex);
    }

    public void UpdateDiscMarker(){
        var discPos = ThrowStateController.Instance.Thrower.Disc.transform.position.XZ();
        var discMinimapCoord = ConvertPointToMiniMapCoord(discPos);
        uiMiniMap.SetDiscMarkerPosition(discMinimapCoord);
    }

    public Vector2 ConvertPointToMiniMapCoord(Vector2 point){
        var worldCenter = transform.position.XZ();
        var localPoint = point - worldCenter;
        var coordRatio = new Vector2(localPoint.x / size.x, localPoint.y / size.y);
        var minimapCoord = VectorUtils.Multiply(uiMiniMap.Size, coordRatio);
        return minimapCoord;
    }

}
