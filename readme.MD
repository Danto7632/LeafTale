# 🖐 LeafTale - Unity 게임 개발 파트

> **손 재활 및 발달 플랫폼의 Unity 게임 클라이언트**  
> LeapMotion 센서를 활용한 실시간 손동작 인식 기반 재활 게임

---

## 📋 프로젝트 개요

LeafTale은 컴퓨터 비전 AI 센서인 **LeapMotion**을 활용하여 손의 움직임을 실시간으로 분석하고, 손 기능 개선과 발달을 돕는 게임 기반의 손 재활 및 발달 플랫폼입니다. 

### 🏗️ 프로젝트 구조
이 프로젝트는 **총 13명**으로 구성된 대규모 팀 프로젝트로, 4개 파트로 나누어 개발되었습니다:

| 파트 | 인원 | 담당 업무 |
|------|------|-----------|
| **🎮 게임 개발** | 4명 | Unity 게임 클라이언트 개발 (현재 레포지토리) |
| **🌐 프론트엔드** | 5명 | 웹 대시보드 및 사용자 인터페이스 |
| **⚙️ 백엔드** | 3명 | 서버 API 및 데이터베이스 관리 |
| **📊 PM** | 1명 | 프로젝트 관리 및 기획 |

> **📍 현재 레포지토리**: Unity로 개발된 **게임 파트** 전용 레포지토리입니다.

### 🎯 개발 목표
- **맞춤형 재활**: 개개인의 손 크기와 발달 상태에 맞춘 최적화된 재활 경험 제공
- **게임화된 재활**: 재미있는 게임을 통해 지속적인 재활 동기 부여
- **실시간 분석**: LeapMotion을 통한 정밀한 손동작 추적 및 분석
- **접근성**: 기존 고가 재활 장비 대비 저비용으로 누구나 접근 가능

---

## 🎬 데모 영상

<div align="center">

[![손동작과 함께하는 게임 플레이](https://img.youtube.com/vi/Lk9xxMedmyE/0.jpg)](https://youtu.be/Lk9xxMedmyE)

**🎮 [손동작과 함께하는 게임 플레이](https://youtu.be/Lk9xxMedmyE)**

[![게임 화면 모음](https://img.youtube.com/vi/eUpXLGqR8mU/0.jpg)](https://youtu.be/eUpXLGqR8mU)

**🖥️ [5개 재활 게임 화면 모음](https://youtu.be/eUpXLGqR8mU)**

[![Unity 개발 화면](https://img.youtube.com/vi/ppTftjDRrrs/0.jpg)](https://youtu.be/ppTftjDRrrs)

**⚙️ [Unity 개발 화면](https://youtu.be/ppTftjDRrrs)**

</div>

---

## 🎮 게임 소개

| 게임 | 설명 | 주요 에셋 |
|------|------|-----------|
| **🧙‍♂️ 날아라 빗자루!**<br/><img src="Assets/StageSelect/Resource/select_broom.png" width="285" alt="빗자루 게임"><br/>**손목 가동범위 및 유연성 개선** | • 손바닥 기울기로 빗자루 조종<br/>• 떨어지는 장애물 피하기<br/>• 아이템 획득으로 시간 연장<br/>• 손목 회전 운동 효과 | <img src="Assets/BroomStickMap/Resource/Player/Player.png" width="115" alt="플레이어"><br/><img src="Assets/BroomStickMap/Resource/Enemy/bird1.png" width="100" alt="새1"><br/><img src="Assets/BroomStickMap/Resource/Enemy/bird2.png" width="100" alt="새2"> |
| **🐱 달려라 고양이!**<br/><img src="Assets/StageSelect/Resource/select_runningcat.png" width="285" alt="고양이 게임"><br/>**손가락 민첩성 및 협응력 훈련** | • 손뒤집기로 점프<br/>• 박수로 방향전환<br/>• 상하운동으로 가속<br/>• 양손 협응력 개발 | <img src="Assets/PlatFromMap/Resource/Map/MagicStick.png" width="100" alt="지팡이"><br/><img src="Assets/PlatFromMap/Resource/Object/coin_gold.png" width="85" alt="코인"><br/><img src="Assets/PlatFromMap/Resource/Map/cloud.png" width="115" alt="구름"> |
| **🎵 울려라 오케스트라!**<br/><img src="Assets/StageSelect/Resource/select_orchestra.png" width="285" alt="오케스트라 게임"><br/>**손동작 정확성 및 반응속도 향상** | • 가위바위보 손동작<br/>• 리듬에 맞춘 매칭<br/>• 좌우 손 독립 인식<br/>• 반응속도 개선 | <img src="Assets/RhythmMap/Resource/Map/Player-piano.png" width="115" alt="피아노"><br/><img src="Assets/RhythmMap/Resource/Note/RockLeft.png" width="95" alt="주먹"><br/><img src="Assets/RhythmMap/Resource/Note/PaperLeft.png" width="95" alt="보"> |
| **🖊️ 그려라 마법진!**<br/><img src="Assets/StageSelect/Resource/select_magic.png" width="285" alt="마법진 게임"><br/>**정밀한 손동작 제어 및 집중력 향상** | • 손 위치로 커서 조종<br/>• 도형 따라 그리기<br/>• 정확도 측정<br/>• 손-눈 협응력 강화 | <img src="Assets/test/Resource/maincha/stop/main1.png" width="115" alt="마법사"><br/><img src="Assets/test/Resource/magicSquare/Circle.png" width="100" alt="원"><br/><img src="Assets/test/Resource/magicSquare/Square.png" width="100" alt="사각형"> |
| **🥕 뽑아라 마법채소!**<br/><img src="Assets/StageSelect/Resource/select_claw.png" width="285" alt="클로우 게임"><br/>**손 쥐기 힘 조절 및 3차원 공간 인식** | • 손바닥 기울기로 이동<br/>• 주먹 쥐기로 집기<br/>• 3D 공간 제어<br/>• 쥐기 힘 조절 훈련 | <img src="Assets/ClawMachineMap/Resource/KakaoTalk_20240829_190732631.png" width="115" alt="당근"><br/><img src="Assets/ClawMachineMap/Resource/KakaoTalk_20240829_193757229.png" width="115" alt="양파"><br/><img src="Assets/ClawMachineMap/Resource/KakaoTalk_20240902_111037623.png" width="115" alt="브로콜리"> |
| **🎬 엔딩 크레딧**<br/><img src="Assets/EndingCredit/Resource/creditBackground.png" width="285" alt="엔딩 크레딧"><br/>**게임 완료 후 결과 화면** | • 게임별 성과 표시<br/>• 재활 진행도 분석<br/>• 개선 사항 제안<br/>• 동기 부여 메시지 | <img src="Assets/EndingCredit/Resource/EndingPng/ending_goodEnding.png" width="130" alt="굿엔딩"><br/><img src="Assets/EndingCredit/Resource/EndingPng/ending_normalEnding.png" width="130" alt="노말엔딩"> |

---

## 🛠️ 기술 스택 (게임 개발 파트)

### 🎮 게임 개발
- **Unity**: 2022.3.x LTS - 게임 엔진
- **C#**: .NET Framework 4.8 - 프로그래밍 언어
- **LeapMotion SDK**: Ultraleap Tracking 5.2+ - 손동작 추적
- **EasyTransitions**: 씬 전환 효과
- **TextMeshPro**: UI 텍스트 렌더링
- **Unity Audio**: 사운드 시스템

### 🎨 디자인
- **Photoshop**: 이미지 편집 및 UI 디자인
- **Figma**: UI/UX 프로토타이핑
- **Aseprite**: 픽셀 아트 및 스프라이트 애니메이션
- **Clip Studio Paint**: 일러스트 및 캐릭터 디자인

### 🔧 버전 관리 및 협업 툴
- **Git**: 소스코드 버전 관리
- **GitHub**: 코드 저장소 및 협업
- **Notion**: 프로젝트 문서화 및 일정 관리

### 💻 플랫폼 지원
- **Windows**: DirectX 11/12
- **macOS**: Metal API
- **개발 권장 사양**: LeapMotion Controller, Unity 2022.3.x+, RAM 8GB+

---

## 📊 프로젝트 프레젠테이션

### 📋 프레젠테이션 자료

| | |
|---|---|
| <img src="https://github.com/user-attachments/assets/d24e6f60-1bb9-421f-8a3c-1a85ce7ca571" width="360" alt="슬라이드1"> | <img src="https://github.com/user-attachments/assets/02bc0929-52e6-41ce-a83e-28113363b8a6" width="360" alt="슬라이드2"> |
| <img src="https://github.com/user-attachments/assets/106144a5-4b6d-4f4c-bdb5-534289ac72df" width="360" alt="슬라이드3"> | <img src="https://github.com/user-attachments/assets/dae54b40-036c-4bf2-9b1a-2dd86aa487a5" width="360" alt="슬라이드4"> |
| <img src="https://github.com/user-attachments/assets/13845ece-69e7-49e4-ba13-5612282b926c" width="360" alt="슬라이드5"> | <img src="https://github.com/user-attachments/assets/915925e1-d7fc-4fc4-8f4e-0bf06d3f946b" width="360" alt="슬라이드6"> |
| <img src="https://github.com/user-attachments/assets/3560fdcb-8795-452a-ae3d-c5fef95b1b1e" width="360" alt="슬라이드7"> | <img src="https://github.com/user-attachments/assets/119f8bbc-f544-4486-9ba9-1f36e8245b54" width="360" alt="슬라이드8"> |
| <img src="https://github.com/user-attachments/assets/9725bebe-74d1-43d7-a424-36c06d16fa01" width="360" alt="슬라이드9"> | <img src="https://github.com/user-attachments/assets/62b26449-c852-4929-838a-7d796c8b0ca0" width="360" alt="슬라이드10"> |
| <img src="https://github.com/user-attachments/assets/5e55f967-e803-4f69-abea-4d23d9a76664" width="360" alt="슬라이드11"> | <img src="https://github.com/user-attachments/assets/9d85ea05-7fee-479d-ac29-ab9a9834d519" width="360" alt="슬라이드12"> |
| <img src="https://github.com/user-attachments/assets/437381ae-59b8-4ae3-b562-959f44872c2a" width="360" alt="슬라이드13"> | <img src="https://github.com/user-attachments/assets/ad7b95ec-a74d-4f17-86ef-6f128316775b" width="360" alt="슬라이드14"> |

---

## 📁 프로젝트 구조

```
LeafTale/
├── Assets/
│   ├── BroomStickMap/          # 빗자루 게임
│   │   ├── Script/Broom/       # 플레이어 조작 스크립트
│   │   ├── Animation/          # 애니메이션 컨트롤러
│   │   └── Resource/           # 게임 리소스
│   ├── PlatFromMap/            # 플랫폼 게임
│   │   ├── Script/             # 고양이 이동, 점프 로직
│   │   └── Resource/           # 스프라이트, 사운드
│   ├── RhythmMap/              # 리듬 게임
│   │   ├── Script/Check/       # 손동작 인식 로직
│   │   └── Animation/          # UI 애니메이션
│   ├── test/                   # 드로잉 게임
│   │   ├── script/             # 그리기 및 정확도 계산
│   │   └── Resource/           # UI 리소스
│   ├── ClawMachineMap/         # 클로우 게임
│   │   ├── Script/             # 클로우 제어 로직
│   │   └── Resource/           # 3D 모델, 텍스처
│   ├── EasyTransitions/        # 씬 전환 시스템
│   ├── Sound/                  # 게임별 사운드 파일
│   ├── UI/                     # 공통 UI 컴포넌트
│   └── Scenes/                 # 게임 씬 파일들
├── ProjectSettings/            # Unity 프로젝트 설정
└── Packages/                   # 패키지 매니페스트
```

---

## 🎯 LeapMotion 손동작 인식

### 지원하는 손동작
| 손동작 | 인식 방법 | 사용 게임 | 설명 |
|--------|-----------|-----------|------|
| **손바닥 기울기** | `hand.PalmNormal` 벡터 분석 | 빗자루, 클로우 | 손바닥을 좌우/상하로 기울여 조종 |
| **주먹 쥐기** | `hand.GrabStrength > 0.9f` | 클로우, 리듬 | 손을 꽉 쥐어 물건 잡기/주먹 인식 |
| **손가락 펴기** | `finger.IsExtended` 체크 | 리듬, 드로잉 | 개별 손가락 펴기 상태 감지 |
| **검지 포인팅** | 검지만 펴진 상태 감지 | 빗자루 시작 | 검지만 펴고 나머지는 구부린 상태 |
| **박수** | 양손 거리 변화 감지 | 플랫폼 | 두 손을 마주쳐 박수치는 동작 |
| **손뒤집기** | 손바닥 방향 변화 감지 | 플랫폼 | 손바닥을 뒤집는 회전 동작 |

### 주요 인식 코드 예시
```csharp
// 손바닥 기울기 감지 (빗자루 게임)
void DetectHandTilt(Hand hand) {
    Vector3 palmNormal = hand.PalmNormal;
    
    if (palmNormal.x > 0.3f) {
        // 오른쪽으로 기울임
        moveDirection.x = palmNormal.x;
    }
    
    if (palmNormal.z < -0.3f) {
        // 아래로 기울임
        moveDirection.y = palmNormal.z;
    }
}

// 주먹 감지 (클로우 게임)
bool IsFist(Hand hand) {
    return hand.GrabStrength > 0.9f;
}

// 가위바위보 감지 (리듬 게임)
void DetectHandPose(Hand hand) {
    if (IsFist(hand)) {
        currentPose = "ROCK";
    } else if (IsScissors(hand)) {
        currentPose = "SCISSOR";
    } else if (IsPaper(hand)) {
        currentPose = "PAPER";
    }
}
```

---

## 📊 데이터 수집 및 분석

### 수집되는 데이터
- **게임별 플레이 시간**: 각 게임의 세션 길이
- **정확도 점수**: 동작 인식 정확도
- **반응 속도**: 자극-반응 시간 측정
- **진행도**: 게임별 달성 레벨
- **손동작 빈도**: 특정 동작의 수행 횟수

### 데이터 전송
게임 데이터는 백엔드 API를 통해 실시간으로 전송되어 웹 대시보드에서 분석됩니다.

---

## 👥 개발팀

| 역할 | 이름 | GitHub | 담당 업무 |
|------|------|--------|-----------|
| **팀장/개발기획** | 김인성 | [@danto7632](https://github.com/danto7632) | 전체 기획, 시스템 설계 |
| **LeapMotion 개발** | 심기준 | [@potato1028](https://github.com/potato1028) | 핸드 트래킹, 동작 인식 |
| **게임 디자인** | 서윤정 | [@SeoYunJoung](https://github.com/SeoYunJoung) | UI/UX, 게임 밸런싱 |
| **게임 디자인** | 김민지 | [@dalsaemi](https://github.com/dalsaemi) | 아트 리소스, 애니메이션 |

---

## 📈 향후 계획

- [ ] **추가 게임 모드**: 새로운 재활 게임 개발
- [ ] **AI 기반 개인화**: 사용자별 맞춤 난이도 조절
- [ ] **VR 지원**: Meta Quest, HTC Vive 플랫폼 확장
- [ ] **다국어 지원**: 영어, 일본어 등 다국어 UI
- [ ] **클라우드 동기화**: 진행도 클라우드 저장
- [ ] **멀티플레이어**: 협동 재활 게임 모드

---

**🖐 건강한 손, 즐거운 재활 - LeafTale과 함께하세요!**