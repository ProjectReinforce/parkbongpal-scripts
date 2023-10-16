// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Threading.Tasks;

// public class RankingUpdate : Ranking
// {
//     [SerializeField] GameObject waitImage;
//     float rotationSpeed = 50.0f;

//     // async void RankChange()
//     // {
//     //     await Task.Run(() => 
//     //     {
//     //         Debug.Log("Run함수 실행 시작!!!!!!!!!!!!!!!!!!!!!!!");
//     //         Debug.Log("GetRankList함수 실행 시작!!!!!!!!!!!!!!!!!!!!!!!");
//     //         Managers.ServerData.GetRankList();
//     //         for(int i = 0; i < PORT_COUNT; i++)
//     //         {
//     //             myRank[i] = i == 0 ? FindMyRankDataByNickname(RankingType.분당골드량) : FindMyRankDataByNickname(RankingType.전투력);
//     //             Debug.Log("AllRankingChartUpdate(i) 위" + myRank[i].rank);
//     //             AllRankingChartUpdate(i);
//     //             Debug.Log("AllRankingChartUpdate(i) 아래" + myRank[i].rank);
//     //             waitImage.SetActive(false);
//     //             Debug.Log("비동기진짜끝-------------------------------------"); 
//     //         }
//     //         SetMyRankByRankingUpdate(myRank);
//     //         AllRankingChartUpdate(0);
//     //     });
//     // }

//     // private void Update()
//     // {
//     //     if (waitImage.activeSelf)
//     //     {
//     //         waitImage.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
//     //     }
//     // }

//     public void RankChanger()
//     {
//         // waitImage.SetActive(true);
//         // RankChange();
//         // 30분마다 불러올 수 있도록 가능한 위치에서 소환할수있또록ㄱㄱㄱ
//         // if (Time.time - lastCallTime < 1800) return;// 30분
//         // lastCallTime = Time.time;
//         Managers.ServerData.GetRankList(); // 서버에서 리시브 받는 부분을 추가해놓아야함
//         for(int i = 0; i < PORT_COUNT; i++)
//         {
//             myRank[i] = i == 0 ? FindMyRankDataByNickname(RankingType.분당골드량) : FindMyRankDataByNickname(RankingType.전투력);  // 미니게임이 들어오면 if나 switch문으로 변경해야함
//             // Debug.Log("AllRankingChartUpdate(i) 위" + myRank[i].rank);
//             AllRankingChartUpdate(i);
//             // Debug.Log("AllRankingChartUpdate(i) 아래" + myRank[i].rank);
//             SetMyRankByRankingUpdate(myRank);
//             // Debug.Log("SetMyRankByRankingUpdate함수 끝나고 들어간 myRank ----------------------" + myRank[i].nickname);
//             // Debug.Log("SetMyRankByRankingUpdate함수 끝나고 들어간 myRank ----------------------" + myRank[i].rank);
//         }
//         AllRankingChartUpdate(0);
//         // UI에서도 이 정보를 알아야함
//     }
//     // 왜 2번 클릭해야 전체세팅이 되는지 알아봐야함

//     public void AllRankingChartUpdate(int _index)
//     {
//         // Debug.Log("allRank함수 안에 들어온 인덱스 ----------------------" + _index);
//         // Debug.Log("AllRankingChartUpdate함수 안에 들어온 myRank 0----------------------" + myRank[0].rank);
//         // Debug.Log("AllRankingChartUpdate함수 안에 들어온 myRank 1----------------------" + myRank[1].rank);
//         for (int i = 0; i < PORT_COUNT; i++)
//         {
//             if(myRank[_index].rank <= 3)
//             {
//                 SetSlotTo123(slotLists[i], ranks[i][_index], myRank[_index], _index);
//             }
//             else
//             {
//                 SetSlotTo(slotLists[i], ranks[i][_index]);
//             }
//         }
//     }
// }
