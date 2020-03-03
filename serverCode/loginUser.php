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

$sql = "SELECT username, password FROM playerlogininfo";
$result = mysqli_query($conn, $sql);


$loginstatus = array('login' => false,'username' => false);
$loginStatus["login"] = false;
$loginStatus["username"] = false;

if (mysqli_num_rows($result) > 0) {
	while($row = mysqli_fetch_assoc($result)) {
		if(strcmp($row["username"] ,$data->username)== 0){
				$loginStatus["username"] = true;
			if(password_verify($data->password,$row["password"])){
					$loginStatus["login"] = true;
			}
		}
    }
}

	
if($loginStatus["login"] == true){
    $sql = "INSERT INTO onlineplayers (username)
		VALUES ('$data->username')";
	mysqli_query($conn, $sql);
}
echo json_encode($loginStatus);

mysqli_close($conn);
?>