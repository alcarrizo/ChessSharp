<?php

$servername = "localhost";
$username = "root";
$gameId = "";
$database = "player";


$conn = mysqli_connect($servername, $username, $gameId, $database);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

$json = file_get_contents('php://input');
$data = json_decode($json);

$sql = "INSERT INTO playerlobby (username, lobbyId)
VALUES ('$data->username', '$data->gameId')";

if (mysqli_query($conn, $sql)) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . " " . mysqli_error($conn);
}

mysqli_close($conn);
?>