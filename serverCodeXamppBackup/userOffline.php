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

$sql = "SELECT username FROM onlineplayers";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
	while($row = mysqli_fetch_assoc($result)) {
		$sql2 = "DELETE FROM onlineplayers WHERE username = '$data->username'";
		mysqli_query($conn, $sql2);
	}
}

mysqli_close($conn);
?>