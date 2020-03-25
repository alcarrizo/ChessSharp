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

$information = array();

$sql = "SELECT num,username,gameId,joinedUser,playerCount FROM playerlobby";
$result = mysqli_query($conn, $sql);

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			
			$newdata =  array (
				'num' => $row["num"],
				'username' => $row["username"],
				'gameId' => $row["gameId"],
				'playerCount' => $row["playerCount"]
				
			);
			
			array_push($information, $newdata);
			
		}
	}
	
	echo json_encode($information);
	
mysqli_close($conn);
?>