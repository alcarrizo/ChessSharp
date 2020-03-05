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


$canCreate = 0;

$information = array('gameId' => "null",'playerCount' => "0");
$information["gameId"] = "null";
$information["playerCount"] = "0";


while($canCreate == 0){
		$permitted_chars = '0123456789abcdefghijklmnopqrstuvwxyz';
		$information["gameId"] = substr(str_shuffle($permitted_chars), 0, 10);
		
	$sql = "SELECT username,gameId,joinedUser,playerCount FROM playerlobby";
	$result = mysqli_query($conn, $sql);

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			if(!strcmp($row["gameId"], $information["gameId"]) == 0){
					$canCreate = 1;
					$information['playerCount'] = "1";
					
			}
		}
	}else{
		$canCreate = 1;
		$information['playerCount'] = "1";
	}
}

$temp = "None";

if($canCreate == 1){
	$sql2 = "INSERT INTO playerlobby (username, gameId,joinedUser, playerCount)
	VALUES ('$data->username', '$information[gameId]', '$temp', '$information[playerCount]')";

	if (mysqli_query($conn, $sql2)) {
		echo json_encode($information);
	} else {
		echo "Error: " . $sql . " " . mysqli_error($conn);
	}
}
mysqli_close($conn);
?>