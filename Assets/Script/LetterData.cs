using UnityEngine;

[CreateAssetMenu(fileName = "NewLetter", menuName = "Letter Data")]
public class LetterData : ScriptableObject
{
    public string letterID;
    [TextArea(3, 5)]
    public string clueText;
    public string targetHouseID; // Samakan dengan ID Rumah tujuan
    public Sprite letterSprite;
}