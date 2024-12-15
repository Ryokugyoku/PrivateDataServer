# FileModule 
## 機能要件
- ユーザ単位にアップロードしたファイルを格納するフォルダを作成する。
- ファイルのハッシュ値・アップロード日、アップロードユーザが追尾できるようにする。

## DB定義
### file_master
| 物理名            | 物理名                           | データ型            | 制約 | 備考|
|------------------|--------------------------------|--------------------|------|----|
| file_id | ファイルID | UUID | 主キー |　デフォルト値としてuuid_generate_v4()を使用 |
| file_name | ファイル名 | Text | Not Null | |
| file_path | ファイルパス | Text | Not Null | |
| create_user | 作成ユーザ | varying(256) | FK | AspNetUsersのUserNameを外部参照する |
| create_date | 登録日時 | timestamp | Not Null | |