using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;



[Serializable]
public class CharacterProperties
{
    public string name;
    public List<Sprite> headSprites = new List<Sprite>();
    public List<Sprite> armSprites = new List<Sprite>();
    public List<Sprite> faceSprites = new List<Sprite>();
    public List<Sprite> mouthSprites = new List<Sprite>();
}

public class ProfileCharacterMaker : MonoBehaviour
{
    [Header("Sprite Lists")]
    [SerializeField] private List<CharacterProperties> characterProperties = new List<CharacterProperties>();

    [Header("Images")]
    [SerializeField] private Image headImage;
    [SerializeField] private Image armImage1, armImage2;
    [SerializeField] private Image faceImage;
    [SerializeField] private Image mouthImage;

    private void Start()
    {
        GetRandomCharacter();
    }

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnNextLevel, OnNextLevel);
    }

    private void OnNextLevel()
    {
        GetRandomCharacter();
    }

    private void GetRandomCharacter()
    {
        // Select a random character properties object
        int randomCharacterIndex = Random.Range(0, characterProperties.Count);
        CharacterProperties randomCharacter = characterProperties[randomCharacterIndex];

        // Randomize and assign sprites for head, arms, and face
        SetRandomCharacter(randomCharacter.headSprites, headImage);
        SetArms(randomCharacter.armSprites, armImage1, armImage2);
        SetRandomCharacter(randomCharacter.faceSprites, faceImage);
        SetRandomCharacter(randomCharacter.mouthSprites, mouthImage);
    }

    private void SetRandomCharacter(List<Sprite> sprites, Image image)
    {
        if (sprites.Count > 0)
        {
            int randomIndex = Random.Range(0, sprites.Count);
            image.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("Sprite list is empty for " + image.name);
        }
    }

    private void SetArms(List<Sprite> sprites, Image image1, Image image2)
    {
        if (sprites.Count > 0)
        {
            int randomIndex = Random.Range(0, sprites.Count);
            image1.sprite = sprites[randomIndex];
            image2.sprite = sprites[randomIndex];
        }
        else
        {
            Debug.LogWarning("Arm sprite list is empty.");
        }
    }
}