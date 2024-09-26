using UnityEngine;
using UnityEngine.UI;

public class ResourceLoader : MonoBehaviour
{
    [SerializeField] private Button loadButton; // 리소스 로드 버튼
    [SerializeField] private Transform loadPosition; // 리소스 생성 위치
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
        // Resources 폴더에서 프리팹 로드
        GameObject loadedPrefab = Resources.Load<GameObject>("Prefabs/ResourceLoadPrefab");
        
        if (loadedPrefab != null)
        {
            // 로드한 프리팹을 화면에 인스턴스화
            Instantiate(loadedPrefab, loadPosition.position, Quaternion.identity);
            DebugLogger.Log("리소스 로드 성공!");
        }
        else
        {
            DebugLogger.LogError("리소스 로드 실패!");
        }
    }
}