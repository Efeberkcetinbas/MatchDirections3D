using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEmojiAttribute", menuName = "Game/Attributes/Emoji")]
public class EmojisEnumAttribute : EnumAttribute
{
    public Emojis value;

    public override string DisplayName => $"Emoji: {value}";
}
