using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // 태그 이름
    public static class Tags
    {
        public const string Player = "Player";
        public const string Enemy = "Enemy";
        // 여기에 다른 태그들을 추가
    }

    // 레이어 이름
    public static class Layers
    {
        public const int Default = 0;
        public const int TransparentFX = 1;
        public const int IgnoreRaycast = 2;
        public const int Water = 4;
        public const int UI = 5;
        // 여기에 다른 레이어들을 추가
    }

    // 씬 이름
    public static class Scenes
    {
        public const string MainScene = "MainScene";
        public const string GameScene = "GameScene";
        public const string TutorialScene = "TutorialScene";
        public const string StageChoiceScene = "StageChoiceScene";
        public const string UnitDictionary = "UnitDictionary";
        public const string GameTestScene = "TestScene";
        // 여기에 다른 씬 이름들을 추가
    }

    public static class EnemyStatRate
    {
        public const int HPRate = 100;
        public const int ADDefRate = 5;
        public const int APDefRate = 5;
    }

    // 기타 상수
    public static class GameSettings
    {
        // 게임 처음 할 경우 세팅 값
        // 사용 가능한 유닛 ID 목록
        public static readonly List<int> InitialUsableUnitIDs = new List<int>()
        {
            0, 1, 2, 3, 4
        };
        // 스테이지 세팅 정보
        public static readonly StageData[] InitialStageDatas = new StageData[3]
        {
            new StageData { Clear = false, Stars = 0 },
            new StageData { Clear = false, Stars = 0 },
            new StageData { Clear = false, Stars = 0 }
        };
        public const int InitialKeyCount = 0;

        // 여기에 다른 게임 설정 상수들을 추가
    }
}
