//using UnityEngine;

//public class HouseTarget : MonoBehaviour, IInteractable
//{
//    [Header("House Identity")]
//    public string houseID; // Misal: "RUMAH_ANJING", "RUMAH_POT_MATI"
//    [SerializeField] private string promptMessage = "Press E to Deliver Mail";

//    public string GetPrompt()
//    {
//        return promptMessage;
//    }

//    public void Interact()
//    {
//        // Panggil MailbagManager untuk membuka UI Tas saat didekati
//        if (MailbagManager.Instance != null)
//        {
//            MailbagManager.Instance.OpenBag(this);
//        }
//    }

//    public bool CheckLetter(LetterData letter)
//    {
//        return letter.targetHouseID == houseID;
//    }
//}