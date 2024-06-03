### TODO: figure out how to detect the ip4 address # so I can run this app on my phone automatically, without having to run `dev show` every time I encounter a new network...

#nmcli dev show | grep 'IP4\.ADDRESS\|IP4.GATEWAY'
#
#wifi="192.168.1."
#ip4_num="000"
#
#full_address="$wifi$ip4_num"
#echo "$full_address"

dotnet watch run --urls http://192.168.1.173:3000




