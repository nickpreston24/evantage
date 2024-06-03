
create_and_run_container() {
  docker build -t $CONTAINER_NAME .
  # docker run -d -p 3000 --name $CONTAINER_NAME $CONTAINER_NAME
  docker run -it --rm --publish 3000:8080 $CONTAINER_NAME $CONTAINER_NAME
} 

print_help() {
  cat <<EOF

  containerize.sh 👀

  Usage:
    ...

  Options:
    --name : name your new containerized .net  app.

EOF
}

# cli switcher
case "$1" in
  --name)
      CONTAINER_NAME=${2}
      create_and_run_container;
    ;;

  *)
    print_help
    ;;
esac