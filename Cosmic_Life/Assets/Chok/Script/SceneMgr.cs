using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    LogoScene,
    Title,
    sample,
}

public class SceneMgr : SingletonBehaviour<SceneMgr>
{
    // 現在のシーン
    private SceneType m_currentScene = SceneType.LogoScene;
    // 遷移完了か
    private bool m_isEnd = true;
    // フェードしたか
    private bool m_isFade = false;
    // フェード時間の長さ
    private float m_duration = 0.5f;
    // 遷移の作業
    private AsyncOperation m_async;

    /// <summary>
    /// Fadeしてから遷移
    /// </summary>
    /// <param name="name">シーンの名前</param>
    public void SceneTransition(SceneType name)
    {
        if (!m_isEnd) return;
        m_isFade = true;
        m_isEnd = false;
        FadeMgr.Instance.FadeOut(m_duration, () => { m_isFade = false; });
        StartCoroutine(transition(name, m_duration));
    }

    IEnumerator transition(SceneType name,float duration)
    {
        yield return new WaitWhile(() => m_isFade);

        yield return SceneManager.LoadSceneAsync(name.ToString(), LoadSceneMode.Additive);

        UnLoadScene(m_currentScene);
        m_currentScene = name;

        if (duration != 0)
        {
            FadeMgr.Instance.FadeIn(duration, () =>
            {
                Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
            });
        }
        else
        {
            Debug.Log(name.ToString() + "_Scene : LoadComplete!!");
        }

    }
    public void UnLoadScene(SceneType name)
    {
        Debug.Log(name.ToString() + "_Scene : UnLoad!!");
        SceneManager.UnloadSceneAsync(name.ToString());
    }


}
