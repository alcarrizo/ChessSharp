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
		    if(strcmp($row["username"], $data->username) == 0 or strcmp($row["joinedUser"], $data->username) == 0){	
			    $newdata =  array (
				    'username' => $row["username"],
				    'gameId' => $row["gameId"],
				    'playerCount' => $row["playerCount"],
				    'joinedUser' => $row["joinedUser"]
					);
					
				echo json_encode($newdata);

			}
		}
	}
	
	
mysqli_close($conn);
?>