# :pushpin: Store-management
>스토어 매니지먼트 시스템(POS)
></br>
>![image](https://github.com/yschii/Store-management/assets/135096712/cb0bbab5-2d7b-4d0e-8f13-3125ba6a2b84)
>설치: 
1. Mssql설치 및 DB환경 구축: https://esoog.tistory.com/entry/C-SQL-server-%EC%82%AC%EC%9A%A9
2. dataset 폴더 내 SM.bak 파일을 MSSMS의 데이터베이스에서 복구 시켜준다.
3. Main.cs 파일에서 con_string 변수의 DB서버 계정 설정을 변경시켜준다.
4. 실행
* GUNA에 대한 라이센스 문제가 뜰 수 있는데, trial 키로 이메일을 입력하고 사용할 수 있다.
</br>

## :pushpin: Contact
- 이메일: winstonys516@naver.com
- 블로그: https://esoog.tistory.com
- 깃헙: https://github.com/yschii
</br>

## 1. 제작 기간 & 참여 인원
- 2023년 8월
- 개인 프로젝트
</br>

## 2. 사용 기술
#### `Back-end`
  - C#
  - .Net Framework 4.7.2
  - MsSQL 19.1(SQL server)
#### 'Etc'
  - Crystal Reports
</br>

## 3. ERD 설계(Entity Relationship Diagram)
![image](https://github.com/yschii/Store-management/assets/135096712/2e50af7b-d3ca-41c7-85b8-c67432fa1afa)
</br>

## 4. 핵심 기능
이 서비스의 핵심 기능은 윈도우 프로그램으로 스토어 관리 및 POS 프로그램 구현 입니다.
사용자는 윈도우 UI 환경에서 
스토어 매니지먼트 시스템의 제어 및 활용이 가능합니다.
</br>

### 4.1. MsSQL 연결 및 로그인 유저 데이터 확인 활용
![image](https://github.com/yschii/Store-management/assets/135096712/e23b6257-4b5b-43f2-b010-a099ffe0204d)
</br>
https://github.com/yschii/Store-management/blob/main/MainClass.cs
</br>

### 4.2. Crystal Reports를 사용하여 보고서 및 영수증 작성
![image](https://github.com/yschii/Store-management/assets/135096712/48605d37-693c-4382-ab12-d57df93d7df2)
![image](https://github.com/yschii/Store-management/assets/135096712/729987ce-6850-45e0-9dc5-502f8466dcf8)
</br>
https://esoog.tistory.com/entry/C-Crystal-Reports-%EC%82%AC%EC%9A%A9
</br>

## 6. 트러블 슈팅
### 6.1. SQL 쿼리 매개변수 '@' 와 축자 문자열 리터럴 '@' 사이의 혼
- Windows Forms는 단일 스레드 모델을 사용하며, UI 컨트롤에 대한 변경은 UI 스레드에서만 안전하게 수행해야 함. 이 코드에서는 UI 업데이트를 수행하기 위해 Invoke를 사용: https://github.com/yschii/Dbgo/blob/main/3.%20%ED%99%95%EC%9E%A51(%ED%8F%AC%EC%9D%B8%ED%8A%B8%20%EC%A0%9C%EB%8F%84%20%ED%82%A4%EC%98%A4%EC%8A%A4%ED%81%AC)/1.%20%ED%82%A4%EC%98%A4%EC%8A%A4%ED%81%AC%20%EC%A3%BC%EB%B0%A9(%EC%84%9C%EB%B2%84)/MainForm.cs
</br>

### 6.2. 원격 DB 구축
- 내부망을 사용하는 환경에서 원격 DB를 구축하기 위해 RDS 서비스를 구매하여 사용 : https://github.com/yschii/Dbgo/blob/main/2.%20php%2C%20mysql/logs.php
- https://aws.amazon.com/ko/rds/
</br>

## 7. 회고 
- SQL 쿼리 매개변수 '@' 와 축자 문자열 리터럴 '@' 비교 : https://esoog.tistory.com/entry/C-%EA%B8%B0%ED%98%B8-%EC%82%AC%EC%9A%A9
  </br>







# 로그인 화면
![image](https://github.com/yschii/Store-management/assets/135096712/46492b5f-f5e7-4729-b884-b66ec09a45a3)
# 대시보드
![image](https://github.com/yschii/Store-management/assets/135096712/cb892977-97d3-4a2e-8738-971a14e68cd6)
# 상품 등록 
![image](https://github.com/yschii/Store-management/assets/135096712/82fd009f-dd40-483a-85e2-8f97f57b2297)
# POS 폼
![image](https://github.com/yschii/Store-management/assets/135096712/104160e4-c924-4e63-bd1a-d6e23e0ce082)
# 주문 폼
![image](https://github.com/yschii/Store-management/assets/135096712/d4e7b895-023c-49d0-9854-4b3fedb544a5)
# 키친 
![image](https://github.com/yschii/Store-management/assets/135096712/0491c240-7337-4e0f-9aab-d5033139cd68)
# 알림판 폼
![image](https://github.com/yschii/Store-management/assets/135096712/805ef84e-1526-4e17-926d-9f098994ff1c)
# 보고서 메뉴 폼
![image](https://github.com/yschii/Store-management/assets/135096712/872ee501-d08c-4924-af6a-96d78725d9ff)
# 보고서 폼
![image](https://github.com/yschii/Store-management/assets/135096712/06bd25ab-f84b-40c1-98d2-caef2d4178de)
