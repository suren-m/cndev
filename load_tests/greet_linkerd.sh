i=0
while true 
do {
      i=$((i+1));
      req="10.0.16.130/strings-app?input=hello-$i";
      echo $req;
      curl -I -s $req;     
      sleep 1s;
   }
done
