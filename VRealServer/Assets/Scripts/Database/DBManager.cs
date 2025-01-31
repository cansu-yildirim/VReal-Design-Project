using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Npgsql;

public class DBManager : MonoBehaviour
{
    private static DBManager _singleton;
	private static NpgsqlConnection conn;
    public static DBManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(DBManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }
    

    private void Awake()
    {
        Singleton = this;
    }
	
	//Public NpgsqlConnection object set within the parameters inside the ConnectionManager class
    private void setConnection(){
        conn = ConnectionManager.getConnection();
    }
    //Returns NpgsqlConnection object
    public static NpgsqlConnection getConnection(){
        return conn;
    }
    //Opens connection to the database
    public void openConnection(){
        conn.Open();
    }
    //Closes connection to the database
    public void closeConnection(){
        conn.Close();
    }
	
	public static User checkUser(User user){
        UsersController uc = new UsersController();
        User temp = uc.selectUser(conn, user.e_mail);
        if(temp.e_mail == null){
            Debug.Log("User: "+user.e_mail+" not exist adding new user to database");
            uc.insertUser(conn, user);
            int id = uc.getLastUser(conn);
            Debug.Log("Logged in as: "+user.e_mail+" user id is: "+id);
            temp = checkUser(temp);
        }
        else {
            Debug.Log("Logged in as: "+user.e_mail);
        }
        return temp;
    }
    public static List<Room> myRooms(User user){//Returns list of rooms with ID and name is usefulll but wall, ceiling, floor, furniture is dummy
        RoomController rc = new RoomController();
        RoomUserController ruc = new RoomUserController();
        List<Room> ret = new List<Room>();
        List<RoomUser> list = ruc.selectUsersRoom(conn, user);
        for(int i = 0; i<list.Count; i++){
            Room room = rc.getRoomName(conn, list[i].scene_id);
            ret.Add(room);
        }
        
        return ret;
    }
    public static void insertRoom(Room room, User user){//Inserts given room to database
        RoomController rc = new RoomController();
        RoomUserController ruc = new RoomUserController();
        
        List<Room> usersRooms = myRooms(user);
        int count = 1;
        string name = room.name;
        for(int i = 0; i<usersRooms.Count; i++){
            if(string.Compare(room.name, usersRooms[i].name) == 0){
                count++;
                rc.updateRoom(conn, room);
            }
        }

        if(count == 1){//Room has unique name under user
            rc.insertRoom(conn, room);
            int id = rc.getLastRoomId(conn);
            RoomUser ru = new RoomUser(1, user.id, id);
            ruc.insertRoomUser(conn, ru);
        }
    }
    public static Room loadRoom(User user, string roomName){//Returns given users room with given room name
        RoomController rc = new RoomController();
        RoomUserController ruc = new RoomUserController();
        List<Room> usersRooms = myRooms(user);
        Room ret = new Room();

        for(int i = 0; i<usersRooms.Count; i++){
            if(string.Compare(roomName, usersRooms[i].name) == 0){
                ret = rc.selectRoom(conn, usersRooms[i].id);
            }
        }
        return ret;
    }


    private void Start()
    {
        setConnection();
    }
    

    

}
