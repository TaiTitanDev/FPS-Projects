using UnityEngine;

public class PlayerDataService : MonoSingleton<PlayerDataService>
{

    private bool isInit = false;
    private PlayerModel playerModel;
    // Start is called before the first frame update

    private void Awake()
    {
        playerModel = new PlayerModel();
    }

    public PlayerModel GetPlayerModel()
    {
        if (playerModel == null)
            playerModel = new PlayerModel();
        return playerModel;
    }

    public void SetPlayerModel(PlayerModel value)
    {
        if(playerModel == null)
            playerModel = new PlayerModel();
        playerModel = value;
    }
}
