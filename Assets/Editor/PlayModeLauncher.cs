using UnityEditor;
using UnityEngine;

public class PlayModeLauncher : EditorWindow
{
    private static EnterPlayModeOptions _originalOptions;
    private static bool _wasOptionsEnabled;

    [MenuItem("Tools/Custom Play Mode Launcher")]
    public static void ShowWindow()
    {
        GetWindow<PlayModeLauncher>("Play Launcher");
    }

    private void OnGUI()
    {
        GUILayout.Label("Custom Play Buttons", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button(" Play (Normal)"))
        {
            //LaunchWithTemporaryPlayMode(EnterPlayModeOptions.ReloadDomain | EnterPlayModeOptions.ReloadScene);
            // 通常（リロードあり）
            LaunchWithTemporaryPlayMode(EnterPlayModeOptions.None);
        }

        if (GUILayout.Button(" Play (Fast)"))
        {
            //LaunchWithTemporaryPlayMode(EnterPlayModeOptions.None);
            // 爆速（リロードなし）
            LaunchWithTemporaryPlayMode(
                EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload
            );
        }
    }

    private void LaunchWithTemporaryPlayMode(EnterPlayModeOptions temporaryOption)
    {
        // 保存
        _originalOptions = EditorSettings.enterPlayModeOptions;
        _wasOptionsEnabled = EditorSettings.enterPlayModeOptionsEnabled;

        // 一時変更
        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions = temporaryOption;

        // イベント登録（重複防止のため一度削除）
        EditorApplication.playModeStateChanged -= RestorePlayModeSettings;
        EditorApplication.playModeStateChanged += RestorePlayModeSettings;

        // 再生
        EditorApplication.isPlaying = true;
    }

    private static void RestorePlayModeSettings(PlayModeStateChange state)
    {
        // 再生終了時に元に戻す
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            EditorSettings.enterPlayModeOptionsEnabled = _wasOptionsEnabled;
            EditorSettings.enterPlayModeOptions = _originalOptions;

            EditorApplication.playModeStateChanged -= RestorePlayModeSettings;
        }
    }
}
