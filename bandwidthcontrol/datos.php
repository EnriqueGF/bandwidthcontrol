<?php
require_once("db.php");

if (isset($_POST['ip'])) {
$nombre = $_POST['nombre'];
$id = $_POST['id'];
$ip = $_POST['ip'];
$usosubida = $_POST['usosubida'];
$usobajada = $_POST['usobajada'];

	  // SQL
         	$conn = new mysqli($servername, $username, $password, $dbname);
         			if ($conn->connect_error) {
					die("Conexion fallida: " . $conn->connect_error);
         			} else {
						
         		$sql = sprintf("REPLACE into usuarios (id, ip, nombre, usosubida, usobajada) values('".$id."', '".$ip."', '".$nombre."', '".$usosubida."', '".$usobajada."')");
				$result = $conn->query($sql);
	}

	
}



?>