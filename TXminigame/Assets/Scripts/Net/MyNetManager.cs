using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class MyNetManager: NetworkManager {

    public int chosenCharacter = 0;

    public class NetworkMessage : MessageBase
    {
        public int chosenClass;
        
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedClass = message.chosenClass;
        GameObject player = Instantiate(spawnPrefabs[selectedClass]) as GameObject;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        NetworkMessage test = new NetworkMessage();

        chosenCharacter = NetChoose.chosenID;
        test.chosenClass = chosenCharacter;
     //   Debug.Log(chosenCharacter);

        ClientScene.AddPlayer(client.connection, 0, test);

    }
}
