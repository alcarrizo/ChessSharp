<?php

$servername = "localhost";
$username = "root";
$password = "";
$database = "test";


$conn = mysqli_connect($servername, $username, $password, $database);

if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}
//https://www.geeksforgeeks.org/how-to-receive-json-post-with-php/
$json = file_get_contents('php://input');
$data = json_decode($json);

$hashed_password = password_hash($data->password, PASSWORD_DEFAULT);

$sql = "INSERT INTO testtable (username, password)
VALUES ('$data->username', '$hashed_password')";

if (mysqli_query($conn, $sql)) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . " " . mysqli_error($conn);
}

mysqli_close($conn);
?>