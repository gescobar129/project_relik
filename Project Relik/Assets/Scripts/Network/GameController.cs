using System.Net.Http;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    private static readonly string BACKEND_URL = "https://us-central1-dao-v-player.cloudfunctions.net/";

    [SerializeField]
    private GameObject Hero = null;

    [SerializeField]
    private TextMeshProUGUI goldText = null;

    [SerializeField]
    private TextMeshProUGUI levelText = null;

    [SerializeField]
    private PauseMenu pauseMenu = null;

    private WalletData walletData;
    private string walletId;
    private string characterToken;
    private CharacterControls controls;

    private Character PlayerCharacter { get; set; }

    private void Awake()
    {
        walletId = PlayerPrefs.GetString("Wallet Id", "");
        controls = new CharacterControls();
        controls.UI.Pause.performed += OpenPauseMenu;
    }
    void Start()
    {
        //Application.OpenURL("https://sparkling-griffin-09b3bd.netlify.app/wallet-login");
        UpdatePlayerWalletStats();
        GlobalEventSystem.GetInstance().AddListener(ObjectKilledGlobalEvent.GetInstance(), ObjectKilled);
        GlobalEventSystem.GetInstance().AddListener(ItemCollectedGlobalEvent.GetInstance(), LootPickedUp);
    }
    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }

    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        pauseMenu.ShowPauseMenu();
    }

    public void ObjectKilled(object sender, ObjectKilledGlobalEvent.EventArgs args)
    {
        if (args.killer == Hero.GetComponent<SwordWielder>().CurrentWeapon.gameObject)
        {
            PostOnKillEnemy();
        }
    }

    public void LootPickedUp(object sender, ItemCollectedGlobalEvent.EventArgs args)
    {
        if (args.collector == Hero)
        {
            if (args.collected.GetComponent<Coin>() != null)
            {
                PostPickUpGold();
            }
        }
    }

    private async void UpdatePlayerWalletStats()
    {
        string url = BACKEND_URL + "getGameAccount" + "?account_id=" + walletId;
        string rawJson = await CallBackend(url);
        walletData = JsonConvert.DeserializeObject<WalletData>(rawJson);
        PlayerCharacter = JsonConvert.DeserializeObject<Character>(walletData.ownedNfts.characters[0].metadata.extra);
        characterToken = walletData.ownedNfts.characters[0].token_id;
        Debug.Log(walletData.accountId);
        UpdateUI();
    }

    private async void PostOnKillEnemy()
    {
        string url = BACKEND_URL + "onKillEnemy" + "?token_id=" + characterToken + "&account_id=" + walletId;
        string rawJson = await CallBackend(url);
         
        walletData.ownedNfts.characters[0] = JsonConvert.DeserializeObject<NftItem>(rawJson);
        PlayerCharacter = JsonConvert.DeserializeObject<Character>(walletData.ownedNfts.characters[0].metadata.extra);
        UpdateUI();
    }

    private async void PostPickUpGold()
    {
        string url = BACKEND_URL + "onPickUpLoot" + "?gold_amount=10" + "&account_id=" + walletId;
        string rawJson = await CallBackend(url);
        var loot = JsonConvert.DeserializeObject<LootPickup>(rawJson);
        walletData.goldBalance = loot.totalBalance;
        UpdateUI();
    }

    private async Task<string> CallBackend(string url)
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string rawJson = await response.Content.ReadAsStringAsync();
                    return rawJson;
                }
                else
                {
                    Debug.Log(response.ReasonPhrase);
                    Debug.Log(await response.Content.ReadAsStringAsync());
                }
            }
        }
        catch (HttpRequestException e)
        {
            Debug.Log("\nException Caught!");
            Debug.LogError(e.Message);
        }

        return "";
    }

    public void UpdateUI()
    {
        goldText.text = walletData.goldBalance.ToString();
        levelText.text = "Lvl: " + PlayerCharacter.stats.lvl.ToString();
    }

    [System.Serializable]
    public struct WalletData
    {
        public string accountId;
        public ulong goldBalance;
        public NearBalance nearBalance;
        public NFTCollection ownedNfts;
    }

    [System.Serializable]
    public struct NearBalance
    {
        public string total;
        public string stateStaked;
        public int staked;
        public string available;
    }

    [System.Serializable]
    public struct NFTCollection
    {
        public List<NftItem> loot;
        public List<NftItem> characters;
    }

    [System.Serializable]
    public struct NftItem
    {
        public string token_id;
        public Metadata metadata;
    }

    [System.Serializable]
    public struct Metadata
    {
        public string title;
        public string description;
        public uint copies;
        public string extra;
    }

    [System.Serializable]
    public struct Character
    {
        public string type;
        public CharacterStats stats;
    }

    [System.Serializable]
    public struct CharacterStats
    {
        public double str;
        public double def;
        public double mag;
        public double luck;
        public double lvl;
        public double exp;
    }

    [System.Serializable]
    public struct LootPickup
    {
        public ulong totalBalance;
    }
}
