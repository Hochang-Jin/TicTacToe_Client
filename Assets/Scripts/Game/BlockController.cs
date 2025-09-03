using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    
    // 1. 모든 블럭 초기화
    public void InitBlocks()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].InitMarker(i);
        }
    }
    
    // 2. 특정 블럭에 마커 표시
    public void SetBlockMarker(int index, Block.MarkerType markerType)
    {
        blocks[index].SetMarker(markerType);
    }
    
    // 3. 특정 블럭의 배경 색을 설정
    public void SetBlockColor()
    {
        // TODO : 게임 로직이 완성되면 구현
    }
}
