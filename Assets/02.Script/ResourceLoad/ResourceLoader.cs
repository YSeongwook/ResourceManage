using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLoader : MonoBehaviour
{
    [SerializeField] private Button loadButton; // 리소스 로드 버튼
    [SerializeField] private Transform loadPosition; // 리소스 생성 위치
    [SerializeField] private string resourceName; // 생성할 프리팹 이름
    [SerializeField] private int numberOfInstances; // 생성할 인스턴스 개수

    private void OnEnable()
    {
        // 버튼 클릭 이벤트 등록
        loadButton.onClick.AddListener(LoadResource);
    }

    private void OnDisable()
    {
        loadButton.onClick.RemoveListener(LoadResource);
    }

    // 버튼을 눌렀을 때 리소스를 로드하는 함수
    private void LoadResource()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject loadedPrefab = Resources.Load<GameObject>("Prefabs/" + resourceName);
            if (loadedPrefab != null)
            {
                Instantiate(loadedPrefab, loadPosition.position + new Vector3(i * 2, 0, 0), Quaternion.identity);
            }
        }

        stopwatch.Stop();
        DebugLogger.Log($"리소스 {numberOfInstances}개 로드 완료. 걸린 시간: {stopwatch.ElapsedMilliseconds} ms");
    }
}