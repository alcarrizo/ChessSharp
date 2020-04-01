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


$sql = "SELECT gameId FROM playerlobby";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
	while($row = mysqli_fetch_assoc($result)) {
		$sql2 = "DELETE FROM playerlobby WHERE gameId = '$data->gameId'";
		mysqli_query($conn, $sql2);
		
	}
}

$sql = "SELECT gameId,lastMove FROM movement";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
	while($row = mysqli_fetch_assoc($result)) {
		$sql2 = "DELETE FROM movement WHERE gameId = '$data->gameId'";
		mysqli_query($conn, $sql2);
		
		$sql2 = "DELETE FROM movement WHERE lastMove = '$data->username'";
		mysqli_query($conn, $sql2);
		
	}
}


mysqli_close($conn);
?>