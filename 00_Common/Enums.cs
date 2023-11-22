using System.ComponentModel;

public enum SceneName
{
    R_Start,
    R_LoadingScene,
    R_Main_V6_JHH,
    R_Main_V6,
    R_Main_V6_SEH
}

public enum Rarity
{
    trash,
    old,
    normal,
    rare,
    unique,
    legendary
}

public enum ReinforceType
{
    Default,
    promote,
    additional,
    normalReinforce,
    magicEngrave,
    soulCrafting,
    refineMent
}

public enum SortType
{
    기본,
    등급순,
    전투력순,
    공격력순,
    공격속도순,
    공격범위순,
    정확도순
}

public enum MessageType
{
    Normal, Notice, Guide
}

public enum Collection
{
    A,B,C,D,E,F,G
}

public enum MagicType
{
    술,묘,유,신,인,해,오,자,진,축,미,사
}

public enum ChartName
{
    gachaPercentage,
    mineData,
    weapon,
    additional,
    normalReinforce,
    magicCarve,
    soulCrafting,
    refinement,
    attendance,
    exp,
    quest,
    skillData,
    decomposit,
    collection,
    minigameRewardPercent
}

public enum StatType
{
    upgradeCount, atk, atkSpeed, atkRange, accuracy, criticalRate, criticalDamage, strength, intelligence, wisdom, technique, charm, constitution
}

public enum StatTypeKor
{
    업그레이드횟수,
    공격력,
    공격속도,
    공격범위,
    정확도,
    치명타율,
    치명타피해,
    근력,
    지능,
    지혜,
    기교,
    매력,
    체질
}

public enum InventoryType
{
    Default,
    Mine,
    Reinforce,
    ReinforceMaterial,
    MiniGame,
    Decomposition
    // SelectMyFavoriteWeapon
}

public enum RewardType
{
    Exp,
    Gold,
    Diamond,
    Soul,
    Ore
}

public enum RecordType
{
    Tutorial,
    LevelUp,
    UseGold,
    GetGold,
    UseDiamond,
    GetDiamond,
    GetItem,
    RegisterItem,
    DisassembleItem,
    ProduceWeapon,
    AdvanceProduceWeapon,
    TryPromote,
    TryAdditional,
    TryReinforce,
    TryMagic,
    TrySoul,
    TryRefine,
    DayAttendance,
    DayTryPromote,
    DayTryReinforce,
    DayTryMagic,
    DayGetBonus,
    DaySeeAds,
    WeekAttendance,
    WeekTryPromote,
    WeekTryReinforce,
    WeekTryMagic,
    WeekGetBonus,
    WeekSeeAds
}

public enum QuestType
{
    Once,
    Day,
    Week
}

public enum TapType
{
    Main_Mine,
    Reinforce,
    MiniGame,
    Pidea,
    Ranking
}

public enum MineStatus
{
    Locked,
    Building,
    Owned
}

public enum RankingType
{
    분당골드량,
    전투력,
    미니게임
}

public enum SfxType
{
    Click,
    Button,
    Win,
    PopupOpen,
    PopupClose
}

public enum BgmType
{
    MainBgm,
    ReinforceBgm,
    MiniGameBgm
}

public enum AdType
{
    Attendance,
    RefreshRank,
    FreeDiamond,
    FreeGold,
    CollectBonus
}

public enum QuestContents
{
    dayContents,
    weekContents,
    onceIngContents,
    onceClearContents
}