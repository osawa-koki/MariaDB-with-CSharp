# MariaDB-with-CSharp

MariaDBをC#で操作するためのサンプルプログラム。  

## 注意点

### MariaDBのバージョンは10.9以下で

MariaDBのバージョンが10.9よりも新しいと`connection.Open()`時にNULLキャストエラーが発生する。  
