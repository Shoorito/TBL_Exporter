# TBL_Exporter(Shoori's TableExporter)
## 소개
TBL_Exporter는 Excel로 작업된 기획 데이터를 프로그램 코드에서 쉽게 사용할 수 있도록 .xml, .json 파일 형태로 변환하는 툴입니다

기본적인 기능은 위의 사항이 전부이지만, Export한 데이터를 압축하고, 암호화하는 기능도 담고 있습니다

데이터 암호화 또는 압축을 통해 변경된 데이터를 사용하실 분들은 아래 **"데이터 복호화 및 압축 해제"** 문단을 참고 부탁드립니다

해당 프로젝트 특성 상 내부 코드 내용이 자주 바뀔 수 있으며 Unsafe 할 수 있다는 점을 미리 공유드립니다

## 사용 방법
<img width="802" height="482" alt="image" src="https://github.com/user-attachments/assets/c8bb37fd-b921-4c23-ab2d-d5e648b768bc" />

+ **TableLoadPath**: Export할 Excel파일들이 위치한 경로를 지정합니다 오른쪽 "..." 버튼 클릭 시 쉽게 경로 지정이 가능합니다
+ **ExportTablePath**: Export할 경로를 지정합니다, 마찬가지로 "..." 버튼 클릭 시 쉽게 경로 지정이 가능합니다
+ **IgnoreSheetsNames**: Export시 내보내지 않을 시트 명들을 지정합니다, "," 쉼표 기호로 여러개 시트명 지정이 가능합니다
  + **IgnoreWildcardColumns**: 시트 내 컬럼명에 특수 문자가 들어가 있을 경우 해당 컬럼 데이터는 무시합니다
+ **SelectConvertMode**: Export 할 데이터 타입을 정할 수 있습니다
  + **.xlsxToXml**: Excel 파일들을 XML 데이터로 변환합니다
  + **.xlsxToJson**: Excel 파일들을 JSON 데이터로 변환합니다
+ **ExportOptions**: Export 시 데이터에 압축, 암호화 같은 별도 설정이 가능합니다
  + **UseEncrypt**: Export한 데이터를 암호화합니다
  + **ToBinary**: Export한 데이터를 LZ4 기반 압축 알고리즘을 사용해 압축합니다
+ **LoadedFileList**: 로드된 Excel 파일들을 보여줍니다, 각 파일을 더블 클릭하여 1개씩 변환 할 수 있습니다
+ **EncryptOption**: 암호화 관련 설정이 가능합니다, "UseEncrypt" 옵션이 체크되어 있어야 표시됩니다
  + **Password**: 데이터를 암호화 할 때 사용할 Key를 지정합니다, 4자리 이상부터 지정 가능합니다
    + **본 툴의 해당 기능을 부적절하게 사용하는 것을 금합니다**
+ **ExportAllTable**: 로드된 모든 Excel 파일들을 변환합니다
+ **Refresh**: UI 요소 및 데이터 상태 등을 Refresh 처리합니다

## 데이터 복호화 및 압축 해제
본 툴을 통해 암호화 및 압축된 데이터를 프로그램에서 사용하기 위해서는 복호화 및 압축 해제 기능을 사용해야 합니다
복호화 및 압축 해제 기능은 하단 **"사용 서드파티"** 항목에 기재된 "SHUtil" 서드파티에 포함되어 있습니다
**상단에 언급드려듯 본 기능을 부적절하게 사용하는 것을 엄격히 금합니다**

### 1. 데이터 압축만 진행한 경우(.bytes 확장자)
1. C#의 File 클래스 기능 내 포함된 "ReadAllBytes"와 같은 기능을 활용하여 해당 파일의 Serialized된 압축 데이터를 가져옵니다
2. SHUtil에 포함된 **"CLZF"** 클래스 내 **"Decompress"** 함수에 1번 과정에서 불러온 Serialized된 데이터를 입력합니다
3. 2번 과정을 통해 반환된 데이터를 "WriteAllBytes"와 같은 기능을 활용하여 압축 전 설정한 데이터 확장자 명으로 저장합니다
4. (선택사항) 반환된 데이터를 저장하지 않고 즉시 사용하고 싶으신 경우 UTF8Encoding.GetString과 같은 기능을 활용하셔도 됩니다

### 2. 데이터 암호화만 진행한 경우(.ens 확장자)
1. 암호화된 파일의 Path 정보를 가져옵니다 
2. SHUtil에 포함된 **"FileUtil"** 클래스의 "Decrypt" 함수에 파일 경로와, 암호화 시 설정한 비밀번호를 입력합니다
+  ※ **암호화 시 비밀번호가 없으면 해당 파일을 복호화 할 수 없으므로 반드시 비밀번호를 별도의 방법으로 저장해주세요**
3. 2번 과정을 통해 반환된 데이터를 "WriteAllBytes"와 같은 기능을 활용하여 압축 전 설정한 데이터 확장자 명으로 저장합니다

### 3. 암호화와 압축 둘 다 진행한 경우(.shoori 확장자)
1. 해당 파일에 대해 먼저 상단 번호 2번(데이터 암호화만 진행한 경우) 과정을 진행합니다
2. 이후 얻어진 데이터(Serialized 된 Byte 데이터)에 대해 상단 번호 1번(데이터 압축만 진행한 경우) 과정을 진행합니다
3. 2번 과정을 통해 반환된 데이터를 "WriteAllBytes"와 같은 기능을 활용하여 압축 전 설정한 데이터 확장자 명으로 저장합니다

## 사용 서드파티
+ [SHUtil(자체개발)](https://github.com/Shoorito/SHUtil)
+ Excel 4.5
