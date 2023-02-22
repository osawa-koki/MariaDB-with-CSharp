using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


string server = "localhost";
string str_db = "my_db";
string user = "root";
string password = "p@ssword1234";


// MariaDBに接続
MySqlConnection conn = new MySqlConnection("Server=" + server + ";Database=" + str_db + ";Uid=" + user + ";Pwd=" + password + ";");

conn.Open();

// SQL文を作成
string sql = "SELECT * FROM users";

// SQL文を実行するためのコマンドを作成
MySqlCommand cmd = new MySqlCommand(sql, conn);

// データを取得
MySqlDataReader reader = cmd.ExecuteReader();

// データを表示
while (reader.Read())
{
    Console.WriteLine(reader["id"] + " " + reader["name"]);
}

// データベースを閉じる
conn.Close();
