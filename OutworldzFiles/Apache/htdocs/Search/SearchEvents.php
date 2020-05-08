<?php
// AGPL 3.0 by Fred Beckhusen
require( "flog.php" );


include("../Metromap/includes/config.php");
  
   
  $dsn = "mysql:host=$CONF_db_server;port=$CONF_db_port;dbname=$CONF_db_database";
  
  $options = [
    PDO::ATTR_EMULATE_PREPARES   => false, // turn off emulation mode for "real" prepared statements
    PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION, //turn on errors in the form of exceptions
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC, //make the default fetch be an associative array
  ];
  try {
    
    $db = new PDO($dsn,  $CONF_db_user, $CONF_db_pass, $options);
    
  } catch (Exception $e) {
    error_log($e->getMessage());
    exit('Something weird happened'); //something a user can understand
  }
     
  $text = $_GET['query'];
  $text = "%$text%";
  $sqldata['text1'] = $text;
  

  $rc = intval($_GET['rp']);
   
  if ($rc == "") {
      $rc = 100;
  }
  

  $sort = $_GET['sortname'];
  if ($sort == 'name') {
    $sort = 'name';
  }if ($sort == 'utc') {
    $sort = 'dateutc';
  } else{
    $sort = 'description';
  }
  
  $ord = $_GET['sortorder'];
  if ($ord == 'asc') {
    $ord = 'asc';
  } else {
    $ord = 'desc';
  }
  
  $qtype = $_GET['qtype'];
  if ($qtype == 'name') {
    $qtype = 'name';
  } else {
    $qtype = 'description';
  }
  
  flog("text= $text");
  flog("qtype= $qtype");
  flog("ord= $ord");
  flog("sort= $sort");
  
  $total = 0;
   
  $page =  $_GET['page'];
  if ($page == "" ) {
    $page = 1;
  }
  
  $stack = array();

  $q = "SELECT * from ossearch.events  
            where " . $qtype . "  like CONCAT('%', :text1, '%')
            order by " . $sort . ' ' .  $ord ;
    

    flog($q );  
    $query = $db->prepare($q);
    $result = $query->execute($sqldata);
    flog($sqldata );  
    class OUT {}
    class Row {}
    
    $out = new OUT();
    $counter= 0;
    while ($row = $query->fetch(PDO::FETCH_ASSOC))
    {
    
        $hop    = "hop://" . $row["gateway"] ;
        $v3     = "secondlife://http|!!" . $row["gateway"] ;
        $hg     = "secondlife://" . $row["gateway"] ;
        
        
        #$link = "<a href=\"$hop\"><img src=\"hop.png\" height=\"24\"></a>";
        $link = "<br><a href=\"$v3\"><img src=\"v3hg.png\" height=\"24\"></a>";
        #$link .= "<br><a href=\"$hg\"><img src=\"hg.png\" height=\"24\"></a>";
  
        
        $description = $row["description"] .  '<br><br><a href="' . $v3 . '">Link: ' . $row["gateway"] . '</a>';
        $name = $row["name"];    
        $time = date("D M j G:i:s T Y", $row["dateUTC"]);
        
        $row = array( "time"        => $time,
                      "name"        => $name,
                      "description" => $description,
                      "duration"    => $row["duration"],
                      "location"    => $link,
                      "utc"         => $row["dateUTC"],
                     );
        
        $rowobj = new Row();
        $rowobj->cell = $row;
        
        #$myJSON = json_encode($rowobj);
        #echo $myJSON . "<br>";
        if ($total >= (($page-1) *$rc) && $total < ($page) *$rc) {
          array_push($stack, $rowobj);
        }
        $total++;
        
    }
    if ($total == 0) {
	  flog("Nothing found");
      $row = array("hop"=>"", "Name"=>"No records","Description"=>"No records","Regionname"=>"No records","Location"=>"No records");
      $rowobj = new Row();
      $rowobj->cell = $row;
      array_push($stack, $rowobj);
    }

    $out->domain = $CONF_domain;
    $out->port = $CONF_port;
    $out->page  = $page;
    $out->total = $total;
    $out->rows  = $stack;
        
    $myJSON = json_encode($out);

    echo $myJSON;
   
   
?>
