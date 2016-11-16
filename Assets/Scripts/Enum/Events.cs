public enum Events
{
    NONE,
    /// <summary>
    /// 敌人死亡
    /// </summary>
    ENEMYDIE,
    /// <summary>
    /// 时间到
    /// </summary>
    TIMEUP,
    /// <summary>
    /// 使用了money
    /// </summary>
    MONEYUSED,
    /// <summary>
    /// 金币数量变化
    /// </summary>
    MONEYCHANGED,
    /// <summary>
    /// 游戏重新开始
    /// </summary>
    GAMERESTART,
    /// <summary>
    /// 返回主页面(选关界面)
    /// </summary>
    MAINMENU,
    /// <summary>
    /// 返回开始界面
    /// </summary>
    BACKTOSTART,
    /// <summary>
    /// 下一关
    /// </summary>
    GAMENEXT,
    /// <summary>
    /// 插页广告关闭
    /// </summary>
    INTERSTITIALCLOSED,
    /// <summary>
    /// 游戏完成
    /// </summary>
    GAMEFINISH,
    /// <summary>
    /// 游戏暂停
    /// </summary>
    GAMEPAUSE,
    /// <summary>
    /// 游戏继续
    /// </summary>
    GAMECONTINUE,
    /// <summary>
    /// 游戏开始
    /// </summary>
    GAMESTART,
    /// <summary>
    /// 使用了医疗包
    /// </summary>
    USEMEDIKIT,
    /// <summary>
    /// 视频广告关闭
    /// </summary>
    VIDEOCLOSED,
    /// <summary>
    /// 退出游戏
    /// </summary>
    GAMEQUIT,
    /// <summary>
    /// 点击了更多游戏
    /// </summary>
    GAMEMORE,
    /// <summary>
    /// 点击了游戏评论
    /// </summary>
    GAMERATE,
    /// <summary>
    /// 敌人走光了
    /// </summary>
    ENEMYCLEARED,
    /// <summary>
    /// 敌人跑了
    /// </summary>
    ENEMYAWAY,
    /// <summary>
    /// 打开夜视
    /// </summary>
    PROJECTION,
    /// <summary>
    /// 打开狙击镜
    /// </summary>
    ZOOM,
    //红外线时间结束
    PROJECTIONTIMEOUT,
    /// <summary>
    /// 动物警觉
    /// </summary>
    ANIMALWARNED,
    /// <summary>
    /// 游戏预览转到开始
    /// </summary>
    PREVIEWSTART,
    /// <summary>
    /// 点击了开始
    /// </summary>
    PLAYCLICKED,
    /// <summary>
    /// 玩家开枪了
    /// </summary>
    FIRED,
    /// <summary>
    /// 点击了more
    /// </summary>
    MORE,
    /// <summary>
    /// 点击了商店界面
    /// </summary>
    SHOP,
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    STOPBGM,
    /// <summary>
    /// 开始背景音乐
    /// </summary>
    STARTBGM,
    /// <summary>
    /// 主菜单加载完毕
    /// </summary>
    MENULOADED,
    PLAYERDIE,
}