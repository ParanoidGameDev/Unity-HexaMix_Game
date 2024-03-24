using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorElementCore : MonoBehaviour {
    [SerializeField] private SpriteRenderer color;
    [SerializeField] private SpriteRenderer powerup;
    [SerializeField] private Light2D emission;
    [SerializeField] private List<Color> tier1Colors;
    [SerializeField] private List<Sprite> powerupSprites;

    private void Awake() {
        Sprite newPowerupSprite = powerupSprites[Random.Range(0, powerupSprites.Count)];
        Color newColor = tier1Colors[Random.Range(0, tier1Colors.Count)];
        color.color = newColor;
        emission.color = newColor;
        powerup.sprite = newPowerupSprite;
        //GetComponent<SpriteRenderer>().sprite = hexagon;
    }

    private void Update() {
    }
}
