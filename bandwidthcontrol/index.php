<html>
<head>
<meta http-equiv="refresh" content="5" >
<title> Gestión de la red </title>
</head>
<body width="100%">
<div id="container">
<div id="header"> 

</div>
<div id="body">
<?php
require_once("db.php");

// OBTENER TODOS LOS USUARIOS Y METERLOS EN LA TABLA

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
die("Conexion fallida: " . $conn->connect_error);
} else {				
$sql = sprintf("SELECT ip, nombre, usosubida, usobajada, timestamp from usuarios ORDER BY timestamp DESC");
$result = $conn->query($sql);

if($result->num_rows > 0)
{ 
	// OUTPUT QUERY
				$contador = 0;
				
		// Obtener datos del array resultante
				while($row = $result->fetch_array())
				  {
					  @${'ip' . "_" . $contador} = $row['ip'];
					  @${'nombre' . "_" . $contador} = $row['nombre'];
					  @${'usosubida' . "_" . $contador} = $row['usosubida'];
					  @${'usobajada' . "_" . $contador} = $row['usobajada'];
					  @${'timestamp' . "_" . $contador} = $row['timestamp'];
						$contador = ($contador + 1);
						
				  }


}
	}


?>

<style type="text/css">
.tg  {border-collapse:collapse;border-spacing:0;border-color:#aaa;}
.tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#aaa;color:#333;background-color:#fff;border-top-width:1px;border-bottom-width:1px;}
.tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:0px;overflow:hidden;word-break:normal;border-color:#aaa;color:#fff;background-color:#f38630;border-top-width:1px;border-bottom-width:1px;}
.tg .tg-6852{font-size:15px;font-family:Verdana, Geneva, sans-serif !important;;vertical-align:top}
.tg .tg-whis{font-size:15px;font-family:Verdana, Geneva, sans-serif !important;;text-align:center;vertical-align:top}
.tg .tg-vh6r{background-color:#FCFBE3;font-size:15px;font-family:Verdana, Geneva, sans-serif !important;;vertical-align:top}
.tg .tg-pzpb{background-color:#FCFBE3;font-size:15px;font-family:Verdana, Geneva, sans-serif !important;;text-align:center;vertical-align:top}
</style>
<center>
<div name="tabla" id="tabla">
<p style="font family=Montserrat">
* Nota: La página se autorefresca por defecto cada 5 segundos. Este valor es modificable en una de las primeras etiquetas meta.
<br>
* Nota: Sólo son mostradas las IPs de 192.168.1x<br>
* Nota: contactar a Enrique si surge algún problema o duda.
</p>
<br> <br>
<table class="tg">
  <tr>
    <th class="tg-6852"><b>IP</b></th>
    <th class="tg-6852"><b>NOMBRE</b></th>
    <th class="tg-6852"><b>USO SUBIDA</b></th>
    <th class="tg-6852"><b>USO BAJADA</b></th>
	<th class="tg-6852"><b>ESTADO</b></th>
  </tr>
  <?php

  $contadorrows = 0;
  while ($contadorrows < $contador) {
	  if(time() - 30 < strtotime(${'timestamp' . "_" . $contadorrows})){
		  $estado = "<font color='green'>"."ONLINE";
		  
		  
	  } else {
		  $estado = "<font color='red'>"."OFFLINE";
		
		  }
		  

	  echo'  <tr>
    <td class="tg-whis">'.@${'ip' . "_" . $contadorrows}.'</td>
    <td class="tg-vh6r">'.@${'nombre' . "_" . $contadorrows}.'</td>
    <td class="tg-whis">'.@${'usosubida' . "_" . $contadorrows}.' KB/s</td>
    <td class="tg-pzpb">'.@${'usobajada' . "_" . $contadorrows}.' KB/s</td>
	<td class="tg-pzpb"><b>'.@$estado.'</b></font></td>
  </tr>';
	$contadorrows++;

	  
  }

  ?>
</table>
</center>

</div>
<div id="footer"> </div>
</div>
</body>
<style>
html,
body {
   margin:0;
   padding:0;
   height:100%;
}
#container {
   min-height:100%;
   position:relative;
}
#header {
   padding:10px;
}
#body {
   padding:10px;
   padding-bottom:60px;   /* Height of the footer */
}
#footer {
   position:absolute;
   bottom:0;
   width:100%;
   height:60px;   /* Height of the footer */
}
</style>

</html>
