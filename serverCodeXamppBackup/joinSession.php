<?php

$servername = "localhost";
$username = "root";
$password = "";
$database = "player";


$conn = mysqli_connect($servername, $username, $password, $database);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

$json = file_get_contents('php://input');
$data = json_decode($json);


$information = array('gameId' => "null",'playerCount' => "0");
$information["gameId"] = "null";
$information["playerCount"] = "0";

$sql = "SELECT username,gameId,joinedUser, playerCount FROM playerlobby";
$result = mysqli_query($conn, $sql);

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			if(strcmp($row["username"], $data["joining"]) == 0){
				if(strcmp($row["joinedUser"], "None") == 0){
				     $sql2 = "INSERT INTO playerlobby (joinedUser)
						VALUES ($data->username)";

					if (mysqli_query($conn, $sql2)) {
						echo json_encode($information);
					} else {
						echo "Error: " . $sql . " " . mysqli_error($conn);
						}
				}else{
					echo "Lobby Full!"
				}
			}
		}
	}
mysqli_close($conn);
?>