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



$sql = "SELECT gameId,lastMove,ifcheck,
		checkMate,forfeit,askForDraw,
		startX,startY,endX,endY,enPassant,
		pawnX,pawnY,castling,rookStartX,
		rookStartY,rookEndX,rookEndY,promotion,
		pawnEvolvesTo FROM movement";
$result = mysqli_query($conn, $sql);	
$inDB = 0;

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			if(strcmp($row["gameId"], $data->gameId) == 0){
				$sql3 = "UPDATE movement SET 
				lastMove = '$data->username', 
				ifcheck = '$data->check',
				checkMate = '$data->checkMate', 
				forfeit = '$data->forfeit', 
				askForDraw = '$data->askForDraw', 
				startX = '$data->startX',
				startY = '$data->startY', 
				endX = '$data->endX',
				endY = '$data->endY', 
				enPassant = '$data->enPassant', 
				pawnX = '$data->pawnX',
				pawnY = '$data->pawnY', 
				castling = '$data->castling', 
				rookStartX = '$data->rookStartX', 
				rookStartY = '$data->rookStartY',
				rookEndX = '$data->rookEndX', 
				rookEndY = '$data->rookEndY', 
				promotion = '$data->promotion', 
				pawnEvolvesTo = '$data->pawnEvolvesTo' 
				Where gameId ='$data->gameId'";
				if (mysqli_query($conn, $sql3)) {
						echo "done";
				} else {
						echo "Error: " . $sql . " " . mysqli_error($conn);
				}
				$inDB = 1;
			}
		}
	}
	if($inDB == 0){
				$sql2 = "INSERT INTO movement (gameId,lastMove,ifcheck,
												checkMate,forfeit,askForDraw,
												startX,startY,endX,endY,enPassant,
												pawnX,pawnY,castling,rookStartX,
												rookStartY ,rookEndX,rookEndY, 
												promotion,pawnEvolvesTo)
					VALUES ('$data->gameId', '$data->username', '$data->check', 
							'$data->checkMate','$data->forfeit','$data->askForDraw',
							'$data->startX','$data->startY','$data->endX', '$data->endY',
							'$data->enPassant','$data->pawnX', '$data->pawnY',
							'$data->castling','$data->rookStartX','$data->rookStartY',
							'$data->rookEndX','$data->rookEndY', '$data->promotion',
							'$data->pawnEvolvesTo')";
				if (mysqli_query($conn, $sql2)) {
						echo "done";
				} else {
						echo "Error: " . $sql . " " . mysqli_error($conn);
				}
	}
mysqli_close($conn);
?>