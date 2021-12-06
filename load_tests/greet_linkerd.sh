i=0
while true 
do {
      i=$((i+1));
      req="10.0.16.109/strings-app?input=hello-$i";
      echo $req;
      curl -I $req;    
      sleep 1s;
   }
done