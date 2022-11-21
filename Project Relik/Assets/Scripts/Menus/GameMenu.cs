using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private Button connectWalletBtn = null;

    [SerializeField]
    private Button playBtn = null;

    [SerializeField]
    private TextMeshProUGUI walletIdText = null;

    [SerializeField]
    private TextMeshProUGUI walletStatus = null;

    private Task walletIdServerTask = null;
    private string walletId = null;

    // Start is called before the first frame update
    void Start()
    {
        walletId = PlayerPrefs.GetString("Wallet Id", "");

        if (walletId == "")
        {
            playBtn.gameObject.SetActive(false);
            walletStatus.gameObject.SetActive(false);
        }
        else
        {
            EnablePlayMode();
        }

        connectWalletBtn.onClick.AddListener(ConnectWallet);
        playBtn.onClick.AddListener(PlayGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void ConnectWallet()
    {
        walletId = PlayerPrefs.GetString("Wallet Id", "");

        if (walletId == "")
        {
            Application.OpenURL(/*"https://localhost:3001/index.html"*/ "https://sparkling-griffin-09b3bd.netlify.app/wallet-login");

            walletIdServerTask = Task.Run(WalletServer);
            await walletIdServerTask;

            EnablePlayMode();
        }
        else
        {
            PlayerPrefs.SetString("Wallet Id", "");
            playBtn.gameObject.SetActive(false);
            walletStatus.gameObject.SetActive(false);
            walletIdText.text = "Wallet Id: ";
            connectWalletBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Connect Wallet";
        }
    }

    private void EnablePlayMode()
    {
        if (walletId != null)
        {
            playBtn.gameObject.SetActive(true);
            walletStatus.gameObject.SetActive(true);
            connectWalletBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Disconnect Wallet";
            walletIdText.text = "Wallet Id: " + walletId;
            PlayerPrefs.SetString("Wallet Id", walletId);
        }
    }

    private void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    private void WalletServer()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:2050/");

        listener.Start();

        Debug.Log("Listening on port 1234...");

        HttpListenerContext ctx = listener.GetContext();
        HttpListenerResponse resp = ctx.Response;
        walletId = ctx.Request.QueryString["walletid"];
        Debug.Log(walletId);

        resp.StatusCode = (int)HttpStatusCode.OK;
        resp.StatusDescription = "Status OK";

        resp.Headers.Set("Content-Type", "text/plain");

        string data = "wallet id received!";
        byte[] buffer = Encoding.UTF8.GetBytes(data);
        resp.ContentLength64 = buffer.Length;

        Stream ros = resp.OutputStream;
        ros.Write(buffer, 0, buffer.Length);

        ros.Close();
        resp.Close();
        listener.Close();
    }
}
