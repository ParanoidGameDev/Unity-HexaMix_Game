using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorElementCore : MonoBehaviour {
    [SerializeField] private bool upgraded;
    [SerializeField] private int type;
    [SerializeField] private int powerupInt;
    [SerializeField] private SpriteRenderer color;
    [SerializeField] private SpriteRenderer powerup;
    [SerializeField] private Light2D emission;
    [SerializeField] private List<Color> tier1Colors;
    [SerializeField] private List<Sprite> powerupSprites;

    private void Awake() {
        powerupInt = Random.Range(0, powerupSprites.Count);
        Sprite newPowerupSprite = powerupSprites[powerupInt];
        powerup.sprite = newPowerupSprite;

        type = Random.Range(0, tier1Colors.Count);
        UpdateColor();
        //GetComponent<SpriteRenderer>().sprite = hexagon;
    }

    private void UpdateColor() {
        Color newColor = tier1Colors[type];
        color.color = newColor;
        emission.color = newColor;
    }

    public void Nivelation(int newType) {
        if(!upgraded) {
            upgraded = true;
            type = newType;
            powerup.gameObject.SetActive(false);
            UpdateColor();
        }
    }

    private void Update() {
    }
    
    public int GetColorIndex() {
        return type;
    }
    public int GetPowerupIndex(){
        return powerupInt;
    }
}
