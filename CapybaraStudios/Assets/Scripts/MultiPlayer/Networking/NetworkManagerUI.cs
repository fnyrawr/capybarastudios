using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn, clientBtn, hostBtn;
    [SerializeField] TextMeshProUGUI playersInGameText;
    private void Awake() {
        Cursor.visible = true;
        serverBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
        });
        hostBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
    /*private void Update() {
        playersInGameText.text = "Players in Game: " + PlayersManager.Instance.PlayersInGame;
    }*/
}