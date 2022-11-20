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

public class GameMenu : MonoBehaviour
{
    [SerializeField]
    private Button connectWalletBtn = null;

    [SerializeField]
    private Button playBtn = null;

    [SerializeField]
    private TextMeshProUGUI walletIdText = null;

    private Task walletIdServerTask = null;
    private string walletId = null;

    // Start is called before the first frame update
    void Start()
    {
        connectWalletBtn.onClick.AddListener(ConnectWallet);

        playBtn.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void ConnectWallet()
    {
        Application.OpenURL("https://localhost:3001/index.html");

        walletIdServerTask = Task.Run(WalletServer);
        await walletIdServerTask;

        EnablePlayMode();
    }

    private void EnablePlayMode()
    {
        if (walletId != null)
        {
            playBtn.gameObject.SetActive(true);
            connectWalletBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Connect New Wallet";
            walletIdText.text = "Wallet Id: " + walletId;
        }
    }

    private void WalletServer()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:1234/wallet-login/");

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
