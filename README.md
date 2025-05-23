-----

# ⚔️ 스파르타 던전 탐험 ⚔️
(던전 탐험까지는 구현 못했습니다)
![image](https://github.com/user-attachments/assets/ae2a80c9-f196-42f8-9641-d34e50beb0cc)
-----

## 🎮 게임 소개

스파르타 던전 탐험은 Unity 3D로 제작된 게임입니다. 플레이어는 동적으로 생성되는 발판 위를 이동하면 됩니다. 다양한 아이템을 활용하고 낙하 대미지를 조심하시면 됩니다.
![image](https://github.com/user-attachments/assets/d10fb685-282d-4bb9-8b9b-77f4282ffa51)
-----

## ✨ 주요 기능

### 🚶‍♂️ 플레이어 조작

  * **이동:** `W A S D` 키
  * **점프:** `Space` 키
  * **아이템 줍기:** `E` 키
  * **아이템 사용:** 마우스 `좌클릭`
  * **아이템 슬롯 전환:** `Tab` 키

### 🗺️ 동적 발판 시스템

  * **진행형 발판:** 발판을 밟기 시작하면 다음 발판이 생성됩니다.
  * **소멸하는 발판:** 다음 발판으로 이동하면 이전에 있던 발판은 자동으로 사라져 플레이어의 집중도를 높입니다.
  * **점프대 발판:** 특정 검은색 발판은 플레이어를 높이 띄워주는 점프대 역할을 합니다. 게임 시작을 위해 점프대를 통해 발판에 도달해야 합니다.

### 📉 낙하 대미지 & 초기화
![image](https://github.com/user-attachments/assets/625a0641-4d50-47b1-bf23-13a9dbfe0fe8)
  * **높이 비례 대미지:** 일정 높이 이상에서 떨어질 경우, 낙하 거리에 비례하여 플레이어의 체력이 감소합니다.
  * **발판 초기화:** 발판에서 벗어나 땅으로 떨어지면, 모든 발판이 초기 위치로 초기화되어 새로운 도전을 시작할 수 있습니다.

### 🎒 인벤토리 & 아이템 시스템
![image](https://github.com/user-attachments/assets/4fa155f2-3cc1-4ad8-8ee5-38fb8083d79c)
![image](https://github.com/user-attachments/assets/f15e3fcc-39de-4a15-90c9-47807526bd33)
  * **아이템:**
      * `E` 키를 눌러 아이템을 저장할 수 있습니다.
      * **버섯 (이미지가 없어서 고기 이미지로 대체):** 체력 회복
      * **당근:** 사용 시 점프력 증가
  * **아이템 슬롯:**
      * `Tab` 키를 눌러 **선택 중인 아이템 슬롯**을 바꿀 수 있고, 그 슬롯으로 아이템이 우선적으로 저장됩니다.
      * **같은 아이템은 한 슬롯에 최대 3개까지 스택(Stack)됩니다.**
      * 선택된 슬롯이 이미 다른 종류의 아이템으로 채워져 있거나, 같은 아이템이라도 최대 수량인 경우, 인벤토리의 다른 빈 슬롯을 찾아 저장됩니다.
      * 인벤토리가 가득 차서 더 이상 아이템을 수용할 수 없을 경우, 아이템은 플레이어 발밑에 드롭됩니다.
![image](https://github.com/user-attachments/assets/2744f5a1-52b1-4474-ad0b-421e0430a0ea)
![image](https://github.com/user-attachments/assets/3ba72d1a-87ee-4195-ab88-ebff5e7d2b4a)
  * **아이템 정보 표시:** 아이템을 바라보면 해당 아이템의 이름과 설명이 화면에 출력되어 플레이어에게 정보를 제공합니다.

-----

## 🛠️ 개발 환경

  * **Unity Engine:** 2022.3.17f1
  * **프로그래밍 언어:** C\#

-----
아하, 제가 사용자님의 의도를 제대로 파악하지 못했네요! 죄송합니다.

말씀하신 **코드 주소(파일 경로)**를 README에 포함하여 각 스크립트가 어디에 위치하는지 명확하게 보여드리겠습니다.

---

## 📂 프로젝트 구조 및 핵심 스크립트

본 프로젝트는 다음과 같은 주요 스크립트들로 구성되어 있으며, 각 스크립트는 프로젝트 내 특정 경로에 위치합니다.

---

### Item (아이템 관련)
* **`ItemSlot.cs`**: 인벤토리 UI의 **개별 아이템 슬롯** 기능을 정의하고 관리합니다.
    * `Sparta_Dungeon/Assets/Scripts/Item/ItemSlot.cs`
* **`ItemObject.cs`**: 게임 월드에 배치된 **아이템 오브젝트**의 상호작용(예: 이름/설명 표시)을 처리합니다.
    * `Sparta_Dungeon/Assets/Scripts/Item/ItemObject.cs`
* **`ItemData.cs`**: **모든 아이템의 공통 속성** (이름, 설명, 타입, 아이콘 등)을 정의하는 ScriptableObject입니다. 에디터에서 새로운 아이템 데이터를 쉽게 생성할 수 있습니다.
    * `Sparta_Dungeon/Assets/Scripts/ScriptableObject/ItemData.cs`

### Player (플레이어 관련)
* **`Interaction.cs`**: 플레이어가 월드 오브젝트(예: 아이템)를 **바라볼 때의 상호작용**을 감지합니다.
    * `Sparta_Dungeon/Assets/Scripts/Player/Interaction.cs`
* **`Player.cs`**: **플레이어의 모든 핵심 기능** (움직임, 상태, 아이템 관리)을 통합하고, 이 플레이어 정보를 `CharacterManager`에 등록합니다.
    * `Sparta_Dungeon/Assets/Scripts/Player/Player.cs`
* **`PlayerCondition.cs`**: 플레이어의 **체력, 점프같은 상태 변화**를 관리하고 게임에 반영합니다.
    * `Sparta_Dungeon/Assets/Scripts/Player/PlayerCondition.cs`
* **`PlayerController.cs`**: **플레이어의 이동, 점프 등 조작**과 관련된 로직을 처리합니다.
    * `Sparta_Dungeon/Assets/Scripts/Player/PlayerController.cs`

### Object (게임 오브젝트 관련)
* **`JumpObject.cs`**: 특정 발판이 **점프대** 역할을 하도록 기능을 구현합니다.
    * `Sparta_Dungeon/Assets/Scripts/Object/JumpObject.cs`

### Manager (관리자)
* **`CharacterManager.cs`**: **플레이어 데이터를 중앙에서 관리**하고 게임 내 어디서든 쉽게 접근할 수 있도록 돕는 **싱글톤 매니저**입니다.
    * `Sparta_Dungeon/Assets/Scripts/Player/CharacterManager.cs`

---
