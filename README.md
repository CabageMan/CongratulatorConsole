# CongratulatorConsole

## Functionality:
- Shows birthdays list;
- Shows todays and upcoming birthdays;
- Adding, deleting and editing birthday records;
- Records are stored either in a .xml file or in a MySQL database;

## To Run:
- In case using FileDatasource just run application and select FileDatasource in Welcome menu;
- In case using database datasource the local MySQL server is needed. Connection to database is implemented in app/Services/MySQLConnection.cs.

The database is accessed with **appSettings.json** file in the **app/** directory, which is not tracked by Git (it is not a good solution, but I have some difficulties with setting UserSecrets). 
Please create and put the **appSettings.json** file in to the **app/** directory. The structure of the appSettings.json file:
```
{
  "CongratulatorDatabase": {
    "UserName": "username",
    "Password": "password"
  }
}
```

On launch the Congratulator creates the needed **database** and **table** if they are not existed.
