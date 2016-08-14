CKSDEV - Strong Name Signing 

CKSDEV allows non project members to create local builds of the product. This enables non-members to 
add features and fix bugs without first consulting with the CKSDEV team. Features which at a later
point in time may be merged into the main codebase. 

To ensure that these builds can be differentiated from the 'real' product compiled by the CKSDEV team 
the strong name key file used by CKSDEV is password protected. A second key file containing only the 
CKSDEV public key can be used to create delay signed builds. These builds will run as the real product
as long as key verification is skipped for the CKSDEV assemblies. 

The Debug and Release builds are delay-signed and can be compiled on any machine without needing a 
password. The DebugSigned and ReleaseSigned builds are signed and password protected. 

In order to compile and run CKSDEV, first run the DisableStrongNameVerification.bat in a Visual Studio 
command prompt (sn.exe must be in the %PATH% environment variable). Then you can use either the Debug 
or Release build configuration to perform a local build. 