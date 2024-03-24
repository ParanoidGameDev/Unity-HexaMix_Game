using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public class BaseGameManager : MonoBehaviour {
    [SerializeField] private GameObject newColor;
    [SerializeField] private GameObject colorPrefab;
    [SerializeField] private List<Color> tier1Colors;
    [SerializeField] private List<GameObject> colorPool;
    [SerializeField] private List<GameObject> colorsList;
    [SerializeField] private List<int> lastColorIndex;
    [SerializeField] private Vector2 clickPosition;
    [SerializeField] private Vector2 worldPosition;
    [SerializeField] private Transform fieldArea;
    [SerializeField] private Vector3 fieldRotation;
    [SerializeField] private float shotAngle;
    [SerializeField] private float divisionAngle;
    [SerializeField] private float distance = 2.0f;
    void Start() {
        SpawnItem();
    }
    void Update() {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
            clickPosition = Input.mousePosition;
            worldPosition = (Vector2)Camera.main.ScreenToWorldPoint(clickPosition) - (Vector2)fieldArea.position;
            fieldRotation = fieldArea.rotation.eulerAngles * -1;
            float fieldZ = fieldRotation.z;
            if (fieldZ < 0) fieldZ += 360f;
            shotAngle = (Mathf.Atan2(worldPosition.y, worldPosition.x) * Mathf.Rad2Deg);
            if (shotAngle < 0) shotAngle += 360f;
            shotAngle = (shotAngle + fieldZ) % 360;

            newColor.transform.parent = fieldArea.transform;
            if (colorsList.Count > 0) {
                divisionAngle = 360.0f / colorsList.Count;
                for(int i = 0; i < colorsList.Count; i++) {
                    if (shotAngle >= divisionAngle * i && shotAngle < divisionAngle * (i + 1)) {
                        if (i < colorsList.Count) {
                            fieldArea.Rotate(Vector3.back * (divisionAngle / 4));
                            colorsList.Insert(i + 1, newColor);
                            lastColorIndex.Insert(0, i + 1);
                        } else {
                            colorsList.Add(newColor);
                            lastColorIndex.Insert(0, i);
                        }
                        break;
                    }
                }
            } else {
                fieldArea.Rotate(Vector3.forward * shotAngle);
                divisionAngle = 360.0f;
                lastColorIndex.Insert(0, 0);
                colorsList.Add(newColor);
            }

            UpdatePositions();

            SpawnItem();
        }
        if(colorsList.Count != 0) {
            RotateField();
        }
    }

    private void UpdatePositions() {
        for (int i = 0; i < colorsList.Count; ++i) {
            divisionAngle = 360.0f / colorsList.Count;
            float positionX = distance * Mathf.Cos(divisionAngle * i * Mathf.Deg2Rad);
            float positionY = distance * Mathf.Sin(divisionAngle * i * Mathf.Deg2Rad);
            colorsList[i].transform.localPosition = new Vector2(positionX, positionY);
        }
    }

    private void RotateField() {
        fieldArea.Rotate(Vector3.forward / (.6f / 1 + colorsList.Count));
    }

    private void SpawnItem() {
        if(colorPool.Count == 0) {
            GeneratePoolColor();
        }
        newColor = Instantiate(colorPool[0], fieldArea.position, Quaternion.identity);
        if(!newColor.activeSelf) newColor.SetActive(true);
        colorPool.RemoveAt(0);
    }

    private void GeneratePoolColor() {
        colorPool.Add(colorPrefab);
        Color newColor = tier1Colors[Random.Range(0, tier1Colors.Count)];
        colorPool[0].GetComponent<SpriteRenderer>().color = newColor;
        colorPool[0].GetComponentInChildren<Light2D>().color = newColor;
    }

    private void OnDrawGizmos() {
        if(worldPosition != null) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(Vector2.down, worldPosition + (Vector2)fieldArea.position);
            for(int i = 0;  i < colorsList.Count; i++) {
                Gizmos.DrawLine(Vector2.down, colorsList[i].transform.position);
            }
        }
    }

    public void UndoPlacement() {
        if(colorsList.Count > 0) {
            newColor.SetActive(false);
            colorPool.Insert(0, newColor);
            newColor = colorsList[lastColorIndex[0]];
            newColor.transform.position = fieldArea.position;
            colorsList.RemoveAt(lastColorIndex[0]);
            lastColorIndex.RemoveAt(0);
            UpdatePositions();
        }
    }
}
