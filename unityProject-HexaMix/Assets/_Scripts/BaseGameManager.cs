using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseGameManager : MonoBehaviour {
    [SerializeField] private GameObject newColour;
    [SerializeField] private List<GameObject> colourPrefabs;
    [SerializeField] private List<GameObject> coloursList;
    [SerializeField] private List<int> lastColourIndex;
    [SerializeField] private Vector2 clickPosition;
    [SerializeField] private Vector2 worldPosition;
    [SerializeField] private Transform fieldArea;
    [SerializeField] private Vector3 fieldRotation;
    [SerializeField] private float shotAngle;
    [SerializeField] private float divisionAngle;
    [SerializeField] private float distance = 2.0f;
    void Start() {
        this.SpawnItem();
    }
    void Update() {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
            this.clickPosition = Input.mousePosition;
            this.worldPosition = (Vector2)Camera.main.ScreenToWorldPoint(clickPosition) - (Vector2)this.fieldArea.position;
            this.fieldRotation = this.fieldArea.rotation.eulerAngles * -1;
            float fieldZ = this.fieldRotation.z;
            if (fieldZ < 0) fieldZ += 360f;
            this.shotAngle = (Mathf.Atan2(this.worldPosition.y, this.worldPosition.x) * Mathf.Rad2Deg);
            if (this.shotAngle < 0) this.shotAngle += 360f;
            this.shotAngle = (this.shotAngle + fieldZ) % 360;

            if (this.coloursList.Count > 0) {
                this.divisionAngle = 360.0f / this.coloursList.Count;
                for(int i = 0; i < this.coloursList.Count; i++) {
                    if (this.shotAngle >= this.divisionAngle * i && this.shotAngle < this.divisionAngle * (i + 1)) {
                        if (i < this.coloursList.Count) {
                            this.coloursList.Insert(i + 1, this.newColour);
                            this.fieldArea.Rotate(Vector3.forward / (this.divisionAngle * 2));
                            this.lastColourIndex.Insert(0, i + 1);
                        } else {
                            this.coloursList.Add(this.newColour);
                            this.lastColourIndex.Insert(0, i);
                        }
                        break;
                    }
                }
            } else {
                this.divisionAngle = 360.0f;
                this.lastColourIndex.Insert(0, 0);
                this.coloursList.Add(this.newColour);
            }
            this.UpdatePositions();

            this.SpawnItem();
        }
        this.RotateField();
    }

    private void UpdatePositions() {
        for (int i = 0; i < this.coloursList.Count; ++i) {
            this.divisionAngle = 360.0f / this.coloursList.Count;
            float positionX = this.distance * Mathf.Cos(this.divisionAngle * i * Mathf.Deg2Rad);
            float positionY = this.distance * Mathf.Sin(this.divisionAngle * i * Mathf.Deg2Rad);
            this.coloursList[i].transform.localPosition = new Vector2(positionX, positionY);
        }
    }

    private void RotateField() {
        this.fieldArea.Rotate(Vector3.forward / (10 / 1 + this.coloursList.Count));
    }

    private void SpawnItem() {
        int randomIndex = Random.Range(0, this.colourPrefabs.Count);
        this.newColour = Instantiate(this.colourPrefabs[randomIndex], this.fieldArea.position, Quaternion.identity, this.fieldArea);
    }

    private void OnDrawGizmos() {
        if(this.worldPosition != null) {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(Vector2.down, worldPosition + (Vector2)this.fieldArea.position);
            for(int i = 0;  i < this.coloursList.Count; i++) {
                Gizmos.DrawLine(Vector2.down, this.coloursList[i].transform.position);
            }
        }
    }

    public void UndoPlacement() {
        GameObject previous = this.coloursList[this.lastColourIndex[0]];
        Destroy(this.newColour);
        Destroy(this.coloursList[this.lastColourIndex[0]]);
        this.coloursList.RemoveAt(this.lastColourIndex[0]);
        this.lastColourIndex.RemoveAt(0);
        this.newColour = previous;
        this.UpdatePositions();
    }
}
