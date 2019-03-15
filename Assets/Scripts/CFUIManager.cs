using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFUIManager : MonoBehaviour
{
    private static CFUIManager _instance;
    public static CFUIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Text[] statusTexts;

    private void Start()
    {
        _instance = this;
    }

    private void Update()
    {
        for (int playerIdx = 0; playerIdx < 2; ++playerIdx)
        {
            statusTexts[playerIdx].text = GetPlayerStatus(playerIdx);
        }
    }

    private string GetPlayerStatus(int playerIdx)
    {
        CFPlayer player = CFFusionManager.Instance.players[playerIdx];

        if (player.IsWaitingForFuse())
        {
            return "Wants to fuse!";
        }
        if (player.FuseCD() == 0.0f)
        {
            return "Ready!";
        }
        return string.Format("{0:0.0}", player.FuseCD());
    }
}
