DriveRemap
===========
DriveRemap is a tool to update drive mappings during a server migration. It will check network drives for paths mapped to an 'old server' and will automatically remap the drives to the 'new server'

## Usage ##

### Moving drives using NETBIOS Names ###
  DriveRemap OldServerName NewServerName   

  If any drives with paths to \\OldServer\Share exist, they will be remapped to \\NewServer\Share

### Moving drives using IP Addresses ###
  DriveRemap 10.1.100.1 10.1.100.2

  If any drives with paths to \\10.1.100.1\Share exist, they will be remapped to \\10.1.100.2\Share
  
### Moving drives to a new server and path ###
  DriveRemap OldServerName NewServerName\NewPath

  If any drives with paths to \\OldServerName\Share exist, they will be remapped to \\NewServerName\NewPath\Share
