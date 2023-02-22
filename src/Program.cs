using dotenv.net;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

DotEnv.Load(options: new DotEnvOptions(trimValues: true));
var envVars = DotEnv.Read();
var connection_string = envVars["CONNECTION_STRING"];

if (connection_string == null)
{
  Console.WriteLine($"Unable to load `.env`.");
  return 1;
}

var builder = new ConfigurationBuilder();
builder.AddCommandLine(args);
var config = builder.Build();
var command = config["command"];

if (command == null)
{
  Console.WriteLine($"`command` arg is null.");
  Console.WriteLine($"Specify command listed below.");
  Console.WriteLine($"\t- select");
  Console.WriteLine($"\t- insert");
  Console.WriteLine($"\t- update");
  Console.WriteLine($"\t- delete");
  return 1;
}

// MariaDBに接続
MySqlConnection conn = new(connection_string);
Console.WriteLine($"Connection string -> {conn.ConnectionString}");

conn.Open();

switch (command)
{
  case "select":
    {
      string sql = "SELECT * FROM users";
      MySqlCommand cmd = new(sql, conn);
      MySqlDataReader reader = cmd.ExecuteReader();
      while (reader.Read())
      {
        Console.WriteLine(reader["id"] + " : " + reader["name"]);
      }
    }
    break;
  case "insert":
    {
      var name = config["name"];
      if (name == null)
      {
        Console.WriteLine($"`name` arg is null.");
        Console.WriteLine($"Specify name to insert.");
        return 1;
      }
      string sql = "INSERT INTO users (name) VALUES (@name)";
      MySqlCommand cmd = new(sql, conn);
      cmd.Parameters.Add(new MySqlParameter("@name", name));
      cmd.ExecuteNonQuery();
    }
    break;
  case "update":
    {
      var id_str = config["id"];
      if (id_str == null)
      {
        Console.WriteLine($"`id` arg is null.");
        Console.WriteLine($"Specify name to insert.");
        return 1;
      }
      if (int.TryParse(id_str, out int id) == false)
      {
        Console.WriteLine($"id({id}) is not integer.");
        Console.WriteLine($"Specify `id` with integer.");
        return 1;
      }
      var name = config["name"];
      if (name == null)
      {
        Console.WriteLine($"`name` arg is null.");
        Console.WriteLine($"Specify name to insert.");
        return 1;
      }
      string sql = "UPDATE users SET name = @name WHERE id = @id";
      MySqlCommand cmd = new(sql, conn);
      cmd.Parameters.Add(new MySqlParameter("@name", name));
      cmd.Parameters.Add(new MySqlParameter("@id", id));
      cmd.ExecuteNonQuery();
    }
    break;
  case "delete":
    {
      var id_str = config["id"];
      if (id_str == null)
      {
        Console.WriteLine($"`id` arg is null.");
        Console.WriteLine($"Specify name to insert.");
        return 1;
      }
      if (int.TryParse(id_str, out int id) == false)
      {
        Console.WriteLine($"id({id}) is not integer.");
        Console.WriteLine($"Specify `id` with integer.");
        return 1;
      }
      string sql = "DELETE FROM users WHERE id = @id";
      MySqlCommand cmd = new(sql, conn);
      cmd.Parameters.Add(new MySqlParameter("@id", id));
      cmd.ExecuteNonQuery();
    }
    break;
  default:
    {
      Console.WriteLine($"`command` arg is invalid.");
      Console.WriteLine($"Specify command listed below.");
      Console.WriteLine($"\t- select");
      Console.WriteLine($"\t- insert");
      Console.WriteLine($"\t- update");
      Console.WriteLine($"\t- delete");
      return 1;
    }
}

// データベースを閉じる
conn.Close();

return 0;
