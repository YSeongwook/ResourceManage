using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics; // 성능 측정을 위한 Stopwatch
using System.Collections;
using System.IO;

public class AssetBundleLoader : MonoBehaviour
{
    [SerializeField] private Button loadButton; // 에셋 번들 로드 버튼
    [SerializeField] private Transform loadPosition; // 프리팹 생성 위치
    [SerializeField] private string bundleName = "testbundle"; // 에셋 번들 이름
    [SerializeField] private string assetName = "AssetBundlePrefab"; // 번들 내의 프리팹 이름
    [SerializeField] private int numberOfInstances = 100; // 생성할 인스턴스 개수

    private string _bundlePath; // 에셋 번들 경로
    private AssetBundle _loadedBundle; // 로드된 에셋 번들
    private GameObject _cachedPrefab; // 캐싱된 프리팹

    private void OnEnable()
    {
        loadButton.onClick.AddListener(OnLoadButtonClicked);
    }

    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(OnLoadButtonClicked);
    }

    private void Start()
    {
        // 에셋 번들의 로컬 경로 설정
        _bundlePath = Path.Combine(Application.streamingAssetsPath, "AssetBundles", bundleName);
    }

    private void OnLoadButtonClicked()
    {
        // 에셋 번들에서 프리팹을 로드하고 여러 인스턴스를 생성
        StartCoroutine(LoadAssetBundleAndSpawnPrefab());
    }

    private IEnumerator LoadAssetBundleAndSpawnPrefab()
    {
        // 한 번도 로드된 적이 없을 때만 에셋 번들을 로드
        if (_loadedBundle == null)
        {
            AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(_bundlePath);
            yield return bundleRequest;
            _loadedBundle = bundleRequest.assetBundle;

            if (_loadedBundle == null)
            {
                DebugLogger.LogError("에셋 번들 로드 실패: " + _bundlePath);
                yield break;
            }
        }

        // 프리팹이 캐싱되지 않았을 때만 로드
        if (_cachedPrefab == null)
        {
            AssetBundleRequest assetRequest = _loadedBundle.LoadAssetAsync<GameObject>(assetName);
            yield return assetRequest;

            _cachedPrefab = assetRequest.asset as GameObject;
            if (_cachedPrefab == null)
            {
                DebugLogger.LogError("프리팹 로드 실패: " + assetName);
                yield break;
            }
        }

        // 성능 측정을 위한 Stopwatch
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // 여러 개의 프리팹 인스턴스 생성
        for (int i = 0; i < numberOfInstances; i++)
        {
            Instantiate(_cachedPrefab, loadPosition.position + new Vector3(i * 2, 0, 0), Quaternion.identity);
        }

        stopwatch.Stop();
        DebugLogger.Log($"프리팹 {numberOfInstances}개 인스턴스화 완료. 걸린 시간: {stopwatch.ElapsedMilliseconds} ms");
    }

    // 필요할 때 에셋 번들 해제 (예: 앱 종료 시)
    private void OnApplicationQuit()
    {
        if (_loadedBundle != null)
        {
            _loadedBundle.Unload(false);
            DebugLogger.Log("에셋 번들 메모리에서 해제됨.");
        }
    }
}
