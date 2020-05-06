<?php

$servername = "localhost";
$username = "id12764393_webbdev";
$password = "AgentP00$";
$database = "id12764393_players";



$conn = mysqli_connect($servername, $username, $password, $database);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

$json = file_get_contents('php://input');
$data = json_decode($json);


$sql = "SELECT username,gameId,joinedUser,playerCount FROM playerlobby";
$result = mysqli_query($conn, $sql);

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			if(strcmp($row["gameId"], $data->gameId) == 0){
				if(strcmp($row["joinedUser"], 'null') == 0){
				    $sql2 = "UPDATE playerlobby SET joinedUser='$data->username' Where gameId='$data->gameId'";
					
					mysqli_query($conn, $sql2);
					$sql3 = "UPDATE playerlobby SET playerCount='2' Where gameId='$data->gameId'";
					
					mysqli_query($conn, $sql3);
					echo "true";
				}else{
					echo "False";
				}
			}else{
			    echo "False";
			}
		}
	}else{
		echo "Error: sql not connected";
	}
mysqli_close($conn);
?>