//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SimpleMan.CoroutineExtensions;

//public class ShaderTest : MonoBehaviour
//{
//    Renderer r;
//    MaterialPropertyBlock propBlock;
//    List<Vector4> posArray = new List<Vector4>();
//    List<Vector4> destArray = new List<Vector4>();
//    public float range = 50f;
//    public float speed = 10f;
//    [Range(1, 3000)]
//    public int unitCount = 100;
//    [Range(1, 100)]
//    public int refreshRate = 60;

//    int lastRefreshRate;
//    int lastCount;
//    float tick = 0.01f;
//    Coroutine curLoop;
//    void Awake(){
//        r = GetComponent<Renderer>();
//        propBlock = new MaterialPropertyBlock();

//        Refresh();
//        lastCount = unitCount;
//        lastRefreshRate = refreshRate;
//        tick = 1f / (float)(refreshRate);

//        curLoop = this.RepeatForever(Loop, tick);
//        this.RepeatForever(() => 
//        {
//            if (lastCount != posArray.Count) Refresh();
//            lastCount = posArray.Count;

//            if (lastRefreshRate != refreshRate){
//                if (curLoop != null) StopCoroutine(curLoop);

//                tick = 1f / (float)(refreshRate);
//                curLoop = this.RepeatForever(Loop, tick);
//            }
//            lastRefreshRate = refreshRate;
//        }
//        , 0.25f);
//    }

//    void Refresh(){
//        posArray.Clear();
//        destArray.Clear();

//        for (int i = 0; i < unitCount; i++){
//            posArray.Add(new Vector3(Random.Range(-range, range), Random.Range(-range, range)));
//            destArray.Add(new Vector3(Random.Range(-range, range), Random.Range(-range, range)));
//        }
//        r.sharedMaterial.SetVectorArray("_UnitPos", posArray);
//        r.sharedMaterial.SetInt("_UnitPos_Length", unitCount);
//    }

//    void Loop(){
//        if (posArray.Count > 0){
//            for (int i = 0; i < posArray.Count; i++){
//                if (posArray[i].DistanceXY(destArray[i]) <= 1f){
//                    destArray[i] = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
//                }
//                posArray[i] = posArray[i] + (destArray[i] - posArray[i]).normalized * speed * tick;
//            }
//            r.sharedMaterial.SetVectorArray("_UnitPos", posArray);
//            r.sharedMaterial.SetInt("_UnitPos_Length", unitCount);
//        }
//    }
//}
