using UnityEngine;
using UnityEngineInternal;

public class RaycastBasics : MonoBehaviour
{
    [Header("최대 거리")]
    public float maxDistance = 100f;

    [Header("말만 맞추고 싶으면 레이어 지정")]
    public LayerMask targetLayers = ~0;

     void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.yellow, 1f);

        if(Physics.Raycast(ray, out RaycastHit hit, maxDistance, targetLayers))
        {
            Debug.Log($"맞춤! name ={hit.collider.name}, point={hit.point}, dist{hit.distance}");
        }
        else
        {
            Debug.Log("못 맞춤!");
        }
    }
}
