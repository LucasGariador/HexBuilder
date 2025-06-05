using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerOnWorldMap : MonoBehaviour
{
    public HexTile currentTile;
    [SerializeField] private int fuel;

    public int GetCurrentFuel(){ return fuel; }

    public float moveSpeed = 2f; // velocidad de movimiento

    private bool isMoving = false;

    public void StartPathMovement(List<HexTile> path)
    {
        if (!isMoving && path != null && path.Count > 0)
            StartCoroutine(MoveAlongPath(path));
    }

    private IEnumerator MoveAlongPath(List<HexTile> path)
    {
        isMoving = true;
        Camera.main.GetComponent<CameraController>()?.StopCameraController();
        for (int i = 0; i < path.Count; i++)
        {
            HexTile tile = path[i];
            Vector3 startPos = transform.position;
            Vector3 endPos = tile.transform.position + new Vector3(0f, 0.3f, 0f);

            // Rotación hacia el próximo punto
            Vector3 direction = (endPos - startPos).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Determinar la dirección del giro para el "banking"
            float angleDiff = Quaternion.Angle(transform.rotation, targetRotation);
            Vector3 cross = Vector3.Cross(transform.forward, direction);
            float bankAmount = Mathf.Clamp(cross.y, -1f, 1f) * 20f; // inclinación hasta 20 grados

            // Rotar suavemente con inclinación (banking)
            Quaternion initialRotation = transform.rotation;
            Quaternion bankRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 0, -bankAmount);

            float rotateTime = 0f;
            while (rotateTime < 1f)
            {
                rotateTime += Time.deltaTime * moveSpeed;
                transform.rotation = Quaternion.Slerp(initialRotation, bankRotation, rotateTime);
                yield return null;
            }

            // Suavemente quitar el banking y volver a rotación normal
            Quaternion cleanRotation = Quaternion.LookRotation(direction);
            float straightenTime = 0f;
            while (straightenTime < 1f)
            {
                straightenTime += Time.deltaTime * moveSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, cleanRotation, straightenTime);
                yield return null;
            }


            // Mover hacia el tile
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * moveSpeed;
                transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }

            currentTile = tile;
        }
        Camera.main.GetComponent<CameraController>()?.StartCameraController();
        isMoving = false;
        CheckFotEventTile(path.Last());
    }

    private void CheckFotEventTile(HexTile lastTile)
    {
        if(lastTile.eventSO)
        {
            Debug.Log("Triggering event for tile: " + lastTile.eventSO.eventName);
            currentTile = lastTile;
            EventManager.Instance.Trigger(lastTile.eventSO);
        }
    }
}
