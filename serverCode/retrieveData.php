<?php

$servername = "localhost";
$username = "root";
$password = "";
$database = "test";


$conn = mysqli_connect($servername, $username, $password, $database);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}
$json = file_get_contents('php://input');
$data = json_decode($json);

$sql = "SELECT username, password FROM testtable";
$result = mysqli_query($conn, $sql);

$status = False;


$loginstatus = array('login' => 0,'username' => 0);
$loginStatus["login"] = 0;
$loginStatus["username"] = 0;

if (mysqli_num_rows($result) > 0) {
	while($row = mysqli_fetch_assoc($result)) {
		
		if(strcmp($row["username"] ,$data->username)==0){
				$loginStatus["username"] = 1;
			if(password_verify($data->password,$row["password"])){
					$status = True;
			}
		}
    }
}

if($status){
	$loginStatus["login"] = 1;
	$loginStatus["username"] = 1;
	
    echo json_encode($loginStatus);
}else{
	if($loginStatus["username"] != 1){
		$loginStatus["username"] = 0;
	}
	$loginStatus["login"] = 0;
    echo json_encode($loginStatus);
}

mysqli_close($conn);
?>