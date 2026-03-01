# Clock Shooter (Sample Architecture Project)

PC 싱글 로그라이크 턴제 전략 디펜스 게임으로, 스팀으로 상용 출시된 게임 Clock Shooter의 샘플 프로젝트입니다.

게임의 전체 시스템과 맵, UI/UX를 설계하였으며, 로컬라이징 및 스팀 리더보드, 스팀 도전과제 등이 구현되었습니다.

https://github.com/user-attachments/assets/fdd0dbda-9463-411d-9007-d64bc86e511a

## 🎮 프로젝트 소개
| 항목 | 내용 |
|------|------|
| **프로젝트 유형** | 개인 프로젝트 (클라 1인) |
| **장르** | Roguelike turn-based strategy defense |
| **엔진** | Unity |
| **언어** | C# |
| **진행 기간** | *2025-12-11 ~ 2026-02-19* |
| **플랫폼** | PC (Steam) |

* 게임 상점 페이지 : https://store.steampowered.com/app/4409860/Clock_Shooter/?beta=0

---

## 🛠 주요 구현 기능

### Platform & Service Integration

1) Steam SDK 연동 및 Steamworks API를 활용한 리더 보드, 도전 과제 시스템 구성
2) Unity Localization 기반 다국어 지원 전략

### Resource & Async Architecture

1) AssetLoaderProvider 기반 Addressables 확장 구조 설계
2) IPoolable 기반 오브젝트 풀 패턴 설계

### Core Game Architecture

1) 턴 기반 게임 시스템 구조 및 몬스터 위치 계산 구조
2) StatusManager 기반 캐릭터 스테이터스 및 상태 관리
3) 몬스터 소환 로직 및 몬스터의 상태 관리 기법
4) FSM과 Animation Event, Cinemachine을 활용한 보스 연출 시퀸스
5) InputSystem 기반 사용자 입력 구조

### Data-Driven Design

1) ScriptableObject와 DataManager를 활용한 데이터 관리 전략
2) 데이터 기반 플레이어 스킬 및 로그라이크식 뽑기 시스템

### UI Architecture

1) UIManager를 활용한 UI 관리와 구조 설계

---

## 💡 기술적 도전

### ✔ 문제 1 : 에셋 로딩 방식의 전환에 있어 Addressables를 도입하기 위한 구조 설계
- 의도
  - 초기부터 Addressables를 적용하면 개발 속도의 비동기 구조 설계 등에서 개발 속도 저하가 예상됨
  - Resources를 통한 에셋 로드 방식으로 빠른 개발 후, Addressables로 전환 시 리팩토링을 최소화하는 구조 전략을 설계
- 해결
  - AssetLoaderProvider를 통한 리소스 로드 방식의 전환 방식 설계
  - DataManager로 에셋 로딩 로직 집중 방식

### ✔ 문제 2 : Steamworks SDK를 활용한 스팀 리더보드, 도전과제 구현
- 의도
  - 게임의 반복 플레이를 유도하기 위한 동기부여로서 리더보드 랭킹 시스템, 도전 과제 등 외부 API 활용 시도 
- 해결
  - 스팀 API를 전담 관리하는 SteamManager 어댑터 구조의 설계
  - 랭킹 시스템을 만들기 위한 점수 계산 로직 설계, 리더보드 등록 및 조회 시스템 및 도전과제 구성
 
### ✔ 문제 3 : 다국어 지원을 위한 Unity Localization 적용
- 의도
  - 게임의 유저층 확보를 위한 다국어 지원 로컬라이징 진행 시도 
- 해결
  - Unity Localization 기능을 통해 튜토리얼, 스킬 데이터 등의 다국어 번역 기능 구현

---

## 📂 폴더 구조 설명

- Core : 게임 전반에서 재사용 가능한 핵심 로직
- Data : 데이터 로드 구조 및 게임 내 데이터 ScriptableObjects
- Gameplay : 실제 플레이 로직
- InfraStructure : 공통 유틸 및 확장 메서드
- Platform : SteamSDK 등 외부 API 활용
- Resources : Addressables 전환 로직 및 최적화 전략
- UI : UI 생성 및 관리 로직

<pre>
Scripts/
│
├─ Core/
│   ├─ Status/
│   │   ├─ Skills/
│   │   └─ Stats/
│   └─ TurnSystem/
│
├─ Data/
│   ├─ DataManager/
│   └─ ScriptableObjects/
│         ├─ Effect/
│         ├─ MonsterDatabase/
│         ├─ Player/
│         ├─ Skill/
│         │  └─ SkillEffectSO/
│         └─ Sound/
│
├─ Gameplay/
│   ├─ Boss/
│   ├─ Effect/
│   ├─ FSM/
│   │   ├─ BossState/
│   │   ├─ MonsterState/
│   │   └─ PlayerState/
│   ├─ Monster/
│   │   ├─ MonsterComp/
│   │   └─ MonsterSummon/
│   ├─ Player/
│   │   ├─ Control/
│   │   └─ Skills/
│   │      ├─ Context/
│   │      └─ SkillSelector/
│   └─ Sound/
│
├─ InfraStructure/
│   ├─ Extension/
│   └─ Utils/
│
├─ Platform/
│   └─ Steam/
│
├─ Resources/
│   ├─ Addressables/
│   └─ Pooling/
│
└─ UI/
</pre>

---

## 🚀 배운 점

- 유지보수성과 확장성을 고려한 기능 분리형 구조 설계의 중요성
- 데이터와 로직의 분리를 통한 게임의 유지보수성 및 밸런싱 확보 전략
- 게임성 자체의 품질화만이 아닌, 리더보드/도전과제를 통한 경쟁 요소, 도전 요소를 통한 게임성 향상 경험
