using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.Networking.Models
{
    public static class NetworkConsts 
    {

        public const string VAR_BATTLE_TYPE = "battleType";
        public const string VAR_PLAYER_VEHICLE = "playerVehicle";
        public const string VAR_CURRENT_COUNTDOWN = "currentCountdownValue";
        public const string VAR_MINUTES_LEFT = "minutesLeft";
        public const string VAR_SECONDS_LEFT = "secondsLeft";
        public const string VAR_CURRENT_GAME_STAGE = "currentGameStage";

        public const string REQ_ADMIN_JOIN_ROOM = "adminJoinRoom";

        public const string RPC_PLAYER_INPUTS = "inbattle.playerInputs";
        public const string RPC_GAME_START_COUNTDOWN = "inbattle.gameStartCountdown";
        public const string RPC_PLAYER_SPAWNED = "inbattle.playerSpawned";
        public const string RPC_PLAYER_SHOT = "inbattle.playerShot";
        public const string RPC_BATTLE_TIMER = "inbattle.battleTimer";
        public const string RPC_SEND_GAME_STATE = "inbattle.sendGameStage";
    }
}
