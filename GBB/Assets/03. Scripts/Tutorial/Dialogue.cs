using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {

    public List<string> sentence01 = new List<string>();
    public List<string> sentence02 = new List<string>();
    public List<string> sentence03 = new List<string>();
    public List<string> sentence04 = new List<string>();
    public List<string> sentence05 = new List<string>();

    public List<List<string>> sentences = new List<List<string>>();

    // Use this for initialization
    void Awake () {
        sentences.Add(sentence01);
        sentences.Add(sentence02);
        sentences.Add(sentence03);
        sentences.Add(sentence04);
        sentences.Add(sentence05);
        sentence01.Add("좌측 하단의 가상패드를 이용하여 캐릭터를 이동 조작할 수 있습니다.");
        sentence01.Add("캐릭터 상단에 파란색 원형으로 빛나는 지역에 캐릭터를 이동시켜 보십시오.");
        sentence01.Add("훌륭합니다. 남은 파란색 원형에 계속하여 캐릭터를 이동시켜 보십시오.");
        sentence01.Add("훌륭합니다. 남은 파란색 원형에 계속하여 캐릭터를 이동시켜 보십시오.");
        sentence01.Add("앞을 가로막던 'X'표시가 사라집니다. 다음 지역으로 이동하실 수 있습니다.");//
        sentence02.Add("이번엔 공격 시스템에 대해 배워보겠습니다.");
        sentence02.Add("캐릭터의 일정 범위 내에 적이 있을 시 자동으로 오브에서 공격이 발사하게 됩니다.");
        sentence02.Add("녹색 원형 범위가 자동 공격이 가능한 거리 입니다.");
        sentence02.Add("소환된 몬스터에게 다가가 자동 공격을 진행해 보십시오.");
        sentence02.Add("훌륭합니다.");
        sentence02.Add("자동 공격 중 해당 범위를 벗어나면 자동 공격이 해제됩니다.");
        sentence02.Add("녹색 원형 범위에 몬스터가 위치하지 않도록 이동해 보십시오.");
        sentence02.Add("훌륭합니다. 범위에서 벗어나면 공격이 멈추니, 적과의 거리를 유지하십시오.");
        sentence02.Add("앞을 가로막던 'X'표시가 사라집니다. 다음 지역으로 이동하실 수 있습니다.");//
        sentence03.Add("이번엔 방어 시스템에 대해 배워보겠습니다.");
        sentence03.Add("캐릭터는 몬스터의 탄환에 피격 시 1의 데미지를 입으며, 좌측 상단 체력게이지가 감소합니다.");
        sentence03.Add("체력이 0이 될 시 캐릭터는 사망합니다.");
        sentence03.Add("몬스터가 탄환 발사 시 우측 하단 방어 버튼을 통해 탄환을 막을 수 있습니다.");
        sentence03.Add("방어로 탄환을 막을 시 좌측 상단 방어 내구도 게이지가 감소하게 됩니다.");
        sentence03.Add("내구도가 없을 시 방어를 할 수 없으며, 일정 시간이 지나면 게이지가 다시 차오릅니다.");
        sentence03.Add("몬스터의 공격을 방어를 이용하여 3번 막아보십시오.");
        sentence03.Add("훌륭합니다.");
        sentence03.Add("캐릭터 하단에 표시된 범위 내 탄환이 진입했을 때 방어를 하면 탄환을 튕깁니다.");
        sentence03.Add("탄환을 튕겨내면 내구도가 감소하지 않으며, 강력하게 공격을 되받아 칠 수 있습니다.");
        sentence03.Add("타이밍에 맞추어 탄환을 튕겨내 보십시오.");
        sentence03.Add("지금입니다. 방패 버튼을 누르세요.");
        sentence03.Add("계속하여 탄환을 튕겨내 보십시오.");
        sentence03.Add("훌륭하군요. 앞을 가로막던 'X'표시가 사라집니다. 다음지역으로 이동하실 수 있습니다.");//
        sentence04.Add("이번엔 스킬 시스템에 대해 배워보겠습니다.");
        sentence04.Add("파란색 원형 지역으로 이동하십시오.");//30
        sentence04.Add("방어와 패링에 성공하면 스킬 게이지가 차오르게 됩니다.");
        sentence04.Add("스킬 게이지가 전부 차오르면 스킬 버튼이 활성화됩니다.");
        sentence04.Add("현재스킬은 EMP로 화면내 모든 적 탄환을 제거하는 스킬입니다.");
        sentence04.Add("게이지를 다 채워드리겠습니다. 스킬을 사용하여 적 탄환을 없애 보십시오.");
        sentence04.Add("훌륭합니다. 이런식으로 위험한 상황에 스킬을 사용하십시오.");
        sentence04.Add("앞을 가로막던 'X'표시가 사라집니다.");
        sentence04.Add("다음 방으로 가서 지금까지 배운 내용을 바탕으로 직접 모든 몬스터를 처치해 보십시오.");
        sentence05.Add("몬스터 3마리와 전투에서 승리하십시오.");
        sentence05.Add("클릭하시면 전투가 시작됩니다.");
    }
}
