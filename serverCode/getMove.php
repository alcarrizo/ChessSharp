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

	if (mysqli_num_rows($result) > 0) {
		while($row = mysqli_fetch_assoc($result)) {
			if(strcmp($row["gameId"], $data->gameId) == 0){
				if(strcmp($row["lastMove"], $data->username) == 0){

				}else
					$move = array(
						'ifcheck' => $row["ifcheck"], 
						'checkMate' => $row["checkMate"],
						'forfeit' => $row["forfeit"],
						'askForDraw' => $row["askForDraw"],
						'startX' => $row["startX"],
						'startY' => $row["startY"],
						'endX' => $row["endX"],
						'endY' => $row["endY"],
						'enPassant' => $row["enPassant"],
						'pawnX' => $row["pawnX"],
						'pawnY' => $row["pawnY"],
						'castling' => $row["castling"],
						'rookStartX' => $row["rookStartX"],
						'rookStartY' => $row["rookStartY"],
						'rookEndX' => $row["rookEndX"],
						'rookEndY' => $row["rookEndY"],
						'promotion' => $row["promotion"],
						'pawnEvolvesTo' => $row["pawnEvolvesTo"],
						'lastMove' => $row["lastMove"]
						);
						
			}
		}
	}



echo json_encode($move);
	
mysqli_close($conn);
?>