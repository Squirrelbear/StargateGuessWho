using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMessage
{
    public abstract class MessageTemplate
    {
        public abstract string GetMessage();
    }

    public class CreatePlayerMessage : MessageTemplate
    {
        public CreatePlayerMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName {get; set;}

        public override string GetMessage()
        {
            return "action=createPlayer&playerName=" + PlayerName;
        }
    }

    public class CreateServerMessage : MessageTemplate
    {
        public CreateServerMessage(string playerAuth)
        {
            PlayerAuth = playerAuth;
        }

        public string PlayerAuth { get; set; }

        public override string GetMessage()
        {
            return "action=createServer&playerAuth=" + PlayerAuth;
        }
    }

    public class JoinServerMessage : MessageTemplate
    {
        public JoinServerMessage(string playerAuth, string sessionCode)
        {
            PlayerAuth = playerAuth;
            SessionCode = sessionCode;
        }

        public string PlayerAuth { get; set; }
        public string SessionCode { get; set; }

        public override string GetMessage()
        {
            return "action=joinServer&playerAuth=" + PlayerAuth
                + "&sessionCode=" + SessionCode;
        }
    }

    public class UpdateSelectionMessage : MessageTemplate
    {
        public UpdateSelectionMessage(string playerAuth, string sessionCode, int characterID, string characterAction)
        {
            PlayerAuth = playerAuth;
            SessionCode = sessionCode;
            CharacterID = characterID;
            CharacterAction = characterAction;
        }

        public string PlayerAuth { get; set; }
        public string SessionCode { get; set; }
        public int CharacterID { get; set; }
        public string CharacterAction { get; set; }

        public override string GetMessage()
        {
            return "action=characterCommand&playerAuth=" + PlayerAuth
                + "&sessionCode=" + SessionCode + "&characterID=" + CharacterID
                + "&characterAction=" + CharacterAction;
        }
    }

    public class StartRoundMessage : MessageTemplate
    {
        public StartRoundMessage(string playerAuth, string sessionCode)
        {
            PlayerAuth = playerAuth;
            SessionCode = sessionCode;
        }

        public string PlayerAuth { get; set; }
        public string SessionCode { get; set; }

        public override string GetMessage()
        {
            return "action=startGame&playerAuth=" + PlayerAuth
                + "&sessionCode=" + SessionCode;
        }
    }

    public class GetStateMessage : MessageTemplate
    {
        public GetStateMessage(string playerAuth, string sessionCode)
        {
            PlayerAuth = playerAuth;
            SessionCode = sessionCode;
        }

        public string PlayerAuth { get; set; }
        public string SessionCode { get; set; }

        public override string GetMessage()
        {
            return "action=getState&playerAuth=" + PlayerAuth
                + "&sessionCode=" + SessionCode;
        }
    }

    public class SetCharacterCollectionMessage : MessageTemplate
    {
        public SetCharacterCollectionMessage(string playerAuth, string sessionCode, string characterSet)
        {
            PlayerAuth = playerAuth;
            SessionCode = sessionCode;
            CharacterSet = characterSet;
        }

        public string PlayerAuth { get; set; }
        public string SessionCode { get; set; }
        public string CharacterSet { get; set; }

        public override string GetMessage()
        {
            return "action=setCharacterCollection&playerAuth=" + PlayerAuth
                + "&sessionCode=" + SessionCode + "&characterSet=" + CharacterSet;
        }
    }
}
