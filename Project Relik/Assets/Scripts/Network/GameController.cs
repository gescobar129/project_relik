using System.Net.Http;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI goldText = null;

    private WalletData walletData;
    // Start is called before the first frame update
    void Start()
    {
        //Application.OpenURL("https://sparkling-griffin-09b3bd.netlify.app/wallet-login");
        UpdatePlayerWalletStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async void UpdatePlayerWalletStats()
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try
        {
            string url = "https://us-central1-dao-v-player.cloudfunctions.net/getGameAccount?account_id=pocket.testnet";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string rawData = await response.Content.ReadAsStringAsync();
                    walletData = JsonUtility.FromJson<WalletData>(rawData);

                    Debug.Log(walletData.accountId);
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
    }

    public void UpdateUI()
    {
        goldText.text = walletData.nearBalance.goldBalance.ToString();
    }

    [System.Serializable]
    public struct WalletData
    {
        public string accountId;
        public NearBalance nearBalance;
    }

    [System.Serializable]
    public struct NearBalance
    {
        public ulong total;
        public ulong stateStaked;
        public int staked;
        public ulong available;
        public ulong goldBalance;
    }
}
