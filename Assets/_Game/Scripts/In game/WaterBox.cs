using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaterBox : Singleton<WaterBox>
{
    [SerializeField] private LayerMask discMask, terrainMask;
    private BoxCollider box;

    private Vector3 boxCenter, boxExtents;

    protected override void Awake() {
        base.Awake();
        this.box = this.GetComponent<BoxCollider>();
        this.boxCenter = VectorUtils.Multiply(box.center, box.transform.lossyScale) + box.transform.position;
        this.boxExtents = VectorUtils.Multiply(box.size, box.transform.lossyScale) / 2;
    }

    public bool IsInsideWater(Collider point){
        var colliders = Physics.OverlapBox(this.boxCenter, this.boxExtents, this.box.transform.rotation, discMask);
        foreach(var col in colliders){
            if(col == point) return true;
        }
        return false;
    }

    public Vector3 GetClosestTerrainPoint(Vector3 point){
        var surfPoint = point.Set(y: transform.position.y);
        if(Physics.Raycast(point, Vector3.down, out var hitInfo, 2, this.terrainMask)){
            var tangent = Vector3.Cross(Vector3.up, hitInfo.normal);
            var checkDir = Vector3.Cross(Vector3.up, tangent);
            if(Physics.Raycast(surfPoint, checkDir, out hitInfo, 100, terrainMask)){
                return hitInfo.point;
            }
        }

        return Vector3.zero;
    }
}
